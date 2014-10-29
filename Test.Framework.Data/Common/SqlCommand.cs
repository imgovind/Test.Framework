using System.Collections.Generic;

namespace Test.Framework.Data
{
    public class SqlCommand
    {
        public SqlCommand(string statement, IList<Parameter> parameters, int timeout = 15, string errorMessage = null)
        {
            Statement = statement;
            Parameters = parameters;
            ErrorMessage = errorMessage;
            Timeout = timeout;
        }

        public string Statement { get; private set; }
        public IList<Parameter> Parameters { get; private set; }
        public string ErrorMessage { get; private set; }
        public int Timeout { get; private set; }
    }
}
