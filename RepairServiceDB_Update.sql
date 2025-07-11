USE [master]
GO
/****** Object:  Database [RepairServiceDB]    Script Date: 19.06.2025 21:20:30 ******/
CREATE DATABASE [RepairServiceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RepairServiceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RepairServiceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RepairServiceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RepairServiceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RepairServiceDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RepairServiceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RepairServiceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RepairServiceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RepairServiceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RepairServiceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RepairServiceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RepairServiceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RepairServiceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RepairServiceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RepairServiceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RepairServiceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RepairServiceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RepairServiceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RepairServiceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RepairServiceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RepairServiceDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RepairServiceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RepairServiceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RepairServiceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RepairServiceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RepairServiceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RepairServiceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RepairServiceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RepairServiceDB] SET RECOVERY FULL 
GO
ALTER DATABASE [RepairServiceDB] SET  MULTI_USER 
GO
ALTER DATABASE [RepairServiceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RepairServiceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RepairServiceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RepairServiceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RepairServiceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RepairServiceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RepairServiceDB] SET QUERY_STORE = OFF
GO
USE [RepairServiceDB]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[DeviceID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceType] [nvarchar](100) NOT NULL,
	[Manufacturer] [nvarchar](100) NOT NULL,
	[Model] [nvarchar](100) NOT NULL,
	[SerialNumber] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Masters]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Masters](
	[MasterID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Specialization] [nvarchar](255) NULL,
	[HireDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[MasterID] [int] NULL,
	[StatusID] [int] NOT NULL,
	[ProblemDescription] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateCompleted] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatuses]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatuses](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RepairLog]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepairLog](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[MasterID] [int] NOT NULL,
	[LogText] [nvarchar](max) NOT NULL,
	[LogDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpareParts]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpareParts](
	[PartID] [int] IDENTITY(1,1) NOT NULL,
	[PartName] [nvarchar](255) NOT NULL,
	[SKU] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[QuantityInStock] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypicalFaults]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypicalFaults](
	[FaultID] [int] IDENTITY(1,1) NOT NULL,
	[FaultName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[RecommendedSolution] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[FaultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsedParts]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsedParts](
	[UsedPartID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[PartID] [int] NOT NULL,
	[QuantityUsed] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UsedPartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19.06.2025 21:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[Login] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (1, N'Сергеев Сергей Сергеевич', N'+79112223344', N'sergeev@email.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (2, N'Антонова Анна Антоновна', N'+79223334455', N'antonova@email.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (3, N'Грехов Владислав Сергеевич', N'+79027592341', NULL)
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (4, N'Кузнецов Олег Викторович', N'+79045678901', N'oleg.k@example.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (5, N'Смирнова Екатерина Андреевна', N'+79056789012', N'kate.s@example.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (6, N'Васильев Андрей Романович', N'+79067890123', NULL)
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (7, N'Зайцева Ирина Александровна', N'+79088901234', N'irina.z@example.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (8, N'Павлов Максим Евгеньевич', N'+79099012345', N'max.p@example.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (9, N'Орлова Наталья Дмитриевна', N'+79100123456', NULL)
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (10, N'Григорьев Артем Геннадьевич', N'+79111234567', N'artem.g@example.com')
INSERT [dbo].[Clients] ([ClientID], [FullName], [PhoneNumber], [Email]) VALUES (11, N'Пробный клиент', N'77777777777', NULL)
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
SET IDENTITY_INSERT [dbo].[Devices] ON 

INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (1, N'Смартфон', N'Apple', N'iPhone 13', N'SN12345XYZ')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (2, N'Ноутбук', N'Apple', N'MacBook Pro 16 2019', N'SN67890ABC')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (3, N'Планшет', N'Samsung', N'Galaxy Tab s9', N'7725417932')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (4, N'Ноутбук', N'Lenovo', N'ThinkPad T14s', N'SN444DDD')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (5, N'Планшет', N'Apple', N'iPad Air 5', N'SN555EEE')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (6, N'Смартфон', N'Google', N'Pixel 7', N'SN666FFF')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (7, N'Ноутбук', N'Dell', N'XPS 15', N'SN777GGG')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (8, N'Игровая консоль', N'Sony', N'PlayStation 5', N'SN888HHH')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (9, N'Смарт-часы', N'Apple', N'Watch Series 8', N'SN999III')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (10, N'Планшет', N'Samsung', N'Galaxy Tab S8', N'SN101JJJ')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (11, N'test', N'test', N'test', N'test')
INSERT [dbo].[Devices] ([DeviceID], [DeviceType], [Manufacturer], [Model], [SerialNumber]) VALUES (12, N'Планшет', N'Apple', N'Ipad air', N'18271237372')
SET IDENTITY_INSERT [dbo].[Devices] OFF
GO
SET IDENTITY_INSERT [dbo].[Masters] ON 

INSERT [dbo].[Masters] ([MasterID], [UserID], [Specialization], [HireDate]) VALUES (1, 1, N'Ремонт ноутбуков', CAST(N'2025-06-11' AS Date))
INSERT [dbo].[Masters] ([MasterID], [UserID], [Specialization], [HireDate]) VALUES (2, 2, N'Ремонт смартфонов', CAST(N'2025-06-11' AS Date))
INSERT [dbo].[Masters] ([MasterID], [UserID], [Specialization], [HireDate]) VALUES (3, 6, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Masters] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (1, 2, 2, 1, 2, N'Ноутбук перегревается и шумит вентиляторами при небольшой нагрузке.', CAST(N'2025-06-11T16:08:14.597' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (2, 1, 1, 2, 1, N'Разбит экран, на изображении полосы.', CAST(N'2025-06-11T16:08:14.597' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (3, 3, 3, 1, 7, N'Не реагирует на нажатие кнопки питания', CAST(N'2025-06-11T17:08:23.703' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (4, 4, 4, 1, 5, N'Ноутбук сильно греется, нужна чистка и замена термопасты.', CAST(N'2024-05-12T10:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (5, 5, 5, 2, 4, N'Разбит экран на планшете, заказан новый дисплей.', CAST(N'2024-05-12T11:30:00.000' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (6, 6, 6, 2, 2, N'Смартфон не заряжается.', CAST(N'2024-05-12T14:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (7, 7, 7, 1, 6, N'Установка дополнительного SSD накопителя.', CAST(N'2024-05-10T09:00:00.000' AS DateTime), CAST(N'2024-05-11T18:00:00.000' AS DateTime))
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (8, 8, 8, 1, 3, N'Консоль не читает диски. Стоимость ремонта 5000р.', CAST(N'2024-05-13T12:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (9, 1, 4, 1, 7, N'Чистка ноутбука от пыли', CAST(N'2024-05-01T15:00:00.000' AS DateTime), CAST(N'2024-05-02T12:00:00.000' AS DateTime))
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (10, 2, 5, 2, 1, N'Планшет не включается', CAST(N'2024-05-13T15:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (11, 7, 11, 2, 1, N'test', CAST(N'2025-06-19T19:47:42.567' AS DateTime), NULL)
INSERT [dbo].[Orders] ([OrderID], [ClientID], [DeviceID], [MasterID], [StatusID], [ProblemDescription], [DateCreated], [DateCompleted]) VALUES (12, 11, 12, 2, 1, N'не работает звук', CAST(N'2025-06-19T19:54:07.760' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderStatuses] ON 

INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (2, N'В диагностике')
INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (5, N'В работе')
INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (7, N'Выдан клиенту')
INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (4, N'Ожидает запчасть')
INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (1, N'Принят в ремонт')
INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (6, N'Ремонт завершен')
INSERT [dbo].[OrderStatuses] ([StatusID], [StatusName]) VALUES (3, N'Требуется согласование с клиентом')
SET IDENTITY_INSERT [dbo].[OrderStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[RepairLog] ON 

INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (1, 1, 1, N'Проведена первичная диагностика. Выявлено большое количество пыли в системе охлаждения. Требуется чистка и замена термопасты.', CAST(N'2025-06-11T16:08:14.597' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (2, 3, 1, N'Так же не работает разъем питания', CAST(N'2025-06-11T18:14:29.163' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (3, 4, 1, N'Ноутбук разобран, выполняется чистка системы охлаждения.', CAST(N'2024-05-12T11:00:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (4, 5, 2, N'Согласована замена дисплея с клиентом. Запчасть заказана.', CAST(N'2024-05-12T15:00:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (5, 6, 2, N'Выявлен неисправный разъем зарядки. Требуется замена.', CAST(N'2024-05-13T10:00:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (6, 7, 1, N'Установка SSD и клонирование системы завершено.', CAST(N'2024-05-11T17:45:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (7, 8, 1, N'Диагностика привода Blu-ray. Требуется замена лазерной головки.', CAST(N'2024-05-13T14:00:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (8, 9, 1, N'Ноутбук почищен и протестирован. Готов к выдаче.', CAST(N'2024-05-02T11:00:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (9, 10, 2, N'Заказ принят в работу.', CAST(N'2024-05-13T15:05:00.000' AS DateTime))
INSERT [dbo].[RepairLog] ([LogID], [OrderID], [MasterID], [LogText], [LogDate]) VALUES (10, 12, 2, N'Работает исправно', CAST(N'2025-06-19T19:55:13.117' AS DateTime))
SET IDENTITY_INSERT [dbo].[RepairLog] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Администратор')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Мастер')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'Менеджер')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[SpareParts] ON 

INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (1, N'Экран для iPhone 13', N'IP13-SCR', N'OLED-дисплей, 6.1 дюйма', 10)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (2, N'Аккумулятор для MacBook Pro 16', N'MBP16-BAT', N'Модель A2141', 5)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (3, N'SSD Samsung 1TB', N'SAM-SSD-1TB', N'NVMe M.2', 14)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (4, N'Тачскрин Redmi Note 8 Pro', N'RN8-TCH', N'TouchScreen, 6.6 дюйма ', 10)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (5, N'Термопаста Arctic MX-4', N'COOL-MX4-4G', N'Термоинтерфейс, 4 грамма', 50)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (6, N'Разъем USB-C для ноутбука Dell XPS', N'DEL-XPS-USBC', N'Плата с разъемом для пайки', 30)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (7, N'Модуль основной камеры для iPhone 13', N'APL-13-CAM', N'Оригинальный модуль камеры', 9)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (8, N'Кулер для игрового ноутбука ASUS ROG', N'ASU-ROG-FAN', N'Вентилятор для CPU', 11)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (9, N'Аккумулятор для MacBook Air M1', N'APL-MBA-M1-BAT', N'Модель A2389', 18)
INSERT [dbo].[SpareParts] ([PartID], [PartName], [SKU], [Description], [QuantityInStock]) VALUES (10, N'Шлейф кнопки включения для iPad Pro 11', N'APL-IPAD11-PWR', N'Гибкий шлейф с контактами', 21)
SET IDENTITY_INSERT [dbo].[SpareParts] OFF
GO
SET IDENTITY_INSERT [dbo].[TypicalFaults] ON 

INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (1, N'Не включается', N'Устройство не реагирует на кнопку включения.', N'1. Проверить зарядное устройство и кабель. 2. Проверить аккумулятор. 3. Диагностика материнской платы.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (2, N'Разбит экран', N'Трещины на стекле или матрице дисплея.', N'Замена дисплейного модуля.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (3, N'Быстро разряжается', N'Аккумулятор не держит заряд.', N'Замена аккумулятора.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (4, N'Не реагирует на касания', N'При нажатии на экран нет отклика.', N'Замена дисплейного модуля. Замена тачскрина')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (5, N'Попала влага', N'Устройство контактировало с жидкостью.', N'Полная разборка, чистка в ультразвуковой ванне, диагностика компонентов.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (6, N'Проблемы с ПО', N'Устройство зависает, перезагружается, приложения вылетают.', N'Сброс до заводских настроек, перепрошивка устройства.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (7, N'Нет звука', N'Не работает динамик или микрофон.', N'Диагностика и замена соответствующего аудио-компонента.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (8, N'Перегрев', N'Устройство сильно нагревается при работе.', N'Чистка системы охлаждения, замена термопасты.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (9, N'Не работает камера', N'Приложение камеры выдает ошибку или черный экран.', N'Замена модуля камеры.')
INSERT [dbo].[TypicalFaults] ([FaultID], [FaultName], [Description], [RecommendedSolution]) VALUES (10, N'Не работает Wi-Fi/Bluetooth', N'Устройство не видит сети или не подключается к устройствам.', N'Диагностика и возможная замена Wi-Fi модуля.')
SET IDENTITY_INSERT [dbo].[TypicalFaults] OFF
GO
SET IDENTITY_INSERT [dbo].[UsedParts] ON 

INSERT [dbo].[UsedParts] ([UsedPartID], [OrderID], [PartID], [QuantityUsed]) VALUES (1, 3, 3, 1)
INSERT [dbo].[UsedParts] ([UsedPartID], [OrderID], [PartID], [QuantityUsed]) VALUES (9, 4, 5, 1)
INSERT [dbo].[UsedParts] ([UsedPartID], [OrderID], [PartID], [QuantityUsed]) VALUES (10, 5, 1, 1)
INSERT [dbo].[UsedParts] ([UsedPartID], [OrderID], [PartID], [QuantityUsed]) VALUES (11, 7, 3, 1)
INSERT [dbo].[UsedParts] ([UsedPartID], [OrderID], [PartID], [QuantityUsed]) VALUES (12, 9, 5, 1)
INSERT [dbo].[UsedParts] ([UsedPartID], [OrderID], [PartID], [QuantityUsed]) VALUES (13, 12, 10, 1)
SET IDENTITY_INSERT [dbo].[UsedParts] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [RoleID], [Login], [PasswordHash], [FullName], [IsActive]) VALUES (1, 2, N'ivanov_ii', N'ivanov', N'Иванов Иван Иванович', 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [Login], [PasswordHash], [FullName], [IsActive]) VALUES (2, 2, N'petrov_pp', N'petrov', N'Петров Петр Петрович', 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [Login], [PasswordHash], [FullName], [IsActive]) VALUES (3, 1, N'admin', N'admin', N'Системный Администратор', 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [Login], [PasswordHash], [FullName], [IsActive]) VALUES (4, 3, N'antoha', N'manager', N'Михайлов Антон Владиславович', 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [Login], [PasswordHash], [FullName], [IsActive]) VALUES (5, 3, N'test', N'test', N'test', 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [Login], [PasswordHash], [FullName], [IsActive]) VALUES (6, 2, N'z210', N'z210', N'Зырянов Артем Владимирович', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Clients__85FB4E38AE3D058E]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[Clients] ADD UNIQUE NONCLUSTERED 
(
	[PhoneNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Devices__048A00080D0FBFC0]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[Devices] ADD UNIQUE NONCLUSTERED 
(
	[SerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Masters__1788CCAD188ED05A]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[Masters] ADD UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__OrderSta__05E7698AC85C0CBA]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[OrderStatuses] ADD UNIQUE NONCLUSTERED 
(
	[StatusName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B61603B452EA0]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__SparePar__CA1ECF0DAEF32920]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[SpareParts] ADD UNIQUE NONCLUSTERED 
(
	[SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__5E55825B3EB144FA]    Script Date: 19.06.2025 21:20:30 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Masters] ADD  DEFAULT (getdate()) FOR [HireDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[RepairLog] ADD  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[SpareParts] ADD  DEFAULT ((0)) FOR [QuantityInStock]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Masters]  WITH CHECK ADD  CONSTRAINT [FK_Masters_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Masters] CHECK CONSTRAINT [FK_Masters_Users]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Clients] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Clients] ([ClientID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Clients]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Devices] FOREIGN KEY([DeviceID])
REFERENCES [dbo].[Devices] ([DeviceID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Devices]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Masters] FOREIGN KEY([MasterID])
REFERENCES [dbo].[Masters] ([MasterID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Masters]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderStatuses] FOREIGN KEY([StatusID])
REFERENCES [dbo].[OrderStatuses] ([StatusID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderStatuses]
GO
ALTER TABLE [dbo].[RepairLog]  WITH CHECK ADD  CONSTRAINT [FK_RepairLog_Masters] FOREIGN KEY([MasterID])
REFERENCES [dbo].[Masters] ([MasterID])
GO
ALTER TABLE [dbo].[RepairLog] CHECK CONSTRAINT [FK_RepairLog_Masters]
GO
ALTER TABLE [dbo].[RepairLog]  WITH CHECK ADD  CONSTRAINT [FK_RepairLog_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RepairLog] CHECK CONSTRAINT [FK_RepairLog_Orders]
GO
ALTER TABLE [dbo].[UsedParts]  WITH CHECK ADD  CONSTRAINT [FK_UsedParts_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[UsedParts] CHECK CONSTRAINT [FK_UsedParts_Orders]
GO
ALTER TABLE [dbo].[UsedParts]  WITH CHECK ADD  CONSTRAINT [FK_UsedParts_SpareParts] FOREIGN KEY([PartID])
REFERENCES [dbo].[SpareParts] ([PartID])
GO
ALTER TABLE [dbo].[UsedParts] CHECK CONSTRAINT [FK_UsedParts_SpareParts]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[SpareParts]  WITH CHECK ADD  CONSTRAINT [CHK_QuantityInStock] CHECK  (([QuantityInStock]>=(0)))
GO
ALTER TABLE [dbo].[SpareParts] CHECK CONSTRAINT [CHK_QuantityInStock]
GO
ALTER TABLE [dbo].[UsedParts]  WITH CHECK ADD  CONSTRAINT [CHK_QuantityUsed] CHECK  (([QuantityUsed]>(0)))
GO
ALTER TABLE [dbo].[UsedParts] CHECK CONSTRAINT [CHK_QuantityUsed]
GO
USE [master]
GO
ALTER DATABASE [RepairServiceDB] SET  READ_WRITE 
GO
