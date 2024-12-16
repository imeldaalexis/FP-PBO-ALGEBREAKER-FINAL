using FP_Algebreaker.Algebreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    public class SoalAlgebraic : Soal
    {
        public int OperationResult { get; private set; }
        public int x;

        public SoalAlgebraic() : base()
        {
            x = _number2;
            OperationResult = _number1 * _number2 + _number3;
        }

        public string GetExpression()
        {
            return $"{_number1} * x + {_number3} = {OperationResult}";
        }
    }
}
