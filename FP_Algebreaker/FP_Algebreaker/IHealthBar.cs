using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    public interface IHealthBar
    {
        ProgressBar HealthBar { get; }
        void UpdateHealthBarPosition();
        void UpdateHealthBar();
    }
}
