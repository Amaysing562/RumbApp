USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Comments_SelectByEntityId]    Script Date: 4/3/2024 5:10:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Adrean Mays>
-- Create date: <12/16/2023>
-- Description: <Proc for Selecting by EntityId on comments table>

-- Code Reviewer:
-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer:
-- Note:
-- =============================================


ALTER proc [dbo].[Comments_SelectByEntityId]

@EntityId int

as
/*

Declare @EntityId int = 1
Execute dbo.Comments_SelectByEntityId
@EntityId

*/

BEGIN


select 
		C.Id
		,Subject
		,Text
		,ParentId
		,EntityTypeId
 		,ET.Name
		,EntityId
		,C.DateCreated
		,C.DateModified
		,CreatedBy = dbo.fn_GetUserJSON(C.CreatedBy)
		
		,IsDeleted

		From dbo.Comments as C inner join dbo.EntityTypes as ET
		on C.EntityTypeId = ET.Id
		
		WHERE C.EntityId = @EntityId and c.IsDeleted = 0
END