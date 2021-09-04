using System.Collections.Generic;

namespace MediumProject.EF_MediumProject_CodeFirst.Entities
{
    public class Topic
    {
        public int TopicID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<StoryTopic> StoryTopics { get; set; }
        public ICollection<UserTopic> UserTopics { get; set; }
    }
}
