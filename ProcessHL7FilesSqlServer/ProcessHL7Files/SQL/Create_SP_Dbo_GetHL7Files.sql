USE [HL7]
GO

/****** Object:  StoredProcedure [dbo].[GetHL7Files]    Script Date: 6/5/2019 3:19:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHL7Files]
	@Date_Of_Service_Year [nvarchar](4),
	@Date_Of_Service_Month [nvarchar](2),
	@Patient_ID [nvarchar](16),
	@Client_Code [nvarchar](16),
	@Requisition_Number [nvarchar](16),
	@Financial_Number [nvarchar](16)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if(@Patient_ID is null)
	begin
	SELECT [Date_Of_Service] as Date_Of_Service
      ,[Patient_ID] as Patient_ID
      ,[Client_Code] as Client_Code
      ,[Patient_Last_Name] as Patient_Last_Name
      ,[Patient_First_Name] as Patient_First_Name
      ,[Dr_Code] as Dr_Code
      ,[Dr_Name] as Dr_Name
      ,[Requisition_Number] as Requisition_Number
      ,[Financial_Number] as Financial_Number
  FROM [HL7].[dbo].[HL7Files]
  where CAST(year([Date_Of_Service]) as nvarchar(4)) = @Date_Of_Service_Year and CAST(MONTH([Date_Of_Service]) as  nvarchar(2)) =  @Date_Of_Service_Year
  and [Financial_Number] = @Financial_Number
  end
  else
  begin
  SELECT [Date_Of_Service] as Date_Of_Service
      ,[Patient_ID] as Patient_ID
      ,[Client_Code] as Client_Code
      ,[Patient_Last_Name] as Patient_Last_Name
      ,[Patient_First_Name] as Patient_First_Name
      ,[Dr_Code] as Dr_Code
      ,[Dr_Name] as Dr_Name
      ,[Requisition_Number] as Requisition_Number
      ,[Financial_Number] as Financial_Number
  FROM [HL7].[dbo].[HL7Files]
  where CAST(year([Date_Of_Service]) as nvarchar(4)) = @Date_Of_Service_Year and CAST(MONTH([Date_Of_Service]) as  nvarchar(2)) =  @Date_Of_Service_Year
  and [Client_Code] = @Client_Code and [Requisition_Number] = @Requisition_Number and [Patient_ID] = @Patient_ID
  end

END


GO


