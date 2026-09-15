USE CalenderApp
GO

CREATE TABLE Lokalen (
    
	LokaalId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Roomnumber int NOT NULL,
    Floornumber int NOT NULL,
	Capacity int  NOT NULL,
	CreateDate datetime NOT NULL,
	CreateBy VARCHAR(MAX) NOT NULL,
	UpdateDate datetime NULL,
	UpdatedBy VARCHAR(MAX) NULL,
	DeleteDate datetime NULL,
	DeletedBy VARCHAR(MAX) NULL
    
)
GO