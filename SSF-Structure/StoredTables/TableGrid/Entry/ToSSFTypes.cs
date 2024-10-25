using System;

namespace TypeSSF.SSF_Structure
{
    public static class ToSSFTypes
    {
        public static object ToText<T>(T value)
        {
            if (value == null || value is null)
                return null;
            Type typ = typeof(T);
            if (typ.IsPrimitive)
            {
                if (typ == typeof(object))
                {
                    if (typ == typeof(string))
                    {
                        return value.ToString() ?? "";
                    }
                    else
                    {
                        var Props = value.GetType().GetProperties();
                        string[] PropArray = new string[Props.Length];
                        for (int i = 0; i < Props.Length; i++)
                        {
                            PropArray[i] = (Props.GetValue(i) ?? "").ToString() ?? "";
                        }
                        return string.Join(", ", PropArray);
                    }
                }
                else
                    return value.ToString() ?? "";
            }
            else
            {
                if (typ == typeof(DateTime))
                    return Convert.ToDateTime(value).ToString();
                else
                {
                    var Props = value.GetType().GetProperties();
                    string[] PropArray = new string[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        PropArray[i] = (Props.GetValue(i) ?? "").ToString() ?? "";
                    }
                    return string.Join(", ", PropArray);
                }
            }
        }

        public static object ToNumber(object value)
        {
            try
            {
                string NewText = (string)ToText(value);
                try
                {
                    return Convert.ToInt32(NewText);
                }
                catch
                {
                    throw new ConvertException();
                }
            }
            catch
            {
                throw new ConvertException();
            }
        }

        public static object ToFloating(object value)
        {
            try
            {
                string NewText = (string)ToText(value);
                try
                {
                    return Convert.ToDouble(NewText);
                }
                catch
                {
                    throw new ConvertException();
                }
            }
            catch
            {
                throw new ConvertException();
            }
        }

        public static object ToDateTime(object value)
        {
            try
            {
                string NewText = (string)ToText(value);
                try
                {
                    return Convert.ToDateTime(NewText);
                }
                catch
                {
                    throw new ConvertException();
                }
            }
            catch
            {
                throw new ConvertException();
            }
        }
    }
}
