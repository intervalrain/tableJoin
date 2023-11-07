using System;
using System.Data;

namespace JoinTest
{
	public static class DataTableExtension
	{
        public static DataTable AddColumn<T>(this DataTable dt, string name)
        {
            dt.Columns.Add(name, typeof(T));
            return dt;
        }
        public static DataTable AddRow(this DataTable dt, object[] objs)
        {
            var row = dt.NewRow();
            int n = dt.Columns.Count;
            if (objs.Length != n) throw new ArgumentOutOfRangeException();
            for (int i = 0; i < n; i++)
            {
                row[i] = objs[i];
            }
            dt.Rows.Add(row);
            return dt;
        }

        public static DataTable LeftJoin(this DataTable dt, DataTable right, string key)
        {
            if (!dt.Columns.Contains(key) || !right.Columns.Contains(key)) return dt;
            var existed = (from col in dt.Columns.OfType<DataColumn>()
                          select col.ColumnName).ToHashSet();
            var colMapping = new Dictionary<string, int>();
            int n = dt.Columns.Count;
            foreach (DataColumn column in right.Columns)
            {
                string col;
                if (column.ColumnName == key) continue;
                else if (existed.Contains(column.ColumnName))
                {
                    col = column.ColumnName + "_1";
                }
                else
                {
                    col = column.ColumnName;
                }
                dt.Columns.Add(col, column.DataType);
                colMapping[col] = n++;
            }
            var rowMapping = new Dictionary<string, int>();
            int keyOrd = dt.Columns.IndexOf(key);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowMapping[Convert.ToString(dt.Rows[i][keyOrd])!] = i;
            }
            int keyOrdR = right.Columns.IndexOf(key);
            for (int i = 0; i < right.Rows.Count; i++)
            {
                string keyStr = Convert.ToString(right.Rows[i][keyOrdR])!;
                if (rowMapping.ContainsKey(keyStr))
                {
                    int rowNum = rowMapping[keyStr];
                    for (int j = 0; j < right.Columns.Count; j++)
                    {
                        string colName = right.Columns[j].ColumnName;
                        if (colName == key) continue;
                        else if (existed.Contains(colName))
                        {
                            dt.Rows[rowNum][colMapping[colName + "_1"]] = right.Rows[i][j];
                        }
                        else
                        {
                            dt.Rows[rowNum][colMapping[colName]] = right.Rows[i][j];
                        }
                    }
                }
            }
            return dt;
        }

        //public static DataTable LeftJoin(this DataTable dt, DataTable right, List<string> keys)
        //{

        //}

        public static void Print(this DataTable dt)
        {
            using (var reader = dt.CreateDataReader())
            {
                int n = reader.FieldCount;
                for (int i = 0; i < n; i++)
                {
                    Console.Write(dt.Columns[i].ColumnName + "\t");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    for (int i = 0; i < n; i++)
                    {
                        Console.Write(reader[i] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}

