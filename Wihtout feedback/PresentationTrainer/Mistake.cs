using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    public class Mistake : PresentationEvent
    {
        
        public static double thresholdVolumetime = 2000;
        
        public enum Type { volume, cadence, posture, handMovement };
        public enum SubType { loud, soft, longPause, longSpeakingTime, badPosture, noHandMovement, tooMuchHandMovement, shortPause, shortSpeakingTime, moduleVolume };
        public enum GravityType { veryLoud, loud, soft, verySoft, veryLongPause, longPause, shortPause, veryLongSpeakingTime, longSpeakingTime, shortSpeakingTime, bad, notEnough, tooMuch };

        public SubType subType; 
        public Type type;
        public GravityType gravityType;        
        
        public bool volumeMistakeLongEnough; //only used for volume mistakes, too check if they surpass the time threshold for making a volume mistake
        public bool shortMistake = false; //if mistake is of short type (either pause or speakingtime) it stil does not have to be a mistake

        public Mistake(Type type, SubType subType, GravityType gravityType)
        {
            timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;            
            this.subType = subType;
            this.type = type;
            this.gravityType = gravityType;
            setGravity(gravityType);
            hasEnded = false;
            volumeMistakeLongEnough = false;
            classtype = "Mistake";
        }
        
        

        public bool checkIfVolumeMistakeLongEnough()
        {
            if (volumeMistakeLongEnough)
            {
                return true;
            }
            else
            {
                double delta = DateTime.Now.TimeOfDay.TotalMilliseconds - timeStarted;
                if (delta >= thresholdVolumetime)
                {
                    volumeMistakeLongEnough = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }            
        }

        // 5 states discerned = 3,1,0,1,3. 
        // 3 states discerned = 2,0,2. 
        // 2 states discerned = 2,0.
        public void setGravity(GravityType GT)
        {
            int points = 0;
            switch (GT)
            {
                case GravityType.veryLoud:
                    {
                        points = 3;                        
                        break;
                    }
                case GravityType.loud:
                    {
                        points = 1;
                        break;
                    }
                case GravityType.verySoft:
                    {
                        points = 3;
                        break;
                    }
                case GravityType.soft:
                    {
                        points = 1;
                        break;
                    }
                case GravityType.veryLongPause:
                    {
                        points = 3;
                        break;
                    }
                case GravityType.longPause:
                    {
                        points = 1;
                        break;
                    }
                case GravityType.veryLongSpeakingTime:
                    {
                        points = 3;
                        break;
                    }
                case GravityType.longSpeakingTime:
                    {
                        points = 1;
                        break;
                    }
                case GravityType.bad:
                    {
                        points = 2;
                        break;
                    }
                case GravityType.notEnough:
                    {
                        points = 2;
                        break;
                    }
                case GravityType.tooMuch:
                    {
                        points = 2;
                        break;
                    }
                case GravityType.shortPause:
                    {
                        if (shortMistake)
                        {
                            points = 10;
                        }  
                        break;
                    }
                case GravityType.shortSpeakingTime:
                    {
                        if (shortMistake)
                        {
                            points = 10;
                        }                        
                        break;
                    }
            }
            this.gravity = points;
            gravityType = GT;
        }

        
        public override String getString()
        {
            string temp = "";
            switch (subType)
            {
                case SubType.loud:
                    {
                        if (gravityType == GravityType.veryLoud)
                        {
                            temp = "Speak a lot softer";
                        }
                        else
                        {
                            temp = "Speak Softer";
                        }
                        break;
                    }
                case SubType.soft:
                    {
                        if (gravityType == GravityType.verySoft)
                        {
                            temp = "Speak a lot Louder";
                        }
                        else
                        {
                            temp = "Speak Louder";
                        }                        
                        break;
                    }
                case SubType.longPause:
                    {
                        temp = "Start Speaking";
                        break;
                    }
                case SubType.longSpeakingTime:
                    {
                        temp = "Stop Speaking";
                        break;
                    }
                case SubType.badPosture:
                    {
                        temp = "Reset Posture"; // better name for this!
                        break;
                    }
                case SubType.noHandMovement:
                    {
                        temp = "More Hand Movement";
                        break;
                    }
                case SubType.tooMuchHandMovement:
                    {
                        temp = "Less Hand Movement";
                        break;
                    }
                case SubType.shortSpeakingTime:
                    {
                        temp = "Speak Longer";
                        break;
                    }
                case SubType.shortPause:
                    {
                        temp = "Pause Longer";
                        break;
                    }
            }
            return temp;
        }

        public static String getStringOfSubType(SubType subType)
        {
            string temp = "";
            switch (subType)
            {
                case SubType.loud:
                    {
                        temp = "loud volume";
                        break;
                    }
                case SubType.soft:
                    {
                        temp = "soft volume";
                        break;
                    }
                case SubType.longPause:
                    {
                        temp = "long pause";
                        break;
                    }
                case SubType.longSpeakingTime:
                    {
                        temp = "long speaking time";
                        break;
                    }
                case SubType.badPosture:
                    {
                        temp = "bad posture"; 
                        break;
                    }
                case SubType.noHandMovement:
                    {
                        temp = "no hand movement";
                        break;
                    }
                case SubType.tooMuchHandMovement:
                    {
                        temp = "too much hand movement";
                        break;
                    }
                case SubType.shortSpeakingTime:
                    {
                        temp = "short speaking time";
                        break;
                    }
                case SubType.shortPause:
                    {
                        temp = "short pause";
                        break;
                    }
                case SubType.moduleVolume:
                    {
                        temp = "no mistake registered";
                        break;
                    }
            }
            return temp;
        }

        public static String getMistakeFeedBack(Mistake.SubType mistake)
        {
            string temp = "x";
            switch (mistake)
            {
                case SubType.loud:
                    {
                        temp = "Speaking too loud offends your audience." + "\n" 
                            + "Speaking a bit softer will remedy this.";
                        break;
                    }
                case SubType.soft:
                    {
                        temp = "Speaking too soft makes it hard for your audience to" + "\n" 
                            + "understand you. Speaking a bit louder will remedy this.";
                        break;
                    }
                case SubType.longPause:
                    {
                        temp = "Pausing for too long can irritate your audience." + "\n" 
                            + "Make sure you pause for a shorter time.";
                        break;
                    }
                case SubType.longSpeakingTime:
                    {
                        temp = "Speaking for too long makes it harder for your audience to" + "\n" 
                            + "understand you. Pausing once in a while remedies this.";
                        break;
                    }
                case SubType.badPosture:
                    {
                        temp = "Keep in mind that you should stand straight and keep your hands" + "\n"
                            + "above your hips. This sends out a message of confidence which" + "\n"
                            + "is important for capturing your audience.";
                        break;
                    }
                case SubType.noHandMovement:
                    {
                        temp = "Use hand movements to emphasize your statements. This will " + "\n"
                            + "make it easier for the audience to remember them.";
                        break;
                    }
                case SubType.tooMuchHandMovement:
                    {
                        temp = "Overusing hand movements negates the positive effect of " + "\n" 
                            + "emphasizing statements. You should use less hand movements.";
                        break;
                    }
                case SubType.shortSpeakingTime:
                    {
                        temp = "Speaking for a short time makes it harder for your audience" + "\n"
                               + "to understand you. Make sure you speak a little longer.";
                        break;
                    }
                case SubType.shortPause:
                    {
                        temp = "Pausing for a short time makes it look like you do not know" + "\n"
                        + "what you are talking about. Make sure you have a little bit " + "\n"
                        + "longer pauses.";
                        break;
                    }
                case SubType.moduleVolume:
                    {
                        temp = "No Feedback availabe for no mistake";
                        break;
                    }
            }
            return temp;
        }
    }
}
