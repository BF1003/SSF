using System.IO.Compression;
using System.Numerics;
using TypeSSF.Collections;
using TypeSSF.SSF_Structure;

namespace TypeSSF
{
    public class SSF
    {
        public DirectoryInfo Path { get; set; }
        public string Name { get; set; }

        public TableList Tables { get; set; }       

        public SSF()
        {
            Tables = new(this);
        }

        public SSF(TableList Tables)
        {
            this.Tables = Tables;
        }

        public SSF(TableList Tables, string Name)
        {
            this.Tables = Tables;
            this.Name = Name;
        }

        public SSF(DirectoryInfo Path)
        {
            this.Path = Path;
            Tables = new(this);
        }

        public SSF(DirectoryInfo Path, string Name)
        {
            this.Path = Path;
            this.Name = Name;
            Tables = new(this);
        }

        public SSF(TableList Tables, DirectoryInfo Path)
        {
            this.Tables = Tables;
            this.Path = Path;
        }

        public bool SaveAsSSF()
        {
            if (Path == null || Name == null)
                return false;

            DirectoryInfo TmpDir = Directory.CreateDirectory(Path.FullName + "\\" + Name);
            List<DirectoryInfo> TmpTables = new();
            List<DirectoryInfo> TmpColumns = new();
            List<DirectoryInfo> TmpRows = new();
            foreach (SSF_Table Table in Tables)
            {
                if (Directory.Exists(TmpDir.FullName + "\\" + Table.Name))
                    Directory.Delete(TmpDir.FullName + "\\" + Table.Name, true);

                TmpTables.Add(TmpDir.CreateSubdirectory(Table.Name));

                TmpColumns.Add(Directory.CreateDirectory(TmpDir.FullName + "\\" + Table.Name + "\\Columns"));
                foreach (SSF_Column Column in Table.Columns)
                {
                    Column.Serialize(this, Table);
                }

                TmpRows.Add(Directory.CreateDirectory(TmpDir.FullName + "\\" + Table.Name + "\\Entries"));
                foreach (SSF_Row Row in Table.Rows)
                {
                    foreach (SSF_Entry Entry in Row.Entries)
                    {
                        Entry.Serialize(this, Table);
                    }
                }
            }

            File.Delete(Path.FullName + "\\" + Name + ".zip");
            ZipFile.CreateFromDirectory(Path.FullName + "\\" + Name, Path.FullName + "\\" + Name + ".zip");
            Directory.Delete(Path.FullName + "\\" + Name, true);
            if (Directory.Exists((Path.FullName + "\\" + Name + ".zip").Replace(".zip", ".ssf")))
                Directory.Delete((Path.FullName + "\\" + Name + ".zip").Replace(".zip", ".ssf"));
            if (File.Exists((Path.FullName + "\\" + Name + ".zip").Replace(".zip", ".ssf")))
                File.Delete((Path.FullName + "\\" + Name + ".zip").Replace(".zip", ".ssf"));
            File.Move(Path.FullName + "\\" + Name + ".zip", (Path.FullName + "\\" + Name + ".zip").Replace(".zip", ".ssf"));

            return true;
        }

