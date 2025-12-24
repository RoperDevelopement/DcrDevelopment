USE [AlanoClub]
GO
 /****** Object:  StoredProcedure [dbo].[spAddAlanoClubReceipt]    Script Date: 12/7/2025 1:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE    PROCEDURE [dbo].[spAddGetMemberID]
	-- Add the parameters for the stored procedure here
	@MemberID int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if(@MemberID != 0)
	begin
    -- Insert statements for procedure here
	INSERT INTO [dbo].[AlanoClubMemberID]
           ([MemberID])
     VALUES
           (@MemberID);
		   end
END
GO