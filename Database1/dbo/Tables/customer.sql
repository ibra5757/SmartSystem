CREATE TABLE [dbo].[customer] (
    [CusID]   INT          IDENTITY (100, 1) NOT NULL,
    [Name]    VARCHAR (60) NOT NULL,
    [Contact] VARCHAR (30) NOT NULL,
    [Address] VARCHAR (50) NOT NULL,
    [Balance] MONEY        DEFAULT ((0.00)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CusID] ASC)
);

