USE [CloudBinMonitor]
GO

INSERT INTO [dbo].[EdocsUpDateInfo]
           ([UpDateShareName]
           ,[UpDateFolderName]
           ,[ColdShare]
           ,[VersionFileName]
           ,[UpDateFileName])
     VALUES
           ('binmonitorupdates'
           ,'EdocsUpdates'
           ,'True'
           ,'EdocsVersion.txt'
           ,'UpDateEdocsSoftware.zip')
GO


