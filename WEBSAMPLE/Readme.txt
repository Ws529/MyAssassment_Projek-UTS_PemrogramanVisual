EXEC sp_configure filestream_access_level, 2
GO
RECONFIGURE
GO

CREATE DATABASE WebApiFileTable
ON PRIMARY
(Name = WebApiFileTable,
FILENAME = 'D:\Databases\FileDB\DB\FTDB.mdf'),
FILEGROUP FTFG CONTAINS FILESTREAM
(NAME = WebApiFileTableFS,
FILENAME='D:\Databases\FileDB\FS')
LOG ON
(Name = WebApiFileTableLog,
FILENAME = 'D:\Databases\FileDB\DB\FTDBLog.ldf')
WITH FILESTREAM (NON_TRANSACTED_ACCESS = FULL,
DIRECTORY_NAME = N'WebApiFileTable');
GO

