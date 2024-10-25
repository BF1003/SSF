using TypeSSF.SSF_Structure;

namespace TypeSSF.Collections
{
    public class RowList(SSF_Table instance) : List<SSF_Row>
    {
        private SSF_Table Instance { get; set; } = instance;

        public new void Add(SSF_Row row)
        {
            row.UpperInstance = Instance;
            base.Add(row);
        }

        public new void AddRange(IEnumerable<SSF_Row> rows)
        {
            foreach (var item in rows)
                item.UpperInstance = Instance;
            base.AddRange(rows);
        }

        public static RowList ToList(IEnumerable<SSF_Row> Values)
        {
            RowList list = new(new SSF_Table());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }

    public static class RowConversion
    {
        public static RowList ToList(this IEnumerable<SSF_Row> Values)
        {
            RowList list = new(new SSF_Table());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }
}
