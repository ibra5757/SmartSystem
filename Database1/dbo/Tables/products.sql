CREATE TABLE [dbo].[products] (
    [ProID]    INT          IDENTITY (100, 1) NOT NULL,
    [ProName]  VARCHAR (30) NOT NULL,
    [CatID]    INT          NOT NULL,
    [SubCatID] INT          NOT NULL,
    CONSTRAINT [PK__products__620295F0F50C0DBD] PRIMARY KEY CLUSTERED ([ProID] ASC),
    CONSTRAINT [FK_ProductsCat] FOREIGN KEY ([CatID]) REFERENCES [dbo].[category] ([CatID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductsSubCat] FOREIGN KEY ([SubCatID]) REFERENCES [dbo].[Subcategory] ([SubCatID])
);

