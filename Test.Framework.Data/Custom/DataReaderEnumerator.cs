using System;
using System.Data;
using System.Collections.Generic;

namespace Test.Framework.Data
{
    public class DataReaderEnumerator : IEnumerator<IDataReader>
    {
        #region Constructor/Members

        public IDataReader DataReader { get; private set; }

        public DataReaderEnumerator(IDataReader dataReader)
        {
            this.DataReader = dataReader;
        }

        #endregion

        #region IEnumerator<IDataReader> Members

        public IDataReader Current
        {
            get { return this.DataReader; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.DataReader.Dispose();
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            return this.DataReader.Read();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
