using System;

namespace CraigsList.Data
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
    }
}
