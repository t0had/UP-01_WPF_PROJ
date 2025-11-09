create database Goman_DB_Payment0;

GO

use Goman_DB_Payment0;

create table Users (
 ID int primary key identity(1, 1) not null,
 [Login] varchar(MAX) not null,
 [Password] varchar(MAX) not null,
 [Role] varchar(50) null,
 FIO varchar(MAX) null,
 Photo nvarchar(MAX) null
)

GO

create table Categories (
 ID int primary key identity(1, 1) not null,
 [Name] varchar(MAX)
)

GO

create table Payments (
 ID int primary key identity(1, 1),
 UserID int references Users (ID),
 CategoryID int references Categories (ID),
 [Date] date,
 [Name] nvarchar(50),
 Num decimal(18, 0),
 Price decimal(18, 0)
)
