USE [BuildInformation]
GO

/****** Object:  StoredProcedure [dbo].[uspUpDatePublishingLocalRevNumbers]    Script Date: 05/02/2016 01:28:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspUpDatePublishingLocalRevNumbers]
		@xmlDocument XML
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DECLARE @xmlDocHandle int
   DECLARE @errorMessage nvarchar(max)
   DECLARE @erMessage nvarchar(max)
   -- Create an internal representation of the XML document.
BEGIN TRY
	EXEC sp_xml_preparedocument @xmlDocHandle OUTPUT, @xmlDocument
end try
BEGIN CATCH
	set @erMessage = @@ERROR
	select @errorMessage = ERROR_MESSAGE()
	set @errorMessage = @erMessage +' '+ @errorMessage
	GOTO SPROC_FAILURE
END CATCH
 BEGIN TRY
	Begin Tran UpdateLocPubTable;	
		Create TABLE #UpDatePubLocRevNumberTable
		(
			[TopicID] [uniqueidentifier] NOT NULL,
			[LocLcid] [nvarchar](6) NOT NULL,
			[DXVersiontoPublish] [nvarchar](10) NOT NULL,
			[ProjectName] [nvarchar](256) NOT NULL,
			[DXServer] [nvarchar](256) NOT NULL

		)
		
	insert into #UpDatePubLocRevNumberTable
		SELECT TopicID,Lcid,DXVersionPublish,Pname,[Server]
			FROM OPENXML (@xmlDocHandle, '/Root/TopicID',2)
				WITH (TopicID  uniqueidentifier '@TopicGuid',
					 Lcid nvarchar(6),
					 DXVersionPublish nvarchar(10),
					   PName nvarchar(256),
					[Server] nvarchar(256)					
					)
		update [dbo].[PublishingLocalReversionNumbers]
			set [DX Version to Publish] = locPub.DXVersiontoPublish
			,[Last Update Time] = getdate()
      			from
					(select TopicID,LocLcid,DXVersiontoPublish,ProjectName,DxServer
						from  #UpDatePubLocRevNumberTable) locPub inner join [dbo].[PublishingLocalReversionNumbers] publoc on locPub.[TopicID]= publoc.[Topic ID] 
							--and publoc.Lcid = locPub.LocLcid and  locPub.DXServer = publoc.[Server] and locPub.DXVersiontoPublish != publoc.[DX Version to Publish]
							where publoc.Lcid = locPub.LocLcid and  locPub.DXServer = publoc.[Server] 
							and locPub.DXVersiontoPublish != publoc.[DX Version to Publish] and publoc.[Project Name] = locPub.ProjectName
	commit tran UpdateLocPubTable;		 
END TRY

BEGIN CATCH
	 Rollback Tran UpdateLocPubTable;
		set @erMessage = @@ERROR
		select @errorMessage = ERROR_MESSAGE()
		set @errorMessage = @erMessage +' '+ @errorMessage
	   GOTO SPROC_FAILURE
     END CATCH

 BEGIN TRY
	Begin Tran AddLocPubTable
		insert into [dbo].[PublishingLocalReversionNumbers]
			([Topic ID],[Lcid],[DX Version to Publish],[Project Name],[Server],[Database])
			SELECT TopicID,Lcid,DXVersionPublish,PName,[Server],[Database]
			FROM OPENXML (@xmlDocHandle, '/Root/TopicID',2)
			WITH (TopicID  uniqueidentifier '@TopicGuid',
				  Lcid nvarchar(6),
				  DXVersionPublish nvarchar(10),
				  PName nvarchar(256),
				[Server] nvarchar(256),
				[Database] nvarchar(256)
			   )
			   as createLocPubVer
		 	 where not exists(select [Topic ID] from [dbo].[PublishingLocalReversionNumbers] publoc 
				 where publoc.[Topic ID] = createLocPubVer.TopicID and publoc.[Lcid] = createLocPubVer.lcid and  publoc.[Server] = createLocPubVer.[Server])
	commit tran AddLocPubTable;		 
END TRY
	BEGIN CATCH
		Rollback Tran AddLocPubTable;
		set @erMessage = @@ERROR
		select @errorMessage = ERROR_MESSAGE()
		set @errorMessage = @erMessage +' '+ @errorMessage
	   GOTO SPROC_FAILURE
     END CATCH

BEGIN TRY
	EXEC sp_xml_removedocument @xmlDocHandle
END TRY
	BEGIN CATCH
		PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));
		SELECT [Error_Line] = ERROR_LINE(), [Error_Number] = ERROR_NUMBER(), [Error_Severity] = ERROR_SEVERITY(), [Error_State] = ERROR_STATE() SELECT [Error_Message] = ERROR_MESSAGE() 
		 set @erMessage = @@ERROR
		select @errorMessage = ERROR_MESSAGE()
		set @errorMessage = @erMessage +' '+ @errorMessage
	   GOTO SPROC_FAILURE
     END CATCH
END
GOTO SPROC_SUCCESS

SPROC_FAILURE:
RAISERROR(@errorMessage,10,1)
RETURN -1

SPROC_SUCCESS:
RETURN 0







GO


