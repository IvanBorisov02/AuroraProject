using AS.Services.Models;
using System.Threading.Tasks;

namespace AS.Services
{
    public interface IProductsService
    {
        Task<bool> CreateProduct(ProductServiceModel productServiceModel, string stringFileName);
    }
}
