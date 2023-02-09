CREATE TABLE [dbo].[purchase] (
    [PurchaseId] INT      IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME CONSTRAINT [DF__purchase__Date__22AA2996] DEFAULT (getdate()) NOT NULL,
    [UserID]     INT      NULL,
    [SupplierID] INT      NOT NULL,
    CONSTRAINT [PK__purchase__6B0A6BBEA8981807] PRIMARY KEY CLUSTERED ([PurchaseId] ASC),
    CONSTRAINT [FK_SalesUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[users] ([UserID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Supplierpurchase] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([SupplierID])
);

