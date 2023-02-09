CREATE TABLE [dbo].[ProDetails] (
    [PDId]     INT          IDENTITY (100, 1) NOT NULL,
    [ProUnit]  VARCHAR (10) NOT NULL,
    [ProId]    INT          NULL,
    [Quantity] INT          NOT NULL,
    [Type]     VARCHAR (20) NOT NULL,
    [Packing]  VARCHAR (50) NOT NULL,
    [U_Price]  INT          NOT NULL,
    CONSTRAINT [PK__ProDetai__58D8D826C3891864] PRIMARY KEY CLUSTERED ([PDId] ASC),
    CONSTRAINT [FK_ProDetails] FOREIGN KEY ([ProId]) REFERENCES [dbo].[products] ([ProID]) ON DELETE CASCADE
);

