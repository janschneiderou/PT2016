using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PresentationTrainer
{
    public class HeroAnalysis
    {
        public MainWindow parent;

        public static double targetLeft1;
        public static double targetLeft2;
        public static double targetTop1;
        public static double targetTop2;

        public bool startCelebration = false;
        public bool finishCelebration = false;
        public bool firstCelebration = false;
        public bool secondCelebration = false;
        public double celebrationStarted;
        public double celebrationContinued;
        public double endCelebration;
        public double timeToMoveCelebration=3000;

        public double counterSmile;
        public double counterLegs;
        public double counterNeck;
        public double counterBack;
        public double timeForAction = 4000;
        public int currentDrop = 0;

        string stringPowerSelection = "Select 3 Super powers that you identify with.";
        string stringPostureTeaching = "Lets learn the Super Hero Posture.";
        string stringInspiration2 = "Select 3 values that you find inspiring in others";
        string stringInspiration1 = "Select your most inspiring personality trait";
        string stringImpact = "Select 3 values that  as Super Hero what will you bring to the world?";
        string stringSavingTown = "Select 3 values that  as Super Hero what will you bring to the world?";
        string stringCelebration = "Celebrate as a hero!";
        string stringCorrectPosture = "Go back to your Super Hero Posture";

        public enum States { Start, PostureTeaching, PowerSelection, Inspiration1, Inspiration2, Impact, SavingTown, Celebration,Finish, Pause, ValueSelection };
        public States myState;
        public BodyFramePreAnalysis bfpa;
        public FaceFramePreAnalisys ffpa;

        public HeroAnalysis(MainWindow parent)
        {
            this.parent = parent;
            myState = States.Start;
            
        }

        public void loadStuff()
        {
            parent.heroMode.countdown.countdownFinished += countdown_countdownFinished;
            parent.heroMode.values.selectionEvent += values_selectionEvent;
            parent.heroMode.HeroLesson.nextEvent += HeroLesson_nextEvent;
            parent.heroMode.HeroAffirmation.nextEvent += HeroAffirmation_nextEvent;
            parent.heroMode.heroPower.nextEvent += heroPower_nextEvent;
            parent.heroMode.inspiration1.selectionEvent += inspiration1_selectionEvent;
            parent.heroMode.heroInspiration2.nextEvent += heroInspiration2_nextEvent;
            parent.heroMode.heroImpact.nextEvent += heroImpact_nextEvent;
            parent.heroMode.heroCelebration.nextEvent += heroCelebration_nextEvent;
            
            
        }

        void heroCelebration_nextEvent(object sender)
        {
            myState = States.Pause;
            parent.heroMode.heroExit.Visibility = System.Windows.Visibility.Visible;
        }

        void heroImpact_nextEvent(object sender)
        {
            myState = States.Pause;
            parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.HeroAffirmation.setStrings(6); //should be 5
            parent.heroMode.HeroAffirmation.Visibility = System.Windows.Visibility.Visible;
            parent.heroMode.heroImpact.Visibility = System.Windows.Visibility.Collapsed;
        }

        void heroInspiration2_nextEvent(object sender)
        {
            myState = States.Pause;
            parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.HeroAffirmation.setStrings(4);
            parent.heroMode.HeroAffirmation.Visibility = System.Windows.Visibility.Visible;
            parent.heroMode.heroInspiration2.Visibility = System.Windows.Visibility.Collapsed;
        }

        void inspiration1_selectionEvent(object sender, string x)
        {

            myState = States.Pause;
            parent.heroMode.coin.Stop();
            parent.heroMode.coin.Play();
            parent.heroMode.HeroAffirmation.inspiringValue = x;
            parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.HeroAffirmation.setStrings(3);
            parent.heroMode.HeroAffirmation.Visibility = System.Windows.Visibility.Visible;
            parent.heroMode.inspiration1.Visibility = System.Windows.Visibility.Collapsed;
        }

        void heroPower_nextEvent(object sender)
        {
            myState = States.Pause;
            parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Collapsed;
            parent.heroMode.HeroAffirmation.setStrings(2);
            parent.heroMode.HeroAffirmation.Visibility = System.Windows.Visibility.Visible;
            parent.heroMode.heroPower.Visibility = System.Windows.Visibility.Collapsed;
        }

        void HeroAffirmation_nextEvent(object sender, int x)
        {
            
            if(x<=7)
            {
                x++;
                parent.heroMode.HeroAffirmation.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.HeroLesson.setStrings(x);
                parent.heroMode.HeroLesson.Visibility = System.Windows.Visibility.Visible;
                
            }
        }

        void HeroLesson_nextEvent(object sender, int x)
        {
            switch(x)
            {
                case 1:
                    myState = States.PostureTeaching;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringPostureTeaching;
                    break;
                case 2:
                    myState = States.PowerSelection;
                    currentDrop = 0;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringPowerSelection;
                    targetLeft1 = 537;
                    targetLeft2 = 647;
                    targetTop1 = 250;
                    targetTop2 = 374;
                    break;
                case 3:
                    myState = States.Inspiration1;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringInspiration1;
                    break;
                case 4:
                    myState = States.Inspiration2;
                    currentDrop = 0;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringInspiration2;
                    targetLeft1 = 537;
                    targetLeft2 = 647;
                    targetTop1 = 250;
                    targetTop2 = 374;
                    break;
                case 5:
                    myState = States.Impact;
                    currentDrop = 0;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringImpact;
                    targetLeft1 = 417;
                    targetLeft2 = 750;
                    targetTop1 = 228;
                    targetTop2 = 560;
                    break;
                case 6:
                    myState = States.SavingTown;
                    currentDrop = 0;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringSavingTown;
                    break;
                case 7:
                    myState = States.Celebration;
                    currentDrop = 0;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Visible;
                    parent.heroMode.instructionsLabel.Content = stringCelebration;
                    break;
                
            }
        }

        private void countdown_countdownFinished(object sender)
        {
            switch(myState)
            {
                case States.PostureTeaching:
                    myState = States.ValueSelection;
                   
                    break;
            }
        }

       
        public void analyseCycle()
        {
            try
            {
               

                bfpa = parent.bodyFrameHandler.bodyFramePreAnalysis;
                ffpa = parent.faceFrameHandler.faceFramePreAnalysis;
                switch (myState)
                {
                    case States.PostureTeaching:
                        postureTeaching();
                        break;
                    case States.ValueSelection:
                    case States.PowerSelection:
                        PowerSelection();
                        break;
                    case States.Inspiration1:
                        InspirationOne();
                        break;
                    case States.Inspiration2:
                        InspirationTwo();
                        break;
                    case States.Impact:
                        Impact();
                        break;
                    case States.Celebration:
                        Celebration();
                        break;
                    
                }
            }
            catch
            {

            }
           
        }

        private void Celebration()
        {
            timeForAction = 6000;
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if(finishCelebration==false)
            {
                 if (checkingPosture() == true)
            {

                parent.heroMode.instructionsLabel.Content = stringCelebration;
                parent.heroMode.heroCelebration.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Visible;

                parent.heroMode.herofeedback.Content = "Raise Arms!";

                if(bfpa.heroWinning==true)
                {
                    if(startCelebration==false)
                    {
                        parent.heroMode.music.Stop();
                        parent.heroMode.music.Play();
                        startCelebration = true;
                        celebrationStarted = currentTime;
                    }
                    if(currentTime-celebrationStarted>timeToMoveCelebration && firstCelebration==false)
                    {
                        parent.heroMode.heroCelebration.instructionRectangle1.Visibility = System.Windows.Visibility.Visible;
                        parent.heroMode.heroCelebration.instructionsLabel1.Visibility = System.Windows.Visibility.Visible;
                        firstCelebration = true;
                        celebrationContinued = currentTime;
                        
                    }
                    if (currentTime - celebrationContinued > timeToMoveCelebration && firstCelebration == true && secondCelebration==false)
                    {
                        parent.heroMode.heroCelebration.instructionRectangle2.Visibility = System.Windows.Visibility.Visible;
                        parent.heroMode.heroCelebration.instructionsLabel2.Visibility = System.Windows.Visibility.Visible;
                        firstCelebration = true;
                        secondCelebration = true;
                        endCelebration = currentTime;
                    }
                    if (currentTime - endCelebration > timeToMoveCelebration && secondCelebration == true )
                    {
                        parent.heroMode.heroCelebration.nextButton.Visibility = System.Windows.Visibility.Visible;
                        parent.heroMode.heroCelebration.nextButton.IsEnabled = true;
                        finishCelebration = true;
                    }
                    
                }

            }
            else
            {
                celebrationStarted = currentTime;
                celebrationContinued = currentTime;
                endCelebration = currentTime;
                parent.heroMode.instructionsLabel.Content = stringCorrectPosture;
                parent.heroMode.heroCelebration.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Visible;
            }
            }
           
        }

        private void Impact()
        {
            if (checkingPosture() == true)
            {
                parent.heroMode.instructionsLabel.Content = stringImpact;
                parent.heroMode.heroImpact.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Collapsed;
                checkDropsImpact();
            }
            else
            {
                parent.heroMode.instructionsLabel.Content = stringCorrectPosture;
                parent.heroMode.heroImpact.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Visible;
            }
        }

        

        private void InspirationTwo()
        {
            if (checkingPosture() == true)
            {

                float xHead = 0.1f;
                float factor = 345.45f;
                float displacement = 573;
                if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
                {
                    xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
                }

                Canvas.SetLeft(parent.heroMode.heroInspiration2.backgroundImg, factor * xHead + displacement);
                Canvas.SetLeft(parent.heroMode.heroInspiration2.ellipse, factor * xHead + displacement + 50);
                Canvas.SetLeft(parent.heroMode.heroInspiration2.line1, factor * xHead + displacement - 40);
                Canvas.SetLeft(parent.heroMode.heroInspiration2.line2, factor * xHead + displacement + 110);
                Canvas.SetLeft(parent.heroMode.heroInspiration2.line3, factor * xHead + displacement + 190);

                targetLeft1 = factor * xHead + displacement;
                targetLeft2 = targetLeft1 + 110;



                parent.heroMode.instructionsLabel.Content = stringInspiration2;
                parent.heroMode.heroInspiration2.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Collapsed;
                checkDropsInspiration2();
            }
            else
            {
                parent.heroMode.instructionsLabel.Content = stringCorrectPosture;
                parent.heroMode.heroInspiration2.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void InspirationOne()
        {
            if (checkingPosture() == true)
            {
                parent.heroMode.instructionsLabel.Content = stringInspiration1;
                parent.heroMode.inspiration1.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Collapsed;
                
            }
            else
            {
                parent.heroMode.instructionsLabel.Content = stringCorrectPosture;
                parent.heroMode.inspiration1.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void GamePlay()
        {
            

        }

       

        private void PowerSelection()
        {
            if(checkingPosture()==true)
            {

                float xHead=0.1f;
                float factor = 345.45f;
                float displacement = 573;
                if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
                {
                    xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
                }
                
                Canvas.SetLeft(parent.heroMode.heroPower.backgroundImg, factor * xHead + displacement);
                Canvas.SetLeft(parent.heroMode.heroPower.ellipse, factor * xHead + displacement+50);
                Canvas.SetLeft(parent.heroMode.heroPower.line1, factor * xHead + displacement-40);
                Canvas.SetLeft(parent.heroMode.heroPower.line2, factor * xHead + displacement+110);
                Canvas.SetLeft(parent.heroMode.heroPower.line3, factor * xHead + displacement + 190);

                targetLeft1 = factor * xHead + displacement;
                targetLeft2 = targetLeft1 + 110;

                parent.heroMode.instructionsLabel.Content = stringPowerSelection;
                parent.heroMode.heroPower.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Collapsed;
                checkDrops();
            }
            else
            {
                parent.heroMode.instructionsLabel.Content = stringCorrectPosture;
                parent.heroMode.heroPower.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroAvatar.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.herofeedback.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.talkBubble.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void checkDrops()
        {
            parent.heroMode.HeroAffirmation.selections = "";
            int dropcount = 0;
            if (parent.heroMode.heroPower.Element1.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element1.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element2.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element2.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element3.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element3.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element4.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element4.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element5.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element5.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element6.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element6.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element7.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element7.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element8.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element8.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element9.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element9.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroPower.Element10.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroPower.Element10.Child).getText() + "\n";
                dropcount++;
            }

            if(currentDrop<dropcount)
            {
                currentDrop = dropcount;
                parent.heroMode.coin.Stop();
                parent.heroMode.coin.Play();
            }

            if (dropcount == 1)
            {
                parent.heroMode.heroPower.line1.Visibility = System.Windows.Visibility.Visible;
            }
            else if (dropcount == 2)
            {
                parent.heroMode.heroPower.line2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (dropcount == 3)
            {
                parent.heroMode.heroPower.line3.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroPower.nextButton.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroPower.nextButton.IsEnabled = true;
                
                parent.heroMode.heroPower.Element10.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element1.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element2.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element3.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element4.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element5.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element6.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element7.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element8.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroPower.Element9.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void checkDropsInspiration2()
        {
            parent.heroMode.HeroAffirmation.selections = "";
            int dropcount = 0;
            if (parent.heroMode.heroInspiration2.Element1.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element1.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element2.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element2.Child).getText() + "\n"; ;
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element3.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element3.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element4.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element4.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element5.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element5.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element6.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element6.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element7.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element7.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element8.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element8.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element9.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element9.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroInspiration2.Element10.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroInspiration2.Element10.Child).getText() + "\n";
                dropcount++;
            }

            if (currentDrop < dropcount)
            {
                currentDrop = dropcount;
                parent.heroMode.coin.Stop();
                parent.heroMode.coin.Play();
            }

            if(dropcount==1)
            {
                parent.heroMode.heroInspiration2.line1.Visibility = System.Windows.Visibility.Visible;
            }
            else if(dropcount==2)
            {
                parent.heroMode.heroInspiration2.line2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (dropcount==3)
            {
                parent.heroMode.heroInspiration2.line3.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroInspiration2.nextButton.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroInspiration2.nextButton.IsEnabled = true;
                parent.heroMode.heroInspiration2.Element10.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element1.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element2.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element3.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element4.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element5.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element6.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element7.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element8.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroInspiration2.Element9.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void checkDropsImpact()
        {
            parent.heroMode.HeroAffirmation.selections = "";
            int dropcount = 0;
            if (parent.heroMode.heroImpact.Element1.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element1.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element2.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element2.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element3.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element3.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element4.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element4.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element5.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element5.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element6.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element6.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element7.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element7.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element8.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element8.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element9.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element9.Child).getText() + "\n";
                dropcount++;
            }
            if (parent.heroMode.heroImpact.Element10.dropCorrecly == true)
            {
                parent.heroMode.HeroAffirmation.selections = parent.heroMode.HeroAffirmation.selections + ((DropInterface)parent.heroMode.heroImpact.Element10.Child).getText() + "\n";
                dropcount++;
            }

            if (currentDrop < dropcount)
            {
                currentDrop = dropcount;
                parent.heroMode.coin.Stop();
                parent.heroMode.coin.Play();
            }

            if (dropcount == 1)
            {
                parent.heroMode.heroImpact.line1.Visibility = System.Windows.Visibility.Visible;
            }
            else if (dropcount == 2)
            {
                parent.heroMode.heroImpact.line2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (dropcount == 3)
            {
                parent.heroMode.heroImpact.line3.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroImpact.nextButton.Visibility = System.Windows.Visibility.Visible;
                parent.heroMode.heroImpact.nextButton.IsEnabled = true;
                parent.heroMode.heroImpact.Element10.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element1.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element2.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element3.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element4.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element5.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element6.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element7.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element8.Visibility = System.Windows.Visibility.Collapsed;
                parent.heroMode.heroImpact.Element9.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public void postureTeaching()
        {
            if(parent.heroMode!=null)
            {
                
              //  parent.heroMode.instructionsLabel.Content = parent.heroMode.InstructionHeroPosture;
                if (bfpa.heroLegRight == false)
                {
                    parent.heroMode.herofeedback.Content = "Separate legs";
                }
                else if (bfpa.heroBack == false)
                {
                    parent.heroMode.herofeedback.Content = "Shoulders Back";
                }
                else if (bfpa.heroNeck == false)
                {
                    parent.heroMode.herofeedback.Content = "Head back";
                }
                else if (bfpa.heroArmsLeft == false)
                {
                    parent.heroMode.herofeedback.Content = "Left Hand on Hip";
                }
                else if (bfpa.heroArmsRight == false)
                {
                    parent.heroMode.herofeedback.Content = "Right Hand on Hip";
                }
                else if (ffpa.smiling == false)
                {
                    parent.heroMode.herofeedback.Content = "Now Smile";
                }
                else
                {
                    parent.heroMode.herofeedback.Content = "That's the posture!";
                    myState = States.Pause;
                    parent.heroMode.instructionRectangle.Visibility = System.Windows.Visibility.Collapsed;
                    parent.heroMode.instructionsLabel.Visibility = System.Windows.Visibility.Collapsed;
                    parent.heroMode.HeroAffirmation.setStrings(1);
                    parent.heroMode.HeroAffirmation.Visibility = System.Windows.Visibility.Visible;

                }
            }
            
            
        }

        public bool checkingPosture()
        {
            bool value = true;

           double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

           if (bfpa.heroLegRight == false ) 
            {
                if (currentTime - counterLegs > timeForAction)
                value = false;
            }
            else
            {
                counterLegs = currentTime;
            }
           if (bfpa.heroBack == false) if( currentTime - counterBack > timeForAction)
            {
                if (currentTime - counterBack > timeForAction)
                value = false;
            }
            else
            {
                counterBack = currentTime;
            }
           if (bfpa.heroNeck == false ) 
            {
                if (currentTime - counterNeck > timeForAction)
                value = false;
            }
            else
            {
                counterNeck = currentTime;
            }
           if (ffpa.smiling == false) 
            {
                if (currentTime - counterSmile > timeForAction)
                value = false;
            }
            else
            {
                counterSmile = currentTime;
            }
            if(value==false)
            {
                postureTeaching();
            }

         
            return value;
        }

        void values_selectionEvent(object sender, string x)
        {
            parent.heroMode.value = x;
            parent.heroMode.values.Visibility = System.Windows.Visibility.Collapsed;
            

        }
    }
}
