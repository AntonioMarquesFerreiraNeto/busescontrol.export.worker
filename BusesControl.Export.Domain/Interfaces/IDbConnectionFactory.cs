using System.Data;

namespace BusesControl.Export.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
