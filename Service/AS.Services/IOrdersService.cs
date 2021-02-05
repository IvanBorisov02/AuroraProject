using AS.Services.Models;
using AS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS.Services
{
    public interface IOrdersService
    {
        Task<bool> Order(string id, string ordererId, string stripeEmail, string stripeToken);

        Task <List<OrderServiceModel>> AllOrders();

    }
}
