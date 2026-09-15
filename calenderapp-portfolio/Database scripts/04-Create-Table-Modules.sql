USE CalenderApp
GO

CREATE TABLE Modules (
    ModuleId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ModuleName nvarchar(50) NOT NULL,
	DirectionDegree nvarchar(50) NOT NULL,
	DirectionDegreeNumber int NOT NULL,
    Abbriviation nvarchar(10) NOT NULL,
	CreateDate datetime NOT NULL,
	CreateBy VARCHAR(MAX) NOT NULL,
	UpdateDate datetime NULL,
	UpdatedBy VARCHAR(MAX) NULL,
	DeleteDate datetime NULL,
	DeletedBy VARCHAR(MAX) NULL
    
)
GO