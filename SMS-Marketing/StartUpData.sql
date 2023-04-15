/****** Object:  Table [dbo].[AppSettings]    Script Date: 4/11/2023 4:35:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Index] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[SettingGroup] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AppSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AppSettings] ON 
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (1, 0, N'Twitter Key', N'Value Goes Here', N'TWITTER')
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (2, 1, N'Twitter Secret', N'Value Goes Here', N'TWITTER')
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (3, 2, N'Allow Images', N'Value Goes Here', N'APP')
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (4, 3, N'Facebook Access Token', N'Value Goes Here', N'FACEBOOK')
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (5, 4, N'Facebook App Id', N'Value Goes Here', N'FACEBOOK')
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (6, 5, N'Twilio Auth Token', N'Value Goes Here', N'TWILIO')
GO
INSERT [dbo].[AppSettings] ([Id], [Index], [Name], [Value], [SettingGroup]) VALUES (7, 6, N'Twilio SID', N'Value Goes Here', N'TWILIO')
GO
SET IDENTITY_INSERT [dbo].[AppSettings] OFF
GO
