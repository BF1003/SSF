using TypeSSF.SSF_Structure;

namespace TypeSSF.Collections
{
    public class TableList(SSF instance) : List<SSF_Table>
    {
        private SSF Instance { get; set; } = instance;

        public new void Add(SSF_Table table)
        {
            table.UpperInstance = Instance;
            base.Add(table);
        }

        public new void AddRange(IEnumerable<SSF_Table> tables)
        {
            foreach (var item in tables)
                item.UpperInstance = Instance;
            base.AddRange(tables);
        }

        public new TableList ToList(IEnumerable<SSF_Table> Values)
        {
            TableList list = new(new SSF_Table());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }

    public static class TableConversion
    {
        public static TableList ToList(this IEnumerable<SSF_Table> Values)
        {
            TableList list = new(new SSF_Table());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }
}
