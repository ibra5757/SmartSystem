CREATE TABLE [dbo].[Subcategory] (
    [SubCatID]   INT          IDENTITY (100, 1) NOT NULL,
    [SubCatname] VARCHAR (30) NOT NULL,
    [CatID]      INT          NULL,
    PRIMARY KEY CLUSTERED ([SubCatID] ASC),
    CONSTRAINT [FK_ProductsCatagory] FOREIGN KEY ([CatID]) REFERENCES [dbo].[category] ([CatID]) ON DELETE CASCADE
);

