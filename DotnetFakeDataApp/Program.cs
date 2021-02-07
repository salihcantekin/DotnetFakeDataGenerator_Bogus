using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Bogus;
using Bogus.DataSets;

namespace DotnetFakeDataApp
{
    public class Program
    {
        static void Main(string[] args)
        {

            Randomizer.Seed = new Random(8675309);

            var addressFaker = new Faker<Address>("tr")
                .RuleFor(i => i.City, i => i.Address.City())
                .RuleFor(i => i.StreetName, i => i.Address.StreetName())
                .RuleFor(i => i.ZipCode, i => i.Address.ZipCode());

            var userFaker = new Faker<User>("tr")
                .RuleFor(i => i.Address, addressFaker)
                .RuleFor(i => i.Age, i => i.Random.Int(18, 65))
                .RuleFor(i => i.FirstName, i => i.Person.FirstName)
                .RuleFor(i => i.LastName, i => i.Person.LastName)
                .RuleFor(i => i.UserName, (i, j) => i.Internet.UserName(j.FirstName, j.LastName))
                .RuleFor(i => i.Id, i => i.Random.Guid())
                .RuleFor(i => i.Gender, i => i.PickRandom<Gender>())
                .RuleFor(i => i.EmailAddress, i => i.Person.Email);


            var generatedObject = userFaker.Generate(3);

            var opt = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string valueAsJson = JsonSerializer.Serialize(generatedObject, opt);

            Console.WriteLine(valueAsJson);
        }
    }

    public enum Gender
    { 
        Male,
        Female
    }

    public class User
    {
        public Guid Id { get; set; }

        public int Age { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String UserName { get; set; }

        public String EmailAddress { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; }

    }


    public class Address
    {
        public String City { get; set; }
        public String ZipCode { get; set; }

        public String StreetName { get; set; }
    }
}
