USE [Publishing]
GO

/****** Object:  StoredProcedure [dbo].[uspPubverifyUpLoadLocFiles]    Script Date: 06/07/2016 10:24:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


 
CREATE PROCEDURE [dbo].[uspPubverifyUpLoadLocFiles](
	@xmlDocument XML

)

-- ****************************************************************************
-- Author: v-darope
-- Create date: 12/10/2015
-- Description: Update the PublishingVerification table
-- ****************************************************************************
AS
BEGIN
DECLARE @xmlDocHandle int
DECLARE @errorMessage nvarchar(max)
DECLARE @erMessage nvarchar(max)
      BEGIN TRY
		Begin Tran PubTran;	
		-- Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @xmlDocHandle OUTPUT, @xmlDocument
			insert into [dbo].[PublishingVerificationUploadLoc]
				 ([Topic ID],[Server],[Database],[Lcid],[DX Version to Publish],[Publish Status],[Published Verification],[PublishingEnvironment],
					[Project Name],[Team Name],[Title],[TocTitle],[TopicType],[Writer],[Embargo End Date],[Create Publishing Date],[SLA Publish Time],[Max Check Publish Time],[Utc DateTime])
			SELECT TopicID,[Server],[Database],Lcid,DXVersionPublish,PublishStatus,PublishedVerification,PublishingEnvironment,ProjectName,TeanName,
				Title,TocTitle,TopicType,Writer,EmbargoEndDate,CreatePublishingDate,SLAPublishTime,MaxCheckPublishTime,DateTimeUtc
			FROM OPENXML (@xmlDocHandle, '/Root/TopicID',2)
			WITH (TopicID  uniqueidentifier '@TopicGuid',
			  [Server] nvarchar(256),
              [Database] nvarchar(256),
			  Lcid nvarchar(6),
			  DXVersionPublish nvarchar(10),
			  PublishStatus nvarchar(50),
			  PublishedVerification nvarchar(50),
			  PublishingEnvironment nvarchar(5),
			  ProjectName nvarchar(256),
			  TeanName nvarchar(256),
			  Title nvarchar(256),
			  TocTitle nvarchar(256),
			  TopicType nvarchar(50),
			  Writer nvarchar(256),
			  EmbargoEndDate datetime,
			  CreatePublishingDate datetime,
			  SLAPublishTime datetime,
			  MaxCheckPublishTime datetime,
			  DateTimeUtc datetime
			   )
			   as createXmldoc
			commit tran PubTran;
      END TRY

      BEGIN CATCH
         Rollback Tran PubTran;
		 	set @erMessage = @@ERROR
			select @errorMessage = ERROR_MESSAGE()
			set @errorMessage = @erMessage +' '+ @errorMessage
			EXEC sp_xml_removedocument @xmlDocHandle
			GOTO SPROC_FAILURE
      END CATCH
 
EXEC sp_xml_removedocument @xmlDocHandle
GOTO SPROC_SUCCESS
END
SPROC_FAILURE:
THROW 60000, @errorMessage, 1; 
RETURN -1

SPROC_SUCCESS:
RETURN 0






























GO


