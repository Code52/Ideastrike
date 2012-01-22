using System.Collections.ObjectModel;
using System.Linq;
using Ideastrike.Nancy.Models.Repositories;

namespace Ideastrike.Nancy.Models.Repositories
{
    public class StatusRepository : GenericRepository<IdeastrikeContext, Status>, IStatusRepository
    {
    }
}