
public enum Types
{
    text = 0,
    number = 1,
    floating = 2,
    datetime = 3,
}

public static class TypesDict
{
    private static Dictionary<Type, Types> _TypesDictionary = new Dictionary<Type, Types>();
    public static Dictionary<Type, Types> TypesDictionary
    {
        get
        {
            if (_TypesDictionary.Count == 0)
            {
                _TypesDictionary.Add(typeof(string), Types.text);
                _TypesDictionary.Add(typeof(int), Types.number);
                _TypesDictionary.Add(typeof(float), Types.floating);
                _TypesDictionary.Add(typeof(double), Types.floating);
                _TypesDictionary.Add(typeof(DateTime), Types.datetime);
            }
            return _TypesDictionary; 
        }
        set
        { _TypesDictionary = value; }
    }

    public static Dictionary<Type, Types> GetTypesDictionaryByValue(Types Value)
    {
        return TypesDictionary.Where(x => x.Value == Value).ToDictionary();
    }

    public static Type GetKeyFromValue(Types Value)
    {
        return TypesDictionary.Where(x => x.Value == Value).First().Key;
    }
}