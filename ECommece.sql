USE [master]
GO
/****** Object:  Database [ECommerceDB]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE DATABASE [ECommerceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ECommerceDB', FILENAME = N'D:\Code\SQL_Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ECommerceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ECommerceDB_log', FILENAME = N'D:\Code\SQL_Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ECommerceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ECommerceDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ECommerceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ECommerceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ECommerceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ECommerceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ECommerceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ECommerceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ECommerceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ECommerceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ECommerceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ECommerceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ECommerceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ECommerceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ECommerceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ECommerceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ECommerceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ECommerceDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ECommerceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ECommerceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ECommerceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ECommerceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ECommerceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ECommerceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ECommerceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ECommerceDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ECommerceDB] SET  MULTI_USER 
GO
ALTER DATABASE [ECommerceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ECommerceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ECommerceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ECommerceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ECommerceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ECommerceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ECommerceDB', N'ON'
GO
ALTER DATABASE [ECommerceDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [ECommerceDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ECommerceDB]
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[BrandID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Logo] [nvarchar](255) NULL,
	[Status] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Slug] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[ImageURL] [nvarchar](255) NULL,
	[ParentCategoryID] [int] NULL,
	[DisplayOrder] [int] NULL,
	[Status] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Price] [decimal](18, 2) NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[OrderCode] [nvarchar](50) NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[ShippingAddress] [nvarchar](255) NULL,
	[ShippingPhone] [nvarchar](20) NULL,
	[ShippingEmail] [nvarchar](100) NULL,
	[ShippingName] [nvarchar](100) NULL,
	[Note] [nvarchar](500) NULL,
	[Status] [nvarchar](50) NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[ImageID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ImageURL] [nvarchar](255) NOT NULL,
	[IsMainImage] [bit] NULL,
	[DisplayOrder] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[BrandID] [int] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Slug] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[Specification] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DiscountPrice] [decimal](18, 2) NULL,
	[Quantity] [int] NULL,
	[ViewCount] [int] NULL,
	[Status] [bit] NULL,
	[IsFeatured] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[UserID] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](500) NULL,
	[Status] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Address] [nvarchar](255) NULL,
	[Avatar] [nvarchar](255) NULL,
	[Role] [nvarchar](20) NULL,
	[Status] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[SocialProvider] [nvarchar](20) NULL,
	[SocialID] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[LastLogin] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[WishlistID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ProductID] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[WishlistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Brands] ON 

INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (1, N'Apple', N'Thương hiệu đến từ Mỹ, nổi tiếng với iPhone, iPad, MacBook', N'iphone_16_pro_max_desert_titan_3552a28ae0_250213145120.png', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:51:20.550' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (2, N'Samsung', N'Tập đoàn điện tử đa quốc gia từ Hàn Quốc', N'samssung_galaxy_s24_fe_xanh_la_4c98f72f73_250213145113.png', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:51:13.190' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (3, N'OPPO', N'Thương hiệu smartphone đến từ Trung Quốc', N'oppo_reno13_f_5g_tim_5_858ba5c2ad_250213145104.png', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:51:04.263' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (4, N'Xiaomi', N'Thương hiệu công nghệ đến từ Trung Quốc', N'xiaomi_14t_pro_black_1_6d563df835_250213145129.png', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:51:29.323' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (5, N'Dell', N'Thương hiệu máy tính đến từ Mỹ', N'dell_latitude_15_3540_9950b79986_250213145406.png', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:54:06.193' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (6, N'Asus', N'Thương hiệu máy tính đến từ Đài Loan', N'2022_asus_tuf_gaming_f15_fx507_jaeger_gray_c8fc1c350e_250213145052.png', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:50:52.217' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (7, N'HP', N'Thương hiệu máy tính đến từ Mỹ', N'2024_5_13_638512086854166670_8F157PA-thumb_250213145253.jpg', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:52:53.157' AS DateTime))
INSERT [dbo].[Brands] ([BrandID], [Name], [Description], [Logo], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (8, N'Lenovo', N'Thương hiệu máy tính đến từ Trung Quốc', N'2022_10_20_638018700298075854_lenovo-gaming-legion-5-15arh7-xam-dam-dd_250213145350.jpg', 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), CAST(N'2025-02-13T14:53:50.537' AS DateTime))
SET IDENTITY_INSERT [dbo].[Brands] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (1, N'Điện thoại', N'dien-thoai', N'Điện thoại di động chính hãng', N'banner_dien_thoai_cu_a497c86b1a.png', NULL, 0, 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), NULL)
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (2, N'Laptop', N'laptop', N'Máy tính xách tay đa dạng nhu cầu', N'laptop_404ec5c838.png', NULL, 0, 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), NULL)
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (3, N'Máy tính bảng', N'may-tinh-bang', N'Máy tính bảng chính hãng', N'may_tinh_bang_423a5069f1.png', NULL, 0, 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), NULL)
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (4, N'Đồng hồ thông minh', N'dong-ho-thong-minh', N'Smartwatch các thương hiệu', N'dong_ho_thong_minh_bde467147b.png', NULL, 0, 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), NULL)
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (5, N'Tai nghe', N'tai-nghe', N'Tai nghe có dây và không dây', N'04.10.3-scaled-0c09efe6.jpeg', NULL, 0, 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), NULL)
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (6, N'Phụ kiện', N'phu-kien', N'Phụ kiện chính hãng', N'phu_kien_pc_3a399d193a.png', NULL, 0, 1, 0, CAST(N'2025-02-10T13:04:23.493' AS DateTime), NULL)
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (7, N'Samsung A15', NULL, N'qasac', NULL, NULL, 1, 1, 1, CAST(N'2025-02-13T08:20:41.297' AS DateTime), CAST(N'2025-02-13T13:23:22.267' AS DateTime))
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (8, N'nopet lincon', N'nopet-lincon', N'jjklmklm;,', NULL, NULL, 1, 1, 1, CAST(N'2025-02-13T08:34:16.393' AS DateTime), CAST(N'2025-02-13T13:23:19.473' AS DateTime))
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (9, N'nopet linconzxcgvgvj', N'nopet-linconzxcgvgvj', N'cfsaed', NULL, 1, 1, 1, 1, CAST(N'2025-02-13T11:47:40.773' AS DateTime), CAST(N'2025-02-13T12:23:22.600' AS DateTime))
INSERT [dbo].[Categories] ([CategoryID], [Name], [Slug], [Description], [ImageURL], [ParentCategoryID], [DisplayOrder], [Status], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (17, N'nopet linconzxcgvgvj', N'nopet-linconzxcgvgvj-250213122305', N'dèvaacjhkn njkjnkjmkl ', N'ban-phim-co-keycool-y75-ice-blue.1698240048_20250213131202.jpg', NULL, 98, 1, 1, CAST(N'2025-02-13T12:23:05.470' AS DateTime), CAST(N'2025-02-13T14:38:05.607' AS DateTime))
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Price], [Quantity]) VALUES (1, 1, 1, CAST(33990000.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Price], [Quantity]) VALUES (2, 2, 4, CAST(27990000.00 AS Decimal(18, 2)), 2)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderCode], [OrderDate], [TotalAmount], [ShippingAddress], [ShippingPhone], [ShippingEmail], [ShippingName], [Note], [Status], [PaymentMethod], [PaymentStatus], [CreatedDate], [ModifiedDate]) VALUES (1, 1, N'DH20250211120918', CAST(N'2025-02-11T12:09:18.247' AS DateTime), CAST(67980000.00 AS Decimal(18, 2)), N'dfdfsdfvsdf', N'1234567876', N'test1@gmail.com', N'gfgfzg viettt', N'kmkm', N'2', N'COD', NULL, CAST(N'2025-02-11T12:09:18.247' AS DateTime), CAST(N'2025-02-13T14:10:13.527' AS DateTime))
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderCode], [OrderDate], [TotalAmount], [ShippingAddress], [ShippingPhone], [ShippingEmail], [ShippingName], [Note], [Status], [PaymentMethod], [PaymentStatus], [CreatedDate], [ModifiedDate]) VALUES (2, 1, N'DH20250213061032', CAST(N'2025-02-13T06:10:32.703' AS DateTime), CAST(55980000.00 AS Decimal(18, 2)), N'dfdfsdfvsdf', N'1234567876', N'test1@gmail.com', N'gfgfzg viettt', N'gcd xcfghcfhfchy', N'2', N'COD', NULL, CAST(N'2025-02-13T06:10:32.703' AS DateTime), CAST(N'2025-02-13T14:10:21.410' AS DateTime))
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductImages] ON 

INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (1, 1, N'iphone-15-pro-max-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (2, 1, N'iphone-15-pro-max-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (3, 1, N'iphone-15-pro-max-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (4, 2, N'samsung-s24-ultra-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (5, 2, N'samsung-s24-ultra-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (6, 2, N'samsung-s24-ultra-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (7, 3, N'oppo-find-n3-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (8, 3, N'oppo-find-n3-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (9, 3, N'oppo-find-n3-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (10, 4, N'dell-gaming-g15-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (11, 4, N'dell-gaming-g15-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (12, 4, N'dell-gaming-g15-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (13, 5, N'asus-rog-strix-g16-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (14, 5, N'asus-rog-strix-g16-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (15, 5, N'asus-rog-strix-g16-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (16, 6, N'macbook-air-m2-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (17, 6, N'macbook-air-m2-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (18, 6, N'macbook-air-m2-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (19, 7, N'ipad-pro-m2-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (20, 7, N'ipad-pro-m2-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (21, 7, N'ipad-pro-m2-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (22, 8, N'tab-s9-ultra-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (23, 8, N'tab-s9-ultra-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (24, 8, N'tab-s9-ultra-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (25, 9, N'apple-watch-s9-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (26, 9, N'apple-watch-s9-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (27, 9, N'apple-watch-s9-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (28, 10, N'galaxy-watch-6-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (29, 10, N'galaxy-watch-6-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (30, 10, N'galaxy-watch-6-3.jpg', 0, 3, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (31, 11, N'airpods-pro-2-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (32, 11, N'airpods-pro-2-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (33, 12, N'galaxy-buds2-pro-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (34, 12, N'galaxy-buds2-pro-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (35, 13, N'sac-apple-20w-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (36, 13, N'sac-apple-20w-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (37, 14, N'pin-samsung-10000mah-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (38, 14, N'pin-samsung-10000mah-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (39, 15, N'op-lung-oppo-find-n3-1.jpg', 1, 1, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImageURL], [IsMainImage], [DisplayOrder], [CreatedDate]) VALUES (40, 15, N'op-lung-oppo-find-n3-2.jpg', 0, 2, CAST(N'2025-02-10T13:06:03.147' AS DateTime))
SET IDENTITY_INSERT [dbo].[ProductImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (1, 1, 1, N'iPhone 15 Pro Max 256GB', N'iphone-15-pro-max-256gb', N'iPhone 15 Pro Max. Titan. Chip A17 Pro. Camera chuyên nghiệp 48MP', NULL, CAST(34990000.00 AS Decimal(18, 2)), CAST(33990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (2, 1, 2, N'Samsung Galaxy S24 Ultra 256GB', N'samsung-galaxy-s24-ultra-256gb', N'Galaxy S24 Ultra với bút S-Pen, camera 200MP, chip Snapdragon 8 Gen 3', NULL, CAST(31990000.00 AS Decimal(18, 2)), CAST(29990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (3, 1, 3, N'OPPO Find N3 5G', N'oppo-find-n3-5g', N'OPPO Find N3 màn hình gập, camera Hasselblad', NULL, CAST(44990000.00 AS Decimal(18, 2)), CAST(42990000.00 AS Decimal(18, 2)), 100, 0, 1, 0, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (4, 2, 5, N'Dell Gaming G15 5525', N'dell-gaming-g15-5525', N'Laptop gaming, AMD Ryzen 7 6800H, 16GB RAM, RTX 3060', NULL, CAST(29990000.00 AS Decimal(18, 2)), CAST(27990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (5, 2, 6, N'Asus ROG Strix G16 2024', N'asus-rog-strix-g16-2024', N'Laptop gaming, Intel Core i9 14900H, 16GB RAM, RTX 4060', NULL, CAST(39990000.00 AS Decimal(18, 2)), CAST(37990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (6, 2, 1, N'MacBook Air 15 inch M2 2023', N'macbook-air-15-inch-m2-2023', N'Laptop mỏng nhẹ, chip M2, 8GB RAM, 256GB SSD', NULL, CAST(32990000.00 AS Decimal(18, 2)), CAST(31990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (7, 3, 1, N'iPad Pro M2 11 inch WiFi 128GB', N'ipad-pro-m2-11-inch-wifi-128gb', N'Máy tính bảng cao cấp, chip M2, màn hình Liquid Retina', NULL, CAST(20990000.00 AS Decimal(18, 2)), CAST(19990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (8, 3, 2, N'Samsung Galaxy Tab S9 Ultra 5G', N'samsung-galaxy-tab-s9-ultra-5g', N'Máy tính bảng màn hình lớn 14.6 inch, Snapdragon 8 Gen 2', NULL, CAST(28990000.00 AS Decimal(18, 2)), CAST(27990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (9, 4, 1, N'Apple Watch Series 9 GPS 41mm', N'apple-watch-series-9-gps-41mm', N'Đồng hồ thông minh, chip S9, màn hình Always-On', NULL, CAST(11990000.00 AS Decimal(18, 2)), CAST(10990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (10, 4, 2, N'Samsung Galaxy Watch 6 Classic 43mm', N'samsung-galaxy-watch-6-classic-43mm', N'Đồng hồ thông minh, vòng bezel xoay, màn hình AMOLED', NULL, CAST(8990000.00 AS Decimal(18, 2)), CAST(7990000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (11, 5, 1, N'AirPods Pro 2 2023', N'airpods-pro-2-2023', N'Tai nghe không dây, chống ồn chủ động, cổng USB-C', NULL, CAST(6990000.00 AS Decimal(18, 2)), CAST(6490000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (12, 5, 2, N'Samsung Galaxy Buds2 Pro', N'samsung-galaxy-buds2-pro', N'Tai nghe không dây, chống ồn, âm thanh 24bit', NULL, CAST(4990000.00 AS Decimal(18, 2)), CAST(4490000.00 AS Decimal(18, 2)), 100, 0, 1, 1, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (13, 6, 1, N'Sạc nhanh Apple 20W Type-C', N'sac-nhanh-apple-20w-type-c', N'Adapter sạc nhanh chính hãng Apple', NULL, CAST(690000.00 AS Decimal(18, 2)), CAST(590000.00 AS Decimal(18, 2)), 100, 0, 1, 0, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (14, 6, 2, N'Pin sạc dự phòng Samsung 10000mAh', N'pin-sac-du-phong-samsung-10000mah', N'Pin sạc dự phòng, hỗ trợ sạc nhanh 25W', NULL, CAST(890000.00 AS Decimal(18, 2)), CAST(790000.00 AS Decimal(18, 2)), 100, 0, 1, 0, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
INSERT [dbo].[Products] ([ProductID], [CategoryID], [BrandID], [Name], [Slug], [Description], [Specification], [Price], [DiscountPrice], [Quantity], [ViewCount], [Status], [IsFeatured], [IsDeleted], [CreatedDate], [ModifiedDate]) VALUES (15, 6, 3, N'Ốp lưng OPPO Find N3 chính hãng', N'op-lung-oppo-find-n3-chinh-hang', N'Ốp lưng bảo vệ chính hãng OPPO', NULL, CAST(590000.00 AS Decimal(18, 2)), CAST(490000.00 AS Decimal(18, 2)), 100, 0, 1, 0, 0, CAST(N'2025-02-10T13:04:23.497' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Password], [Email], [FullName], [Phone], [Address], [Avatar], [Role], [Status], [IsDeleted], [SocialProvider], [SocialID], [CreatedDate], [LastLogin], [ModifiedDate]) VALUES (1, N'test1@gmail.com', N'e10adc3949ba59abbe56e057f20f883e', N'test1@gmail.com', N'gfgfzg viettt', NULL, NULL, NULL, N'Customer', 1, 0, NULL, NULL, CAST(N'2025-02-10T16:44:38.270' AS DateTime), CAST(N'2025-02-13T06:09:25.117' AS DateTime), CAST(N'2025-02-13T06:09:25.117' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Email], [FullName], [Phone], [Address], [Avatar], [Role], [Status], [IsDeleted], [SocialProvider], [SocialID], [CreatedDate], [LastLogin], [ModifiedDate]) VALUES (2, N'admin@gmail.com', N'e10adc3949ba59abbe56e057f20f883e', N'admin@gmail.com', N'admin Admin', NULL, NULL, NULL, N'Admin', 1, 0, NULL, NULL, CAST(N'2025-02-11T17:53:40.420' AS DateTime), CAST(N'2025-02-13T14:27:43.910' AS DateTime), CAST(N'2025-02-13T14:27:43.910' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Email], [FullName], [Phone], [Address], [Avatar], [Role], [Status], [IsDeleted], [SocialProvider], [SocialID], [CreatedDate], [LastLogin], [ModifiedDate]) VALUES (3, N'viet@gmail.com', N'E10ADC3949BA59ABBE56E057F20F883E', N'viet@gmail.com', N'viet', N'34534134513', N'scasasc', NULL, N'0', 1, NULL, NULL, NULL, CAST(N'2025-02-13T13:50:43.287' AS DateTime), CAST(N'2025-02-13T14:25:31.877' AS DateTime), CAST(N'2025-02-13T14:25:31.877' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Categori__BC7B5FB6A0B6B5F9]    Script Date: 2/13/2025 2:57:27 PM ******/
ALTER TABLE [dbo].[Categories] ADD UNIQUE NONCLUSTERED 
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Categories_Slug]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Categories_Slug] ON [dbo].[Categories]
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Orders__999B52290F7F0124]    Script Date: 2/13/2025 2:57:27 PM ******/
ALTER TABLE [dbo].[Orders] ADD UNIQUE NONCLUSTERED 
(
	[OrderCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Orders_OrderCode]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_OrderCode] ON [dbo].[Orders]
