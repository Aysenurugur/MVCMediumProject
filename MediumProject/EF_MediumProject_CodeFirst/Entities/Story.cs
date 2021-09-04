using System;
using System.Collections.Generic;

namespace MediumProject.EF_MediumProject_CodeFirst.Entities
{
    public class Story
    {
        public int StoryID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public short ClickCount { get; set; }
        public short ReadTimeInMinutes { get; set; }
        public User User { get; set; }
        public ICollection<StoryTopic> StoryTopics { get; set; }
    }
}
