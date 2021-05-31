CREATE TABLE [dbo].[Track]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[DigitalAssetId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(1024),
	[CompilationType] INT,
	[ArtistId]  UNIQUEIDENTIFIER NOT NULL,
	[CompilationId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT Track_Artist_Id_Fk FOREIGN KEY (ArtistId) REFERENCES Artist(Id),
	CONSTRAINT Track_Compilation_Id FOREIGN KEY (CompilationId) REFERENCES Compilations(Id)
)
