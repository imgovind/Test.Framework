using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class EntityValidator : IValidator
    {
        private Validation[] validations;
        public EntityValidator(params Validation[] validations)
        {
            this._Message = string.Empty;
            this.validations = validations;
        }

        private bool _IsValid { get; set; }
        public bool IsValid { get { return _IsValid; } }

        private string _Message { get; set; }
        public string Message { get { return _Message; } }

        public void Validate()
        {
            var errors = new StringBuilder();

            foreach (Validation validation in validations)
            {
                bool invalid = validation.Expression.Compile().Invoke();

                if (invalid)
                {
                    errors.AppendLine(string.Format("{0}", validation.ErrorMessage));
                }
            }

            if (errors.Length > 0)
            {
                this._IsValid = false;
                this._Message = errors.ToString();
            }
            else
            {
                this._IsValid = true;
            }
        }
    }
}
