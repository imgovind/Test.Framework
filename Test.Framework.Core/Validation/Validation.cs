using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class Validation
    {
        public Validation(Expression<Func<bool>> expression, string errorMessage)
        {
            Ensure.Argument.IsNotNull(expression, "expression");
            Ensure.Argument.IsNotEmpty(errorMessage, "errorMessage");

            Expression = expression;
            ErrorMessage = errorMessage;
        }


        public Expression<Func<bool>> Expression
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
