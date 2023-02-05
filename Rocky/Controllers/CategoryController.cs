using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Data;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Utility;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    // Для получения доступа к контроллеру нужна роль админа
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        // Контекст базы данных
        private readonly ICategoryRepository _catRepo;

        // Извлекаем зависимость из контейнера зависимостей services
        // Иньекция зависимостей создает и передает обьект в конструктор
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRepo.GetAll();
            return View(objList);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View();
        }


        // POST - CREATE
        [HttpPost]
        // Проверка в POST на действительность токена
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            // Валидны ли правила определенные для модели
            if (ModelState.IsValid)
            {
                _catRepo.Add(obj);
                _catRepo.Save();
                // Перенаправление в метод Index этого же контроллера
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _catRepo.Find(id.GetValueOrDefault());
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
        public IActionResult Edit(Category obj)
        {
            // Валидны ли правила определенные для модели
            if (ModelState.IsValid)
            {
                _catRepo.Update(obj);
                _catRepo.Save();
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
            var obj = _catRepo.Find(id.GetValueOrDefault());
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
            var obj = _catRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _catRepo.Remove(obj);
            _catRepo.Save();
            // Перенаправление в метод Index этого же контроллера
            return RedirectToAction("Index");
        }
    }
}
