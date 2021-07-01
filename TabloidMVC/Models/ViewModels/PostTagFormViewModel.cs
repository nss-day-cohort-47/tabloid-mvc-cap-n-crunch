using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagFormViewModel
    {
        [DisplayName("Post")]
        public int postId { get; set; }
        public List<Tag> TagOptions { get; set; }

        public List<int> TagIdsToAdd { get; set; }

        public List<int> TagIdsToRemove { get; set; }

        public List<PostTag> TagsToPost { get; set; }
    }
}
