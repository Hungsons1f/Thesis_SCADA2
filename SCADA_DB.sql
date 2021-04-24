CREATE DATABASE Scada
GO 

USE Scada
GO

CREATE TABLE UserRole
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	DisplayName NVARCHAR(MAX)
)
GO

CREATE TABLE Users
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	UserName NVARCHAR(100),
	Password NVARCHAR(MAX),
	IdRole INT NOT NULL

	FOREIGN KEY (IdRole) REFERENCES UserRole(Id)
)
GO

CREATE TABLE ProcessData
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Timestamp datetime,
	Furnace_Temp float,

	PCondense_Flow float,
	PCondense_Press float,
	HLPHeater_Temp float,
	HLPHeater_Press float,
	HDeaerator_Temp float,
	HDeaerator_Press float,
	PSupply_Flow float,
	PSupply_Press float,
	HHPHeater_Temp float,
	HHPHeater_Press float,

	HBoiler_Temp float,
	HBoiler_Press float,
	TurbineH_Temp float,
	TurbineH_Press float,
	TurbineI_Temp float,
	TurbineI_Press float,
	TurbineL_Temp float,
	TurbineL_Press float,
	Turbine_Freq float,

	HCondenser_Temp float,
	PCircular_Flow float,
)
GO




----------------------------------------------------------------------

INSERT INTO dbo.UserRole(DisplayName) VALUES(N'Quản trị viên')
GO

INSERT INTO dbo.UserRole(DisplayName) VALUES(N'Kỹ thuật viên')
GO

INSERT INTO dbo.Users
(
    DisplayName,
    UserName,
    Password,
    IdRole
)
VALUES
(   N'Admin', -- DisplayName - nvarchar(max)
    N'Admin', -- UserName - nvarchar(100)
    N'060a48fb6dfbd38f47e8273e49799573', -- Password - nvarchar(max)
    1    -- IdRole - int
    )
GO

INSERT INTO dbo.Users
(
    DisplayName,
    UserName,
    Password,
    IdRole
)
VALUES
(   N'Engineer', -- DisplayName - nvarchar(max)
    N'Engineer', -- UserName - nvarchar(100)
    N'8d9e2ae63a06f7754bebc6642056279f', -- Password - nvarchar(max)
    2    -- IdRole - int
    )
GO

DELETE FROM dbo.ProcessData
Go