using System;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Extensions
{
    public static class LisaaKayttaja
    {
        public static void LisaaUusiKayttaja()
        {
            Console.Write("Syötä käyttäjän etunimi: ");
            var firstName = Console.ReadLine();

            Console.Write("Syötä käyttäjän sukunimi: ");
            var lastName = Console.ReadLine();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Etunimi ja sukunimi ovat pakollisia.");
                return;
            }

            var optionsBuilder = new DbContextOptionsBuilder<BackendDbContext>();
            optionsBuilder.UseSqlite("Data Source=../BackEnd/backend.db");

            using (var context = new BackendDbContext(optionsBuilder.Options))
            {
                var user = new User { FirstName = firstName, LastName = lastName };
                if (context.Users != null)
                {
                    context.Users.Add(user);
                }
                else
                {
                    Console.WriteLine("Error: Users DbSet is null.");
                    return;
                }
                context.SaveChanges();
            }

            Console.WriteLine("Käyttäjä lisätty onnistuneesti.");
        }
    }
}