# MyAssassment_Projek-UTS_PemrogramanVisual
MyAssassment - Projek UTS Pemrograman Visual (Desktop)

Proses setup proyek ini melibatkan dua bagian utama: backend (Web Service) dan hosting (Configuration Web).

Setting Web Service (Backend ASP.NET Web API):

Konfigurasi API: Logika routing API diatur dalam file App_Start/WebApiConfig.cs. File ini kemudian "dinyalakan" saat aplikasi pertama kali berjalan dengan memanggil GlobalConfiguration.Configure(WebApiConfig.Register) dari file Global.asax.cs.

Pembuatan Model: Model data C# (seperti NilaiModel.cs) dibuat di dalam folder Models untuk menentukan struktur data yang akan diterima dari frontend.

Pembuatan Controller: API Controller (seperti PenilaianController.cs) dibuat di folder Controllers. Ini adalah "otak" backend yang berisi endpoint (misal: [HttpPost, Route("input")]) untuk menerima data, memvalidasinya, menghitung nilai akhir, dan menyimpannya ke database.

Configuration Web (Frontend & Hosting):

Hosting di IIS: Aplikasi ini di-deploy menggunakan Internet Information Services (IIS). Prosesnya meliputi:

Membuat Application Pool baru ("MyAssessment").

Menambahkan Website baru ("MyAssessment").

Mengatur Physical Path (Jalur Fisik) agar menunjuk ke folder build aplikasi (misal: C:\Framework MyAssessment\WEBSAMPLE).

Mengatur Bindings (Pengikatan) untuk menetapkan port (misal: port 9000).

Konfigurasi Koneksi: File connections.config adalah file vital yang menyimpan semua connection string. File ini dibaca oleh backend untuk mengetahui cara terhubung ke SQL Server (menggunakan kunci UserDBContext) dan MongoDB (menggunakan kunci MongoDBData).

