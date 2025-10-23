using Task_1.Models;
using Task_1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_1
{
    class Program
    {
        static void Main(string[] args)
        {
            AddSampleData();
            LoadAuthorsWithBooks();
            Console.WriteLine("finished!");
            Console.ReadKey();
        }

        static void AddSampleData()
        {
            using (var db = new ApplicationContext())
            {
                db.Books.RemoveRange(db.Books);
                db.Authors.RemoveRange(db.Authors);
                db.SaveChanges();

                var author = new Author
                {
                    First_name = "Leo",
                    Last_name = "Tolstoy",
                    Books = new List<Book>
                    {
                        new Book { Title = "War and Peace", Publishing_date = new DateTime(1869, 1, 1) },
                        new Book { Title = "Anna Karenina", Publishing_date = new DateTime(1877, 1, 1) },
                        new Book { Title = "Sunday", Publishing_date = new DateTime(1899, 1, 1) }
                    }
                };

                var author2 = new Author
                {
                    First_name = "Fedor",
                    Last_name = "Dostoevsky",
                    Books = new List<Book>
                    {
                        new Book { Title = "Crime and punishment", Publishing_date = new DateTime(1866, 1, 1) },
                        new Book { Title = "Idiot", Publishing_date = new DateTime(1869, 1, 1) }
                    }
                };

                db.Authors.AddRange(author, author2);
                db.SaveChanges();

                Console.WriteLine("Data added successfully!");
            }
        }

        static void LoadAuthorsWithBooks()
        {
            using (var db = new ApplicationContext())
            {
                var authorsWithBooks = db.Authors
                    .Include(a => a.Books)
                    .ToList();

                Console.WriteLine("\nAuthors with books");

                foreach (var author in authorsWithBooks)
                {
                    Console.WriteLine($"\nAuthor: {author.Full_name}");
                    Console.WriteLine($"Number of books: {author.Books.Count}");

                    foreach (var book in author.Books.OrderBy(b => b.Publishing_date))
                    {
                        Console.WriteLine($"  - {book.Title} ({book.Publishing_date:yyyy})");
                    }
                }

                Console.WriteLine("\nBooks published after 1870");

                var recentBooks = db.Authors
                    .Include(a => a.Books.Where(b => b.Publishing_date.Year > 1870))
                    .Where(a => a.Books.Any(b => b.Publishing_date.Year > 1870))
                    .ToList();

                foreach (var author in recentBooks)
                {
                    Console.WriteLine($"\nAuthor: {author.Full_name}");
                    foreach (var book in author.Books.Where(b => b.Publishing_date.Year > 1870))
                    {
                        Console.WriteLine($"  - {book.Title} ({book.Publishing_date:yyyy})");
                    }
                }
            }
        }
    }
}