        public static SSF OpenSSF(DirectoryInfo Path, string Name)
        {
            string FileName = Path.FullName + "\\_" + Name;
            using (ClearTmpFile Clear = new(FileName))
            {

                SSF Storage = new();
                Storage.Name = Name;
                Storage.Path = Path;


                try
                {
                    File.Delete(FileName + ".ssf");
                    File.Delete(FileName + ".zip");
                    if (Directory.Exists(FileName))
                        Directory.Delete(FileName, true);
                    File.Copy(Path.FullName + "\\" + Name + ".ssf", FileName + ".ssf");
                    File.Move(FileName + ".ssf", FileName + ".zip");
                    ZipFile.ExtractToDirectory(FileName + ".zip", FileName);
                    File.Delete(FileName + ".zip");

                    foreach (string tablePath in Directory.GetDirectories(FileName))
                    {
                        string table = tablePath.Split("\\")[tablePath.Split("\\").Length - 1];

                        if (Storage.Tables.Any(t => t.Name == table))
                            throw new Exception("Duplicate TableNames found: " + table);
                        Storage.Tables.Add(new SSF_Table(table));

                        SSF_Table Tab = Storage.Tables.Where(x => x.Name == table).First();

                        foreach (string col in Directory.GetFiles(tablePath + "\\Columns"))
                        {
                            string colu = col.Split("\\")[col.Split("\\").Length - 1].Replace(".xml", "");
                            SSF_Column column = SSF_Column.Deserialize(Storage, Tab, colu, true);
                            Tab.Columns.Add(column);
                        }

                        int MaxRowID = 0;
                        foreach (string tmp in Directory.GetFiles(tablePath + "\\Entries"))
                        {
                            string Entry = tmp.Split("\\")[tmp.Split("\\").Length - 1].Replace(".xml", "");
                            if (Convert.ToInt32(Entry.Split("_T_")[0]) > MaxRowID)
                                MaxRowID = Convert.ToInt32(Entry.Split("_T_")[0]);
                        }
                        for (int i = 0; i < MaxRowID; i++)
                        {
                            Tab.Rows.Add(new SSF_Row(i));
                        }


                        List<string> Entries = Directory.GetFiles(tablePath + "\\Entries").ToList();
                        for (int i = 0; i < Entries.Count; i++)
                        {
                            try
                            {
                                string entry = Entries[i].Split("\\")[Entries[i].Split("\\").Length - 1].Replace(".xml", "");
                                BigInteger RowID = Convert.ToInt64(entry.Split("_T_")[0]);
                                string ColumnName = entry.Split("_T_")[1];

                                SSF_Entry Deserialized = SSF_Entry.Deserialize(Storage, Tab, RowID, ColumnName, true);
                                //if (Deserialized.Value.ToString() == "Cell010")
                                //Debugger.Break();


                                if (Tab.Rows.Any(x => x.ID == RowID))
                                    Tab.Rows.Where(x => x.ID == RowID).First().Entries.Add(Deserialized);
                                else
                                {
                                    continue;
                                    throw new Exception();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }

                        //List<SSF_Row> SortedRows = new();
                        //foreach (SSF_Row row in Tab.Rows)
                        //{
                        //    List<SSF_Entry> RowEntries = row.Entries.OrderBy(x => x.ColumnName).ToList();
                        //    SortedRows.Add(row);
                        //    SortedRows[SortedRows.Count - 1].Entries = RowEntries;
                        //}

                        RowList NewRows = new(Tab);
                        int counter = 0;
                        foreach (SSF_Row row in Tab.Rows)
                        {
                            EntriesList NewEntries = new(row);
                            foreach (SSF_Column col in Tab.Columns)
                            {
                                NewEntries.Add(row.Entries.Where(x => x.ColumnName == col.Name).First());
                            }
                            NewRows.Add(row);
                            NewRows[counter].Entries = 
                                NewEntries;
                            counter++;
                        }
                        counter = 0;

                        //foreach (string ent in Directory.GetFiles(tablePath + "\\Entries").OrderBy(x => x).ToList())
                        //{
                        //    string entry = ent.Split("\\")[ent.Split("\\").Length - 1].Replace(".xml", "");
                        //    BigInteger RowID = Convert.ToInt64(entry.Split("_T_")[0]);
                        //    BigInteger ColumnID = Convert.ToInt64(entry.Split("_T_")[1]);

                        //    SSF_Entry DeEntry = SSF_Entry.Deserialize(Storage, Storage.Tables.Where(x => x.Name == table).First(), RowID, ColumnID);

                        //    SSF_Table tab = Storage.Tables.Where(x => x.Name == table).First();
                        //    tab.Rows.Where(x => x.ID == RowID).First().Entries.Add(DeEntry);
                        //    //Storage.Tables.Where(x => x.Name == table).First().Rows.Where(x => x.ID == RowID).First().Entries.Add(DeEntry);

                        //}
                    }

                    File.Delete(FileName + ".zip");
                    ZipFile.CreateFromDirectory(FileName, FileName + ".zip");
                    Directory.Delete(FileName, true);
                    File.Delete((FileName + ".zip").Replace(".zip", ".ssf"));
                    File.Move(FileName + ".zip", (FileName + ".zip").Replace(".zip", ".ssf"));

                    return Storage;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new();
                }
            }
        }
    }
}