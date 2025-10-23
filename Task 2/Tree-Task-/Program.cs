using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tree_Task_.Data;
using Tree_Task_.Models;

class Program
{
    private static ApplicationDbContext context = new ApplicationDbContext();

    static void Main()
    {
        // Инициализация базы данных
        InitializeDatabase();

        // Главное меню
        ShowMainMenu();
    }

    static void InitializeDatabase()
    {
        try
        {
            context.Database.EnsureCreated();
            Console.WriteLine("База данных инициализирована");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации БД: {ex.Message}");
        }
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("    МЕНЕДЖЕР ПРОДУКТОВ");
            Console.WriteLine("================================");
            Console.WriteLine("1. Показать все продукты");
            Console.WriteLine("2. Добавить новый продукт");
            Console.WriteLine("3. Редактировать продукт");
            Console.WriteLine("4. Удалить продукт");
            Console.WriteLine("5. Поиск продуктов");
            Console.WriteLine("6. Информация о базе данных");
            Console.WriteLine("0. Выход");
            Console.WriteLine("=================================");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllProducts();
                    break;
                case "2":
                    AddNewProduct();
                    break;
                case "3":
                    EditProduct();
                    break;
                case "4":
                    DeleteProduct();
                    break;
                case "5":
                    SearchProducts();
                    break;
                case "6":
                    ShowDatabaseInfo();
                    break;
                case "0":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowAllProducts()
    {
        Console.Clear();
        Console.WriteLine("СПИСОК ВСЕХ ПРОДУКТОВ");
        Console.WriteLine("====================================");

        try
        {
            var products = context.Products.OrderBy(p => p.Id).ToList();

            if (!products.Any())
            {
                Console.WriteLine("Продукты не найдены");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Название",-20} {"Цена",-10} {"Категория",-15}");
                Console.WriteLine(new string('-', 55));

                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id,-5} {product.Name,-20} {product.Price,-10:C} {product.Category,-15}");
                }

                Console.WriteLine($"\nВсего продуктов: {products.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        WaitForKeyPress();
    }

    static void AddNewProduct()
    {
        Console.Clear();
        Console.WriteLine("ДОБАВЛЕНИЕ НОВОГО ПРОДУКТА");
        Console.WriteLine("====================================");

        try
        {
            Console.Write("Введите название продукта: ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым!");
                WaitForKeyPress();
                return;
            }

            Console.Write("Введите цену продукта: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Неверный формат цены!");
                WaitForKeyPress();
                return;
            }

            Console.Write("Введите категорию продукта: ");
            var category = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(category))
            {
                category = "General";
            }

            var product = new Product
            {
                Name = name.Trim(),
                Price = price,
                Category = category.Trim()
            };

            context.Products.Add(product);
            context.SaveChanges();

            Console.WriteLine($"\nПродукт \"{product.Name}\" успешно добавлен с ID: {product.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
        }

