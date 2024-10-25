using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeSSF.SSF_Structure;

namespace TypeSSF.Queries
{
    public static class QuerySSF
    {
        public static List<SSF_Row> GetRows(Func<SSF_Row, bool> Select, SSF storage)
        {
            List<SSF_Row> rows = new();
            foreach (SSF_Table table in storage.Tables)
            {
                rows.AddRange(QueryTable.GetRows(Select, table));
            }
            return rows;
        }

        public static List<SSF_Row> GetRows(this SSF storage, Func<SSF_Row, bool> Select) => GetRows(Select, storage);
    }
}
