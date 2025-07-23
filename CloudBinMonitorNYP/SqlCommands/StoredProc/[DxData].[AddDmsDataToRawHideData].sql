USE [DxData]
GO

/****** Object:  StoredProcedure [DxData].[AddDmsDataToRawHideData]    Script Date: 10/13/2015 4:22:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [DxData].[AddDmsDataToRawHideData]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	--Check the Pre-requisites for running this stored procedure.
	--Before running this stored procedure make sure that PrepAddLibraryDailyData
	--Stored Procedure is run and the data is populated in to that table.
	IF OBJECT_ID('tempDmsToRawhide') IS NULL
	BEGIN
		RAISERROR (N'Please Execute %s before you execute this stored procedure.', -- Message text.
			   11, -- Severity,
			   1, -- State,
			   N'PrepDmsDataToRawHideInfo');
		GOTO SPROC_FAILURE
	END

	BEGIN TRY
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;	
		DECLARE @MaxRawHideId int;	
		DECLARE @ReleaseID nvarchar(240);	
		DECLARE @Title nvarchar(1024);	
		DECLARE @InternalWriter nvarchar(1024);	
		DECLARE @ContentSummary nvarchar(1024);	
		DECLARE @ActualPublishDate datetime;
		DECLARE @SupplementalURL nvarchar(max);	
		DECLARE @ContentReleaseURL nvarchar(max);	
SET NOCOUNT ON;
	set  @MaxRawHideId = (select max([RawHideID]) from [DxData].[RawHideInfo])
	if(@MaxRawHideId is null)
	begin
	set  @MaxRawHideId = 0;
	end
	print @MaxRawHideId 
	DECLARE DmsRawHodeTableCursor CURSOR FAST_FORWARD 
	for select
		[ReleaseID],[Title],[InternalWriter]
		,[ContentSummary],[ActualPublishDate]
		,[SupplementalURL],[ContentReleaseURL]
  FROM   [dbo].[tempDmsToRawhide]
   open DmsRawHodeTableCursor;
		  fetch next from DmsRawHodeTableCursor
		  into
			@ReleaseID,@Title,@InternalWriter,
			@ContentSummary,@ActualPublishDate,
		    @SupplementalURL, @ContentReleaseURL
		   WHILE @@FETCH_STATUS = 0
	begin
	if not exists(select 1 from  [DxData].[RawHideInfo] where  [DxData].[RawHideInfo].UUID = @ReleaseID)
	begin
		set  @MaxRawHideId = @MaxRawHideId + 1;
		INSERT INTO [DxData].[RawHideInfo]
			([RawHideID]
			,[ActualPublishDate]
           ,[ContentSummary]
           ,[ContentReleaseURL]
           ,[InternalWriter]
           ,[Title]
           ,[UUID]
           ,[SupplementalURL]
           )
		values(
			@MaxRawHideId
			,@ActualPublishDate,@ContentSummary,
			@ContentReleaseURL,@InternalWriter
			,@Title,@ReleaseID,@SupplementalURL
			)
	end	--- update rawhide info with new values
	else
	begin
		update  [DxData].[RawHideInfo]
		set [DxData].[RawHideInfo].[ActualPublishDate] = @ActualPublishDate,
			[DxData].[RawHideInfo].[ContentSummary] = @ContentSummary,
			[DxData].[RawHideInfo].[ContentReleaseURL] = @ContentReleaseURL,
			[DxData].[RawHideInfo].[Title] = @Title,
            [DxData].[RawHideInfo].[SupplementalURL] = @SupplementalURL
			where  [DxData].[RawHideInfo].UUID = @ReleaseID
	end
	  fetch next from DmsRawHodeTableCursor
		  into
			@ReleaseID,@Title,@InternalWriter,
			@ContentSummary,@ActualPublishDate,
		    @SupplementalURL, @ContentReleaseURL
	end
	CLOSE  DmsRawHodeTableCursor;
DEALLOCATE  DmsRawHodeTableCursor; 

	END TRY
	BEGIN CATCH
		   SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
		RAISERROR (@ErrorMessage, -- Message text.
                   @ErrorSeverity, -- Severity.
                   @ErrorState -- State.
                  );
		RETURN(-1);
		 
	END CATCH
--	DROP TABLE tempDmsToRawhide
GOTO SPROC_PASS
SPROC_FAILURE:
	PRINT 'ERROR: Failed to upload RawHide Data'
	RETURN -1
SPROC_PASS:
IF OBJECT_ID('tempDmsToRawhide') IS not NULL
	DROP TABLE tempDmsToRawhide
	RETURN 1

 

END












GO


