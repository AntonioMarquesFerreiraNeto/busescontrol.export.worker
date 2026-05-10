using BusesControl.Export.Core.Entities;

namespace BusesControl.Export.Core.Interfaces
{
    public interface IFinancialRepository
    {
        Task<IEnumerable<FinancialModel>> GetAll();
    }
}
