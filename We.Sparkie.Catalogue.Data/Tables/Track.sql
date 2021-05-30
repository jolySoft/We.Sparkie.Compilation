CREATE TABLE [dbo].[Track]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[DigitalAssetId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(1024),
	[CompilationType] INT,
	[Artist_Id]  UNIQUEIDENTIFIER NOT NULL,
	[Compilation_Id] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT Track_Artist_Id_Fk FOREIGN KEY (Artist_Id) REFERENCES Artist(Id),
	CONSTRAINT Track_Compilation_Id FOREIGN KEY (Compilation_Id) REFERENCES Compilation(Id)
)
