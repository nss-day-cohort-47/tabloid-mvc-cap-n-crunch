using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
           
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPosts()
        {
            int userId = GetCurrentUserProfileId();
            var myposts = _postRepository.GetAllPublishedPostsByUser( userId);
            return View(myposts);
        }
        //Need data for posts and tags
        //Tags has to be a list of tags associated with the post
        //Create a View model with post info and a list of Tags
        //Add the view model to the Details controller
        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            var tags = _tagRepository.GetTagsByPostId(id);

            PostTagDetailViewModel vm = new PostTagDetailViewModel
            {
                Post = post,
                Tags = tags
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            int currentUser = GetCurrentUserProfileId();
            Post post = _postRepository.GetPublishedPostById(id);
            if (post.UserProfileId == currentUser)
            {
                return View(post);
            }
            else
            {
                return Unauthorized();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        public IActionResult Edit(int id)
        {
            Post post= _postRepository.GetPublishedPostById(id);
            int userId = GetCurrentUserProfileId();
            if (post == null || userId != post.UserProfileId)
            {
                return NotFound();
            }
            return View(post);
        }
        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Post post)
        {
            //try
            //{
                post.UserProfileId = GetCurrentUserProfileId();
                _postRepository.UpdatePost(post);
                return RedirectToAction("Index");
            //}
            //catch (Exception ex)
            //{
            //    return View(post);
            //}
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        // Get 
        public IActionResult AddTagToPost(int id)
        {
            var tags = _tagRepository.GetAllTags();
            PostTagFormViewModel vm = new PostTagFormViewModel()
            {
                postId = id,
                TagOptions = tags,
                TagIds = new List<int>()
            };
            return View(vm);
        }

        [HttpPost]
        //POST
        public IActionResult AddTagToPost(int id, List<int> tagIds )
        {
             var postTagIds = _postRepository.GetPostTagsByPostId(id);
            List<int> tags = new List<int>();
            foreach (var postTagId in postTagIds)
            {
                tags.Add(postTagId.TagId);
            
            }
            foreach (var tagId in tagIds)
            {
                if (tags.Contains(tagId))
                {
                    continue;
                }
                else 
                { 
                    _postRepository.AddPostTag(id, tagId);

                }
                
            }
            //int userId = GetCurrentUserProfileId();
            //var post = _postRepository.GetUserPostById(id, userId);
            //if (post == null)
            //{
            //    return NotFound();
            //}
            return RedirectToAction($"Details", new {id});
           

        }

    }
}
//GET
//list of all tags(object)-TagOptions
//list of int -Tags to add (empty)- TagIdsToAdd
//List of int -tags to delete (empty)- TagIdsToRemove
//list of PostTag -tags have already been selected -TagsToPost