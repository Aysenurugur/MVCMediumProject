namespace MediumProject.EF_MediumProject_CodeFirst.Entities
{
    public class UserTopic
    {
        public int UserID { get; set; }
        public int TopicID { get; set; }

        public User User { get; set; }
        public Topic Topic { get; set; }
    }
}
