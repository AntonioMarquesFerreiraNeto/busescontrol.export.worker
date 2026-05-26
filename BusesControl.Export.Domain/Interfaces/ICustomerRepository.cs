using BusesControl.Export.Core.Entities;

namespace BusesControl.Export.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerModel>> GetAll();
    }
}
