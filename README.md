# 🧮 MyAssessment — Sistem Penilaian Siswa  
**Developer:** PT. Hyoka Wanssss  
**Teknologi:** ASP.NET Web API, AngularJS, SQL Server, MongoDB, IIS

---

## 📘 Deskripsi Proyek

**MyAssessment** adalah aplikasi web untuk mengelola dan mengaudit nilai siswa.  
Proyek ini dikembangkan sebagai **studi kasus arsitektur Polyglot Persistence**, di mana dua jenis database digunakan secara bersamaan:

- **SQL Server** → menyimpan data nilai utama (terstruktur & transaksional)  
- **MongoDB** → mencatat log perubahan (audit trail) dengan fleksibilitas tinggi  

Aplikasi ini menunjukkan bagaimana satu sistem dapat menyeimbangkan **akurasi data** dan **volume log besar** dengan memanfaatkan kekuatan dua teknologi database berbeda.

---

## 🏗️ Arsitektur Proyek

Proyek ini terdiri dari dua bagian utama:

### 1. Backend (Web Service)
Dibangun menggunakan **ASP.NET Web API** dan di-host melalui **IIS**.

**Struktur utama:**
- **Routing API:**  
  Diatur dalam `App_Start/WebApiConfig.cs` dan dipanggil dari `Global.asax.cs` menggunakan `GlobalConfiguration.Configure(WebApiConfig.Register)`.
- **Model:**  
  File seperti `NilaiModel.cs` di folder `Models/` mendefinisikan struktur data nilai siswa.
- **Controller:**  
  File seperti `PenilaianController.cs` di folder `Controllers/` menangani request, validasi, perhitungan nilai akhir, dan penyimpanan data ke SQL & MongoDB.

---

### 2. Configuration Web (Frontend & Hosting)
Frontend dibangun sebagai **Single Page Application (SPA)** dengan **AngularJS**.

**Detail Konfigurasi:**
- **Hosting IIS:**  
  1. Membuat *Application Pool* baru (`MyAssessment`)  
  2. Menambahkan *Website* baru dengan jalur fisik `C:\Framework MyAssessment\WEBSAMPLE`  
  3. Mengatur port binding (contoh: `9000`)
- **Connection Settings:**  
  File `connections.config` menyimpan *connection string* untuk `UserDBContext` (SQL Server) dan `MongoDBData` (MongoDB).
- **Routing SPA:**  
  File `app.js` memetakan URL seperti `/#/input-nilai` ke:
  - Template: `templates/penilaian/input-nilai.html`
  - Controller: `InputNilaiController.js`

---

## 🧠 Alasan Pemilihan Proyek

Proyek **MyAssessment** dipilih karena merupakan contoh ideal untuk mendemonstrasikan **arsitektur hybrid data**.  
Aplikasi ini menjawab kebutuhan nyata dunia pendidikan:

- **Akurasi & Konsistensi:**  
  Nilai siswa (Tugas, UTS, UAS) disimpan secara aman di SQL Server.  
- **Audit & Skalabilitas:**  
  Setiap perubahan nilai dicatat otomatis di MongoDB, sehingga riwayat bisa dilacak tanpa membebani database utama.

### ✨ Penamaan
- **"MyAssessment"** → menekankan kesan personal (“My”) dan profesional (“Assessment”).  
- **"PT. Hyoka Wanssss"** → “Hyoka” (評価) berarti *evaluasi* dalam bahasa Jepang, menggambarkan esensi proyek. “Wanssss” adalah identitas khas developer.

---

## 🔄 Bisnis Proses Utama

### **Proses: Input dan Audit Nilai Siswa**

**Aktor:** Guru  
**Sistem:** Aplikasi MyAssessment (IIS, ASP.NET API, SQL Server, MongoDB)

**Alur Lengkap:**

1. **Login** → Guru memasukkan kredensial, diverifikasi di `MyAssessment_auth` (SQL Server).  
2. **Navigasi** → Guru membuka menu `Penilaian → Input Nilai`.  
3. **Input Data** → Frontend (AngularJS) menampilkan `input-nilai.html`.  
4. **Kirim Data** → Data dikirim via POST ke `http://localhost:9000/api/penilaian/input`.  
5. **Proses Backend:**
   - Validasi oleh `PenilaianController.cs`  
   - Hitung `NilaiAkhir = 30% Tugas + 30% UTS + 40% UAS`
6. **Simpan ke Database:**
   - **SQL Server:** data utama disimpan ke `dbo.Grades`
   - **MongoDB:** salinan JSON log disimpan ke `GradeAuditLog`
7. **Respon ke Frontend** → Muncul notifikasi: *“Nilai berhasil disimpan.”*

---

## 🧩 Model Data (Hybrid Architecture)

### **1. SQL Server (Data Operasional)**
Digunakan untuk data terstruktur dan relasional.

- **Database:** `MyAssessment_auth`  
  - `AspNetUsers`  
  - `AspNetRoles`  
  - `AspNetUserRoles`  
  - `RoleMenuMapping`  
- **Database:** `MyAssessment`  
  - `dbo.Grades` — menyimpan nilai final siswa  

### **2. MongoDB (Data Audit Log)**
Digunakan untuk mencatat setiap perubahan nilai (append-only).

- **Collection:** `GradeAuditLog`  
  Berisi snapshot data lengkap + action (“CREATE” / “UPDATE”) + timestamp + `NilaiId`.

🔗 **Relasi lintas sistem:**  
`Id` dari `dbo.Grades` (SQL Server) disimpan sebagai `NilaiId` di `GradeAuditLog` (MongoDB),  
memungkinkan pelacakan riwayat nilai berdasarkan satu kunci.

---

## 👥 Skema Tim Proyek

Proyek ini dikerjakan oleh **satu orang (solo developer)** dengan berbagai peran dalam satu waktu:

| Peran | Tanggung Jawab |
|-------|-----------------|
| **Project Manager / Product Owner** | Menentukan requirement & scope proyek |
| **Database Administrator (DBA)** | Mendesain model data, membuat ERD, dan script SQL |
| **Backend Developer** | Membangun Web API & logika data (C#, SQL, MongoDB) |
| **Frontend Developer** | Membuat tampilan SPA (AngularJS, HTML, JS, CSS) |
| **DevOps / System Admin** | Men-setup IIS, koneksi database, dan deployment |
| **Quality Assurance (QA)** | Menguji alur sistem dan integrasi antar database |

> 👨‍💻 Developer: **Wawan Suwandi** (PT. Hyoka Wanssss)

---



