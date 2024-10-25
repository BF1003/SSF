using TypeSSF.Collections;
using TypeSSF.SSF_Structure;

namespace TypeSSF.Queries
{
    public static class QueryTable
    {
        public static RowList GetRows(Func<SSF_Row, bool> Select, SSF_Table table)
        {

            RowList rows = table.Rows.Where(x => Select(x)).ToList();
            if (rows.Any())
                return rows;
            return new(table);
        }
        public static RowList GetRows(this SSF_Table table, Func<SSF_Row, bool> Select) => GetRows(Select, table);

        public static SSF_Row GetRow(Func<SSF_Row, bool> Select, SSF_Table table)
        {
            RowList rows = table.Rows.Where((x) => Select(x)).ToList();
            if (rows.Count() > 0)
                return rows.First();
            else
                return new();
        }
        public static SSF_Row GetRow(this SSF_Table table, Func<SSF_Row, bool> Select) => GetRow(Select, table);
        public static SSF_Row GetRow(this SSF_Table table, int RowID) => GetRow(x => x.ID == RowID, table);

        public static EntriesList GetEntries(Func<SSF_Entry, bool> Select, SSF_Table table)
        {
            EntriesList entries = new(new SSF_Row());
            foreach (var row in table.Rows)
            {
                List<SSF_Entry> elements = row.Entries.Where(x => Select(x)).ToList();
                if (elements.Any())
                    entries.AddRange(elements);
            }
            return entries;
        }
        public static EntriesList GetEntries(this SSF_Table table, Func<SSF_Entry, bool> Select) => GetEntries(Select, table);

        public static SSF_Entry GetEntry(Func<SSF_Entry, bool> Select, SSF_Table table)
        {
            foreach (SSF_Row row in table.Rows)
            {
                EntriesList elements = row.Entries.Where(x => Select(x)).ToList();
                if (elements.Any())
                    return elements.First();
            }

            return new();
        }


        public static EntriesList GetEntries(string WhereValue, SSF_Table table) => GetEntries(x => x.Value.ToString() == WhereValue, table);
        public static SSF_Entry GetEntry(string WhereValue, SSF_Table table)
            => GetEntry(x => x.Value.ToString() == WhereValue, table);
    }
}
