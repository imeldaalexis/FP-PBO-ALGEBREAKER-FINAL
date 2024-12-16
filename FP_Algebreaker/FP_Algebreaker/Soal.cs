using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Algebreaker
    {
        public abstract class Soal
        {
            protected Random _random = new Random();
            protected int _number1;
            protected int _number2;
            protected int _number3;

            public Soal()
            {
                _number1 = _random.Next(1, 10);  // Generate a random number between 1 and 100
                _number2 = _random.Next(1, 10);  // Generate a random number between 1 and 100
                _number3 = _random.Next(1, 10);  // Generate a random number between 1 and 100
            }

        }
    }
}
