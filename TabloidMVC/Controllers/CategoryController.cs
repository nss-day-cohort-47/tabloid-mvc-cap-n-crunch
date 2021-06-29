using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        //        public IActionResult Details(int id)
        //        {
        //            var post = _postRepository.GetPublishedPostById(id);
        //            if (post == null)
        //            {
        //                int userId = GetCurrentUserProfileId();
        //                post = _postRepository.GetUserPostById(id, userId);
        //                if (post == null)
        //                {
        //                    return NotFound();
        //                }
        //            }
        //            return View(post);
        //        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {

                _categoryRepository.AddCategory(category);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        //GET
        public IActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);
            //if (isAdmin == true)
            //{
                return View(category);
            //}
            //else
            //{
            //    return Unauthorized();
            //}
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }

        //        public IActionResult Edit(int id)
        //        {
        //            Post post = _postRepository.GetPublishedPostById(id);
        //            int userId = GetCurrentUserProfileId();
        //            if (post == null || userId != post.UserProfileId)
        //            {
        //                return NotFound();
        //            }
        //            return View(post);
        //        }
        //        // POST: DogsController/Edit/5
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult Edit(int id, Post post)
        //        {
        //            //try
        //            //{
        //            post.UserProfileId = GetCurrentUserProfileId();
        //            _postRepository.UpdatePost(post);
        //            return RedirectToAction("Index");
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    return View(post);
        //            //}
        //        }

        //        private int GetCurrentUserProfileId()
        //        {
        //            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            return int.Parse(id);
        //        }
        //    }
        //}
    }
}
