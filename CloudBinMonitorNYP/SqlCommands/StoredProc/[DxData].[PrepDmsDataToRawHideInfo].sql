USE [DxData]
GO

/****** Object:  StoredProcedure [DxData].[PrepDmsDataToRawHideInfo]    Script Date: 10/13/2015 2:27:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







ALTER PROCEDURE [DxData].[PrepDmsDataToRawHideInfo]
AS
BEGIN

	-- This stored Procedure is used to truncate the RawHideInfo table and upload data to RawHideInfo table
	
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	--Delete the RawHideInfo table
	DELETE DxData.RawHideInfo
	
	--Check if the Temp table exists and drop it before creating it again.
	IF OBJECT_ID('tempDmsToRawhide') IS NOT NULL
	BEGIN
		DROP TABLE tempDmsToRawhide
	END

	CREATE TABLE [dbo].[tempDmsToRawhide](
	[ReleaseID]  [nvarchar](240)  NOT NULL,
	[Title] [nvarchar](1024) NOT NULL,
	[InternalWriter] [nvarchar](1024) NOT NULL,
	[ContentSummary] [nvarchar](1024) NULL,
	[ActualPublishDate] [datetime] NOT NULL,
	[SupplementalURL] [nvarchar](max) NOT NULL,
	[ContentReleaseURL] [nvarchar](max) NULL,
 CONSTRAINT [PK_tempDmsToRawhide] PRIMARY KEY CLUSTERED 
(
	[ReleaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


END








GO


