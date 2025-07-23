USE [BuildInformation]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingHisory]    Script Date: 3/1/2016 10:50:27 AM ******/
DROP PROCEDURE [dbo].[uspPublishingHisory]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingHisory]    Script Date: 3/1/2016 10:50:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO



 
CREATE PROCEDURE [dbo].[uspPublishingHisory](
	@TopiID [uniqueidentifier],
	@EventLogCrDate dateTime,
	@DXVersionPublish nvarchar(10),
	@DxDatabase nvarchar (256),
	@PubEnv nvarchar (5),
	@PName nvarchar (256),
	@TName nvarchar (256),
	@EndPoint nvarchar (10),
	@Lcid nvarchar (10)
)
-- ****************************************************************************
-- Author: v-darope
-- Create date: 12/10/2015
-- Description: Update the PublishingVerification table
-- ****************************************************************************
AS
BEGIN
 
      BEGIN TRY
	  if @Lcid = '1033'
	  begin
		if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and Year([Publishing Verify Date]) = '1900' and Year([Create Publishing Date]) >= '2016'
			and @DXVersionPublish = [DX Version to Publish] and @DxDatabase = [Database] and [Lcid] = @Lcid)
			goto NoRecords

			if exists (select 1 from [dbo].[PublishingHistory] where @TopiID = [Topic ID] and @EventLogCrDate = [CreateHistoryDate]
			and @DXVersionPublish = [DX Version to Publish] and @DxDatabase = [Database] and [Lcid] = @Lcid)
			goto NoRecords

			Begin Tran PubTran;	
				if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and @EventLogCrDate = [Create Publishing Date] and  @DxDatabase = [Database] and [Lcid] = @Lcid)
				begin
					insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,[DX Version to Publish],[Published Version],[Publish Status],[PublishingEnvironment],@EndPoint,[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and @EventLogCrDate = [Create Publishing Date] and  @DxDatabase = [Database] and [Lcid] = @Lcid)
				end

				else if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,[Create Publishing Date],101) and  @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				begin
					insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,[DX Version to Publish],[Published Version],[Publish Status],[PublishingEnvironment],@EndPoint,[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,[Create Publishing Date],101)and  @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				end

  				else if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				begin
				insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,[DX Version to Publish],[Published Version],[Publish Status],[PublishingEnvironment],@EndPoint,[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				end

				else if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,-1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				begin
					insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,[DX Version to Publish],[Published Version],[Publish Status],@EndPoint,[PublishingEnvironment],[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,-1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				end

				else if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,-1,[Create Publishing Date]),101) and  @DxDatabase = [Database] and [Lcid] = @Lcid)
				begin
					insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,@DXVersionPublish,[Published Version],[Publish Status],@EndPoint,[PublishingEnvironment],[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,-1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and [Lcid] = @Lcid)
				end

				else if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and [Lcid] = @Lcid)
				begin
					insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,@DXVersionPublish,[Published Version],[Publish Status],@EndPoint,[PublishingEnvironment],[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and convert(date,@EventLogCrDate,101) = convert(date,dateadd(d,1,[Create Publishing Date]),101)and  @DxDatabase = [Database] and [Lcid] = @Lcid)
				end

				else if exists (select 1 from [dbo].[PublishingVerification] where @TopiID = [Topic ID] and @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and Year([Publishing Verify Date]) <> '1900' and [Lcid] = @Lcid)
				begin
					insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,[DX Version to Publish],[Published Version],[Publish Status],@EndPoint,[PublishingEnvironment],[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @TopiID = [Topic ID] and @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid)
				end

				else 
				begin
					insert into [dbo].[PublishingHistory]
					values(@TopiID,'OCPUBCMS',@DxDatabase,@DXVersionPublish,'N/A'
					,'Not Found',@PubEnv,@EndPoint,@PName,@TName,@EventLogCrDate,@EventLogCrDate,@Lcid)
					
				end
			commit tran PubTran;
	end

	else
	begin
		if not exists (select 1 from [dbo].[PublishingHistory] where @TopiID = [Topic ID] and @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish]  and [Lcid] = @Lcid and [CreateHistoryDate] = @EventLogCrDate )
		begin
			Begin Tran PubTran;	
				insert into [dbo].[PublishingHistory]
					([Topic ID],[Server],[Database],[DX Version to Publish],[Published Version]
					,[Publish Status],[PublishingEnvironment],[Publishing EndPoint],[Project Name],[Team Name],[Publishing Verify Date],[CreateHistoryDate],[Lcid])
					(select [Topic ID],[Server],@DxDatabase,[DX Version to Publish],[Published Version],[Publish Status],@EndPoint,[PublishingEnvironment],[Project Name],
					[Team Name],[Publishing Verify Date],@EventLogCrDate,@Lcid from [dbo].[PublishingVerification]
					where @DxDatabase = [Database] and @DXVersionPublish = [DX Version to Publish] and [Lcid] = @Lcid and [Create Publishing Date] = @EventLogCrDate and @TopiID = [Topic ID])
			commit tran PubTran;
		end
	end
      END TRY
	     BEGIN CATCH
         Rollback Tran PubTran;
		 PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));
		 SELECT [Error_Line] = ERROR_LINE(), [Error_Number] = ERROR_NUMBER(), [Error_Severity] = ERROR_SEVERITY(), [Error_State] = ERROR_STATE() SELECT [Error_Message] = ERROR_MESSAGE() 
      END CATCH
 NoRecords: 
END

















GO


