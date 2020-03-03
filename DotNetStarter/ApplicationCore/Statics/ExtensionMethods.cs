using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ApplicationCore.Statics
{
    public static class ExtensionMethods
    {
        public static string[] GridDateFormatArray = new string[] { "dd/MM/yyyy", "MM/dd/yyyy" };
        public const string DateFormat = "yyyy-MMM-dd";

        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            var columnNames = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                string cName = dt.Rows[0][column.ColumnName].ToString();
                if (!dt.Columns.Contains(cName) && cName != "")
                {
                    column.ColumnName = cName;
                }

                columnNames.Add(column.ColumnName);
            }

            dt.Rows[0].Delete();
            dt.AcceptChanges();

            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Any(x => x.Equals(pro.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (typeof(bool).IsAssignableFrom(pro.PropertyType))
                        {
                            var value = row[pro.Name] != DBNull.Value && (row[pro.Name].ToString().Equals("1") || row[pro.Name].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) || row[pro.Name].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase));
                            pro.SetValue(objT, value);
                        }
                        else if (typeof(string).IsAssignableFrom(pro.PropertyType))
                        {
                            var value = row[pro.Name] == DBNull.Value ? string.Empty : Convert.ChangeType(row[pro.Name], pro.PropertyType);
                            pro.SetValue(objT, value);
                        }
                        else
                            pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pro.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        public static DataSet DataReaderToDataSet(IDataReader reader)
        {
            var ds = new DataSet();
            DataTable table;
            do
            {
                int fieldCount = reader.FieldCount;
                table = new DataTable();
                for (int i = 0; i < fieldCount; i++)
                {
                    table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }
                table.BeginLoadData();
                var values = new Object[fieldCount];
                while (reader.Read())
                {
                    reader.GetValues(values);
                    table.LoadDataRow(values, true);
                }
                table.EndLoadData();

                ds.Tables.Add(table);

            } while (reader.NextResult());
            reader.Close();
            return ds;
        }

        public static int GetEnumValue(Type enumType, string value)
        {
            if (value == "Integer")
                value = "Int32";
            else if (value == "Float")
                value = "Double";
            int i = -1;
            i = (int)Enum.Parse(enumType, value);
            return i;
        }
    }
}
