using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {

        private readonly ITagRepository _tagRepository;
        private readonly IUserProfileRepository _userProfileRepository;


        public TagController(ITagRepository tagRepository, IUserProfileRepository userProfileRepository)
        {
            _tagRepository = tagRepository;
            _userProfileRepository = userProfileRepository;

        }
        // GET: TagController
        public ActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            UserProfile user = GetCurrentUser();
            if(user.UserTypeId == 1)
            {
                return View(tags);
            }
            else
            {
                return Unauthorized();
            }
        }

        //// GET: TagController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: TagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.AddTag(tag);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(tag);
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {

            Tag tag = _tagRepository.GetTagById(id);
            //int userId = GetCurrentUserProfileId();
            if (tag == null )
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagController/Delete/5
        public IActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        private UserProfile GetCurrentUser()
        {
            int currentUserId = GetCurrentUserProfileId();
            UserProfile user = _userProfileRepository.GetUserById(currentUserId);
            return user;
        }
    }
}

