using MvcProject2.Models;

namespace MvcProject2.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
    }
}