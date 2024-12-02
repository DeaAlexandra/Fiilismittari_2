using Microsoft.EntityFrameworkCore;
using BackEnd.Extensions;
using BackEnd.Models;

namespace Backend.Models
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nValitse toiminto:");
                Console.WriteLine("1. Lisää käyttäjä");
                Console.WriteLine("3. Lopeta");

                Console.Write("Valinta: ");
                var valinta = Console.ReadLine();

                switch (valinta)
                {
                    case "1":
                        LisaaKayttaja();
                        break;
                    case "3":
                        Console.WriteLine("Ohjelma lopetetaan.");
                        return;
                    default:
                        Console.WriteLine("Virheellinen valinta.");
                        break;
                }
            }
        }

        static void LisaaKayttaja()
        {
            Console.Write("Syötä käyttäjän nimi: ");
            var firstname = Console.ReadLine();

            Console.Write("Syötä käyttäjän sähköpostiosoite: ");
            var lastname = Console.ReadLine();

            using (var context = new BackendDbContext())
            {
                var user = new User { FirstName = firstname, LastName = lastname };
                BackEndDbContext.Users.Add(user);
                context.SaveChanges();
            }

            Console.WriteLine("Käyttäjä lisätty onnistuneesti.");
        }
    }
}