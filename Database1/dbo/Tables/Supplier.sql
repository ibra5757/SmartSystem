CREATE TABLE [dbo].[Supplier] (
    [SupplierID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (60) NOT NULL,
    [Contact]    VARCHAR (20) NOT NULL,
    [Company]    VARCHAR (30) NULL,
    PRIMARY KEY CLUSTERED ([SupplierID] ASC)
);

