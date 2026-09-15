USE CalenderApp
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Docenten]') AND type in (N'U'))
DROP TABLE [dbo].[Docenten]
CREATE TABLE Docenten (
    DocentId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Firstname nvarchar(50) NOT NULL,
    Lastname nvarchar(50) NOT NULL,
	AbbriviationName nvarchar(10) NOT NULL,
	ImageUrl nvarchar(MAX) NULL,
	CreateDate datetime NOT NULL,
	CreateBy VARCHAR(MAX) NOT NULL,
	UpdateDate datetime NULL,
	UpdatedBy VARCHAR(MAX) NULL,
	DeleteDate datetime NULL,
	DeletedBy VARCHAR(MAX) NULL
)	
GO