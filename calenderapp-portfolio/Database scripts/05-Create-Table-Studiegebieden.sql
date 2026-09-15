USE CalenderApp
GO

CREATE TABLE Studiegebieden (
    StudiegebiedId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(50) NOT NULL,
    Abbriviation nvarchar(10) NOT NULL,
	CreateDate datetime NOT NULL,
	CreateBy VARCHAR(MAX) NOT NULL,
	UpdateDate datetime NULL,
	UpdatedBy VARCHAR(MAX) NULL,
	DeleteDate datetime NULL,
	DeletedBy VARCHAR(MAX) NULL
    
)
GO