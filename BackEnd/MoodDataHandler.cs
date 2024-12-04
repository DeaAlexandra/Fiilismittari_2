using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public static class MoodDataHandler
    {
        public static async Task LisaaTietoja(BackendDbContext backendDbContext)
        {
            Console.Write("Anna käyttäjän nimi: ");
            var username = Console.ReadLine();

            // Hae käyttäjän ID BackendDbContextista
            var userId = await backendDbContext.Users
                .Where(Users => Users.FirstName + " " + Users.LastName == username)
                .Select(Users => Users.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("Käyttäjää ei löytynyt.");
                return;
            }

            Console.WriteLine($"Käyttäjän ID: {userId}");

            // Lisää tietoa backend-tietokantaan
            Console.Write("Anna mielialan arvo (1-7): ");
            if (int.TryParse(Console.ReadLine(), out int moodValue))
            {
                var moodDataService = new MoodDataService(backendDbContext);
                await moodDataService.SaveMoodValue(userId, moodValue);
                Console.WriteLine("Mielialadata tallennettu onnistuneesti!");
            }
            else
            {
                Console.WriteLine("Virheellinen arvo. Yritä uudelleen.");
            }
        }
    }
}