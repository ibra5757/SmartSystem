CREATE TABLE [dbo].[returneditems] (
    [SalesID]     VARCHAR (30) NULL,
    [Description] VARCHAR (30) NOT NULL,
    [ProID]       INT          NULL,
    [PDId]        INT          NULL,
    [Quantity]    INT          NOT NULL,
    [price]       MONEY        DEFAULT ((0.00)) NOT NULL,
    [Discount]    MONEY        DEFAULT ((0.00)) NOT NULL,
    [Total]       MONEY        DEFAULT ((0.00)) NOT NULL,
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME     DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_ReturneditemProduct] FOREIGN KEY ([ProID]) REFERENCES [dbo].[products] ([ProID]),
    CONSTRAINT [FK_ReturneditemSales] FOREIGN KEY ([SalesID]) REFERENCES [dbo].[sales] ([SalesId]) ON DELETE CASCADE,
    CONSTRAINT [FK_returneditemsProDetails] FOREIGN KEY ([PDId]) REFERENCES [dbo].[ProDetails] ([PDId])
);

