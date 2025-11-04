// Lokasi: WEBSAMPLE/Controllers/PenilaianController.cs

using System;
using System.Configuration; // Untuk membaca connections.config
using System.Data.SqlClient; // Untuk SQL Server
using System.Web.Http;
using System.Security.Claims; // Untuk mendapatkan nama user
using System.Linq;
using MongoDB.Driver; // Untuk MongoDB
using WEBSAMPLE.Models; // PENTING: Untuk mengakses NilaiModel Anda

[RoutePrefix("api/penilaian")]
public class PenilaianController : ApiController
{
    // --- KONEKSI DATABASE ---
    
    // Baca koneksi string dari file connections.config Anda
    private readonly string _sqlConnectionString = ConfigurationManager.ConnectionStrings["IdentityDB"].ConnectionString; // Sesuaikan "IdentityDB" jika namanya beda
    
    // Koneksi MongoDB
    private readonly IMongoDatabase _mongoDatabase;

    public PenilaianController()
    {
        // Inisialisasi koneksi MongoDB
        string mongoConnectionString = ConfigurationManager.ConnectionStrings["MongoDBData"].ConnectionString;
        var mongoClient = new MongoClient(mongoConnectionString);
        
        // Ambil nama database dari connection string (misal: "mongodb://.../MyAssessment")
        var dbName = MongoUrl.Create(mongoConnectionString).DatabaseName; 
        _mongoDatabase = mongoClient.GetDatabase(dbName);
    }

    // --- ENDPOINT UNTUK INPUT NILAI ---
    // API ini akan dipanggil oleh: POST /api/penilaian/input
    
    [HttpPost]
    [Route("input")]
    [Authorize] // Memastikan hanya user yang sudah login yang bisa akses
    public IHttpActionResult InputNilai([FromBody] NilaiModel data)
    {
        // 1. Validasi data yang masuk
        if (data == null || !ModelState.IsValid)
        {
            // Jika data tidak valid (misal: nilai > 100), kirim balik error
            return BadRequest(ModelState);
        }

        // Dapatkan nama user yang sedang login
        var claimsIdentity = User.Identity as ClaimsIdentity;
        string inputBy = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value ?? "System";

        // 2. Kalkulasi Nilai Akhir (Sesuai JS Anda: 30% Tugas + 30% UTS + 40% UAS)
        data.NilaiAkhir = (data.Tugas * 0.3m) + (data.UTS * 0.3m) + (data.UAS * 0.4m);
        data.TanggalInput = DateTime.Now;
        data.InputBy = inputBy;

        int newSqlId = 0; // Untuk menyimpan ID dari SQL dan dipakai di log Mongo

        // 3. Simpan ke SQL Server (Database Utama)
        try
        {
            using (var conn = new SqlConnection(_sqlConnectionString))
            {
                // Kita gunakan OUTPUT INSERTED.Id untuk langsung mendapatkan ID baris baru
                string sqlQuery = @"
                    INSERT INTO dbo.Grades (Nama, NIS, Mapel, Semester, Tugas, UTS, UAS, NilaiAkhir, Keterangan, TanggalInput, InputBy)
                    OUTPUT INSERTED.Id 
                    VALUES (@Nama, @NIS, @Mapel, @Semester, @Tugas, @UTS, @UAS, @NilaiAkhir, @Keterangan, @TanggalInput, @InputBy)";
                
                using (var cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Nama", data.Nama);
                    cmd.Parameters.AddWithValue("@NIS", data.NIS);
                    cmd.Parameters.AddWithValue("@Mapel", data.Mapel);
                    cmd.Parameters.AddWithValue("@Semester", data.Semester);
                    cmd.Parameters.AddWithValue("@Tugas", data.Tugas);
                    cmd.Parameters.AddWithValue("@UTS", data.UTS);
                    cmd.Parameters.AddWithValue("@UAS", data.UAS);
                    cmd.Parameters.AddWithValue("@NilaiAkhir", data.NilaiAkhir);
                    cmd.Parameters.AddWithValue("@Keterangan", (object)data.Keterangan ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TanggalInput", data.TanggalInput);
                    cmd.Parameters.AddWithValue("@InputBy", data.InputBy);

                    conn.Open();
                    // ExecuteScalar akan mengembalikan ID baris baru
                    newSqlId = (int)cmd.ExecuteScalar(); 
                }
            }
        }
        catch (Exception ex)
        {
            // Jika gagal simpan ke SQL, kirim error
            return InternalServerError(ex);
        }

        // 4. Simpan Log ke MongoDB (Jejak Audit)
        try
        {
            var logCollection = _mongoDatabase.GetCollection<NilaiLog>("GradeAuditLog");

            var log = new NilaiLog
            {
                NilaiId = newSqlId, // ID dari tabel SQL
                Action = "CREATE",
                Data = data,
                Timestamp = DateTime.UtcNow,
                UserAgent = Request.Headers.UserAgent.ToString(),
                IpAddress = Request.GetOwinContext().Request.RemoteIpAddress
            };
            
            logCollection.InsertOne(log);
        }
        catch (Exception ex)
        {
            // Jika log gagal, jangan batalkan transaksi utama,
            // Cukup catat errornya (di dunia nyata, ini akan dicatat ke file log error)
            System.Diagnostics.Debug.WriteLine("Gagal logging ke MongoDB: " + ex.Message);
        }

        // 5. Kirim balasan Sukses ke Frontend
        return Ok(new ApiResponse { Success = true, Message = "Nilai berhasil disimpan." });
    }

    // TODO: Buat endpoint lain di sini (misal: untuk Laporan Nilai)
    // [HttpGet]
    // [Route("")] // /api/penilaian
    // public IHttpActionResult GetLaporanNilai()
    // {
    //    // Logika untuk SELECT * FROM dbo.Grades
    // }
}