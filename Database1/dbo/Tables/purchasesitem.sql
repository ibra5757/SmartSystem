CREATE TABLE [dbo].[purchasesitem] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [PurchaseId] INT NOT NULL,
    [ProID]      INT NULL,
    [PDId]       INT NULL,
    [Quantity]   INT NOT NULL,
    [SupplierID] INT NULL,
    CONSTRAINT [PK_purchasesitem] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_purchaseitemProDetails] FOREIGN KEY ([PDId]) REFERENCES [dbo].[ProDetails] ([PDId]),
    CONSTRAINT [FK_purchaseitemProduct] FOREIGN KEY ([ProID]) REFERENCES [dbo].[products] ([ProID]),
    CONSTRAINT [FK_purchasesitem_purchase] FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[purchase] ([PurchaseId]),
    CONSTRAINT [FK_Supplierpurchaseitems] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([SupplierID]) ON DELETE SET NULL
);

