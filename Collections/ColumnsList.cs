using TypeSSF.SSF_Structure;

namespace TypeSSF.Collections
{
    public class ColumnList(SSF_Table instance) : List<SSF_Column>
    {
        private SSF_Table Instance { get; set; } = instance;

        public new void Add(SSF_Column column)
        {
            column.UpperInstance = Instance;
            base.Add(column);
        }

        public new void AddRange(IEnumerable<SSF_Column> columns)
        {
            foreach (var item in columns)
                item.UpperInstance = Instance;
            base.AddRange(columns);
        }

        public new ColumnList ToList(IEnumerable<SSF_Column> Values)
        {
            ColumnList list = new(new SSF_Table());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }

    public static class ColumnConversion
    {
        public static ColumnList ToList(this IEnumerable<SSF_Column> Values)
        {
            ColumnList list = new(new SSF_Table());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }
}
