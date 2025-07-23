USE [Publishing]
GO

/****** Object:  Table [dbo].[PublishingVerificationUploadLoc]    Script Date: 06/07/2016 10:23:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PublishingVerificationUploadLoc](
	[Topic ID] [uniqueidentifier] NOT NULL,
	[Server] [nvarchar](256) NULL,
	[Database] [nvarchar](256) NOT NULL,
	[Lcid] [nvarchar](6) NULL,
	[DX Version to Publish] [nvarchar](10) NOT NULL,
	[Published Version] [nvarchar](10) NULL,
	[Publish Status] [nvarchar](50) NOT NULL,
	[Published Verification] [nvarchar](50) NOT NULL,
	[PublishingEnvironment] [nvarchar](5) NULL,
	[PublishingUrl] [nvarchar](256) NULL,
	[Project Name] [nvarchar](256) NOT NULL,
	[Team Name] [nvarchar](256) NOT NULL,
	[Title] [nvarchar](256) NULL,
	[TocTitle] [nvarchar](256) NULL,
	[TopicType] [nvarchar](50) NULL,
	[Writer] [nvarchar](256) NULL,
	[Embargo End Date] [datetime] NOT NULL,
	[Publishing Verify Date] [datetime] NULL,
	[Create Publishing Date] [datetime] NULL,
	[SLA Publish Time] [datetime] NULL,
	[Max Check Publish Time] [datetime] NULL,
	[Utc DateTime] [datetime] NOT NULL
) ON [PRIMARY]

GO


