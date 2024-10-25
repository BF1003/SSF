using System.Numerics;
using TypeSSF.Collections;

namespace TypeSSF.SSF_Structure
{
    public class SSF_Row
    {
        internal SSF_Table UpperInstance { get; set; }
        public BigInteger ID { get; set; } = 0;
        public EntriesList Entries { get; set; }

        public SSF_Row()
        {
            Entries = new(this);
        }

        public SSF_Row(BigInteger ID)
        {
            this.ID = ID;
            Entries = new(this);
        }
    }
}
