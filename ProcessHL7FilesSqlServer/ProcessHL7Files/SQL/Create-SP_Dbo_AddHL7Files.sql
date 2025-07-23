USE [HL7]
GO

/****** Object:  StoredProcedure [dbo].[AddHL7Files]    Script Date: 5/31/2019 8:42:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[AddHL7Files]
	@Date_Of_Service [date],
	@Patient_ID [nvarchar](16),
	@Client_Code [nvarchar](16),
	@Patient_Last_Name [nvarchar](50),
	@Patient_First_Name [nvarchar](50),
	@Dr_Code [nvarchar](16),
	@Dr_Name [nvarchar](50),
	@Requisition_Number [nvarchar](16),
	@Financial_Number [nvarchar](16)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--if(not exists(select * from [dbo].[HL7Files]
		--			where (([Date_Of_Service] =@Date_Of_Service) and ([Patient_ID]=@Patient_ID) and ([Client_Code] = @Client_Code)
			---				and ([Patient_Last_Name] = @Patient_Last_Name) and ([Patient_First_Name] = @Patient_First_Name) and
				--			([Dr_Code] = @Dr_Code) and ([Dr_Name] = @Dr_Name) and ([Requisition_Number] = @Requisition_Number) and ([Financial_Number] = @Financial_Number))
				if(not exists(select * from [dbo].[HL7Files]
					where (([Date_Of_Service] = @Date_Of_Service) and ([Patient_ID]=@Patient_ID))
	))
	begin
	INSERT INTO [dbo].[HL7Files]
	 ([Date_Of_Service]
           ,[Patient_ID]
           ,[Client_Code]
           ,[Patient_Last_Name]
           ,[Patient_First_Name]
           ,[Dr_Code]
           ,[Dr_Name]
           ,[Requisition_Number]
           ,[Financial_Number])
	    VALUES(
           @Date_Of_Service,
           @Patient_ID,
           @Client_Code,
           @Patient_Last_Name,
           @Patient_First_Name,
           @Dr_Code,
           @Dr_Name,
           @Requisition_Number,
           @Financial_Number)
	 end
	 else
		begin
			update [dbo].[HL7Files]
					set [Client_Code] =  @Client_Code
						,[Patient_Last_Name] = @Patient_Last_Name
						,[Patient_First_Name] = @Patient_First_Name
						,[Dr_Code] = @Dr_Code
						,[Dr_Name] = @Dr_Name
						,[Requisition_Number] = @Requisition_Number
						,[Financial_Number] = @Financial_Number
				where (([Date_Of_Service] = @Date_Of_Service) and ([Patient_ID]=@Patient_ID))
		end

END
GO


