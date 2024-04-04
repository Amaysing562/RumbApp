using RumbApp.Models.Domain.Comments;
using RumbApp.Models.Requests.Comment;
using RumbApp.Models.Requests.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumbApp.Services.Interfaces
{
    public interface ICommentService
    {
        void Delete(int Id);
        void Update(CommentUpdateRequest model, int userId);
        int Add(CommentAddRequest model, int currentUserId);

        List<Comment> GetByEntityId(int EntityId);


    }
}