CREATE TABLE [dbo].[sales] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [SalesId]     VARCHAR (30) NOT NULL,
    [InvoiceId]   VARCHAR (30) NOT NULL,
    [BillNo]      VARCHAR (30) NOT NULL,
    [CusID]       INT          NULL,
    [TotalAmount] MONEY        NOT NULL,
    [Date]        DATETIME     DEFAULT (getdate()) NOT NULL,
    [Status]      VARCHAR (30) DEFAULT ('Sold') NOT NULL,
    [Discount]    MONEY        DEFAULT ((0.00)) NOT NULL,
    [GrandTotal]  MONEY        NOT NULL,
    [PaidAmount]  MONEY        DEFAULT ((0.00)) NOT NULL,
    [UserID]      INT          NULL,
    PRIMARY KEY CLUSTERED ([SalesId] ASC),
    CHECK ([Status]='Returned' OR [Status]='Sold'),
    CONSTRAINT [FK_SalesCustomer] FOREIGN KEY ([CusID]) REFERENCES [dbo].[customer] ([CusID]) ON DELETE CASCADE,
    CONSTRAINT [FK_SalesUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[users] ([UserID]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([BillNo] ASC)
);

