using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models.Model_Repository.Abstract;

namespace MediumProject.Models.Model_Repository.Concrete
{
    public class StoryVMRepository : IStoryVMRepository
    {
        public StoryVMRepository(IStoryRepository storyRepository, IUserVMRepository userVMRepository, IUserRepository userRepository)
        {
            this.storyRepository = storyRepository;
            this.userVMRepository = userVMRepository;
            this.userRepository = userRepository;
        }
        IStoryRepository storyRepository;
        IUserVMRepository userVMRepository;
        IUserRepository userRepository;
        public Story ConvertStoryModelToStory(StoryVM storyVM)
        {
            Story story = new Story()
            {
                StoryID = storyVM.StoryID,
                Title = storyVM.Title,
                Description = storyVM.Description,
                ReadTimeInMinutes = storyVM.ReadTimeInMinutes,
                UserID = storyVM.UserID
            };

            return story;
        }
        public StoryVM ConvertStoryToStoryModel(Story story)
        {
            story.User = userRepository.GetUserById(story.UserID);
            StoryVM storyVM = new StoryVM()
            {
                StoryID = story.StoryID,
                UserID = story.UserID,
                CreatedDate = story.CreatedDate,
                Description = story.Description,
                ReadTimeInMinutes = story.ReadTimeInMinutes,
                Title = story.Title,
                User = userVMRepository.ConvertUserToUserModel(story.User)
            };
            return storyVM;
        }
    }
}
