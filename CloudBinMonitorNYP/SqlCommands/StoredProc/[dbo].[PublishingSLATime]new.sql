USE [BuildInformation]
GO

/****** Object:  Table [dbo].[PublishingSLATime]    Script Date: 03/15/2016 11:25:42 AM ******/
DROP TABLE [dbo].[PublishingSLATime]
GO

/****** Object:  Table [dbo].[PublishingSLATime]    Script Date: 03/15/2016 11:25:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PublishingSLATime](
	[Publishing Environment] [nvarchar](6) NOT NULL,
	[SLA Publish Time] [int] NOT NULL,
	[Max Check Publish Time] [int] NOT NULL,
 CONSTRAINT [PK_PublishingSLATime] PRIMARY KEY CLUSTERED 
(
	[Publishing Environment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


