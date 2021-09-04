namespace MediumProject.EF_MediumProject_CodeFirst.Entities
{
    public class StoryTopic
    {
        public int StoryID { get; set; }
        public int TopicID { get; set; }

        public Story Story { get; set; }
        public Topic Topic { get; set; }
    }
}
