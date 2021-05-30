CREATE TABLE [dbo].[Compilation]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[CompilationType] INT NOT NULL,
	[CreatedBy] NVARCHAR(256) NOT  NULL
)
