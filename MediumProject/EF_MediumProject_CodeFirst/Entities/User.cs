using System;
using System.Collections.Generic;

namespace MediumProject.EF_MediumProject_CodeFirst.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<Story> Stories { get; set; }
        public ICollection<UserTopic> UserTopics { get; set; }
    }
}
