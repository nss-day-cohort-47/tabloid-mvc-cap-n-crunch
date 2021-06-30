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

        public List<int> TagIds { get; set; }
    }
}
