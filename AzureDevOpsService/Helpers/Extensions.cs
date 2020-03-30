using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace AzureDevOpsService.Helpers
{
    public static class Extensions
    {
        public static List<string> WIAsList(this string value)
        {
            List<string> values = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {

                string tempVal = string.Empty;
                int count = 0;
                bool isAdded = false;
                var arryList = value.Split(',');
                foreach (var id in arryList)
                {
                    count++;
                    tempVal = tempVal + "," + id;
                    if (count > 0)
                    {
                        isAdded = false;
                    }

                    if (count >= 380)
                    {
                        tempVal = tempVal.Trim(',');
                        values.Add(tempVal);
                        count = 0;
                        isAdded = true;
                        tempVal = string.Empty;
                    }

                }
                if (!isAdded)
                {
                    tempVal = tempVal.Trim(',');
                    values.Add(tempVal);
                }
            }
            return values;
        }

        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }


        public static bool Log(this StringBuilder contents, string fileName=null)
        {
            try
            {
                string breakLine = "|********************************************************************** ~ End ~ **********************************************************************| \n\n";
                string filePath = HostingEnvironment.MapPath("~/Logs");
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHssss");
                }
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = string.Format("{0}/{1}.txt", filePath, fileName);
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();

                    File.AppendAllText(filePath, contents.ToString() + breakLine);
                    return true;
                }
                else if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, contents.ToString() + breakLine);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
