CREATE TABLE [dbo].[salesitem] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [SalesID]  VARCHAR (30) NULL,
    [Brand]    VARCHAR (30) NOT NULL,
    [SubBrand] VARCHAR (30) NOT NULL,
    [ProID]    INT          NULL,
    [PDId]     INT          NULL,
    [Quantity] INT          NOT NULL,
    [price]    MONEY        DEFAULT ((0.00)) NOT NULL,
    [Discount] MONEY        DEFAULT ((0.00)) NOT NULL,
    [Total]    MONEY        DEFAULT ((0.00)) NOT NULL,
    CONSTRAINT [FK_ProductSalesitem] FOREIGN KEY ([ProID]) REFERENCES [dbo].[products] ([ProID]),
    CONSTRAINT [FK_SalesitemProDetails] FOREIGN KEY ([PDId]) REFERENCES [dbo].[ProDetails] ([PDId]),
    CONSTRAINT [FK_SalesitemSales] FOREIGN KEY ([SalesID]) REFERENCES [dbo].[sales] ([SalesId]) ON DELETE CASCADE
);

