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
            return View(post);
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
            Post post = _postRepository.GetPublishedPostById(id);
            if (post.UserProfileId == GetCurrentUserProfileId())
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
                TagOptions = tags
            };
            return View(vm);
        }

        [HttpPost]

        public IActionResult AddTagToPost(int postId, List<int> tagIds )
        {
            foreach (var tagId in tagIds)
            {
                _postRepository.AddPostTag(postId, tagId);
                
            }
            return View("Index");
           

        }

    }
}
