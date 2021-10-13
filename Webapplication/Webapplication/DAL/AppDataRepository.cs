using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.DAL
{
    public class AppDataRepository : IAppDataRepository
    {
        private readonly ApplicationDbContext _DB;

        public AppDataRepository(ApplicationDbContext DB)
        {
            _DB = DB;
        }


        //calls to db that manage the cruises goes here
    }
}
