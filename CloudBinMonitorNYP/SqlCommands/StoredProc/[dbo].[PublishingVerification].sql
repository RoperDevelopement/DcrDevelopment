USE [BuildInformation]
GO

/****** Object:  Table [dbo].[PublishingVerification]    Script Date: 12/14/2015 12:47:20 PM ******/
DROP TABLE [dbo].[PublishingVerification]
GO

/****** Object:  Table [dbo].[PublishingVerification]    Script Date: 12/14/2015 12:47:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PublishingVerification](
	[PublishingId] [int] IDENTITY(1,1) NOT NULL,
	[Topic ID] [uniqueidentifier] NOT NULL,
	[Server] [nvarchar](256) NULL,
	[Database] [nvarchar](256) NOT NULL,
	[Lcid] [nvarchar](4) NOT NULL,
	[DX Version to Publish] [nvarchar](10) NOT NULL,
	[Published Version] [nvarchar](10) NULL,
	[Publish Status] [nvarchar](25) NOT NULL,
	[Published Verification] [nvarchar](20) NOT NULL,
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
	[Times Checked Published] [int] NOT NULL,
 CONSTRAINT [PK__Publishi__00B0CF1D9EF6BE01] PRIMARY KEY CLUSTERED 
(
	[Topic ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


