using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Data;
using Rocky_Models;
using Rocky_Utility;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    // Для получения доступа к контроллеру нужна роль админа
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        // Контекст базы данных
        private readonly ApplicationDbContext _db;

        // Извлекаем зависимость из контейнера зависимостей services
        // Иньекция зависимостей создает и передает обьект в конструктор
        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _db.ApplicationType;
            return View(objList);
        }

        // GET CREATE
        public IActionResult Create()
        {
            return View();
        }


        // POST CREATE
        [HttpPost]
        // Проверка в POST на действительность токена
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            _db.ApplicationType.Add(obj);
            _db.SaveChanges();
            // Перенаправление в метод Index этого же контроллера
            return RedirectToAction("Index");
        }

        // GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - EDIT
        [HttpPost]
        // Проверка в POST на действительность токена
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            // Валидны ли правила определенные для модели
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj);
                _db.SaveChanges();
                // Перенаправление в метод Index этого же контроллера
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        // GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - DELETE
        [HttpPost]
        // Проверка в POST на действительность токена
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(obj);
            _db.SaveChanges();
            // Перенаправление в метод Index этого же контроллера
            return RedirectToAction("Index");
        }
    }
}
