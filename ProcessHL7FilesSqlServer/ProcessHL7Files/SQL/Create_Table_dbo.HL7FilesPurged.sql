USE [HL7]
GO

/****** Object:  Table [dbo].[HL7Files]    Script Date: 5/31/2019 2:15:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HL7FilesPurged](
	[RowId] [int]  NOT NULL,
	[Date_Of_Service] [date] NOT NULL,
	[Patient_ID] [nvarchar](16) NOT NULL,
	[Client_Code] [nvarchar](16) NULL,
	[Patient_Last_Name] [nvarchar](50) NULL,
	[Patient_First_Name] [nvarchar](50) NULL,
	[Dr_Code] [nvarchar](16) NULL,
	[Dr_Name] [nvarchar](50) NULL,
	[Requisition_Number] [nvarchar](16) NULL,
	[Financial_Number] [nvarchar](16) NULL,
 CONSTRAINT [PK_HL7FilesPurged] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


