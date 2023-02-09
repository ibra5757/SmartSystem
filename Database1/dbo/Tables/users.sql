CREATE TABLE [dbo].[users] (
    [UserID]   INT           IDENTITY (100, 1) NOT NULL,
    [Name]     VARCHAR (60)  NOT NULL,
    [UserName] VARCHAR (30)  NOT NULL,
    [Password] VARCHAR (255) NOT NULL,
    [CNIC]     VARCHAR (20)  NOT NULL,
    [Contact]  VARCHAR (20)  NOT NULL,
    [Role]     VARCHAR (30)  NOT NULL,
    [IsActive] BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);

