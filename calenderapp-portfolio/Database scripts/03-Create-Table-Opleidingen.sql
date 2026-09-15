USE CalenderApp
GO

CREATE TABLE Opleidingen (
    OpleidingId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    NameCourse nvarchar(50) NOT NULL,
    AbbriviationCourse nvarchar(10) NOT NULL,
	LengteUren int NOT NULL,
    Weekdays int NOT NULL,
	Startdate datetime NOT NULL,
	Enddate datetime NOT NULL,
	CreateDate datetime NOT NULL,
	CreateBy VARCHAR(MAX) NOT NULL,
	UpdateDate datetime NULL,
	UpdatedBy VARCHAR(MAX) NULL,
	DeleteDate datetime NULL,
	DeletedBy VARCHAR(MAX) NULL	
)
GO