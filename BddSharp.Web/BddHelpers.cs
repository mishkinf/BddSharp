using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BddSharp.Web
{
    public class BddHelpers
    {
        public static void ClearDatabase(DbContext context)
        {
            context.Database.ExecuteSqlCommand("Exec sp_msforeachtable 'Alter Table ? NOCHECK Constraint all' " +
                                               "Exec sp_msforeachtable 'Delete From ?' " +
                                               "Exec sp_msforeachtable 'Alter Table ? With Check Check Constraint all'");
        }
    }
}
