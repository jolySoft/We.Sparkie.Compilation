CREATE TABLE [dbo].[Compilations]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[CompilationType] INT NOT NULL,
	[CreatedBy] NVARCHAR(256) NOT  NULL
)
