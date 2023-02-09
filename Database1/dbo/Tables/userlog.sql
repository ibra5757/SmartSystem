CREATE TABLE [dbo].[userlog] (
    [LogID]    INT          IDENTITY (100, 1) NOT NULL,
    [UserID]   INT          NULL,
    [Activity] VARCHAR (60) NOT NULL,
    [Date]     DATETIME     DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([LogID] ASC),
    CONSTRAINT [FK_UserUserlog] FOREIGN KEY ([UserID]) REFERENCES [dbo].[users] ([UserID]) ON DELETE SET NULL
);

