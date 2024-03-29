USE [master]
GO
/****** Object:  Database [SmartInventory]    Script Date: 06/06/2023 12:32:13 am ******/
CREATE DATABASE [SmartInventory]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SmartInventory', FILENAME = N'C:\FinalYearNew\FinalYear\Databasee\SmartInventory.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SmartInventory_log', FILENAME = N'C:\FinalYearNew\FinalYear\Databasee\SmartInventory_log.ldf' , SIZE = 784KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SmartInventory] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SmartInventory].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SmartInventory] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SmartInventory] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SmartInventory] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SmartInventory] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SmartInventory] SET ARITHABORT OFF 
GO
ALTER DATABASE [SmartInventory] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SmartInventory] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [SmartInventory] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SmartInventory] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SmartInventory] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SmartInventory] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SmartInventory] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SmartInventory] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SmartInventory] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SmartInventory] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SmartInventory] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SmartInventory] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SmartInventory] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SmartInventory] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SmartInventory] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SmartInventory] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SmartInventory] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SmartInventory] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SmartInventory] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SmartInventory] SET  MULTI_USER 
GO
ALTER DATABASE [SmartInventory] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SmartInventory] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SmartInventory] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SmartInventory] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [SmartInventory]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category](
	[CatID] [int] IDENTITY(100,1) NOT NULL,
	[Catname] [varchar](30) NOT NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_Category_CreatedDate]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[CatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Company]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(100,1) NOT NULL,
	[CompanyName] [varchar](60) NOT NULL,
	[Contact] [varchar](30) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[Balance] [money] NOT NULL DEFAULT ((0.00)),
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_Company_CreatedDate]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[CusID] [int] IDENTITY(100,1) NOT NULL,
	[Name] [varchar](60) NOT NULL,
	[Contact] [varchar](30) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[Balance] [money] NOT NULL DEFAULT ((0.00)),
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_Customer_CreatedDate]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[CusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CusID] [int] NULL,
	[Date] [datetime] NULL,
	[Description] [varchar](50) NULL,
	[Debit] [money] NULL,
	[Credit] [money] NULL,
	[Balance] [money] NULL,
	[CompID] [int] NULL,
 CONSTRAINT [PK_Ledger] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PODetails]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PODetails](
	[PODetail_Id] [int] IDENTITY(1,1) NOT NULL,
	[POID] [int] NOT NULL,
	[PDID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[BatchNo] [varchar](20) NOT NULL,
 CONSTRAINT [PK__PODetail__DF2EF8B2B0F2B7B5] PRIMARY KEY CLUSTERED 
(
	[PODetail_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProDetails]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProDetails](
	[PDId] [int] IDENTITY(100,1) NOT NULL,
	[ProId] [int] NULL,
	[ProductUnit] [varchar](10) NOT NULL,
	[ProductType] [varchar](50) NOT NULL,
	[Packing] [int] NULL,
	[CostPrice] [money] NULL,
	[UnitPrice] [money] NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_ProDetails_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK__ProDetai__58D8D826D77DDEB9] PRIMARY KEY CLUSTERED 
(
	[PDId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Products]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Products](
	[ProID] [int] IDENTITY(100,1) NOT NULL,
	[ProName] [varchar](30) NOT NULL,
	[CatID] [int] NULL,
	[SubCatID] [int] NULL,
	[Code] [varchar](30) NOT NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_Products_CreatedDate]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[ProID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseOrderMaster]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaseOrderMaster](
	[POID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL CONSTRAINT [DF__PurchaseOr__Date__276EDEB3]  DEFAULT (getdate()),
	[UserID] [int] NULL,
	[CompanyId] [int] NULL,
	[BillNo] [varchar](50) NULL,
 CONSTRAINT [PK__Purchase__5F02A2F4693C11FC] PRIMARY KEY CLUSTERED 
(
	[POID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesOrderMaster]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalesOrderMaster](
	[Date] [datetime] NOT NULL CONSTRAINT [DF__SalesOrder__Date__30F848ED]  DEFAULT (getdate()),
	[SOID] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [varchar](30) NOT NULL,
	[CusID] [int] NULL,
	[TotalAmount] [money] NOT NULL,
	[Discount] [money] NOT NULL CONSTRAINT [DF__SalesOrde__Disco__31EC6D26]  DEFAULT ((0.00)),
	[GrandTotal] [money] NOT NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK__SalesOrd__A7FF3362858D42AE] PRIMARY KEY CLUSTERED 
(
	[SOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__SalesOrd__11F28418D2CBAB51] UNIQUE NONCLUSTERED 
(
	[BillNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SODetails]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SODetails](
	[SODetail_Id] [int] IDENTITY(1,1) NOT NULL,
	[SOID] [int] NOT NULL,
	[PDID] [int] NOT NULL,
	[S_Quantity] [int] NOT NULL,
	[BatchIdNo] [varchar](20) NULL,
	[U_price] [money] NOT NULL CONSTRAINT [DF__SODetails__U_pri__36B12243]  DEFAULT ((0.00)),
	[Discount] [money] NOT NULL CONSTRAINT [DF__SODetails__Disco__37A5467C]  DEFAULT ((0.00)),
	[TotalPrice] [money] NOT NULL CONSTRAINT [DF__SODetails__Total__38996AB5]  DEFAULT ((0.00)),
 CONSTRAINT [PK__SODetail__5517BD9B8EEEB961] PRIMARY KEY CLUSTERED 
(
	[SODetail_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubCategory](
	[SubCatID] [int] IDENTITY(100,1) NOT NULL,
	[SubCatname] [varchar](30) NOT NULL,
	[CatID] [int] NULL,
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_SubCategory_CreatedDate]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[SubCatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserLog]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserLog](
	[LogID] [int] IDENTITY(100,1) NOT NULL,
	[UserID] [int] NULL,
	[Activity] [varchar](60) NOT NULL,
	[Date] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(100,1) NOT NULL,
	[Name] [varchar](60) NOT NULL,
	[Contact] [varchar](20) NOT NULL,
	[CNIC] [varchar](20) NOT NULL,
	[UserName] [varchar](30) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Role] [varchar](30) NOT NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[ExtractTotalQuantity]    Script Date: 06/06/2023 12:32:14 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ExtractTotalQuantity]
AS
SELECT        PDID, SUM(Quantity) AS TotalQuantity
FROM            dbo.PODetails
GROUP BY PDID

GO
ALTER TABLE [dbo].[Ledger]  WITH CHECK ADD  CONSTRAINT [FK_Ledger_Company] FOREIGN KEY([CompID])
REFERENCES [dbo].[Company] ([CompanyID])
GO
ALTER TABLE [dbo].[Ledger] CHECK CONSTRAINT [FK_Ledger_Company]
GO
ALTER TABLE [dbo].[Ledger]  WITH CHECK ADD  CONSTRAINT [FK_Ledger_Customer] FOREIGN KEY([CusID])
REFERENCES [dbo].[Customer] ([CusID])
GO
ALTER TABLE [dbo].[Ledger] CHECK CONSTRAINT [FK_Ledger_Customer]
GO
ALTER TABLE [dbo].[PODetails]  WITH CHECK ADD  CONSTRAINT [FK_PDetails] FOREIGN KEY([PDID])
REFERENCES [dbo].[ProDetails] ([PDId])
GO
ALTER TABLE [dbo].[PODetails] CHECK CONSTRAINT [FK_PDetails]
GO
ALTER TABLE [dbo].[PODetails]  WITH CHECK ADD  CONSTRAINT [FK_PODetails] FOREIGN KEY([POID])
REFERENCES [dbo].[PurchaseOrderMaster] ([POID])
GO
ALTER TABLE [dbo].[PODetails] CHECK CONSTRAINT [FK_PODetails]
GO
ALTER TABLE [dbo].[ProDetails]  WITH CHECK ADD  CONSTRAINT [FK_proprodetails] FOREIGN KEY([ProId])
REFERENCES [dbo].[Products] ([ProID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ProDetails] CHECK CONSTRAINT [FK_proprodetails]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_catpro] FOREIGN KEY([CatID])
REFERENCES [dbo].[Category] ([CatID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_catpro]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_subcatpro] FOREIGN KEY([SubCatID])
REFERENCES [dbo].[SubCategory] ([SubCatID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_subcatpro]
GO
ALTER TABLE [dbo].[PurchaseOrderMaster]  WITH CHECK ADD  CONSTRAINT [FK_companyPOM] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([CompanyID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[PurchaseOrderMaster] CHECK CONSTRAINT [FK_companyPOM]
GO
ALTER TABLE [dbo].[PurchaseOrderMaster]  WITH CHECK ADD  CONSTRAINT [FK_userPOM] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[PurchaseOrderMaster] CHECK CONSTRAINT [FK_userPOM]
GO
ALTER TABLE [dbo].[SalesOrderMaster]  WITH CHECK ADD  CONSTRAINT [FK_SalesCustomer] FOREIGN KEY([CusID])
REFERENCES [dbo].[Customer] ([CusID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[SalesOrderMaster] CHECK CONSTRAINT [FK_SalesCustomer]
GO
ALTER TABLE [dbo].[SalesOrderMaster]  WITH CHECK ADD  CONSTRAINT [FK_SalesUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[SalesOrderMaster] CHECK CONSTRAINT [FK_SalesUser]
GO
ALTER TABLE [dbo].[SODetails]  WITH CHECK ADD  CONSTRAINT [FK_SODetails] FOREIGN KEY([SOID])
REFERENCES [dbo].[SalesOrderMaster] ([SOID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SODetails] CHECK CONSTRAINT [FK_SODetails]
GO
ALTER TABLE [dbo].[SODetails]  WITH CHECK ADD  CONSTRAINT [FK_SODetailsProDetails] FOREIGN KEY([PDID])
REFERENCES [dbo].[ProDetails] ([PDId])
GO
ALTER TABLE [dbo].[SODetails] CHECK CONSTRAINT [FK_SODetailsProDetails]
GO
ALTER TABLE [dbo].[SubCategory]  WITH CHECK ADD  CONSTRAINT [FK_catsubcat] FOREIGN KEY([CatID])
REFERENCES [dbo].[Category] ([CatID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[SubCategory] CHECK CONSTRAINT [FK_catsubcat]
GO
ALTER TABLE [dbo].[UserLog]  WITH CHECK ADD  CONSTRAINT [FK_UserUserlog] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLog] CHECK CONSTRAINT [FK_UserUserlog]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PODetails"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ExtractTotalQuantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ExtractTotalQuantity'
GO
USE [master]
GO
ALTER DATABASE [SmartInventory] SET  READ_WRITE 
GO
