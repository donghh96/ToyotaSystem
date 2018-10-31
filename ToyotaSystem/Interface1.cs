using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyotaSystem
{
    interface IDataRecord
    {
        void updateProperty(string propertyName, object value);
        string getPkColumnName();
        object getPkValue();
        string getTableName();
    }
}
