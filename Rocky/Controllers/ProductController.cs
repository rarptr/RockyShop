using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocky.Controllers
{
    // Для получения доступа к контроллеру нужна роль админа
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        // Контекст базы данных
        private readonly ApplicationDbContext _db;
        // Доступ к wwwroot
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Извлекаем зависимость из контейнера зависимостей services
        // Иньекция зависимостей создает и передает обьект в конструктор
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // Жадная загрузка
            IEnumerable<Product> objList = _db.Product.Include(u => u.Category)
                                                      .Include(u => u.ApplicationType);

            //// Слишком много обращений к бд
            //// Каждому элементу списка продуктов присваивается первая категория, совпавшая по Id
            //foreach (var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            //};

            return View(objList);
        }

        // GET - UPSERT
        // Метод для создания и удаления
        public IActionResult Upsert(int? id)
        {
            //// Коллекция ключ:значение для выпадающего списка
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //// Передача коллекции во View 
            //// Значение живет в пределах одного http запроса
            //// ViewBag - оболочка поверх ViewData
            //ViewBag.CategoryDropDown = CategoryDropDown;

            //var product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                // this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);

            }
        }


        // POST - UPSERT
        [HttpPost]
        // Проверка в POST на действительность токена
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                // Получение отправленных файлов
                var files = HttpContext.Request.Form.Files;
                // Путь к папке wwwroot
                string webRootPath = _webHostEnvironment.WebRootPath;

                // Проверка операции создание/ редактирование
                if (productVM.Product.Id == 0)
                {
                    // Создание новой сущности

                    // Путь до папки с картинками
                    string upload = webRootPath + WC.ImagePath;
                    // Имя файла
                    string fileName = Guid.NewGuid().ToString();
                    // Расширение файла
                    string extension = Path.GetExtension(files[0].FileName);

                    // Пересоздаем файл в новое место
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    // Сохранение пути до изображения
                    productVM.Product.Image = fileName + extension;

                    // Добавляем товар
                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    // Обновление сущности

                    // Извлекаем сущность с бд по ID
                    // Контексту явно запрещено отслеживать результат запроса с помощью AsNoTracking,
                    // т.к. мы знаем, что изменения _db.Product не будет
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                    // Получено ли новое изображение для изменения
                    if (files.Count > 0)
                    {
                        // Путь до папки с картинками
                        string upload = webRootPath + WC.ImagePath;
                        // Имя файла
                        string fileName = Guid.NewGuid().ToString();
                        // Расширение файла
                        string extension = Path.GetExtension(files[0].FileName);

                        // Ссылка на старое фото
                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        // Если старое фото существует, то удаляем его
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        // Пересоздаем файл в новое место
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        // Ссылка на новое фото
                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    // Обновляем сущность в бд
                    _db.Product.Update(productVM.Product);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);
        }

        // GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //// Извлечение Product 
            //Product product = _db.Product.Find(id);
            //// Извлечение категории Product 
            //product.Category = _db.Category.Find(product.CategoryId);

            // Жадная загрузка
            // Модификация с загрузкой категории
            Product product = _db.Product.Include(u => u.Category)
                                         .Include(u => u.ApplicationType)
                                         .FirstOrDefault(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST - DELETE
        [HttpPost, ActionName("Delete")]
        // Проверка в POST на действительность токена
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            // Путь до папки с картинками
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            // Ссылка на фото
            var oldFile = Path.Combine(upload, obj.Image);

            // Если фото существует, то удаляем его
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            // Удаляем сущность из базы данных
            _db.Product.Remove(obj);
            _db.SaveChanges();
            // Перенаправление в метод Index этого же контроллера
            return RedirectToAction("Index");
        }
    }
}
