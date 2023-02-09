CREATE TABLE [dbo].[Batch] (
    [BatchId] INT            IDENTITY (1, 1) NOT NULL,
    [PDId]    INT            NOT NULL,
    [Batch]   VARBINARY (50) NOT NULL,
    CONSTRAINT [FK_Btach_ProDetails] FOREIGN KEY ([PDId]) REFERENCES [dbo].[ProDetails] ([PDId]),
    CONSTRAINT [UQ__Btach__902DBBCD21D29A9C] UNIQUE NONCLUSTERED ([Batch] ASC)
);

