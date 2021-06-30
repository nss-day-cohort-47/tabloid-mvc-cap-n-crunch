using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    {
        public int PostId { get; set; }
        public Comment Comment { get; set; }
        public List<Comment> CommentList { get; set; }
    }
}
