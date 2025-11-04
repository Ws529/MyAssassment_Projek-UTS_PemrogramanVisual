/*
================================================================
SCRIPT: CreateDatabase.sql
PROYEK: MyAssessment
TUJUAN: Membuat tabel 'Grades' di database 'MyAssessment'
================================================================
*/

-- PENTING: Pastikan Anda telah memilih database 'MyAssessment' 
-- (bukan 'MyAssessment_auth' atau 'master') di SSMS sebelum menjalankan ini.
USE MyAssessment;
GO

-- 1. Hapus tabel jika sudah ada (agar bisa dijalankan ulang jika ada kesalahan)
IF OBJECT_ID('dbo.Grades', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Grades;
    PRINT 'Tabel dbo.Grades lama telah dihapus.';
END
GO

-- 2. Buat tabel 'Grades'
-- Strukturnya harus cocok persis dengan 'NilaiModel.cs'
CREATE TABLE dbo.Grades (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Kunci unik untuk setiap nilai
    Nama NVARCHAR(100) NOT NULL,
    NIS NVARCHAR(20) NOT NULL,
    Mapel NVARCHAR(50) NOT NULL,
    Semester INT NOT NULL,
    Tugas DECIMAL(5, 2) NOT NULL,    -- (5 digit total, 2 di belakang koma, misal: 95.50)
    UTS DECIMAL(5, 2) NOT NULL,
    UAS DECIMAL(5, 2) NOT NULL,
    NilaiAkhir DECIMAL(5, 2) NOT NULL, -- Hasil kalkulasi (30% + 30% + 40%)
    Keterangan NVARCHAR(500) NULL,
    TanggalInput DATETIME NOT NULL,    -- Waktu data disimpan
    InputBy NVARCHAR(100) NULL        -- User yang menginput (dari log audit)
);
GO

PRINT 'Tabel dbo.Grades berhasil dibuat.';

-- 3. (Opsional tapi Sangat Direkomendasikan) Buat Index
-- Ini akan mempercepat pencarian data saat membuat laporan
CREATE INDEX IX_Grades_NIS 
ON dbo.Grades(NIS);

CREATE INDEX IX_Grades_Mapel 
ON dbo.Grades(Mapel);

PRINT 'Index untuk performa berhasil dibuat.';
GO

-- 4. (Opsional) Masukkan beberapa data contoh untuk tes
INSERT INTO dbo.Grades (Nama, NIS, Mapel, Semester, Tugas, UTS, UAS, NilaiAkhir, Keterangan, TanggalInput, InputBy)
VALUES 
('Siswa Contoh 1', '1001', 'Matematika', 1, 80.00, 85.00, 90.00, 85.50, 'Siswa yang rajin', GETDATE(), 'adminit'),
('Siswa Contoh 2', '1002', 'IPA', 1, 75.00, 70.00, 80.00, 75.50, 'Perlu bimbingan', GETDATE(), 'adminit');
GO

PRINT 'Data contoh berhasil dimasukkan.';
PRINT 'Setup database SQL Server selesai.';