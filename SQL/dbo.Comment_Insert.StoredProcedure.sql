USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Comments_Insert]    Script Date: 4/3/2024 5:10:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Adrean Mays>
-- Create date: <12/15/2023>
-- Description: <Proc for inserting into comments table, and entity table>

-- Code Reviewer:
-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer:
-- Note:
-- =============================================


ALTER proc [dbo].[Comments_Insert]

				@Subject nvarchar(50)
				,@Text nvarchar(3000)
				,@ParentId int
				,@EntityTypeId int
				,@EntityId int
				,@CreatedBy int
				,@IsDeleted bit
				,@Id int OUTPUT

as
/* -----------------------Test Code

Declare 
		@Subject nvarchar(50) = 'new Subject'
		,@Text nvarchar(3000) = 'Cool place'
		, @ParentId int  = 2
		,@EntityTypeId int = 2
		,@EntityId int = 1
		
		,@CreatedBy int = 8
		,@IsDeleted bit = 0
		,@Id int = 0

Execute dbo.Comments_Insert
			@Subject
			,@Text
			,@ParentId
			,@EntityTypeId
			,@EntityId
			,@CreatedBy
			,@IsDeleted
			,@Id OUTPUT

		

*/ -----------------------

BEGIN

INSERT INTO dbo.Comments
				(Subject
				, Text
				, ParentId
				, EntityTypeId
				, EntityId
				, CreatedBy
				, IsDeleted)
		VALUES 
				(@Subject
				,@Text
				,@ParentId
				,@EntityTypeId
				,@EntityId
				,@CreatedBy
				,@IsDeleted)

				SET @Id = SCOPE_IDENTITY()

END