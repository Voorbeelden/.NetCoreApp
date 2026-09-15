USE CalenderApp
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Planningen]') AND type in (N'U'))
DROP TABLE [dbo].[Planningen]
GO
CREATE TABLE Planningen (
    PlanningId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    DocentId int NOT NULL,
    OpleidingId int NOT NULL,
	LokaalId int NOT NULL,
	ModeleId int NOT NULL,
	StudiegebiedId int NOT NULL,
	Description VARCHAR(2500) NULL,
	Code varchar(100) NOT NULL,
	Lesday varchar(MAX) NOT NULL,
	StartLes varchar(MAX) NOT NULL,
	EndLes varchar(MAX) NOT NULL,
	CreateDate datetime NOT NULL,
	CreateBy VARCHAR(MAX) NOT NULL,
	UpdateDate datetime NULL,
	UpdatedBy VARCHAR(MAX) NULL,
	DeleteDate datetime NULL,
	DeletedBy VARCHAR(MAX) NULL
	CONSTRAINT FK_DocentPlanning FOREIGN KEY (DocentId) REFERENCES Docenten(DocentId),
	CONSTRAINT FK_OpleidingPlanning FOREIGN KEY (OpleidingId) REFERENCES Opleidingen(OpleidingId),
	CONSTRAINT FK_LokaalPlanning FOREIGN KEY (LokaalId) REFERENCES Lokalen(LokaalId),
	CONSTRAINT FK_ModulePlanning FOREIGN KEY (ModeleId) REFERENCES Modules(ModuleId),
	CONSTRAINT FK_StudiegebiedPlanning FOREIGN KEY (StudiegebiedId) REFERENCES Studiegebieden(StudiegebiedId)
)
GO