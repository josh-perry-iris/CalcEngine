using System;
using System.Collections.Generic;

namespace CalcEngine.Functions
{
    public class ExtendedTally : Tally
    {
        List<decimal> _vals;

        public ExtendedTally(bool numbersOnly) : base(numbersOnly)
        {
            _vals = new List<decimal>();
        }

        public ExtendedTally() : base()
        {
            _vals = new List<decimal>();
        }

        public override void AddValue(object value)
        {
            // conversions
            if (!_numbersOnly)
            {
                // arguments that contain text evaluate as 0 (zero). 
                // empty text ("") evaluates as 0 (zero).
                if (value == null || value is string)
                {
                    value = 0;
                }
                // arguments that contain TRUE evaluate as 1; 
                // arguments that contain FALSE evaluate as 0 (zero).
                if (value is bool)
                {
                    value = (bool)value ? 1 : 0;
                }
            }

            // convert all numeric values to decimals
            if (value != null)
            {
                var typeCode = Type.GetTypeCode(value.GetType());
                if (typeCode >= TypeCode.Char && typeCode <= TypeCode.Decimal)
                {
                    value = Convert.ChangeType(value, typeof(decimal), System.Globalization.CultureInfo.CurrentCulture);
                    _vals.Add((decimal)value);
                }
            }

            // tally
            if (value is decimal)
            {
                var dbl = (decimal)value;
                _sum += dbl;
                _sum2 += dbl * dbl;
                _cnt++;
                if (_cnt == 1 || dbl < _min)
                {
                    _min = dbl;
                }
                if (_cnt == 1 || dbl > _max)
                {
                    _max = dbl;
                }
            }
        }

        public decimal Median()
        {
            var assessments = _vals.ToArray();

            if (_vals.Count <= 0)
            {
                return 0;
            }

            Array.Sort(assessments);

            var arrayLength = assessments.Length;

            if (arrayLength % 2 == 0)
            {
                return (assessments[(arrayLength / 2) - 1] + assessments[arrayLength / 2]) / 2;
            }

            return assessments[(int)Math.Floor((decimal)arrayLength / 2)];
        }
    }
}