        WaitForKeyPress();
    }

    static void EditProduct()
    {
        Console.Clear();
        Console.WriteLine("РЕДАКТИРОВАНИЕ ПРОДУКТА");
        Console.WriteLine("====================================");

        try
        {
            Console.Write("Введите ID продукта для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID!");
                WaitForKeyPress();
                return;
            }

            var product = context.Products.Find(id);
            if (product == null)
            {
                Console.WriteLine("Продукт с таким ID не найден!");
                WaitForKeyPress();
                return;
            }

            Console.WriteLine($"\nТекущие данные продукта:");
            Console.WriteLine($"Название: {product.Name}");
            Console.WriteLine($"Цена: {product.Price:C}");
            Console.WriteLine($"Категория: {product.Category}");
            Console.WriteLine("\nВведите новые данные (оставьте пустым чтобы не менять):");

            Console.Write("Новое название: ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                product.Name = newName.Trim();
            }

            Console.Write("Новая цена: ");
            var priceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal newPrice))
            {
                product.Price = newPrice;
            }

            Console.Write("Новая категория: ");
            var newCategory = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                product.Category = newCategory.Trim();
            }

            context.SaveChanges();
            Console.WriteLine($"\nПродукт с ID {product.Id} успешно обновлен!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при редактировании: {ex.Message}");
        }

        WaitForKeyPress();
    }

    static void DeleteProduct()
    {
        Console.Clear();
        Console.WriteLine("УДАЛЕНИЕ ПРОДУКТА");
        Console.WriteLine("====================================");

        try
        {
            Console.Write("Введите ID продукта для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID!");
                WaitForKeyPress();
                return;
            }

            var product = context.Products.Find(id);
            if (product == null)
            {
                Console.WriteLine("Продукт с таким ID не найден!");
                WaitForKeyPress();
                return;
            }

            Console.WriteLine($"\nВы уверены, что хотите удалить продукт:");
            Console.WriteLine($"ID: {product.Id}, Название: {product.Name}, Цена: {product.Price:C}");
            Console.Write("Подтвердите удаление (y/n): ");

            if (Console.ReadLine()?.ToLower() == "y")
            {
                context.Products.Remove(product);
                context.SaveChanges();
                Console.WriteLine("Продукт успешно удален!");
            }
            else
            {
                Console.WriteLine("Удаление отменено");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
        }

        WaitForKeyPress();
    }

    static void SearchProducts()
    {
        Console.Clear();
        Console.WriteLine("ПОИСК ПРОДУКТОВ");
        Console.WriteLine("===============================");
        Console.WriteLine("1. Поиск по названию");
        Console.WriteLine("2. Поиск по категории");
        Console.WriteLine("3. Поиск по диапазону цен");
        Console.Write("Выберите тип поиска: ");

        var choice = Console.ReadLine();

        try
        {
            IQueryable<Product> query = context.Products;

            switch (choice)
            {
                case "1":
                    Console.Write("Введите название для поиска: ");
                    var name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        query = query.Where(p => p.Name.Contains(name.Trim()));
                    }
                    break;

                case "2":
                    Console.Write("Введите категорию для поиска: ");
                    var category = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        query = query.Where(p => p.Category.Contains(category.Trim()));
                    }
                    break;

                case "3":
                    Console.Write("Введите минимальную цену: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal minPrice))
                    {
                        query = query.Where(p => p.Price >= minPrice);
                    }

                    Console.Write("Введите максимальную цену: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
                    {
                        query = query.Where(p => p.Price <= maxPrice);
                    }
                    break;

                default:
                    Console.WriteLine("Неверный выбор!");
                    WaitForKeyPress();
                    return;
            }

            var results = query.OrderBy(p => p.Id).ToList();

            Console.WriteLine($"\nРезультаты поиска ({results.Count} найдено):");
            if (results.Any())
            {
                Console.WriteLine($"{"ID",-5} {"Название",-20} {"Цена",-10} {"Категория",-15}");
                Console.WriteLine(new string('-', 55));

                foreach (var product in results)
                {
                    Console.WriteLine($"{product.Id,-5} {product.Name,-20} {product.Price,-10:C} {product.Category,-15}");
                }
            }
            else
            {
                Console.WriteLine("Продукты не найдены");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске: {ex.Message}");
        }

        WaitForKeyPress();
    }

    static void ShowDatabaseInfo()
    {
        Console.Clear();
        Console.WriteLine("ИНФОРМАЦИЯ О БАЗЕ ДАННЫХ");
        Console.WriteLine("================================");

        try
        {
            var productCount = context.Products.Count();
            Console.WriteLine($"Количество продуктов: {productCount}");

            if (productCount > 0)
            {
                var avgPrice = context.Products.Average(p => p.Price);
                var maxPrice = context.Products.Max(p => p.Price);
                var minPrice = context.Products.Min(p => p.Price);

                Console.WriteLine($"Средняя цена: {avgPrice:C}");
                Console.WriteLine($"Максимальная цена: {maxPrice:C}");
                Console.WriteLine($"Минимальная цена: {minPrice:C}");

                var categories = context.Products
                    .GroupBy(p => p.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToList();

                Console.WriteLine("\nКатегории:");
                foreach (var cat in categories)
                {
                    Console.WriteLine($"  {cat.Category}: {cat.Count} продуктов");
                }
            }

            // Информация о таблице
            Console.WriteLine("\nСтруктура таблицы Products:");
            var connection = context.Database.GetDbConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA table_info(Products)";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Колонка: {reader["name"]}, Тип: {reader["type"]}, PK: {reader["pk"]}");
            }
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        WaitForKeyPress();
    }

    static void WaitForKeyPress()
    {
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}