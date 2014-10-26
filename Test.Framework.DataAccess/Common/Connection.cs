using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.DataAccess
{
    public class Connection 
        : Disposable
    {
        #region Constructors/Members

        private IDbConnection connection;

        public Connection(string connectionName)
        {
            this.connection = Container.Resolve<IDbConnection>(connectionName);
        }

        #endregion Constructors/Members

        public IDbConnection Get()
        {
            this.connection.OpenConnection();
            return this.connection;
        }

        #region Disposable Overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.connection != null)
            {
                if (this.connection.State != ConnectionState.Closed)
                {
                    this.connection.Close();
                    this.connection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion  Disposable Overrides
    }
}
