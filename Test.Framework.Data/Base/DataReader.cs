using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Test.Framework.Extensions;

namespace Test.Framework.Data
{
    public class DataReader : IEnumerable
    {
        private readonly IDataReader reader;
        private readonly Dictionary<string, int> columnOrdinals = new Dictionary<string, int>();

        public DataReader(IDataReader reader)
        {
            this.reader = reader;
        }

        public int GetColumn(string name)
        {
            if (!columnOrdinals.ContainsKey(name))
            {
                var i = reader.GetOrdinal(name);
                columnOrdinals.Add(name, i);
            }
            return columnOrdinals[name];
        }

        public T GetValue<T>(string name)
        {
            return (T)reader.GetValue(GetColumn(name));
        }

        public bool GetBoolean(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetBoolean(iCol);
        }

        public bool GetBoolean(string name, bool defaultValue)
        {
            return GetBooleanNullable(name) ?? defaultValue;
        }

        public bool? GetBooleanNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
                return reader.GetBoolean(iCol);
            return null;
        }

        public string GetString(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetString(iCol);
        }

        public string GetStringNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
                return reader.GetString(iCol);
            return string.Empty;
        }

        public byte GetByte(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetByte(iCol);
        }

        public int GetInt(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetInt32(iCol);
        }

        public int GetInt(string name, int defaultValue)
        {
            return GetIntNullable(name) ?? defaultValue;
        }

        public int? GetIntNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
                return reader.GetInt32(iCol);
            return null;
        }

        public decimal GetDecimal(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetDecimal(iCol);
        }

        public decimal GetDecimal(string name, decimal defaultValue)
        {
            return GetDecimalNullable(name) ?? defaultValue;
        }

        public decimal? GetDecimalNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
                return reader.GetDecimal(iCol);
            return null;
        }

        public double GetDouble(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetDouble(iCol);
        }

        public double GetDouble(string name, double defaultValue)
        {
            return GetDoubleNullable(name) ?? defaultValue;
        }

        public double? GetDoubleNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
                return reader.GetDouble(iCol);
            return null;
        }

        public Guid GetGuid(string name)
        {
            var iCol = GetColumn(name);
            return reader.GetGuid(iCol);
        }

        public DateTime GetDateTime(string name)
        {
            var icol = GetColumn(name);
            return reader.GetDateTime(icol);
        }

        public DateTime? GetDateTimeNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
                return reader.GetDateTime(iCol);
            else
                return null;
        }

        public IEnumerator GetEnumerator()
        {
            return new DataReaderEnumerator(this.reader);
        }

        public Guid GetGuidIncludeEmpty(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
            {
                var iVal = reader.GetString(iCol);
                if (iVal.IsNotNullOrEmpty() && iVal.IsGuid())
                {
                    return iVal.ToGuid();
                }
            }
            return Guid.Empty;
        }

        public DateTime GetDateTimeWithMin(string name)
        {
            var iCol = GetColumn(name);
            if (!reader.IsDBNull(iCol))
            {
                return reader.GetDateTime(iCol);
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}
