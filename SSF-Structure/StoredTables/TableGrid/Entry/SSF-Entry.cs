using System.Numerics;
using System.Xml.Serialization;

namespace TypeSSF.SSF_Structure
{
    public class SSF_Entry
    {
        internal SSF_Row UpperInstance { get; set; }

        private string _ColumnName { get; set; } = string.Empty;
        public string ColumnName 
        {
            get
            { return _ColumnName; }
            set
            {
                _ColumnName = value;                
            }
        }

        public BigInteger RowID { get; set; } = 0;

        private object _Value { get; set; } = string.Empty;

        public object Value
        {
            get
            { return _Value; }
            set
            {
                try
                {
                    if (ValType == Types.text)
                    {
                        if (value.GetType() != typeof(string))
                            value = ToSSFTypes.ToText(value);
                        else
                            value = value;
                    }
                    if (ValType == Types.number)
                    {
                        if (value.GetType() != typeof(int))
                            value = ToSSFTypes.ToNumber(value);
                        else
                            value = value;
                    }
                    if (ValType == Types.floating)
                    {
                        if (value.GetType() != typeof(float) || _Value.GetType() != typeof(double))
                            value = ToSSFTypes.ToFloating(value);
                        else
                            value = value;
                    }
                    if (ValType == Types.number)
                    {
                        if (value.GetType() != typeof(DateTime))
                            value = ToSSFTypes.ToDateTime(value);
                        else
                            _Value = value;
                    }
                }
                catch
                {
                    _Value = null;
                }
            }
        }

        public Types ValType { get; set; } = Types.text;

        public SSF_Entry()
        {
        }

        public SSF_Entry(string ColumnName, BigInteger RowID)
        {
            this.ColumnName = ColumnName;
            this.RowID = RowID;
        }

        public SSF_Entry(string ColumnName, BigInteger RowID, object Value)
        {
            this.ColumnName = ColumnName;
            this.RowID = RowID;
            this.Value = Value;
        }

        public SSF_Entry(string ColumnName, BigInteger RowID, Types ValType)
        {
            this.ColumnName = ColumnName;
            this.RowID = RowID;
            this.ValType = ValType;
        }

        public SSF_Entry(string ColumnName, BigInteger RowID, object Value, Types ValType)
        {
            this.ColumnName = ColumnName;
            this.RowID = RowID;
            this.Value = Value;
            this.ValType = ValType;
        }

        public void Serialize(SSF Storage, SSF_Table Table)
        {
            XmlSerializer serializer = new(typeof(SSF_Entry));
            File.Delete($"{Storage.Path.FullName}\\{Storage.Name}\\{Table.Name}\\Entries\\{RowID.ToString()}_T_{ColumnName}.xml");
            FileStream fs = File.Create($"{Storage.Path.FullName}\\{Storage.Name}\\{Table.Name}\\Entries\\{RowID.ToString()}_T_{ColumnName}.xml");
            serializer.Serialize(fs, this);
            fs.Close();
        }

        public static SSF_Entry Deserialize(SSF Storage, SSF_Table Table, BigInteger RowID, string ColumnName, bool TmpFolder = false)
        {

            XmlSerializer serializer = new(typeof(SSF_Entry));
            string Path = $"{Storage.Path.FullName}\\{(TmpFolder ? "_" : "")}{Storage.Name}\\{Table.Name}\\Entries\\{RowID.ToString()}_T_{ColumnName}.xml";
            FileStream fs = new(Path, FileMode.Open, FileAccess.Read);
            SSF_Entry entry = (SSF_Entry)(serializer.Deserialize(fs) ?? new());
            fs.Close();
            entry.ColumnName = ColumnName;
            entry.RowID = RowID;
            return entry;
        }

        public object Return()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null && this != null) return false;
            SSF_Entry entry = obj as SSF_Entry ?? new SSF_Entry("-1", -1);
            if (entry.ColumnName == "-1" && entry.RowID == -1) return false;
            if (entry.ColumnName == this.ColumnName && entry.RowID == this.RowID && entry.Value == this.Value
                && entry.ValType == this.ValType)
                return true;
            return base.Equals(obj);
        }
    }
}
