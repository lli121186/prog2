using Android.Content;
using Android.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prog2.Tests
{
    internal class AdresseRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options; return new AppDbContext(options);
        }
        [Fact]
        public async Task TestAdresseRepository_AddAdresse_Success()
        {            // Arrangevar context = GetDbContext();
            var repo = new AdresseRepository(context);
            var adresse = new Adresse { Vorname = "Max", Nachname = "Mustermann", Strasse = "Musterstrasse 1", PLZ = 8000 };

            // Actawait repo.AddAsync(adresse);
            var result = await repo.GetAllAsync();

            // Assert
            Assert.Contains(result, a => a.Vorname == "Max");
        }
    }
}

