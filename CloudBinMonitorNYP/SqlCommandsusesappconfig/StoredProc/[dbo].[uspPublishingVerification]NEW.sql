USE [BuildInformation]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerification]    Script Date: 03/15/2016 11:28:29 AM ******/
DROP PROCEDURE [dbo].[uspPublishingVerification]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerification]    Script Date: 03/15/2016 11:28:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO









 
CREATE PROCEDURE [dbo].[uspPublishingVerification](
	@xmlDocument XML,
	@createrVerifyPub nvarchar(20),
	@pubEnv nvarchar(20),
	@pubLcid nvarchar(4)

)

-- ****************************************************************************
-- Author: v-darope
-- Create date: 12/10/2015
-- Description: Update the PublishingVerification table
-- ****************************************************************************
AS
BEGIN
DECLARE @xmlDocHandle int
      BEGIN TRY
		Begin Tran PubTran;	
		-- Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @xmlDocHandle OUTPUT, @xmlDocument
		if @createrVerifyPub = 'createpublish'
		begin
			insert into [dbo].[PublishingVerification]
				 ([Topic ID],[Server],[Database],[Lcid],[DX Version to Publish],[Publish Status],[Published Verification],[PublishingEnvironment],
					[Project Name],[Team Name],[Title],[TocTitle],[TopicType],[Writer],[Embargo End Date],[Create Publishing Date],[SLA Publish Time],[Max Check Publish Time])
			SELECT TopicID,[Server],[Database],Lcid,DXVersionPublish,PublishStatus,PublishedVerification,PublishingEnvironment,ProjectName,TeanName,
				Title,TocTitle,TopicType,Writer,EmbargoEndDate,CreatePublishingDate,SLAPublishTime,MaxCheckPublishTime
			FROM OPENXML (@xmlDocHandle, '/Root/TopicID',2)
			WITH (TopicID  uniqueidentifier '@TopicGuid',
			  [Server] nvarchar(256),
              [Database] nvarchar(256),
			  Lcid nvarchar(4),
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
			  MaxCheckPublishTime datetime
			   )
			   as createXmldoc
			where not exists(select [Topic ID] from [dbo].[PublishingVerification] pubver
			 where pubver.[Topic ID] =
			 (case(@pubEnv) when 'mtps' then
				(select pubvermtps.[Topic ID] from [dbo].[PublishingVerification] pubvermtps where createXmldoc.TopicID = pubvermtps.[Topic ID] and pubvermtps.[Published Verification] = 'Created' and createXmldoc.DXVersionPublish = pubvermtps.[DX Version to Publish] and pubvermtps.Lcid = @pubLcid)
			  when 'soc' then
				(select pubversoc.[Topic ID] from [dbo].[PublishingVerification] pubversoc where createXmldoc.TopicID = pubversoc.[Topic ID] and pubversoc.[Create Publishing Date] = createXmldoc.CreatePublishingDate and pubversoc.[Lcid] = @pubLcid)
				--(select pubversoc.[Topic ID] from [dbo].[PublishingVerification] pubversoc where createXmldoc.TopicID = pubversoc.[Topic ID] and pubversoc.[DX Version to Publish] = createXmldoc.DXVersionPublish)
				end
			 )
			 )
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
				from #PubVerUploadTable) pt
				where pt.PubId = [dbo].[PublishingVerification].PublishingId
				
		end
		commit tran PubTran;
      END TRY

      BEGIN CATCH
         Rollback Tran PubTran;
		 PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));
		 SELECT [Error_Line] = ERROR_LINE(), [Error_Number] = ERROR_NUMBER(), [Error_Severity] = ERROR_SEVERITY(), [Error_State] = ERROR_STATE() SELECT [Error_Message] = ERROR_MESSAGE() 
      END CATCH
 
   EXEC sp_xml_removedocument @xmlDocHandle

END









GO


