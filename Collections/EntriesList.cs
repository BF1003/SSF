using TypeSSF.SSF_Structure;

namespace TypeSSF.Collections
{
    public class EntriesList(SSF_Row instance) : List<SSF_Entry>
    {
        private SSF_Row Instance { get; set; } = instance;

        public new void Add(SSF_Entry entry)
        {
            entry.UpperInstance = Instance;
            base.Add(entry);
        }

        public new void AddRange(IEnumerable<SSF_Entry> entries)
        {
            foreach (var item in entries)
                item.UpperInstance = Instance;
            base.AddRange(entries);
        }

        public static new EntriesList ToList(IEnumerable<SSF_Entry> Values)
        {
            EntriesList list = new(new SSF_Row());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }

    public static class EntryConversion
    {
        public static EntriesList ToList(this IEnumerable<SSF_Entry> Values)
        {
            EntriesList list = new(new SSF_Row());
            foreach (var item in Values)
                list.Add(item);
            return list;
        }
    }
}
