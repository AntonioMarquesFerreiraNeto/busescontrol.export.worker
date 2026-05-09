using BusesControl.Export.Core.Entities;

namespace BusesControl.Export.Core.Interfaces
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractModel>> GetAll();
    }
}
