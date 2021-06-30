using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();

        public void AddTag(Tag tag);

        void DeleteTag(int id);

        Tag GetTagById(int id);
        void UpdateTag(Tag tag);
        List<Tag> GetTagsByPostId(int postId);
    }
}
