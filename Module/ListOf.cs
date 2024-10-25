using System.Diagnostics;
using TypeSSF.SSF_Structure;

namespace TypeSSF
{
    public class ListOf<T, TInstance>(TInstance upperInstance) : List<T>
    {
        public object UpperInstance = upperInstance;

        public new void Add(T Item, string classname)
        {
            base.Add(Item);
            var tmp = new StackTrace().GetFrame(1);
            if (tmp.ToString().Contains("<filename unknown>:0:0") == false)
                Debugger.Break();
            var ExpTypes = tmp.ToString();

            //Activator.CreateInstance(mod0[0]);
        }

        public new void Add(T item)
        {
            base.Add(item);
        }

        public ListOf<T, TInstance> Where(Func<T, bool> Select)
        {
            List<T> Tmp = new();
            base.ForEach(x => Tmp.Add(x));
            Tmp = Tmp.Where(x => Select(x)).ToList();
            ListOf<T, TInstance> values = new(upperInstance);
            values.AddRange(Tmp);
            return values;
        }
    }
}
