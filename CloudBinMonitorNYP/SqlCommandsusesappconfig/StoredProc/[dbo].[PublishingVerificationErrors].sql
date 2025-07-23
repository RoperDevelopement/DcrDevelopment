USE [BuildInformation]
GO

/****** Object:  Table [dbo].[PublishingVerificationErrors]    Script Date: 1/12/2016 4:47:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PublishingVerificationErrors](
	[PublishingErrorId] [int] IDENTITY(1,1) NOT NULL,
	[Topic ID] [uniqueidentifier] NULL,
	[Error Create Date] [datetime] NOT NULL,
	[Server] [nvarchar](256) NULL,
	[Database] [nvarchar](256) NULL,
	[Lcid] [nvarchar](4) NULL,
	[PublishingEnvironment] [nvarchar](5) NULL,
	[PublishingUrl] [nvarchar](256) NULL,
	[Project Name] [nvarchar](256) NULL,
	[Team Name] [nvarchar](256) NULL,
	[Title] [nvarchar](256) NULL,
	[TocTitle] [nvarchar](256) NULL,
	[Publishing ScriptCommand] [nvarchar](25) NULL,
	[Error Message] [nvarchar](max) NULL,
	[PS Error Message] [nvarchar](max) NULL,
 CONSTRAINT [PK__Publishi__11A0DE1F8FFGBE01] PRIMARY KEY CLUSTERED 
(
	[PublishingErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


