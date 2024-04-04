using RumbApp.Data;
using RumbApp.Data.Providers;
using RumbApp.Models.Domain;
using RumbApp.Models.Domain.Comments;
using RumbApp.Models.Requests.Comment;
using RumbApp.Models.Requests.Comments;
using RumbApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace RumbApp.Services
{
    public class CommentService : ICommentService
	{
		IDataProvider _data = null;
		ILookUpService _lookUp = null;

		public CommentService(IDataProvider data, ILookUpService lookUp)
		{
			_data = data;
			_lookUp = lookUp;
		}
		public int Add(CommentAddRequest model, int userId)
		{
			int id = 0;
			string procName = "[dbo].[Comments_Insert]";

            _data.ExecuteNonQuery(procName,
				inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@CreatedBy", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                }, returnParameters: delegate (SqlParameterCollection returncol)
				{
					object oId = returncol["@Id"].Value;

					int.TryParse(oId.ToString(), out id);
				});
			return id;
		}
        public void Update(CommentUpdateRequest model, int userId)
		{
			string procName = "[dbo].[Comments_Update]";
			_data.ExecuteNonQuery(procName,
				inputParamMapper: delegate (SqlParameterCollection col)
				{
                    AddCommonParams(model, col);
                    col.AddWithValue("@ModifiedBy", userId);
                    col.AddWithValue("@Id", model.Id);
				}, returnParameters: null);
        }
        public void Delete(int Id)
        {
            string procName = "[dbo].[Comments_Delete]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", Id);
                }, returnParameters: null);
        }
        public List<Comment> GetByEntityId(int EntityId)
		{
			string procName = "[dbo].[Comments_SelectByEntityId]";
            List<Comment> list = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
			{
				paramCollection.AddWithValue("@EntityId", EntityId);
			},singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Comment comment = MapSingleComment(reader);
                if (list == null)
                {
                    list = new List<Comment>();
                }
                list.Add(comment);
            });
			return list;

        }

        private Comment MapSingleComment(IDataReader reader)
        {
            Comment aComment = new Comment();
            int startingIndex = 0;

            aComment.Id = reader.GetSafeInt32(startingIndex++);
            aComment.Subject = reader.GetString(startingIndex++);
            aComment.Text = reader.GetString(startingIndex++);
            aComment.ParentId = reader.GetSafeInt32(startingIndex++);
            aComment.EntityType = _lookUp.MapSingleLookUp(reader, ref startingIndex);
            aComment.EntityId = reader.GetSafeInt32(startingIndex++);
            aComment.DateCreated = reader.GetDateTime(startingIndex++);
            aComment.DateModified = reader.GetDateTime(startingIndex++);
            aComment.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            aComment.IsDeleted = reader.GetSafeBool(startingIndex++);
            return aComment;
        }

        private static void AddCommonParams(CommentAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Subject", model.Subject);
            col.AddWithValue("@Text", model.Text);
            col.AddWithValue("@ParentId", model.ParentId);
            col.AddWithValue("@EntityTypeId", model.EntityTypeId);
            col.AddWithValue("@EntityId", model.EntityId);
            col.AddWithValue("@IsDeleted", model.IsDeleted);
        }
	}
}
