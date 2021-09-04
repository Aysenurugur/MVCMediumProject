using MediumProject.EF_MediumProject_CodeFirst.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediumProject.Models.Model_Repository.Abstract
{
    public interface ITopicVMRepository
    {
        List<TopicVM> ConvertTopicListToTopicModelList(List<Topic> topics);
        TopicVM ConvertTopicToTopicModel(Topic topic);
        List<Topic> ConvertTopicModelListToTopicList(List<int> topicVMs);
        List<Topic> ConvertTopicModelListToTopicList(List<TopicVM> topicVMs);
    }
}
