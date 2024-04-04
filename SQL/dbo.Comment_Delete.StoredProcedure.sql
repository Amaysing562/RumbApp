USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Comments_Delete]    Script Date: 4/3/2024 5:10:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Adrean Mays>
-- Create date: <12/16/2023>
-- Description: <Proc for Deleteing Comments table information, and updating IsDeleted>

-- Code Reviewer:
-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer:
-- Note:
-- =============================================

ALTER proc [dbo].[Comments_Delete]
		@Id int

as
/* --------------------Test code

Declare 
		@Id int = 3

Execute dbo.Comments_Delete
	@Id

*/

BEGIN

UPDATE dbo.Comments
	SET
		[IsDeleted] = 1

	WHERE Id = @Id

END