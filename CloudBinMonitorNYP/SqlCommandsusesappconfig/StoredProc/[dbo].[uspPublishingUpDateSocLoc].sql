USE [BuildInformation]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingUpDateSocLoc]    Script Date: 03/15/2016 04:39:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,v-darope>
-- Create date: <Create Date,03/15/2016>
-- Description:	<Description,When adding the loc soc files it is to slow to query the db to get the informaiton on the team name so run a sp>
-- =============================================
CREATE PROCEDURE [dbo].[uspPublishingUpDateSocLoc]
	 as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	  BEGIN TRY
		Begin Tran UpDateLoc;	
    update [dbo].[PublishingVerification]
      set  [Team Name] = rt.[Team Name]
      ,[Title] = rt.[TOC Title]
      ,[TocTitle] = rt.[Title] 
      ,[TopicType] =rt.[Topic Type]
      ,[Writer] = rt.Writer 
      ,[Embargo End Date] = bd.[Embargo End Date]
  FROM [dbo].[PublishingVerification] pv join [DDCMS_MOD_IW].[dbo].[Reporting_Topic] rt on pv.[Topic ID] = rt.[Topic ID]
  join [DDCMS_MOD_IW].[dbo].[Reporting_Topic_BeginDates] as bd on bd.[Topic ID] = pv.[Topic ID]
  where pv.[Database] = 'DDCMS_MOD_IW' and pv.Lcid != '1033'
  and pv.[Team Name] = 'NotFound' and Year(pv.[Embargo End Date]) = '1900' 
  commit tran  UpDateLoc;
		
      END TRY

      BEGIN CATCH
         Rollback Tran  UpDateLoc;
		 PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));
		 SELECT [Error_Line] = ERROR_LINE(), [Error_Number] = ERROR_NUMBER(), [Error_Severity] = ERROR_SEVERITY(), [Error_State] = ERROR_STATE() SELECT [Error_Message] = ERROR_MESSAGE() 
		 Return -1
      END CATCH

END

GO


