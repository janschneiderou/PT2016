/**
 * ****************************************************************************
 * Copyright (C) 2016 Open Universiteit Nederland
 * <p/>
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * <p/>
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * <p/>
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library.  If not, see <http://www.gnu.org/licenses/>.
 * <p/>
 * Contributors: Jan Schneider
 * ****************************************************************************
 */
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
