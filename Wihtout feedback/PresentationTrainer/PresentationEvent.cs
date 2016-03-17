using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    public abstract class PresentationEvent
    {
        public string classtype;
        public double timeStarted; // use DateTime.Now.TimeOfDay.TotalMilliseconds
        public double timeEnded;
        public int gravity; // gravity of a mistake
        public bool hasEnded;

        public abstract String getString();

        public void ended()
        {
            if (!hasEnded)
            {
                timeEnded = DateTime.Now.TimeOfDay.TotalMilliseconds;
                hasEnded = true;
            }
        }

    }
}
