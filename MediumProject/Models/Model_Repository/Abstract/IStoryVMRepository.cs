using MediumProject.EF_MediumProject_CodeFirst.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediumProject.Models.Model_Repository.Abstract
{
    public interface IStoryVMRepository
    {
        Story ConvertStoryModelToStory(StoryVM storyVM);
        StoryVM ConvertStoryToStoryModel(Story story);
    }
}
