using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public static class LisaaKayttaja
    {
        public static async Task LisaaUusiKayttaja(UserManager<IdentityUser> userManager)
        {
            Console.Write("Anna käyttäjän etunimi: ");
            var firstName = Console.ReadLine();

            Console.Write("Anna käyttäjän sukunimi: ");
            var lastName = Console.ReadLine();

            var user = new IdentityUser
            {
                UserName = firstName + "." + lastName,
                Email = firstName + "." + lastName + "@example.com"
            };

            var result = await userManager.CreateAsync(user, "DefaultPassword123!");

            if (result.Succeeded)
            {
                Console.WriteLine("Käyttäjä lisätty onnistuneesti!");
            }
            else
            {
                Console.WriteLine("Virhe käyttäjän luomisessa.");
            }
        }
    }
}