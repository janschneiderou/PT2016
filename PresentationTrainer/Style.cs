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
