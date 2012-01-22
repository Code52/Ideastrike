using Devtalk.EF.CodeFirst;
using Ideastrike.Nancy.Models;

namespace Ideastrike.Nancy
{
    public class DatabaseInitializer : DontDropDbJustCreateTablesIfModelChanged<IdeastrikeContext>
    {
        

    }
}