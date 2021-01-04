using Microsoft.AspNet.Identity;
using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SocialPlatform.Controllers
{
    public class GroupsController : Controller
    {
        // GET: Group
        [Authorize]
        public ActionResult Index(int? frommaybe)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            int from = frommaybe ?? 0;

            try {
                var groups = user.Groups.ToList().OrderByDescending(gr => gr.Members.Count());

                if (from < 0 || from > groups.Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                Group group_ret = null;
                if (from != 0)
                    group_ret = groups.ElementAt(from - 1);

                return View(group_ret);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
        }
        // GET: Group
        [Authorize]
        public ActionResult Explore(int? frommaybe)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int from = frommaybe ?? 0;
            
            try
            {
                var groups = db.Groups.ToList().OrderByDescending(gr => gr.Members.Count());

                if (from < 0 || from > groups.Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                Group group_ret = null;
                if (from != 0)
                    group_ret = groups.ElementAt(from - 1);

                return View(group_ret);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
        }

        [Authorize]
        public ActionResult New()
		{
            Group group = new Group();
            return View(group);
		}

        [Authorize]
        [HttpPost]
        public ActionResult New(Group group)
		{
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    group.User = user;
                    group.Members = new HashSet<ApplicationUser>();
                    group.Members.Add(user);

                    Wall group_wall = new Wall();
                    db.Walls.Add(group_wall);
                    db.SaveChanges();
                    group.Wall = group_wall;
                    db.Groups.Add(group);
                    db.SaveChanges();
                    TempData["message"] = "Grupul a fost creat!";
                }
                else
                    return View(group);
            }
            catch (Exception) {
                return View(group);
            }
            return RedirectToAction("Show", new { Id = group.GroupId });
        }

        [Authorize]
        public ActionResult Show(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Groups.Find(Id));
        }

        [Authorize]
        public ActionResult Edit(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                Group group = db.Groups.Find(Id);
                var user = db.Users.Find(User.Identity.GetUserId());
                if (group.UserId != user.Id)
                    throw new Exception();
                return View(group);
            }
            catch (Exception)
            {
                TempData["message"] = "Group doesn't exist, or insuficient rights!";
                return Index(0);
            }
        }

        [Authorize]
        [HttpPut]
        public ActionResult Edit(Group group)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                Group old_group = db.Groups.Find(group.GroupId);
                var user = db.Users.Find(User.Identity.GetUserId());
                if (old_group.UserId != user.Id)
                    throw new Exception();

                old_group.Name = group.Name;

                if (ModelState.IsValid)
                    db.SaveChanges();
                else
                    return View(group);

                TempData["message"] = "Group was successfully modified!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["message"] = "Group doesn't exist, or insuficient rights!";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Post)]
        public ActionResult Delete(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                Group group = db.Groups.Find(Id);
                var user = db.Users.Find(User.Identity.GetUserId());
                if (group.UserId != user.Id && !User.IsInRole("Administrator"))
                    throw new Exception();

                SocialWorker worker = new SocialWorker();
                worker.DeleteGroup(Id, ref db);
            }
            catch(Exception e)
            {
                TempData["message"] = "Group doesn't exist, or insuficient rights!";
                return Index(0);    
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Leave(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                Group group = db.Groups.Find(Id);
                string group_name = group.Name;
                var user = db.Users.Find(User.Identity.GetUserId());
                if (group.User == user)
                {
                    TempData["status"] = "Esti owner-ul grupului, nu poti iesi din propriul grup";
                    return RedirectToAction("Index");
                }
                if (!group.Members.Remove(user))
                {
                    TempData["status"] = "Nu faci parte din grupul " + group_name;
                    return RedirectToAction("Index");
                }

                db.SaveChanges();
                TempData["status"] = "Ai iesit din grupul " + group_name;
                return RedirectToAction("Index");
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["status"] = "Grup inexistent sau alta eroare";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Join(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                Group group = db.Groups.Find(Id);
                string group_name = group.Name;
                var user = db.Users.Find(User.Identity.GetUserId());
                if (group.Members.Contains(user))
                {
                    TempData["status"] = "Deja faci parte din grupul " + group_name;
                    return RedirectToAction("Index");
                }
                group.Members.Add(user);
                db.SaveChanges();

                TempData["status"] = "Ai iesit din grupul " + group_name;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["status"] = "Nu faci parte din grup sau grup inexistent";
                return RedirectToAction("Index");
            }
        }

    }
}
