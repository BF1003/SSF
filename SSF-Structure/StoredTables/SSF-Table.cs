using System.Runtime.CompilerServices;
using TypeSSF.Collections;

namespace TypeSSF.SSF_Structure
{
    public class SSF_Table : SSF
    {
        internal SSF UpperInstance { get; set; }
        public string? Name { get; set; } = null;
        public ColumnList Columns { get; set; }
        private RowList _Rows { get; set; }
        public RowList Rows
        {
            get
            { return _Rows; }
            set
            {                
                _Rows = value;
                //foreach (var col in Columns)
                //{
                //    foreach (var row in _Rows)
                //    {
                //        row.Entries.Where(x => x.ColumnName == col.Name).First().ValType = col.ColumnType;
                //    }
                //}
            }
        }

        public SSF_Table()
        {
            Columns = new ColumnList(this);
            Rows = new RowList(this);
        }

        public SSF_Table(string Name)
        {
            this.Name = Name;
            Columns = new ColumnList(this);
            Rows = new RowList(this);
        }

        public SSF_Table(ColumnList columns)
        {
            Columns = columns;
            Rows = new RowList(this);
        }

        public SSF_Table(RowList rows)
        {
            Rows = rows;
            Columns = new ColumnList(this);
        }

        public SSF_Table(ColumnList columns, RowList rows)
        {
            Columns = columns;
            Rows = rows;
        }
    }
}
