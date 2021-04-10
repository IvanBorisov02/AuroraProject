using AS.Data;
using AS.Data.Models;
using AS.Services;
using AS.Services.Models;
using AS.Web.Models;
using AutoMapperConfiguration;
using AutoMapperTestConfiguration;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class ProductsServiceTests
    {
        [SetUp]
        public void Setup() {}

        [Test]
        public async Task CreateProductTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ProductCreateViewModel).Assembly.GetTypes(),
                typeof(ASUser).Assembly.GetTypes(),
                typeof(ProductServiceModel).Assembly.GetTypes());

            var options = new DbContextOptionsBuilder<ASDbContext>().UseSqlServer("Server=.\\SQLEXPRESS;Database=AShopDB;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            ASDbContext db = new ASDbContext(options);

            ProductsService productsService = new ProductsService(db);

            ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel()
            {
                Category = "Jacket",
                Description = "Testingg",
                GenderType = "Man",
                Price = 50,
                Quantity = 60,
                Name = "testProduct123"
            };
            ProductServiceModel model = productCreateViewModel.To<ProductServiceModel>();

            var result = await productsService.CreateProduct(model, "");

            Assert.AreEqual(true, result);

        }

    }
}
