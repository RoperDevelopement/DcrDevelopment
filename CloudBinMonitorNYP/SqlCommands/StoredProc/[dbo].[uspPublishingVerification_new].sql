USE [Publishing]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerification]    Script Date: 06/07/2016 09:04:54 AM ******/
DROP PROCEDURE [dbo].[uspPublishingVerification]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerification]    Script Date: 06/07/2016 09:04:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO








 
CREATE PROCEDURE [dbo].[uspPublishingVerification](
	@xmlDocument XML,
	@createrVerifyPub nvarchar(20),
	@pubEnv nvarchar(20),
	@pubLcid nvarchar(6)

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
		if @createrVerifyPub = 'createpublish'
		begin
			insert into [dbo].[PublishingVerification_Upload]
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
		
		
		end
		else
		begin
			Create TABLE #PubVerUploadTable
			(
				PubId  int,
				PublishedVersion nvarchar(10),
				PublishStatus nvarchar(50),
				PublishedVerification nvarchar(50),
				PublishingVerifyDate datetime,
				PubUrl nvarchar (256)
			)
			insert into #PubVerUploadTable
			Select PubId,PublishedVersion,PublishStatus,PublishedVerification,PublishingVerifyDate,PubUrl
				FROM OPENXML (@xmlDocHandle, '/Root/TopicID',2)
			WITH ( PubId int,
					PublishedVersion nvarchar(10),
					PublishStatus nvarchar(50),
					PublishedVerification nvarchar(50),
					PublishingVerifyDate datetime,
					PubUrl nvarchar (256)
			   )
			   update [dbo].[PublishingVerification]
			   set  [Published Version] = pt.PublishedVersion
					,[Publish Status] = pt.PublishStatus
					,[Published Verification] = pt.PublishedVerification
					,[Publishing Verify Date] = pt.PublishingVerifyDate
					,[PublishingUrl] = pt.PubUrl
			   from
			   (select PubId,
					  PublishedVersion,
					  PublishStatus,
					  PublishedVerification,
				      PublishingVerifyDate,
					  PubUrl
				--from #PubVerUploadTable) pt
				--where pt.PubId = [dbo].[PublishingVerification].PublishingId
				from #PubVerUploadTable) pt join [dbo].[PublishingVerification] pid on pt.PubId = pid.[PublishingId]
				where [Publishing Verify Date] is null or YEAR([Publishing Verify Date]) = '1900'
				
		end
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


