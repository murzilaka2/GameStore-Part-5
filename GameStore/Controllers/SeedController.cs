using GameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers
{
    public class SeedController : Controller
    {
        private readonly ApplicationContext _context;

        public SeedController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Count = _context.Products.Count();
            return View(_context.Products.Include(e => e.Category).OrderBy(e => e.Id).Take(20));
        }
        [HttpPost]
        public IActionResult CreateProductionData()
        {
            ClearData();
            _context.Categories.AddRange
                (
                    new Category
                    {
                        Name = "Шутеры",
                        Description = "Наделай шуму",
                        Products = new Product[]
                    {
                        new Product { Name = "WarZone", Description = "Мультиплатформенная компьютерная игра в жанре многопользовательского шутера от первого лица.",
                        PurchasePrice = 50, RetailPrice = 70},
                         new Product { Name = "DOOM Eternal", Description = "DOOM Eternal смогла объединить механики традиционного шутера с элементами слэшера.",
                        PurchasePrice = 76, RetailPrice = 90},
                          new Product { Name = "Battlefield V", Description = "В 2019 году Infinity Ward выпустила, пожалуй, самую топовую Call of Duty за последние несколько лет. ",
                        PurchasePrice = 100, RetailPrice = 120},
                           new Product { Name = "Wolfenstein 2", Description = "Bulletstorm стала лебединой песней студии People Can Fly. Это накачанный адреналином шутер с отмороженным наёмником в главной роли.",
                        PurchasePrice = 35, RetailPrice = 50},
                    }
                    },
                     new Category
                     {
                         Name = "Стратегии",
                         Description = "Думай чем можешь",
                         Products = new Product[]
                    {
                        new Product { Name = "Civilization 5", Description = "Пожалуй, лучшая современная часть «Цивилизации». В меру глубокая и понятная даже для новичка",
                        PurchasePrice = 50, RetailPrice = 70},
                         new Product { Name = "Age of Empires 2", Description = "Вторую часть AOE можно считать современной классикой стратегий в реальном времени. ",
                        PurchasePrice = 76, RetailPrice = 90},
                          new Product { Name = "Warcraft 3 ", Description = "История принца Артаса, который буквально продал душу ради своего народа, знакома миллионам игроков.",
                        PurchasePrice = 100, RetailPrice = 120},
                           new Product { Name = "SimCity 4 ", Description = "Четвёртую часть градостроительного симулятора от создателя The Sims Уилла Райта по праву можно считать одной из лучших игр в жанре.",
                        PurchasePrice = 35, RetailPrice = 50},
                    }
                     },
                      new Category
                      {
                          Name = "Фатинги",
                          Description = "Ударил - беги",
                          Products = new Product[]
                    {
                        new Product { Name = "Mortal Kombat 11", Description = "Самое полное издание Mortal Kombat 11. Погружайтесь в две кинематографические сюжетные кампании.",
                        PurchasePrice = 50, RetailPrice = 70},
                         new Product { Name = "Street Fighter V: Champion Edition", Description = "Последняя часть легендарной серии файтингов, с которой и зародился жанр, получила самое крупное обновление.",
                        PurchasePrice = 76, RetailPrice = 90},
                          new Product { Name = "SOULCALIBUR VI", Description = "Душа внутри пылает – и пылает она еще ярче и пуще прежнего. Серия возвращается к своим корням с легендарными поединками с оружием.",
                        PurchasePrice = 100, RetailPrice = 120},
                           new Product { Name = "TEKKEN 7", Description = "И вот, спустя 20 лет, война между членами клана Мисима, наконец, достигла своего апогея.",
                        PurchasePrice = 35, RetailPrice = 50},
                    }
                      }

                );
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult CreateSeedData(int count)
        {
            ClearData();
            if (count > 0)
            {
                _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
                _context.Database.ExecuteSqlRaw("DROP PROCEDURE IF EXISTS CreateSeedData");
                _context.Database.ExecuteSqlRaw($@"
                CREATE PROCEDURE CreateSeedData 
                @RowCount decimal
                AS
                BEGIN
                SET NOCOUNT ON
                DECLARE @i INT = 0;
                DECLARE @catId INT;
                DECLARE @CatCount INT = @RowCount / 10;
                DECLARE @pprice DECIMAL(5,2);
                DECLARE @rprice DECIMAL(5,2);
                BEGIN TRANSACTION
                WHILE @i < @CatCount
                BEGIN 
                INSERT INTO Categories (Name,Description)
                VALUES (CONCAT('Category-',@i),
                'Test Data Category');
                SET @catId = SCOPE_IDENTITY();
                DECLARE @j INT = 1;
                WHILE @j <= 10
                BEGIN
                SET @pprice = RAND() * (500-5+1);
                SET @rprice = (RAND() * @pprice) + @pprice;
                INSERT INTO Products (Name,CategoryId,PurchasePrice,RetailPrice,Description)
                VALUES (CONCAT('Product',@i,'-',@j),@catId,@pprice,@rprice,CONCAT('Description',@i,'-',@j))
                SET @j = @j + 1
                END
                SET @i = @i + 1
                END
                COMMIT
                END");
                _context.Database.BeginTransaction();
                _context.Database.ExecuteSqlRaw($"EXEC CreateSeedData @RowCount = {count}");
                _context.Database.CommitTransaction();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult ClearData()
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
            _context.Database.BeginTransaction();
            _context.Database.ExecuteSqlRaw("DELETE FROM Orders");
            _context.Database.ExecuteSqlRaw("DELETE FROM Categories");
            _context.Database.CommitTransaction();
            return RedirectToAction(nameof(Index));
        }
    }
}
