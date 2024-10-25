using System.Data;
using TypeSSF.Collections;
using TypeSSF.SSF_Structure;

namespace TypeSSF
{
    public static class Converter
    {
        public static DataTable SSF_Table_To_DataTable(SSF_Table table)
        {
            DataTable dt = new();

            table.Columns.ForEach(x => dt.Columns.Add(x.Name, TypesDict.GetKeyFromValue(x.ColumnType)));

            for (int r = 0; r < table.Rows.Count; r++)
            {
                EntriesList list = new(new SSF_Row());
                for (int e = 0; e < table.Rows[r].Entries.Count(); e++)
                {
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        SSF_Entry ent = table.Rows[r].Entries.Where(x => x.ColumnName == table.Columns[c].Name).First();
                        if (list.Contains(ent) == false)
                            list.Add(ent);
                    }
                }
                object[] row = list.Select(x => x.Return()).ToArray();
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
