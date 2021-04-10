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

        [Test]
        public async Task EditProductTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ProductCreateViewModel).Assembly.GetTypes(),
                typeof(ASUser).Assembly.GetTypes(),
                typeof(ProductServiceModel).Assembly.GetTypes());

            var options = new DbContextOptionsBuilder<ASDbContext>().UseSqlServer("Server=.\\SQLEXPRESS;Database=AShopDB;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            ASDbContext db = new ASDbContext(options);

            ProductsService productsService = new ProductsService(db);

            
            //Creating test product
            ProductCreateViewModel initialProduct = new ProductCreateViewModel()
            {
                Category = "Coat",
                Description = "asdasdasdas",
                GenderType = "Man",
                Price = 50,
                Quantity = 60,
                Name = "testProduct123"
            };
            ProductServiceModel model = initialProduct.To<ProductServiceModel>();

            await productsService.CreateProduct(model, "");
            var testProduct = await db.Products.Include(p => p.Category).Include(p => p.GenderType).FirstOrDefaultAsync(p => p.Name == initialProduct.Name);

            //initializing edit model
            ProductServiceModel changedProduct = new ProductServiceModel()
            {
                CategoryServiceModel = new CategoryServiceModel { Name = "T-shirt" },
                GenderTypeServiceModel = new GenderTypeServiceModel { Name = "Woman" },
                Description = "TestinggCHANGED",
                Price = 50,
                Quantity = 60,
                Name = "testProduct123CHANGED"
            };

            var result = await productsService.EditProduct(changedProduct, "", testProduct.Id);

            Assert.AreEqual(true, result);

        }

        [Test]
        public async Task DeleteProductTest()
        {

            AutoMapperConfig.RegisterMappings(typeof(ProductCreateViewModel).Assembly.GetTypes(),
                typeof(ASUser).Assembly.GetTypes(),
                typeof(ProductServiceModel).Assembly.GetTypes());

            var options = new DbContextOptionsBuilder<ASDbContext>().UseSqlServer("Server=.\\SQLEXPRESS;Database=AShopDB;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            ASDbContext db = new ASDbContext(options);

            ProductsService productsService = new ProductsService(db);


            //Creating test product
            ProductCreateViewModel initialProduct = new ProductCreateViewModel()
            {
                Category = "Coat",
                Description = "asdasdasdas",
                GenderType = "Man",
                Price = 50,
                Quantity = 60,
                Name = "testProduct123"
            };
            ProductServiceModel model = initialProduct.To<ProductServiceModel>();

            await productsService.CreateProduct(model, "");
            var testProduct = await db.Products.Include(p => p.Category).Include(p => p.GenderType).FirstOrDefaultAsync(p => p.Name == initialProduct.Name);

            var result = await productsService.DeleteProduct(testProduct.Id);

            Assert.AreEqual(true, result);

        }

    }
}
