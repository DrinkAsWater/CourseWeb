using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Interface
{
  public interface IShopService
    {
        Task<bool> AddShopOrderAsync(Guid userId, Guid courseSceduleId);

        Task<IEnumerable<ShopOrderModel>> GetShopOrderListAsync(Guid userId);

        Task<bool> DeleteShopOrderAsync(Guid scheduleId);

    }
}
