using System;


namespace RumbApp.Models.Domain.Comments
{
    public class Comment
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public int ParentId { get; set; }
        public LookUp EntityType { get; set; }
        public int EntityId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
        public bool IsDeleted { get; set; }


    }
}