Konfigurasi Frontend (SPA): Karena ini adalah Single Page Application (SPA), file app.js digunakan untuk mengatur routing di sisi klien. File ini memetakan URL (misal: .../#/input-nilai) ke file HTML (templates/penilaian/input-nilai.html) dan file JavaScript (InputNilaiController.js) yang sesuai.

2. Buat sebuah Nama Project, Dan jelaskan kenapa Menggunakan Project Tersebut 

Nama Proyek: MyAssessment

Nama Perusahaan (Developer): PT. Hyoka Wanssss

Alasan Pemilihan Proyek: Proyek "MyAssessment" (Sistem Penilaian Siswa) ini dipilih karena merupakan studi kasus yang ideal untuk mendemonstrasikan arsitektur Polyglot Persistence (menggunakan beberapa database berbeda). Proyek ini memecahkan satu masalah bisnis (input nilai) dengan dua kebutuhan data yang sangat berbeda:

Kebutuhan Akurasi (SQL Server): Data nilai siswa (Tugas, UTS, UAS) adalah data transaksional yang harus akurat, terstruktur, dan konsisten. SQL Server sempurna untuk menyimpan data nilai final ini.

Kebutuhan Audit & Volume (MongoDB): Institusi perlu melacak setiap perubahan nilai (siapa, kapan, apa yang diubah). Data log ini bervolume sangat besar dan skemanya bisa fleksibel. MongoDB sangat cepat untuk "menelan" (mencatat) data log baru tanpa membebani database SQL Server utama.

Proyek ini secara efektif menunjukkan cara menggunakan teknologi yang tepat (SQL Server) untuk data terstruktur dan teknologi yang tepat (MongoDB) untuk data tidak terstruktur (log) dalam satu aplikasi yang sama.

Alasan Penamaan:

"MyAssessment": Nama ini dipilih agar mudah diingat. "My" memberikan kesan personal bagi pengguna (guru), dan "Assessment" (Penilaian) adalah istilah profesional yang mencakup seluruh proses (Tugas, UTS, UAS), bukan hanya "Nilai".

"PT. Hyoka Wanssss": Nama perusahaan ini menggabungkan keahlian dan identitas. "Hyoka" (評価) adalah bahasa Jepang untuk "Evaluasi" atau "Penilaian", yang memposisikan perusahaan sebagai ahli di bidang ini. "Wanssss" adalah identitas personal dari developer.

3. Buatkan Bisnis Proses nya, dan dijelaskan 

Bisnis proses utama adalah "Proses Input dan Audit Nilai Siswa".

Aktor: Guru (Pengguna) Sistem: Aplikasi MyAssessment (IIS, ASP.NET API, SQL Server, MongoDB)

Alur Proses:

Autentikasi: Guru melakukan login ke aplikasi. Sistem memvalidasi kredensialnya ke database MyAssessment_auth (SQL Server).

Navigasi: Guru mengklik menu "Penilaian" -> "Input Nilai" di sidebar.

Inisiasi Frontend: Aplikasi (AngularJS) memuat halaman input-nilai.html dan InputNilaiController.js.

Input Data: Guru mengisi formulir (Nama Siswa, NIS, Mapel, Nilai Tugas, UTS, UAS) lalu mengklik "Simpan".

Pengiriman Frontend: InputNilaiController.js memvalidasi data dan mengirimkannya (via POST) ke endpoint backend di http://localhost:9000/api/penilaian/input.

Penerimaan Backend (IIS & ASP.NET):

IIS menerima panggilan API dan meneruskannya ke PenilaianController.cs.

Controller memvalidasi data menggunakan NilaiModel.cs.

Controller menghitung NilaiAkhir menggunakan rumus (misal: 30% Tugas + 30% UTS + 40% UAS).

Penyimpanan Database (Inti Proses):

(SQL Server): Controller menjalankan INSERT untuk menyimpan data nilai utama (Nama, NIS, NilaiAkhir, dll.) ke tabel dbo.Grades di database MyAssessment. Sistem mendapatkan Id dari data yang baru disimpan.

(MongoDB): Setelah SQL berhasil, Controller membuat satu dokumen JSON (log) yang berisi salinan lengkap data nilai, Action: "CREATE", Timestamp, dan NilaiId (dari SQL). Dokumen ini disimpan ke koleksi GradeAuditLog di MongoDB.

Respon: Sistem backend mengirim balasan "Sukses" ke frontend.

Konfirmasi: Frontend menampilkan notifikasi "Nilai berhasil disimpan" kepada Guru.

4. Buatkan Model Data nya, dan dijelaskan 

Model data proyek ini adalah model Hybrid atau Polyglot Persistence, yang sengaja memisahkan data operasional dan data audit.

Grup 1: SQL Server (Data Operasional & Keamanan) Model ini digunakan untuk data yang terstruktur, relasional, dan menuntut konsistensi (ACID).

Database MyAssessment_auth: Mengelola keamanan. AspNetUsers (pengguna) terhubung ke AspNetRoles (peran) melalui tabel AspNetUserRoles. Peran ini kemudian dihubungkan ke Menus melalui RoleMenuMapping untuk mengontrol hak akses sidebar.

Database MyAssessment: Menyimpan data inti aplikasi. Tabel utamanya adalah dbo.Grades, yang berisi data final nilai siswa (NIS, Mapel, Tugas, UTS, UAS, NilaiAkhir).

Grup 2: MongoDB (Data Audit Log) Model ini digunakan untuk data yang bervolume tinggi, semi-terstruktur, dan bersifat append-only (hanya ditambah, tidak pernah diubah).

Koleksi GradeAuditLog: Setiap dokumen di sini adalah "foto" (snapshot) dari data nilai pada saat data itu dibuat atau diubah. Ini berisi action ("CREATE" atau "UPDATE"), data lengkap, timestamp, dan NilaiId.

Hubungan Lintas Sistem (Kunci Penjelasan): Model data ini dihubungkan oleh satu key (kunci). Kolom Id (Primary Key) dari tabel dbo.Grades (SQL Server) disimpan sebagai field NilaiId di dalam setiap dokumen di GradeAuditLog (MongoDB). Ini memungkinkan kita untuk melacak seluruh riwayat perubahan untuk satu data nilai spesifik hanya dengan mem-filter NilaiId di MongoDB.

5. Buatkan schema Team dalam Mengerjakan project tersebut 

Karena proyek ini dikerjakan oleh satu orang (Solo Developer), skema tim mencerminkan berbagai "peran" atau "topi" yang harus dipakai oleh satu developer tersebut untuk menyelesaikan proyek dari awal hingga akhir.

Skema Tim: "Full-Stack Developer (Solo)"

Developer: Wawan Suwandi (PT. Hyoka Wanssss)

Pembagian Peran dan Tanggung Jawab:

Project Manager / Product Owner:

Tugas: Mendefinisikan requirement proyek (sesuai Poin 1-6), menentukan scope (penilaian siswa), dan memastikan proyek selesai tepat waktu.

Database Administrator (DBA):

Tugas: Merancang Model Data (Poin 4), membuat diagram ERD, dan menulis script SQL (CreateDatabase.sql) untuk membuat tabel dbo.Grades.

Backend Developer:

Tugas: Melakukan Setting Web Service (Poin 1). Membuat API (C#), PenilaianController.cs, NilaiModel.cs, dan menulis logika untuk terhubung dan menyimpan data ke SQL Server dan MongoDB.

Frontend Developer:

Tugas: Melakukan Configuration Web (Poin 1). Membuat Mockup (Poin 6), halaman HTML (input-nilai.html), styling (CSS), dan logika frontend (InputNilaiController.js, app.js).

DevOps & System Administrator:

Tugas: Melakukan Setting IIS (Poin 1), menginstal library (NuGet MongoDB.Driver), dan melakukan deployment (mem-publish) aplikasi agar bisa diakses melalui http://localhost:9000/.

Quality Assurance (QA) / Tester:

Tugas: Memastikan semua alur (Bisnis Proses, Poin 3) berjalan, tombol "Simpan" berfungsi, dan data benar-benar masuk ke kedua database.

Deskripsi Repositori GitHub
MyAssessment (PT. Hyoka Wanssss)

Aplikasi web sistem penilaian siswa (Student Gradebook) yang dibangun sebagai studi kasus arsitektur Polyglot Persistence.

Proyek ini mendemonstrasikan bagaimana ASP.NET Web API (dihosting di IIS) mengelola data transaksional di SQL Server (nilai akhir siswa) sambil secara bersamaan mencatat jejak audit (audit log) yang immutable (abadi) ke MongoDB setiap kali ada perubahan data.

Frontend dibangun sebagai Single Page Application (SPA) menggunakan AngularJS.

Teknologi Utama:

Hosting: IIS (Internet Information Services)

Backend: C# ASP.NET Web API

Database Operasional (Nilai): SQL Server

Database Audit (Log): MongoDB

Frontend: AngularJS (SPA), HTML, JavaScript
