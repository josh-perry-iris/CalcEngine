using System;
using System.Collections;
using raminrahimzada;

namespace CalcEngine
{
    public class Tally
    {
        protected decimal _sum, _sum2, _cnt, _min, _max;
        protected bool _numbersOnly;

        public Tally(bool numbersOnly)
        {
            _numbersOnly = numbersOnly;
        }
        public Tally()
        {
        }

        public void Add(Expression e)
        {
            // handle enumerables
            var ienum = e as IEnumerable;
            if (ienum != null)
            {
                foreach (var value in ienum)
                {
                    AddValue(value);
                }
                return;
            }

            // handle expressions
            AddValue(e.Evaluate());
        }
        public virtual void AddValue(object value)
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
        public decimal Count() { return _cnt; }
        public decimal Sum() { return _sum; }
        public decimal Average() { return _sum / _cnt; }
        public decimal Min() { return _min; }
        public decimal Max() { return _max; }
        public decimal Range() { return _max - _min; }
        public decimal VarP()
        {
            var avg = Average();
            return _cnt <= 1 ? 0 : _sum2 / _cnt - avg * avg;
        }
        public decimal StdP()
        {
            var avg = Average();
            return _cnt <= 1 ? 0 : DecimalMath.Sqrt(_sum2 / _cnt - avg * avg);
        }
        public decimal Var()
        {
            var avg = Average();
            return _cnt <= 1 ? 0 : (_sum2 / _cnt - avg * avg) * _cnt / (_cnt - 1);
        }
        public decimal Std()
        {
            var avg = Average();
            return _cnt <= 1 ? 0 : DecimalMath.Sqrt((_sum2 / _cnt - avg * avg) * _cnt / (_cnt - 1));
        }
    }
}
