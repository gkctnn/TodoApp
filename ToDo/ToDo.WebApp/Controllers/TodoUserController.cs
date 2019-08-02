using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDo.Bussiness;
using ToDo.Bussiness.Results;
using ToDo.Entities;
using ToDo.WebApp.Filters;

namespace ToDo.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class TodoUserController : Controller
    {
        private readonly ITodoUserManager _todoUserManager;
        public TodoUserController(ITodoUserManager todoUserManager)
        {
            _todoUserManager = todoUserManager;
        }

        public ActionResult Index()
        {
            return View(_todoUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TodoUser todoUser = _todoUserManager.Find(x => x.Id == id.Value);

            if (todoUser == null)
            {
                return HttpNotFound();
            }

            return View(todoUser);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TodoUser todoUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<TodoUser> res = _todoUserManager.Insert(todoUser);

                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(todoUser);
                }

                return RedirectToAction("Index");
            }

            return View(todoUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TodoUser todoUser = _todoUserManager.Find(x => x.Id == id.Value);

            if (todoUser == null)
            {
                return HttpNotFound();
            }

            return View(todoUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TodoUser todoUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<TodoUser> res = _todoUserManager.Update(todoUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(todoUser);
                }

                return RedirectToAction("Index");
            }
            return View(todoUser);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TodoUser todoUser = _todoUserManager.Find(x => x.Id == id.Value);

            if (todoUser == null)
            {
                return HttpNotFound();
            }

            return View(todoUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TodoUser todoUser = _todoUserManager.Find(x => x.Id == id);
            _todoUserManager.Delete(todoUser);

            return RedirectToAction("Index");
        }
    }
}
