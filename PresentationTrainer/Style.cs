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
    public class Style : PresentationEvent
    {
      
        public enum Type { goodPause, goodSpeakingTime, goodPosture }; //goodSpeakingTime includes goodVolume and handMovement
        public Type type;
        

        public Style(Type type)
        {
            this.type = type;
            setGravity();
            timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
            hasEnded = false;
            classtype = "Style";
        }

        public void setGravity()
        {
            switch (type)
            {
                case Type.goodPause:
                    {
                        gravity = 3;
                        break;
                    }
                case Type.goodPosture:
                    {
                        gravity = 3;
                        break;
                    }
                case Type.goodSpeakingTime:
                    {
                        gravity = 3;
                        break;
                    }
            }
        }
        public override string getString()
        {
            String temp = "";
            switch (type)
            {
                case Type.goodPause:
                    {
                        temp = "good pause! :-D";
                        break;
                    }
                case Type.goodPosture:
                    {
                        temp = "You have a good posture";
                        break;
                    }
                case Type.goodSpeakingTime:
                    {
                        temp = "Well spoken and good emphasis";
                        break;
                    }
            }
            return temp;
        }
    }
}