(
	[OrderCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Orders_Status]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_Status] ON [dbo].[Orders]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_UserID]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_UserID] ON [dbo].[Orders]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Products__BC7B5FB68602CF23]    Script Date: 2/13/2025 2:57:27 PM ******/
ALTER TABLE [dbo].[Products] ADD UNIQUE NONCLUSTERED 
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_BrandID]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_BrandID] ON [dbo].[Products]
(
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_CategoryID]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_CategoryID] ON [dbo].[Products]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products_Name]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_Name] ON [dbo].[Products]
(
	[Name] ASC
)
INCLUDE([Price],[DiscountPrice],[Status]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products_Slug]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_Slug] ON [dbo].[Products]
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_Status_IsDeleted]    Script Date: 2/13/2025 2:57:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_Status_IsDeleted] ON [dbo].[Products]
(
	[Status] ASC,
	[IsDeleted] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E417B2FAEA]    Script Date: 2/13/2025 2:57:27 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D105349207AD76]    Script Date: 2/13/2025 2:57:27 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ProductImages] ADD  DEFAULT ((0)) FOR [IsMainImage]
GO
ALTER TABLE [dbo].[ProductImages] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[ProductImages] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [IsFeatured]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('Customer') FOR [Role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Wishlist] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD FOREIGN KEY([ParentCategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([BrandID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
/****** Object:  StoredProcedure [dbo].[sp_GetOrderStatistics]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure cho thống kê đơn hàng
CREATE PROCEDURE [dbo].[sp_GetOrderStatistics]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        COUNT(*) AS TotalOrders,
        SUM(TotalAmount) AS TotalRevenue,
        Status,
        CONVERT(DATE, CreatedDate) AS OrderDate
    FROM Orders
    WHERE CreatedDate BETWEEN @StartDate AND @EndDate
    GROUP BY Status, CONVERT(DATE, CreatedDate)
    ORDER BY OrderDate
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProductsPaging]    Script Date: 2/13/2025 2:57:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure cho phân trang sản phẩm
CREATE PROCEDURE [dbo].[sp_GetProductsPaging]
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @CategoryID INT = NULL,
    @BrandID INT = NULL,
    @SearchKeyword NVARCHAR(100) = NULL,
    @SortBy NVARCHAR(50) = 'CreatedDate',
    @SortDirection NVARCHAR(4) = 'DESC'
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT p.*, c.Name AS CategoryName, b.Name AS BrandName,
           COUNT(*) OVER() AS TotalRecords
    FROM Products p
    LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
    LEFT JOIN Brands b ON p.BrandID = b.BrandID
    WHERE p.IsDeleted = 0 
    AND p.Status = 1'
    
    IF @CategoryID IS NOT NULL
        SET @SQL = @SQL + N' AND p.CategoryID = @CategoryID'
    
    IF @BrandID IS NOT NULL
        SET @SQL = @SQL + N' AND p.BrandID = @BrandID'
    
    IF @SearchKeyword IS NOT NULL
        SET @SQL = @SQL + N' AND p.Name LIKE N''%'' + @SearchKeyword + ''%'''
    
    SET @SQL = @SQL + N' ORDER BY p.' + @SortBy + ' ' + @SortDirection + '
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY'
    
    EXEC sp_executesql @SQL,
        N'@PageNumber INT, @PageSize INT, @CategoryID INT, @BrandID INT, @SearchKeyword NVARCHAR(100)',
        @PageNumber, @PageSize, @CategoryID, @BrandID, @SearchKeyword
END
GO
USE [master]
GO
ALTER DATABASE [ECommerceDB] SET  READ_WRITE 
GO
