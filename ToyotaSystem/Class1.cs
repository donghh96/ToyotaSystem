using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyotaSystem
{
    public abstract class AbstractDataRecord : IDataRecord
    {
        public abstract string getPkColumnName();
        public abstract object getPkValue();
        public abstract string getTableName();
        public void updateProperty(string propertyName, object value)
        {
            string updateSQL = "update " + getTableName() + " set " + propertyName + "='" + value + "'where " + getPkColumnName() + " = '" + getPkValue() + "'";
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(updateSQL, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
