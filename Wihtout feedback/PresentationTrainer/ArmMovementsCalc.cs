using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    public class ArmMovementsCalc
    {

        public enum Gesture { nogesture, small, medium, big };//TODO
        public Gesture currentGesture;
        public Gesture previousGesture;

        public double MinAngleLeftArmShoulderLineA = 0;
        public double MinAngleLeftArmShoulderLineB = 0;
        public double MinAngleLeftArmShoulderLineC = 0;

        public double MinAngleRightArmShoulderLineA = 0;
        public double MinAngleRightArmShoulderLineB = 0;
        public double MinAngleRightArmShoulderLineC = 0;


        public double MaxAngleLeftArmShoulderLineA = 0;
        public double MaxAngleLeftArmShoulderLineB = 0;
        public double MaxAngleLeftArmShoulderLineC = 0;

        public double MaxAngleRightArmShoulderLineA = 0;
        public double MaxAngleRightArmShoulderLineB = 0;
        public double MaxAngleRightArmShoulderLineC = 0;


        public double MinAngleLeftForearmLeftArmA = 0;
        public double MinAngleLeftForearmLeftArmB = 0;
        public double MinAngleLeftForearmLeftArmC = 0;

        public double MinAngleRightForearmRightArmA = 0;
        public double MinAngleRightForearmRightArmB = 0;
        public double MinAngleRightForearmRightArmC = 0;

        public double MaxAngleLeftForearmLeftArmA = 0;
        public double MaxAngleLeftForearmLeftArmB = 0;
        public double MaxAngleLeftForearmLeftArmC = 0;

        public double MaxAngleRightForearmRightArmA = 0;
        public double MaxAngleRightForearmRightArmB = 0;
        public double MaxAngleRightForearmRightArmC = 0;

        bool GrowingAngleLeftArmShoulderLineA = true;
        bool GrowingAngleLeftArmShoulderLineB = true;
        bool GrowingAngleLeftArmShoulderLineC = true;

        bool GrowingAngleRightArmShoulderLineA = true;
        bool GrowingAngleRightArmShoulderLineB = true;
        bool GrowingAngleRightArmShoulderLineC = true;

        bool GrowingAngleLeftForearmLeftArmA = true;
        bool GrowingAngleLeftForearmLeftArmB = true;
        bool GrowingAngleLeftForearmLeftArmC = true;

        bool GrowingAngleRightForearmRightArmA = true;
        bool GrowingAngleRightForearmRightArmB = true;
        bool GrowingAngleRightForearmRightArmC = true;

        public int SwingAngleLeftArmShoulderLineA = 0;
        public int SwingAngleLeftArmShoulderLineB = 0;
        public int SwingAngleLeftArmShoulderLineC = 0;

        public int SwingAngleRightArmShoulderLineA = 0;
        public int SwingAngleRightArmShoulderLineB = 0;
        public int SwingAngleRightArmShoulderLineC = 0;

        public int SwingAngleLeftForearmLeftArmA = 0;
        public int SwingAngleLeftForearmLeftArmB = 0;
        public int SwingAngleLeftForearmLeftArmC = 0;

        public int SwingAngleRightForearmRightArmA = 0;
        public int SwingAngleRightForearmRightArmB = 0;
        public int SwingAngleRightForearmRightArmC = 0;


        public double leftArmAngleChange = 0;
        public double rightArmAngleChange = 0;
        public double gestureSize = 0;


        JudgementMaker parent;
        public ArmMovementsCalc(JudgementMaker parent)
        {
            this.parent = parent;
        }

        public void resetMaxAndMin()
        {
            setPreviousAngles();
            currentGesture = Gesture.nogesture;

            MinAngleLeftArmShoulderLineA = parent.bfpa.prevAngleLeftArmShoulderLineA;
            MinAngleLeftArmShoulderLineB = parent.bfpa.prevAngleLeftArmShoulderLineB;
            MinAngleLeftArmShoulderLineC = parent.bfpa.prevAngleLeftArmShoulderLineC;

            MinAngleRightArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
            MinAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
            MinAngleRightArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;

            MaxAngleLeftArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
            MaxAngleLeftArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
            MaxAngleLeftArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;

            MaxAngleRightArmShoulderLineA = parent.bfpa.prevAngleLeftArmShoulderLineA;
            MaxAngleRightArmShoulderLineB = parent.bfpa.prevAngleLeftArmShoulderLineB;
            MaxAngleRightArmShoulderLineC = parent.bfpa.prevAngleLeftArmShoulderLineC;


            MinAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
            MinAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
            MinAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;

            MinAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
            MinAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
            MinAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;

            MaxAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
            MaxAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
            MaxAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;

            MaxAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
            MaxAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
            MaxAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;


            GrowingAngleLeftArmShoulderLineA = true;
            GrowingAngleLeftArmShoulderLineB = true;
            GrowingAngleLeftArmShoulderLineC = true;

            GrowingAngleRightArmShoulderLineA = true;
            GrowingAngleRightArmShoulderLineB = true;
            GrowingAngleRightArmShoulderLineC = true;

            GrowingAngleLeftForearmLeftArmA = true;
            GrowingAngleLeftForearmLeftArmB = true;
            GrowingAngleLeftForearmLeftArmC = true;

            GrowingAngleRightForearmRightArmA = true;
            GrowingAngleRightForearmRightArmB = true;
            GrowingAngleRightForearmRightArmC = true;

            SwingAngleLeftArmShoulderLineA = 0;
            SwingAngleLeftArmShoulderLineB = 0;
            SwingAngleLeftArmShoulderLineC = 0;

            SwingAngleRightArmShoulderLineA = 0;
            SwingAngleRightArmShoulderLineB = 0;
            SwingAngleRightArmShoulderLineC = 0;

            SwingAngleLeftForearmLeftArmA = 0;
            SwingAngleLeftForearmLeftArmB = 0;
            SwingAngleLeftForearmLeftArmC = 0;

            SwingAngleRightForearmRightArmA = 0;
            SwingAngleRightForearmRightArmB = 0;
            SwingAngleRightForearmRightArmC = 0;
        }

        void setPreviousAngles()
        {
            parent.bfpa.prevAngleLeftArmShoulderLineA = parent.bfpa.angleLeftArmShoulderLineA;
            parent.bfpa.prevAngleLeftArmShoulderLineB = parent.bfpa.angleLeftArmShoulderLineB;
            parent.bfpa.prevAngleLeftArmShoulderLineC = parent.bfpa.angleLeftArmShoulderLineC;

            parent.bfpa.prevAngleRightArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
            parent.bfpa.prevAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
            parent.bfpa.prevAngleRightArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;

            parent.bfpa.prevAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
            parent.bfpa.prevAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
            parent.bfpa.prevAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;

            parent.bfpa.prevAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
            parent.bfpa.prevAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
            parent.bfpa.prevAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;
        }

        public void calcArmMovements()
        {
            //if(parent.bfpa.prevAngleRightForearmRightArmA==2000)
            //{
            //    setPreviousAngles();
            //}

            getGrowingValues();
            setMaxMinValues();
            calcCurrentGestureSize();
            setPreviousAngles();
            if(previousGesture!=currentGesture)
            {
                MainWindow.stringGestures = MainWindow.stringGestures + System.Environment.NewLine + currentGesture.ToString() +
                    " " + DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
            previousGesture = currentGesture;

        }
        void calcCurrentGestureSize()
        {
            
           

            ///leftArm
            leftArmAngleChange = Math.Abs(MaxAngleLeftArmShoulderLineA - MinAngleLeftArmShoulderLineA) +
                Math.Abs(MaxAngleLeftArmShoulderLineB - MinAngleLeftArmShoulderLineB) +
                Math.Abs(MaxAngleLeftArmShoulderLineC - MinAngleLeftArmShoulderLineC)+
                Math.Abs(MaxAngleLeftForearmLeftArmA - MinAngleLeftForearmLeftArmA )+
                Math.Abs(MaxAngleLeftForearmLeftArmB - MinAngleLeftForearmLeftArmB )+
               Math.Abs( MaxAngleLeftForearmLeftArmC - MinAngleLeftForearmLeftArmC);

            ///// Right Arm
            rightArmAngleChange = Math.Abs(MaxAngleRightArmShoulderLineA - MinAngleRightArmShoulderLineA) +
                Math.Abs(MaxAngleRightArmShoulderLineB - MinAngleRightArmShoulderLineB) +
                Math.Abs(MaxAngleRightArmShoulderLineC - MinAngleRightArmShoulderLineC) +
                Math.Abs(MaxAngleRightForearmRightArmA - MinAngleRightForearmRightArmA) +
                Math.Abs(MaxAngleRightForearmRightArmB - MinAngleRightForearmRightArmB) +
                Math.Abs( MaxAngleRightForearmRightArmC - MinAngleRightForearmRightArmC);

            if(rightArmAngleChange> leftArmAngleChange)
            {
                gestureSize = rightArmAngleChange;
            }
            else
            {
                gestureSize = leftArmAngleChange;
            }
           // gestureSize = rightArmAngleChange;
            

            if(gestureSize<20)
            {
                currentGesture = Gesture.nogesture;
            }
            else if(gestureSize<40)
            {
                currentGesture = Gesture.small;
            }
            else if(gestureSize<60)
            {
                currentGesture = Gesture.medium;
            }
            else
            {
                currentGesture = Gesture.big;
            }

        }
        void setMaxMinValues()
        {
           //////// Left shoulder
            if(parent.bfpa.angleLeftArmShoulderLineA <MinAngleLeftArmShoulderLineA )
            {
                
                MinAngleLeftArmShoulderLineA = parent.bfpa.angleLeftArmShoulderLineA;
            }
            if (parent.bfpa.angleLeftArmShoulderLineA > MaxAngleLeftArmShoulderLineA)
            {
                MaxAngleLeftArmShoulderLineA = parent.bfpa.angleLeftArmShoulderLineA;
            }

            if (parent.bfpa.angleLeftArmShoulderLineB < MinAngleLeftArmShoulderLineB)
            {
                MinAngleLeftArmShoulderLineB = parent.bfpa.angleLeftArmShoulderLineB;
            }
            if (parent.bfpa.angleLeftArmShoulderLineB > MaxAngleLeftArmShoulderLineB)
            {
                MaxAngleLeftArmShoulderLineB = parent.bfpa.angleLeftArmShoulderLineB;
            }

            if (parent.bfpa.angleLeftArmShoulderLineC < MinAngleLeftArmShoulderLineC)
            {
                MinAngleLeftArmShoulderLineC = parent.bfpa.angleLeftArmShoulderLineC;
            }
            if (parent.bfpa.angleLeftArmShoulderLineC > MaxAngleLeftArmShoulderLineC)
            {
                MaxAngleLeftArmShoulderLineC = parent.bfpa.angleLeftArmShoulderLineC;
            }

            ////// Right Shoulder
            if (parent.bfpa.angleRightArmShoulderLineA < MinAngleRightArmShoulderLineA)
            {
                MinAngleRightArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
            }
            if (parent.bfpa.angleRightArmShoulderLineA > MaxAngleRightArmShoulderLineA)
            {
                MaxAngleRightArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
            }

            if (parent.bfpa.angleRightArmShoulderLineB < MinAngleRightArmShoulderLineB)
            {
                MinAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
            }
            if (parent.bfpa.angleRightArmShoulderLineB > MaxAngleRightArmShoulderLineB)
            {
                MaxAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
            }

            if (parent.bfpa.angleRightArmShoulderLineC < MinAngleRightArmShoulderLineC)
            {
                MinAngleRightArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;
            }
            if (parent.bfpa.angleRightArmShoulderLineC > MaxAngleRightArmShoulderLineC)
            {
                MaxAngleRightArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;
            }

            /////// LeftForearm
            if (parent.bfpa.angleLeftForearmLeftArmA < MinAngleLeftForearmLeftArmA)
            {
                MinAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
            }
            if (parent.bfpa.angleLeftForearmLeftArmA > MaxAngleLeftForearmLeftArmA)
            {
                MaxAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
            }

            if (parent.bfpa.angleLeftForearmLeftArmB < MinAngleLeftForearmLeftArmB)
            {
                MinAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
            }
            if (parent.bfpa.angleLeftForearmLeftArmB > MaxAngleLeftForearmLeftArmB)
            {
                MaxAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
            }

            if (parent.bfpa.angleLeftForearmLeftArmC < MinAngleLeftForearmLeftArmC)
            {
                MinAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;
            }
            if (parent.bfpa.angleLeftForearmLeftArmC > MaxAngleLeftForearmLeftArmC)
            {
                MaxAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;
            }


        ///////Right Forearm
            if (parent.bfpa.angleRightForearmRightArmA < MinAngleRightForearmRightArmA)
            {
                MinAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
            }
            if (parent.bfpa.angleRightForearmRightArmA > MaxAngleRightForearmRightArmA)
            {
                MaxAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
            }

            if (parent.bfpa.angleRightForearmRightArmB < MinAngleRightForearmRightArmB)
            {
                MinAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
            }
            if (parent.bfpa.angleRightForearmRightArmB > MaxAngleRightForearmRightArmB)
            {
                MaxAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
            }

            if (parent.bfpa.angleRightForearmRightArmC < MinAngleRightForearmRightArmC)
            {
                MinAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;
            }
            if (parent.bfpa.angleRightForearmRightArmC > MaxAngleRightForearmRightArmC)
            {
                MaxAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;
            }

        }
        void getGrowingValues()
        {
            bool growingVariable;

            /////////// left shoulder
            growingVariable = calcIsGrowing(parent.bfpa.angleLeftArmShoulderLineA,
                parent.bfpa.prevAngleLeftArmShoulderLineA);
            if (GrowingAngleLeftArmShoulderLineA != growingVariable)
            {
                GrowingAngleLeftArmShoulderLineA = growingVariable;
                if(MaxAngleLeftArmShoulderLineA-MinAngleLeftArmShoulderLineA>10)
                {
                    SwingAngleLeftArmShoulderLineA++;
                }
                MaxAngleLeftArmShoulderLineA = parent.bfpa.angleLeftArmShoulderLineA;
                MinAngleLeftArmShoulderLineA = parent.bfpa.angleLeftArmShoulderLineA;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleLeftArmShoulderLineB,
                parent.bfpa.prevAngleLeftArmShoulderLineB);
            if (GrowingAngleLeftArmShoulderLineB != growingVariable)
            {
                GrowingAngleLeftArmShoulderLineB = growingVariable;
                if (MaxAngleLeftArmShoulderLineB - MinAngleLeftArmShoulderLineB > 10)
                {
                    SwingAngleLeftArmShoulderLineB++;
                }
                MaxAngleLeftArmShoulderLineB = parent.bfpa.angleLeftArmShoulderLineB;
                MinAngleLeftArmShoulderLineB = parent.bfpa.angleLeftArmShoulderLineB;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleLeftArmShoulderLineC,
                parent.bfpa.prevAngleLeftArmShoulderLineC);
            if (GrowingAngleLeftArmShoulderLineC != growingVariable)
            {
                GrowingAngleLeftArmShoulderLineC = growingVariable;
                if (MaxAngleLeftArmShoulderLineC - MinAngleLeftArmShoulderLineC > 10)
                {
                    SwingAngleLeftArmShoulderLineC++;
                }
                MaxAngleLeftArmShoulderLineC = parent.bfpa.angleLeftArmShoulderLineC;
                MinAngleLeftArmShoulderLineC = parent.bfpa.angleLeftArmShoulderLineC;
            }

            ///////////right shoulder
            growingVariable = calcIsGrowing(parent.bfpa.angleRightArmShoulderLineA,
                parent.bfpa.prevAngleRightArmShoulderLineA);
            if (GrowingAngleRightArmShoulderLineA != growingVariable)
            {
                GrowingAngleRightArmShoulderLineA = growingVariable;
                if (MaxAngleRightArmShoulderLineA - MinAngleRightArmShoulderLineA > 10)
                {
                    SwingAngleRightArmShoulderLineA++;
                }
                MaxAngleRightArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
                MinAngleRightArmShoulderLineA = parent.bfpa.angleRightArmShoulderLineA;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleRightArmShoulderLineB,
                parent.bfpa.prevAngleRightArmShoulderLineB);
            if (GrowingAngleRightArmShoulderLineB != growingVariable)
            {
                GrowingAngleRightArmShoulderLineB = growingVariable;
                if (MaxAngleRightArmShoulderLineB - MinAngleRightArmShoulderLineB > 10)
                {
                    SwingAngleRightArmShoulderLineB++;
                }
                MaxAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
                MinAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleRightArmShoulderLineC,
                 parent.bfpa.prevAngleRightArmShoulderLineC);
            if (GrowingAngleRightArmShoulderLineC != growingVariable)
            {
                GrowingAngleRightArmShoulderLineC = growingVariable;
                if (MaxAngleRightArmShoulderLineC - MinAngleRightArmShoulderLineC > 10)
                {
                    SwingAngleRightArmShoulderLineC++;
                }
                MaxAngleRightArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;
                MinAngleRightArmShoulderLineC = parent.bfpa.angleRightArmShoulderLineC;
            }

            ///////////left forearm
            growingVariable = calcIsGrowing(parent.bfpa.angleLeftForearmLeftArmA,
               parent.bfpa.prevAngleLeftForearmLeftArmA);
            if (GrowingAngleLeftForearmLeftArmA != growingVariable)
            {
                GrowingAngleLeftForearmLeftArmA = growingVariable;
                if (MaxAngleLeftForearmLeftArmA - MinAngleLeftForearmLeftArmA > 10)
                {
                    SwingAngleLeftForearmLeftArmA++;
                }
                MaxAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
                MinAngleLeftForearmLeftArmA = parent.bfpa.angleLeftForearmLeftArmA;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleLeftForearmLeftArmB,
               parent.bfpa.prevAngleLeftForearmLeftArmB);
            if (GrowingAngleLeftForearmLeftArmB != growingVariable)
            {
                GrowingAngleLeftForearmLeftArmB = growingVariable;
                if (MaxAngleLeftForearmLeftArmB - MinAngleLeftForearmLeftArmB > 10)
                {
                    SwingAngleLeftForearmLeftArmB++;
                }
                MaxAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
                MinAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleLeftForearmLeftArmC,
               parent.bfpa.prevAngleLeftForearmLeftArmC);
            if (GrowingAngleLeftForearmLeftArmC != growingVariable)
            {
                GrowingAngleLeftForearmLeftArmC = growingVariable;
                if (MaxAngleLeftForearmLeftArmC - MinAngleLeftForearmLeftArmC > 10)
                {
                    SwingAngleLeftForearmLeftArmC++;
                }
                MaxAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;
                MinAngleLeftForearmLeftArmC = parent.bfpa.angleLeftForearmLeftArmC;
            }

            /////////// right forearm
            growingVariable = calcIsGrowing(parent.bfpa.angleRightForearmRightArmA,
              parent.bfpa.prevAngleRightForearmRightArmA);
            if (GrowingAngleRightForearmRightArmA != growingVariable)
            {
                GrowingAngleRightForearmRightArmA = growingVariable;
                if (MaxAngleRightForearmRightArmA - MinAngleRightForearmRightArmA > 10)
                {
                    SwingAngleRightForearmRightArmA++;
                }
                MaxAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
                MinAngleRightForearmRightArmA = parent.bfpa.angleRightForearmRightArmA;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleRightForearmRightArmB,
              parent.bfpa.prevAngleRightForearmRightArmB);
            if (GrowingAngleRightForearmRightArmB != growingVariable)
            {
                GrowingAngleRightForearmRightArmB = growingVariable;
                if (MaxAngleRightForearmRightArmB - MinAngleRightForearmRightArmB > 10)
                {
                    SwingAngleRightForearmRightArmB++;
                }
                MaxAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
                MinAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;
            }
            growingVariable = calcIsGrowing(parent.bfpa.angleRightForearmRightArmC,
              parent.bfpa.prevAngleRightForearmRightArmC);
            if (GrowingAngleRightForearmRightArmC != growingVariable)
            {
                GrowingAngleRightForearmRightArmC = growingVariable;
                if (MaxAngleRightForearmRightArmC - MinAngleRightForearmRightArmC > 10)
                {
                    SwingAngleRightForearmRightArmC++;
                }
                MaxAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;
                MinAngleRightForearmRightArmC = parent.bfpa.angleRightForearmRightArmC;
            }

        }
        bool calcIsGrowing(double current, double previous)
        {
            bool isGrowing = true;
            if(current<previous)
            {
                isGrowing = false;
            }
            return isGrowing;
        }
        
    }
}
