CREATE TABLE [dbo].[salesledger] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME     DEFAULT (getdate()) NOT NULL,
    [Explanation] VARCHAR (50) NOT NULL,
    [Debit]       MONEY        DEFAULT ((0.00)) NOT NULL,
    [Credit]      MONEY        DEFAULT ((0.00)) NOT NULL,
    [Balance]     MONEY        DEFAULT ((0.00)) NOT NULL,
    [CusID]       INT          NULL,
    [Status]      VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BalancesheetCustomer] FOREIGN KEY ([CusID]) REFERENCES [dbo].[customer] ([CusID])
);

