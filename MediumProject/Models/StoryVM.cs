using System;
using System.ComponentModel.DataAnnotations;

namespace MediumProject.Models
{
    public class StoryVM
    {
        public int StoryID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "You have to give a title to your story.")]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You can't publish empty stories.")]
        public string Description { get; set; }

        public short ReadTimeInMinutes { get; set; }
        public DateTime CreatedDate { get; set; }

        public UserVM User { get; set; }
    }
}
