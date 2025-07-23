USE [Publishing]
GO

/****** Object:  StoredProcedure [dbo].[uspPublishingVerification_Update]    Script Date: 06/07/2016 09:03:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO










 
CREATE PROCEDURE [dbo].[uspPublishingVerification_Update]



-- ****************************************************************************
-- Author: v-darope
-- Create date: 12/10/2015
-- Description: Update the PublishingVerification table
-- ****************************************************************************
AS
BEGIN


			insert into [dbo].[PublishingVerification]
				 ([Topic ID],[Server],[Database],[Lcid],[DX Version to Publish],[Publish Status],[Published Verification],[PublishingEnvironment],
					[Project Name],[Team Name],[Title],[TocTitle],[TopicType],[Writer],[Embargo End Date],[Create Publishing Date],[SLA Publish Time],[Max Check Publish Time],[Utc DateTime])
			SELECT [Topic ID],[Server],[Database],[Lcid],[DX Version to Publish],[Publish Status],[Published Verification],[PublishingEnvironment],
					[Project Name],[Team Name],[Title],[TocTitle],[TopicType],[Writer],[Embargo End Date],[Create Publishing Date],[SLA Publish Time],[Max Check Publish Time],[Utc DateTime]
			from [Publishing].[dbo].[PublishingVerification_Upload] as createXmldoc
			where not exists(select [Topic ID] from [dbo].[PublishingVerification] pubver
			where pubver.[Topic ID] =
			(select top 1 pubvermtps.[Topic ID] from [dbo].[PublishingVerification] pubvermtps where createXmldoc.[Topic ID] = pubvermtps.[Topic ID] and createXmldoc.[Utc DateTime] = pubvermtps.[Utc DateTime] and pubvermtps.Lcid =  createXmldoc.Lcid)
			)
		delete pul
from [dbo].[PublishingVerification_Upload] pul
inner join [dbo].[PublishingVerification] pv on pul.[Topic ID] = pv.[Topic ID]
where pul.[Utc DateTime] = pv.[Utc DateTime] and pul.lcid =  pv.Lcid

		
GOTO SPROC_SUCCESS
END

SPROC_SUCCESS:
RETURN 0






























GO


