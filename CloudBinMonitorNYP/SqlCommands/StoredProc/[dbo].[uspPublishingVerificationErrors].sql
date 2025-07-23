USE [BuildInformation]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerificationErrors]    Script Date: 3/3/2016 9:38:34 AM ******/
DROP PROCEDURE [dbo].[uspPublishingVerificationErrors]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerificationErrors]    Script Date: 3/3/2016 9:38:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO






 
CREATE PROCEDURE [dbo].[uspPublishingVerificationErrors](
	@errorXmlDocument XML
	

)

-- ****************************************************************************
-- Author: v-darope
-- Create date: 1/12/2015
-- Description: Update the PublishingVerificationError table
-- ****************************************************************************
AS
BEGIN
DECLARE @xmlDocHandle int
      BEGIN TRY
		Begin Tran PubTran;	
		-- Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @xmlDocHandle OUTPUT, @errorXmlDocument
			insert into [dbo].[PublishingVerificationErrors]
				 ([Topic ID],[Error Create Date],[Server],[Database],[Lcid],
				 [PublishingEnvironment],[PublishingUrl],[Project Name],
				 [Publishing ScriptCommand],[Error Message],[PS Error Message])
			Select TopicID,ErrorCreateDate,[Server],[Database],Lcid,PublishingEnvironment,
					PublishingUrl,ProjectName,PublishingScriptCommand,ErrorMessage,PSErrorMessage
			FROM OPENXML (@xmlDocHandle, '/Root/TopicID',2)
			WITH (TopicID  uniqueidentifier '@TopicGuid',
					ErrorCreateDate datetime,
					[Server] nvarchar(256),
					[Database] nvarchar(256),
					Lcid nvarchar(4),
					PublishingEnvironment nvarchar(5),
					PublishingUrl nvarchar(256),
					ProjectName nvarchar(256),
					PublishingScriptCommand nvarchar(25),
					ErrorMessage nvarchar(max),
					PSErrorMessage nvarchar(max)
			   )
			   as createXmldoc
			where not exists(select pubvererror.[Topic ID] from [dbo].[PublishingVerificationErrors] pubvererror
			 where pubvererror.[Error Message] = createXmldoc.ErrorMessage and
			   substring(pubvererror.[PS Error Message],1,10) = substring(createXmldoc.PSErrorMessage,1,10) 
			 and  Convert(date,pubvererror.[Error Create Date],101) = Convert(date,createXmldoc.ErrorCreateDate,101)
			 )
		commit tran PubTran;
		
      END TRY

      BEGIN CATCH
         Rollback Tran PubTran;
		 PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));
		 SELECT [Error_Line] = ERROR_LINE(), [Error_Number] = ERROR_NUMBER(), [Error_Severity] = ERROR_SEVERITY(), [Error_State] = ERROR_STATE() SELECT [Error_Message] = ERROR_MESSAGE() 
      END CATCH
    EXEC sp_xml_removedocument @xmlDocHandle
	if Exists (select 1 from [dbo].[PublishingVerificationErrors] where [Topic ID] = cast(cast(0 as binary) as uniqueidentifier) and Convert(date,[Error Create Date],101) = Convert(date,GetDate(),101))
	  begin
			update [dbo].[PublishingVerificationErrors]
			set [Team Name] = 'N\A'
				,[Title] = 'N\A'
				,[TocTitle] = 'N\A'
				where [Topic ID] = cast(cast(0 as binary) as uniqueidentifier)
		end
	BEGIN TRY
	Begin Tran PubTranUpdateError;	
		if Exists (select 1 from [dbo].[PublishingVerificationErrors] where [Team Name] is null or [Title] is null or [Project Name] = 'all')
		begin
			update [dbo].[PublishingVerificationErrors]
			set [Project Name] = pubver.[Project Name]
				,[Team Name] = pubver.[Team Name]
				,[Title] = pubver.[Title]
				,[TocTitle] =pubver.[TocTitle]
			from
				(select [Topic ID],[Project Name],[Team Name],[Title],[TocTitle] from [dbo].[PublishingVerification]) pubver
				where [dbo].[PublishingVerificationErrors].[Topic ID] = pubver.[Topic ID]
		end
		commit tran PubTranUpdateError;
		  END TRY

      BEGIN CATCH
         Rollback Tran PubTranUpdateError;
		 PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));
		 SELECT [Error_Line] = ERROR_LINE(), [Error_Number] = ERROR_NUMBER(), [Error_Severity] = ERROR_SEVERITY(), [Error_State] = ERROR_STATE() SELECT [Error_Message] = ERROR_MESSAGE() 
      END CATCH
	  

END








GO


