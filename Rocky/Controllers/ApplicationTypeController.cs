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
    public class ApplicationTypeController : Controller
    {

        private readonly IApplicationTypeRepository _appTypeRepo;

        // Извлекаем зависимость из контейнера зависимостей services
        // Иньекция зависимостей создает и передает обьект в конструктор
        public ApplicationTypeController(IApplicationTypeRepository appTypeRepo)
        {
            _appTypeRepo = appTypeRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _appTypeRepo.GetAll();
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
            _appTypeRepo.Add(obj);
            _appTypeRepo.Save();
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
            var obj = _appTypeRepo.Find(id.GetValueOrDefault());
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
                _appTypeRepo.Update(obj);
                _appTypeRepo.Save();
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
            var obj = _appTypeRepo.Find(id.GetValueOrDefault());
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
            var obj = _appTypeRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _appTypeRepo.Remove(obj);
            _appTypeRepo.Save();
            // Перенаправление в метод Index этого же контроллера
            return RedirectToAction("Index");
        }
    }
}
