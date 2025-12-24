
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spAddEditAlanoClubPrices]
 @ProductName nvarchar(50),
 @MemberPrice money,
 @NonMemberPrice money,
 @Delete bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @idProd int; 
set	@idProd = (select ID from AlanoClubProducts where ProductName=@ProductName);
if(@Delete = 1)
begin
		if(EXISTS(select 1 from AlanoClubPrices  where  ProductID = @idProd))
		delete from AlanoClubPrices  where [ProductID] = @idProd;
end
else	if(not EXISTS(select 1 from AlanoClubPrices  where  ProductID = @idProd))
		begin
				INSERT INTO [dbo].[AlanoClubPrices]
				([ProductID]
				,[ClubPrice]
				,[ClubNonMemberPrice]
				,[DateAdded])
		VALUES
           (@idProd
           ,@MemberPrice
           ,@NonMemberPrice
          ,GETDATE());
		   end
else
begin
UPDATE [dbo].[AlanoClubPrices]
   SET [ClubPrice] = @MemberPrice
      ,[ClubNonMemberPrice] = @NonMemberPrice
      ,[DateAdded] = GETDATE()
	  where [ProductID] = @idProd;
end
END
 

 
