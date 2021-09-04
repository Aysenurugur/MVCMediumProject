using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediumProject.Models
{
    public class UserWithStories
    {
        public UserWithStories()
        {
            Stories = new List<StoryVM>();
        }
        public UserVM User { get; set; }
        public List<StoryVM> Stories { get; set; }
    }
}
