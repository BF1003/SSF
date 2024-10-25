using System.Xml;
using System.Xml.Serialization;

namespace TypeSSF.SSF_Structure
{
    [XmlRoot]
    public class SSF_Column
    {
        internal SSF_Table UpperInstance { get; set; }

        [XmlElement]
        public string Name { get; set; } = string.Empty;

        [XmlElement]
        public string Title { get; set; } = string.Empty;

        [XmlElement]
        public Types ColumnType { get; set; } = Types.text;


        public SSF_Column()
        {
            
        }

        public SSF_Column(string Title)
        {
            this.Title = Title;
            this.Name = Title;
        }

        public SSF_Column(Types ColumnType)
        {
            this.ColumnType = ColumnType;
        }

        public SSF_Column(string Title, Types ColumnType)
        {
            this.Title = Title;
            this.Name = Title;
            this.ColumnType = ColumnType;
        }

        public SSF_Column(string Title, string Name, Types ColumnType)
        {
            this.Title = Title;
            this.Name = Title;
            this.ColumnType = ColumnType;
        }

        public void Serialize(SSF Storage, SSF_Table Table)
        {
            try
            {
                XmlSerializer serializer = new(typeof(SSF_Column));
                File.Delete($"{Storage.Path.FullName}\\{Storage.Name}\\{Table.Name}\\Columns\\{Title}.xml");
                FileStream fs = File.Create($"{Storage.Path.FullName}\\{Storage.Name}\\{Table.Name}\\Columns\\{Title}.xml");
                //XmlWriter writer = XmlWriter.Create($"{Storage.Path.FullName}\\{Storage.Name}\\{Table.Name}\\Columns\\{Title}.xml");
                serializer.Serialize(fs, this);
                //serializer.Serialize(writer, this);
                fs.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public static SSF_Column Deserialize(SSF Storage, SSF_Table Table, string Title, bool TmpFolder = false)
        {

            XmlSerializer serializer = new(typeof(SSF_Column));
            string Path = $"{Storage.Path.FullName}\\{(TmpFolder ? "_" : "")}{Storage.Name}\\{Table.Name}\\Columns\\{Title.ToString()}.xml";
            FileStream fs = new(Path, FileMode.Open, FileAccess.Read);
            SSF_Column col = (SSF_Column)(serializer.Deserialize(fs) ?? new());
            fs.Close();
            return col;
        }
    }
}
