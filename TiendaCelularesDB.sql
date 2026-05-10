-- Script to create TiendaCelularesDB and schema for the inventory application
-- Adjust database name or file locations as needed before running on your SQL Server instance

IF DB_ID(N'TiendaCelularesDB') IS NULL
BEGIN
    CREATE DATABASE [TiendaCelularesDB];
END
GO

USE [TiendaCelularesDB];
GO

-- Brands
IF OBJECT_ID(N'dbo.Brands', N'U') IS NULL
BEGIN
CREATE TABLE dbo.Brands
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
END
GO

-- Categories
IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
CREATE TABLE dbo.Categories
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
END
GO

-- Suppliers
IF OBJECT_ID(N'dbo.Suppliers', N'U') IS NULL
BEGIN
CREATE TABLE dbo.Suppliers
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    ContactEmail NVARCHAR(200) NULL,
    ContactPhone NVARCHAR(50) NULL
);
END
GO

-- MobilePhones
IF OBJECT_ID(N'dbo.MobilePhones', N'U') IS NULL
BEGIN
CREATE TABLE dbo.MobilePhones
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Marca NVARCHAR(100) NOT NULL,
    Modelo NVARCHAR(100) NOT NULL,
    Color NVARCHAR(50) NULL,
    Stock INT NOT NULL DEFAULT 0,
    Precio DECIMAL(18,2) NOT NULL DEFAULT(0),
    BrandId UNIQUEIDENTIFIER NULL,
    CategoryId UNIQUEIDENTIFIER NULL,
    SupplierId UNIQUEIDENTIFIER NULL,
    SKU NVARCHAR(50) NULL,
    IMEI NVARCHAR(50) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_MobilePhones_Brand FOREIGN KEY (BrandId) REFERENCES dbo.Brands(Id) ON DELETE SET NULL,
    CONSTRAINT FK_MobilePhones_Category FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id) ON DELETE SET NULL,
    CONSTRAINT FK_MobilePhones_Supplier FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(Id) ON DELETE SET NULL
);
CREATE INDEX IX_MobilePhones_Marca_Modelo ON dbo.MobilePhones(Marca, Modelo);
END
GO

-- Orders
IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
BEGIN
CREATE TABLE dbo.Orders
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CustomerName NVARCHAR(200) NULL,
    CustomerEmail NVARCHAR(200) NULL
);
END
GO

-- OrderItems
IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NULL
BEGIN
CREATE TABLE dbo.OrderItems
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    OrderId UNIQUEIDENTIFIER NOT NULL,
    MobilePhoneId UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Order FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_MobilePhone FOREIGN KEY (MobilePhoneId) REFERENCES dbo.MobilePhones(Id) ON DELETE NO ACTION
);
CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems(OrderId);
END
GO

-- StockMovements
IF OBJECT_ID(N'dbo.StockMovements', N'U') IS NULL
BEGIN
CREATE TABLE dbo.StockMovements
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    MobilePhoneId UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    MovementType NVARCHAR(50) NOT NULL,
    OccurredAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Notes NVARCHAR(MAX) NULL,
    CONSTRAINT FK_StockMovements_MobilePhone FOREIGN KEY (MobilePhoneId) REFERENCES dbo.MobilePhones(Id) ON DELETE CASCADE
);
CREATE INDEX IX_StockMovements_MobilePhoneId ON dbo.StockMovements(MobilePhoneId);
END
GO

-- Optional: seed minimal data (brands, categories, suppliers)
IF NOT EXISTS (SELECT 1 FROM dbo.Brands)
BEGIN
    INSERT INTO dbo.Brands (Id, Name) VALUES (NEWID(), N'XPhone'), (NEWID(), N'Alpha');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Categories)
BEGIN
    INSERT INTO dbo.Categories (Id, Name) VALUES (NEWID(), N'Smartphone'), (NEWID(), N'Feature');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers)
BEGIN
    INSERT INTO dbo.Suppliers (Id, Name, ContactEmail) VALUES (NEWID(), N'Default Supplier', N'supplier@example.com');
END
GO
