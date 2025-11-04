// WEBSAMPLE/Models/NilaiModel.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace WEBSAMPLE.Models
{
    public class NilaiModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama siswa wajib diisi")]
        [StringLength(100)]
        public string Nama { get; set; }

        [Required(ErrorMessage = "NIS wajib diisi")]
        [StringLength(20)]
        public string NIS { get; set; }

        [Required(ErrorMessage = "Mata pelajaran wajib dipilih")]
        [StringLength(50)]
        public string Mapel { get; set; }

        [Required(ErrorMessage = "Semester wajib dipilih")]
        [Range(1, 2, ErrorMessage = "Semester harus 1 atau 2")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Nilai tugas wajib diisi")]
        [Range(0, 100, ErrorMessage = "Nilai tugas harus antara 0-100")]
        public decimal Tugas { get; set; }

        [Required(ErrorMessage = "Nilai UTS wajib diisi")]
        [Range(0, 100, ErrorMessage = "Nilai UTS harus antara 0-100")]
        public decimal UTS { get; set; }

        [Required(ErrorMessage = "Nilai UAS wajib diisi")]
        [Range(0, 100, ErrorMessage = "Nilai UAS harus antara 0-100")]
        public decimal UAS { get; set; }

        public decimal NilaiAkhir { get; set; }

        [StringLength(500)]
        public string Keterangan { get; set; }

        public DateTime TanggalInput { get; set; }

        public string InputBy { get; set; }

        // Calculated properties
        public string Grade
        {
            get
            {
                if (NilaiAkhir >= 90) return "A";
                if (NilaiAkhir >= 80) return "B";
                if (NilaiAkhir >= 70) return "C";
                if (NilaiAkhir >= 60) return "D";
                return "E";
            }
        }

        public string Status
        {
            get
            {
                return NilaiAkhir >= 70 ? "Lulus" : "Tidak Lulus";
            }
        }
    }

    // Response model
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    // MongoDB Log Model
    public class NilaiLog
    {
        public string Id { get; set; }
        public int NilaiId { get; set; }
        public string Action { get; set; }
        public NilaiModel Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
    }
}