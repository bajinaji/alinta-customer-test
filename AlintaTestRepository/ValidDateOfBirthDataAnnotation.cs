using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CustomDataAnnotations
{
    public class ValidDateOfBirthDataAnnotation : ValidationAttribute
    {
        public ValidDateOfBirthDataAnnotation()
        {
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var dt = (DateTime) value;
                if (dt < new DateTime(1920, 01, 01) || dt > DateTime.Now)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
