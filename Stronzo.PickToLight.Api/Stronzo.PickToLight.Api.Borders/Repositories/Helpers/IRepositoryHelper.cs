using System.Data;

namespace Stronzo.PickToLight.Api.Borders.Repositories.Helpers;

public interface IRepositoryHelper
{
    IDbConnection GetConnection();
}
