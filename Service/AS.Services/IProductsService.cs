using AS.Services.Models;
using System.Threading.Tasks;

namespace AS.Services
{
    public interface IProductsService
    {
        Task<bool> CreateProduct(ProductServiceModel productServiceModel, string stringFileName);

        Task<bool> EditProduct(ProductServiceModel productServiceModel, string stringFileName, string id);

        Task<bool> DeleteProduct(string id);

        Task<bool> DecreaseQuantity(string id, int amount);
    }
}
