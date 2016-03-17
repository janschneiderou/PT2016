using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PresentationTrainer
{
    
   public class RulesAnalyzer
    {
       
        MainWindow parent;

        bool noMistake = true;
        bool freeStyle = true;

        public bool interruption = true;
        public bool interrupted = false;

        public delegate void HmmmEvent(object sender, EventArgs e);
        public event HmmmEvent hmmmEvent;
        public delegate void TooLoudEvent(object sender, string e);
        public event TooLoudEvent tooLoudEvent;
        public delegate void MoveMoreEvent(object sender, string e);
        public event MoveMoreEvent moveMoreEvent;
        public delegate void BadPostureEvent(object sender, EventArgs e);
        public event BadPostureEvent badPostureEvent;
        public delegate void NoMistakeEvent(object sender, EventArgs e);
        public event NoMistakeEvent noMistakeEvent;
        public delegate void PeriodicMovementsEvent(object sender, string e);
        public event PeriodicMovementsEvent periodicMovementsEvent;

        public delegate void FeedBackEvent(object sender, PresentationEvent x);
        public event FeedBackEvent feedBackEvent;
        public delegate void VolumeMistakeEvent(object sender, PresentationEvent x);
        public event VolumeMistakeEvent volumeMistakeEvent;
        public delegate void CadenceMistakeEvent(object sender, PresentationEvent x);
        public event CadenceMistakeEvent cadenceMistakeEvent;
        public delegate void PostureMistakeEvent(object sender, PresentationEvent x);
        public event PostureMistakeEvent postureMistakeEvent;
        public delegate void HandMovementMistakeEvent(object sender, PresentationEvent x);
        public event HandMovementMistakeEvent handMovementMistakeEvent;

        PeriodicMovements periodicMovements;
        BodyFramePreAnalysis bfpa;
        Body oldBody;
        AudioPreAnalysis apa;

        //List works same as array when calling: Mistake x = volumeMistakeList[0]
        // example for new mistake: Mistake x = new Mistake(2, Mistake.Type.badPosture);
        // integers are the reference for the feedbackList
        private List<Mistake> volumeMistakeList = new List<Mistake>(); 
        private List<Mistake> cadenceMistakeList = new List<Mistake>(); 
        private List<Mistake> postureMistakeList = new List<Mistake>(); 
        private List<Mistake> handMovementMistakeList = new List<Mistake>();
        private List<Mistake> shortMistakeList = new List<Mistake>();

       //Copy used to save mistakes that were removed during an interruption
        private List<Mistake> volumeMistakeListCopy = new List<Mistake>(); 
        private List<Mistake> cadenceMistakeListCopy = new List<Mistake>(); 
        private List<Mistake> postureMistakeListCopy = new List<Mistake>(); 
        private List<Mistake> handMovementMistakeListCopy = new List<Mistake>(); 
        private List<Mistake> shortMistakeListCopy = new List<Mistake>();

        public List<PresentationEvent> feedBackList = new List<PresentationEvent>(); //first integer is number in the queue, the second integer stands for which mistake list the feedback belongs to
        public PresentationEvent fired = null;
        public bool firedHasEnded = false;
        private int[] interruptions = new int[9]; 
       //where each entry respectively stands for 0:loud, 1:soft, 2:longSpeakingTime, 3:longPause, 4:shortSpeakingTime, 5:shortPause, 6:badPosture, 7:noHandMovement, 8:tooMuchHandMovement 

        private List<Style> pauseStyleList = new List<Style>();
        private List<Style> postureStyleList = new List<Style>();
        private List<Style> speakingTimeStyleList = new List<Style>();


        bool volumeMistake = false;
        bool cadenceMistake = false;
        bool postureMistake = false;
        bool handMovementMistake = false;
        private bool shortMistake = false;

        public double startHandMovingTime = -1;

        bool postureStyleFlag = false;
       //bool other style Flags

       public static int nrOfMistakes = 0;
       public static int nrOfVolumeMistakes = 0;
       public static int nrOfCadenceMistakes = 0;
       public static int nrOfPostureMistakes = 0;
       public static int nrOfHandMovementMistakes = 0;
       public static int nrOfShortMistakes = 0;
       public static double averageMistakePoints = 0;
       public static double standardDeviationOfMistakePoints = 0;
       public static Mistake biggestOfAllMistakes = new Mistake(Mistake.Type.volume, Mistake.SubType.moduleVolume, Mistake.GravityType.bad);
       public static Mistake.SubType mostRepeatedMistake = Mistake.SubType.moduleVolume;
       public static int repetitions = 0;
       public static double pointsOfBiggestOfAllMistakes = 0;

       public static Mistake.SubType triggered = Mistake.SubType.moduleVolume;
       public static int REPETITION_THRESHOLD = 8;
       public static int MAX_POINTS_THRESHOLD = 50;       
       public static bool isRepetitionInterruption = false; //false means max points interruption

       public static int nrOfStyles = 0;
       public static double averageStylePoints = 0;
       public static double standardDeviationOfStylePoints = 0;
       public static Style biggestOfAllStyles = null;
       public static Style mostRepeatedStyle = null;

        public RulesAnalyzer(MainWindow parent)
        {
            this.parent = parent;
            periodicMovements = new PeriodicMovements();

        }

        public void setFreeStyle(bool fs)
        {
            freeStyle = fs;
        }

        public void AnalyseRules()
        {
            noMistake = true;
            bfpa = this.parent.bodyFrameHandler.bodyFramePreAnalysis;
            apa = this.parent.audioHandler.audioPreAnalysis;
   
            checkPeriodicMovements();
           

            //DETECT HAND MOVEMENT CODE 
            bfpa.calcIsMovingArms();
             /**Structure of switch blocks: 
             * If good:
             - end mistake that is busy, if there is one.
             * If not good:
             -Same Type Mistake Busy
             --Change gravity to worst kind
             --set bool to true
             -Different Type Mistake Busy
             --End that mistake and add the new one
             --set bool to true
             -No Mistake Busy
             --Start new mistake
             --set bool to true
             * */

             volumeMistake = false;
             cadenceMistake = false;
             postureMistake = false;
             handMovementMistake = false;

            if(parent.freestyleMode!=null)
            {
                if (parent.freestyleMode.myState == PresentationTrainer.FreestyleMode.currentState.play)
                {
                    checkMistakes();
                    checkStyle();
                    Mistake biggestMistake = findBiggestMistake();
                    //Style biggestStyle = findBiggestStyle();
                    fireBiggestEvent(biggestMistake, null);
                    
                    
                }
            }
             
             saveCurrentBodyAsOldBody();
             
        }

        
               
        public void checkStyle()
        {
            checkPostureStyle();
            // check other styles
            checkPauseStyle(); // Not correctly implemented yet
        }

        public void checkPostureStyle()
        {
            if (bfpa.bodyPosture == BodyFramePreAnalysis.Posture.good)
            {
                if (bfpa.resetPosture && !apa.isSpeaking)
                {
                    if (postureStyleList.Count > 0)
                    {
                        if (postureStyleList[postureStyleList.Count - 1].hasEnded)
                        {
                            Style z = new Style(Style.Type.goodPosture);
                            postureStyleList.Add(z);
                            postureStyleFlag = true;
                        }
                    }
                    else
                    {
                        Style z = new Style(Style.Type.goodPosture);
                        postureStyleList.Add(z);
                        postureStyleFlag = true;
                    }
                }
                else if (apa.isSpeaking && postureStyleList.Count > 0)
                {
                    postureStyleList[postureStyleList.Count - 1].ended();
                    postureStyleFlag = false;
                }
            }
            else
            {
                if (postureStyleList.Count > 0)
                    postureStyleList[postureStyleList.Count - 1].ended();
                postureStyleFlag = false;
            }
        }

        private void checkPauseStyle()
        {
            if (!apa.isSpeaking && apa.voiceCadence == AudioPreAnalysis.Cadence.goodPause)
            {
                int size = postureStyleList.Count;
                if (size > 0)
                {
                    if (!postureStyleList[size-1].hasEnded){
                        postureStyleList[size - 1].ended();
                        Style z = new Style(Style.Type.goodPause);
                        postureStyleList.Add(z);
                    }
                }
                else
                {
                    Style z = new Style(Style.Type.goodPause);
                    postureStyleList.Add(z);
                }
                    
            }            
        }

        public void checkMistakes()
        {
            checkVolumeMistake();
            checkCadenceMistake();
            checkPostureMistake();
            checkHandMovementMistake();
            //checkShortMistake();
            // checkHmmmMistake();
            //checkPeriodicMovementsMistake();
        }

       public void checkVolumeMistake()
       {
           switch (apa.voiceVolume)
           {
               case AudioPreAnalysis.Volume.loud:
                   {
                       if (volumeMistakeList.Count > 0)
                       {
                           Mistake x = volumeMistakeList[volumeMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.loud, Mistake.GravityType.loud);
                               volumeMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.soft)
                           {
                               x.ended();
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.loud, Mistake.GravityType.loud);
                               volumeMistakeList.Add(z);
                           }
                           //else, do nothing, since if previous was loud mistake, no change, and if previous was veryLoud mistake, dont change gravity.
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.loud, Mistake.GravityType.loud);
                           volumeMistakeList.Add(z);
                       }
                       volumeMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Volume.veryLoud:
                   {
                       if (volumeMistakeList.Count > 0)
                       {
                           Mistake x = volumeMistakeList[volumeMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.loud, Mistake.GravityType.veryLoud);
                               volumeMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.soft)
                           {
                               x.ended();
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.loud, Mistake.GravityType.veryLoud);
                               volumeMistakeList.Add(z);
                           }
                           else if (x.gravityType == Mistake.GravityType.loud)
                           {
                               x.setGravity(Mistake.GravityType.veryLoud);
                           }
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.loud, Mistake.GravityType.veryLoud);
                           volumeMistakeList.Add(z);
                       }
                       volumeMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Volume.soft:
                   {
                       if (volumeMistakeList.Count > 0)
                       {
                           Mistake x = volumeMistakeList[volumeMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.soft, Mistake.GravityType.soft);
                               volumeMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.loud)
                           {
                               x.ended();
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.soft, Mistake.GravityType.soft);
                               volumeMistakeList.Add(z);
                           }
                           //else, do nothing, since if previous was soft mistake, no change, and if previous was verySoft mistake, dont change gravity.
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.soft, Mistake.GravityType.soft);
                           volumeMistakeList.Add(z);
                       }
                       volumeMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Volume.verySoft:
                   {
                       if (volumeMistakeList.Count > 0)
                       {
                           Mistake x = volumeMistakeList[volumeMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.soft, Mistake.GravityType.verySoft);
                               volumeMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.loud)
                           {
                               x.ended();
                               Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.soft, Mistake.GravityType.verySoft);
                               volumeMistakeList.Add(z);
                           }
                           else if (x.gravityType == Mistake.GravityType.soft)
                           {
                               x.setGravity(Mistake.GravityType.verySoft);
                           }
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.volume, Mistake.SubType.soft, Mistake.GravityType.verySoft);
                           volumeMistakeList.Add(z);
                       }
                       volumeMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Volume.good:
                   {
                       if (volumeMistakeList.Count > 0)
                       {
                           Mistake x = volumeMistakeList[volumeMistakeList.Count - 1];
                           if (!x.hasEnded)
                           {
                               x.ended();
                           }
                       }
                       //since bool is set to false at the start, no need to do it again.
                       break;
                   }

           }
       }

       public void checkCadenceMistake()
       {
           switch (apa.voiceCadence)
           {
               case AudioPreAnalysis.Cadence.longPause:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longPause, Mistake.GravityType.longPause);
                               cadenceMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.shortPause)
                           {
                               x.setGravity(Mistake.GravityType.longPause);
                               x.subType = Mistake.SubType.longPause;
                           }
                           else if (x.subType != Mistake.SubType.longPause)
                           {
                               if (x.subType == Mistake.SubType.shortSpeakingTime)
                               {
                                   x.shortMistake = true;
                                   shortMistake = true;
                                   x.setGravity(Mistake.GravityType.shortSpeakingTime);
                                   shortMistakeList.Add(x);
                                   cadenceMistakeList.Remove(x);
                               }
                               else
                               {
                                   x.ended();
                               }                               
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longPause, Mistake.GravityType.longPause);
                               cadenceMistakeList.Add(z);
                           }
                           //else, do nothing, since if previous was longPause mistake, no change, and if previous was veryLongPause mistake, dont change gravity.
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longPause, Mistake.GravityType.longPause);
                           cadenceMistakeList.Add(z);
                       }
                       cadenceMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Cadence.veryLongPause:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longPause, Mistake.GravityType.veryLongPause);
                               cadenceMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.shortPause)
                           {
                               x.setGravity(Mistake.GravityType.veryLongPause);
                               x.subType = Mistake.SubType.longPause;
                           }
                           else if (x.subType != Mistake.SubType.longPause)
                           {
                               if (x.subType == Mistake.SubType.shortSpeakingTime)
                               {
                                   x.shortMistake = true;
                                   shortMistake = true;
                                   x.setGravity(Mistake.GravityType.shortSpeakingTime);
                                   shortMistakeList.Add(x);
                                   cadenceMistakeList.Remove(x);
                               }
                               else
                               {
                                   x.ended();
                               }        
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longPause, Mistake.GravityType.veryLongPause);
                               cadenceMistakeList.Add(z);
                           }
                           else if (x.gravityType == Mistake.GravityType.longPause)
                           {
                               x.setGravity(Mistake.GravityType.veryLongPause);
                           }
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longPause, Mistake.GravityType.veryLongPause);
                           cadenceMistakeList.Add(z);
                       }
                       cadenceMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Cadence.longSpeakingTime:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longSpeakingTime, Mistake.GravityType.longSpeakingTime);
                               cadenceMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.shortSpeakingTime)
                           {
                               x.setGravity(Mistake.GravityType.longSpeakingTime);
                               x.subType = Mistake.SubType.longSpeakingTime;
                           }
                           else if (x.subType != Mistake.SubType.longSpeakingTime)
                           {
                               if (x.subType == Mistake.SubType.shortPause)
                               {
                                   x.shortMistake = true;
                                   shortMistake = true;
                                   x.setGravity(Mistake.GravityType.shortPause);
                                   shortMistakeList.Add(x);
                                   cadenceMistakeList.Remove(x);
                               }
                               else
                               {
                                   x.ended();
                               }        
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longSpeakingTime, Mistake.GravityType.longSpeakingTime);
                               cadenceMistakeList.Add(z);
                           }
                           //else, do nothing, since if previous was longPause mistake, no change, and if previous was veryLongNoPause mistake, dont change gravity.
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longSpeakingTime, Mistake.GravityType.longSpeakingTime);
                           cadenceMistakeList.Add(z);
                       }
                       cadenceMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Cadence.veryLongSpeakingTime:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longSpeakingTime, Mistake.GravityType.veryLongSpeakingTime);
                               cadenceMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.shortSpeakingTime)
                           {
                               x.setGravity(Mistake.GravityType.veryLongSpeakingTime);
                               x.subType = Mistake.SubType.longSpeakingTime;
                           }
                           else if (x.subType != Mistake.SubType.longSpeakingTime)
                           {
                               if (x.subType == Mistake.SubType.shortPause)
                               {
                                   x.shortMistake = true;
                                   shortMistake = true;
                                   x.setGravity(Mistake.GravityType.shortPause);
                                   shortMistakeList.Add(x);
                                   cadenceMistakeList.Remove(x);
                               }
                               else
                               {
                                   x.ended();
                               }        
                               Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longSpeakingTime, Mistake.GravityType.veryLongSpeakingTime);
                               cadenceMistakeList.Add(z);
                           }
                           else if (x.gravityType == Mistake.GravityType.longSpeakingTime)
                           {
                               x.setGravity(Mistake.GravityType.veryLongSpeakingTime);
                           }
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.longSpeakingTime, Mistake.GravityType.veryLongSpeakingTime);
                           cadenceMistakeList.Add(z);
                       }
                       cadenceMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Cadence.goodPause:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
                           if (!x.hasEnded)
                           {
                               x.ended();
                           }
                       }
                       //since bool is set to false at the start, no need to do it again.
                       break;
                   }
               case AudioPreAnalysis.Cadence.goodSpeakingTime:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
                           if (!x.hasEnded)
                           {
                               x.ended();
                           }
                       }
                       //since bool is set to false at the start, no need to do it again.
                       break;
                   }
               case AudioPreAnalysis.Cadence.shortPause:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];

                           //if (x.subType == Mistake.SubType.shortSpeakingTime)
                           //{
                           //    x.shortMistake = true;
                           //    shortMistake = true;
                           //    x.setGravity(Mistake.GravityType.shortSpeakingTime);
                           //    shortMistakeList.Add(x);
                           //    cadenceMistakeList.Remove(x);
                           //}
                           //else 
                               if (!x.hasEnded)
                           {
                               x.ended();
                           }
                          
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.shortPause, Mistake.GravityType.shortPause);
                           cadenceMistakeList.Add(z);
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.shortPause, Mistake.GravityType.shortPause);
                           cadenceMistakeList.Add(z);
                       }
                       //pauseMistake = true;
                       break;
                   }
               case AudioPreAnalysis.Cadence.shortSpeakingTime:
                   {
                       if (cadenceMistakeList.Count > 0)
                       {
                           Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];

                           //if (x.subType == Mistake.SubType.shortPause)
                           //{
                           //    x.shortMistake = true;
                           //    shortMistake = true;
                           //    x.setGravity(Mistake.GravityType.shortPause);
                           //    shortMistakeList.Add(x);
                           //    cadenceMistakeList.Remove(x);
                           //}
                           //else 
                               if (!x.hasEnded)
                           {
                               x.ended();
                           }
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.shortSpeakingTime, Mistake.GravityType.shortSpeakingTime);
                           cadenceMistakeList.Add(z);
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.cadence, Mistake.SubType.shortSpeakingTime, Mistake.GravityType.shortSpeakingTime);
                           cadenceMistakeList.Add(z);
                       }
                       //pauseMistake = true;
                       break;
                   }
           }
       }

       public void checkPostureMistake()
       {
           switch (bfpa.bodyPosture)
           {
               case BodyFramePreAnalysis.Posture.bad:
                   {
                       if (postureMistakeList.Count > 0)
                       {
                           Mistake x = postureMistakeList[postureMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.posture, Mistake.SubType.badPosture, Mistake.GravityType.bad);
                               postureMistakeList.Add(z);
                           }
                           // if x has not ended, no need to do anything!
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.posture, Mistake.SubType.badPosture, Mistake.GravityType.bad);
                           postureMistakeList.Add(z);
                       }
                       postureMistake = true;
                       break;
                   }
               case BodyFramePreAnalysis.Posture.good:
                   {
                       if (postureMistakeList.Count > 0)
                       {
                           Mistake x = postureMistakeList[postureMistakeList.Count - 1];
                           if (!x.hasEnded)
                           {
                               x.ended();
                           }
                       }
                       //since bool is set to false at the start, no need to do it again.
                       break;
                   }
           }
       }

       public void checkHandMovementMistake()
       {
           checkEnoughHandMovement();
           switch (bfpa.handMovement)
           {
               case BodyFramePreAnalysis.HandMovement.good:
                   {
                       if (handMovementMistakeList.Count > 0)
                       {
                           Mistake x = handMovementMistakeList[handMovementMistakeList.Count - 1];
                           if (!x.hasEnded)
                           {
                               x.ended();
                           }
                       }
                       //since bool is set to false at the start, no need to do it again.
                       break;
                   }
               case BodyFramePreAnalysis.HandMovement.notEnough:
                   {
                       if (handMovementMistakeList.Count > 0)
                       {
                           Mistake x = handMovementMistakeList[handMovementMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.handMovement, Mistake.SubType.noHandMovement, Mistake.GravityType.notEnough);
                               handMovementMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.tooMuchHandMovement)
                           {
                               x.ended();
                               Mistake z = new Mistake(Mistake.Type.handMovement, Mistake.SubType.noHandMovement, Mistake.GravityType.notEnough);
                               handMovementMistakeList.Add(z);
                           }
                           //if notEnoughMovement mistake, nothing changes
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.handMovement, Mistake.SubType.noHandMovement, Mistake.GravityType.notEnough);
                           handMovementMistakeList.Add(z);
                       }
                       handMovementMistake = true;
                       break;
                   }
               case BodyFramePreAnalysis.HandMovement.tooMuch:
                   {
                       if (handMovementMistakeList.Count > 0)
                       {
                           Mistake x = handMovementMistakeList[handMovementMistakeList.Count - 1];
                           if (x.hasEnded)
                           {
                               Mistake z = new Mistake(Mistake.Type.handMovement, Mistake.SubType.tooMuchHandMovement, Mistake.GravityType.tooMuch);
                               handMovementMistakeList.Add(z);
                           }
                           else if (x.subType == Mistake.SubType.noHandMovement)
                           {
                               x.ended();
                               Mistake z = new Mistake(Mistake.Type.handMovement, Mistake.SubType.tooMuchHandMovement, Mistake.GravityType.tooMuch);
                               handMovementMistakeList.Add(z);
                           }
                           //if tooMuchMovement mistake, nothing changes
                       }
                       else
                       {
                           Mistake z = new Mistake(Mistake.Type.handMovement, Mistake.SubType.tooMuchHandMovement, Mistake.GravityType.tooMuch);
                           handMovementMistakeList.Add(z);
                       }
                       handMovementMistake = true;
                       break;
                   }
           }
       }

       private void checkEnoughHandMovement()
       {
           if(apa.isSpeaking)
           {
               TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
               if(bfpa.areHandsMoving)
               {
                   bfpa.handMovement = BodyFramePreAnalysis.HandMovement.good;
                   startHandMovingTime=-1;
               }
               else if(startHandMovingTime==-1)
               {
                   
                   startHandMovingTime = now.TotalMilliseconds;
                   bfpa.handMovement = BodyFramePreAnalysis.HandMovement.good;
               }
               else if (now.TotalMilliseconds-startHandMovingTime>1200)
               {
                   bfpa.handMovement = BodyFramePreAnalysis.HandMovement.notEnough;
               }
           }
           else
           {
               startHandMovingTime = -1;
               bfpa.handMovement = BodyFramePreAnalysis.HandMovement.good;
           }
       }       

       public Mistake findBiggestMistake()
       {
           //-Check for each input if there is a mistake, by a boolean set in the switch blocks, therefore no need to check if lists are not empty.
           //-Find biggest mistake (use getPointsOfMistake method
           Mistake biggestMistake = null;
           double mistakeGravity = 0;
           if (volumeMistake)
           {
               Mistake x = volumeMistakeList[volumeMistakeList.Count - 1];
               if (x.checkIfVolumeMistakeLongEnough())
               {
                   double g = getPointsOfMistake(x);
                   if (g > mistakeGravity)
                   {
                       biggestMistake = x;
                       mistakeGravity = g;
                   }
               }               
           }
           if (cadenceMistake)
           {
               Mistake x = cadenceMistakeList[cadenceMistakeList.Count - 1];
               if (x.subType != Mistake.SubType.shortPause || x.subType != Mistake.SubType.shortSpeakingTime)
               {
                   double g = getPointsOfMistake(x);
                   if (g > mistakeGravity)
                   {
                       biggestMistake = x;
                       mistakeGravity = g;
                   }
               }               
           }
           if (postureMistake)
           {
               Mistake x = postureMistakeList[postureMistakeList.Count - 1];
               double g = getPointsOfMistake(x);
               if (g > mistakeGravity)
               {
                   biggestMistake = x;
                   mistakeGravity = g;
               }
           }
           if (handMovementMistake)
           {
               Mistake x = handMovementMistakeList[handMovementMistakeList.Count - 1];
               double g = getPointsOfMistake(x);
               if (g > mistakeGravity)
               {
                   biggestMistake = x;
                   mistakeGravity = g;
               }
           }
           if (shortMistake)
           {
               Mistake x = shortMistakeList[shortMistakeList.Count - 1];
               double g = getPointsOfMistake(x);
               if (x.hasEnded)
               {
                   int y = 0;
               }
               if (g > mistakeGravity)
               {
                   biggestMistake = x;
                   mistakeGravity = g;
               }
           }
           else
           {
               int size = shortMistakeList.Count;
               if (size > 0)
               {
                   for (int i = 0; i < size; i++)
                   {
                       Mistake x = shortMistakeList[i];
                       //if it has been more than the threshold of seconds, dont display the mistake anymore
                       if (!x.hasEnded && DateTime.Now.TimeOfDay.TotalMilliseconds - x.timeStarted > 1000)
                       {
                           x.ended();
                       }  
                   }                               
               }               
           }
           shortMistake = false; //shortMistake has only one chance of being fired, shouldnt have this later on.

           return biggestMistake;
       }

       private Style findBiggestStyle()
       {
           Style x = null;
           int gravityX = 0;
           if (postureStyleFlag)
           {
               Style z = postureStyleList[postureStyleList.Count - 1];
               if (z.gravity > gravityX)
               {
                   x = z;
               }
           }

           return x;
       }

       private void fireBiggestEvent(Mistake biggestMistake, Style biggestStyle)
       {
           double mistakeGravity=0;
           double styleGravity = 0;
           if (biggestMistake != null)
               mistakeGravity = getPointsOfMistake(biggestMistake);
           if (biggestStyle != null)
               styleGravity = biggestStyle.gravity;

           //If no mistake, nothing will be done (mistakes should have been ended in switch blocks)
           //-Compare biggest mistake with Current feedback 
           //--If no current feedback add this mistake to feedbacklist  
           //--If current feedback has ended, add this mistake to feedbackList.
           //--If bigger than current feedback, add this mistake to feedbacklist (note that since the feedbacklist only contains references to mistakes, you only have to add new mistakes)
           //--If smaller or equal than current feedback, do nothing.            
           if (mistakeGravity > styleGravity)
           {              
                feedBackList.Add(biggestMistake);               
           }
           else if (styleGravity > mistakeGravity)
           {               
                feedBackList.Add(biggestStyle);               
           }

           if (MainWindow.myState == MainWindow.States.freestyle)
           {
               int size = feedBackList.Count;
               if (size > 0)
               {
                   try
                   {
                       if (interruption)
                       {
                           //makeReport();
                           mostRepeatedMistake = getMostRepeatedMistake();
                           biggestOfAllMistakes = (Mistake)feedBackList[feedBackList.Count - 1];
                           if (repetitions > REPETITION_THRESHOLD && !interrupted)
                           {
                               isRepetitionInterruption = true;
                               triggered = mostRepeatedMistake;
                               increaseInterruptions(triggered);
                               clearReport();
                               //parent.freestyleMode.loadInterruption();                                  
                               parent.freestyleMode.doInterruption();
                               //makeLog();                                   
                               interrupted = true;
                               //Mistake x = (Mistake) feedBackList[feedBackList.Count -1];
                               //resetForMistake(x);
                           }
                           else if (getPointsOfMistake(biggestOfAllMistakes) > MAX_POINTS_THRESHOLD && !interrupted)
                           {
                               isRepetitionInterruption = false;
                               pointsOfBiggestOfAllMistakes = getPointsOfMistake(biggestOfAllMistakes);
                               triggered = biggestOfAllMistakes.subType;
                               increaseInterruptions(triggered);
                               clearReport();
                               //parent.freestyleMode.loadInterruption();
                               parent.freestyleMode.doInterruption();
                               //makeLog();
                               interrupted = true;
                               //Mistake x = (Mistake)feedBackList[feedBackList.Count - 1];
                               //resetForMistake(x);
                           }

                       }

                       PresentationEvent temp = feedBackList[size - 1];
                       if (fired == null || fired != temp)
                       {                           
                           feedBackEvent(this, temp);
                           fired = temp;
                           firedHasEnded = false;
                           
                       }                       
                       else //fired == temp
                       {
                           if (temp.hasEnded && !firedHasEnded)
                           {
                               feedBackEvent(this, temp);
                               firedHasEnded = true;
                               
                           }                           
                       }
                       
                   }
                   catch
                   {

                   }

               }

           }
           else 
           {
               //sendEvent(volumeMistakeList[volumeMistakeList.Count - 1]);
               //sendEvent(cadenceMistakeList[cadenceMistakeList.Count - 1]);
               //sendEvent(postureMistakeList[postureMistakeList.Count - 1]);
               //sendEvent(handMovementMistakeList[handMovementMistakeList.Count - 1]);
           }
       }

        public void checkPeriodicMovements()
        {
           
            if (BodyFramePreAnalysis.bodyOld  != null && bfpa.body!=null)
           {
               bool result = periodicMovements.checPeriodicMovements(bfpa.body);
                if(result)
                {
                    periodicMovementsEvent(this, periodicMovements.fired);
                }
           }
        }

        public void saveCurrentBodyAsOldBody()
        {

            bfpa.setOldBody();
            
        }

        //NOT CALIBRATED YET
        public double getPointsOfMistake(Mistake mistake)
        {
            double points = 0;
            double lengthOfMistake = DateTime.Now.TimeOfDay.TotalMilliseconds - mistake.timeStarted;
            //can be divided by 250, 500 or 1000, depending on how important the steps of time are.
            double lengthPoints = lengthOfMistake / 1000;
            
            int repeated = getRepetition(mistake);
                        

            //Calibrate this formula!
            //lengthPoints can also be a multiplier
            //repeated can also be a multiplier
            points = mistake.gravity +lengthPoints + repeated;
            // Alternative (base of the exponents still to be determent):
            //double lengthPointsMultiplier = Math.Pow(1.2, lengthPoints);
            //double repeatedMultiplier = Math.Pow(1.2, repeated);
            //points = mistake.gravity * lengthPointsMultiplier * repeatedMultiplier;
            return points;
        }

        public int getRepetition(Mistake mistake)
        {
            int repeated = 0;
            switch (mistake.type)
            {
                case Mistake.Type.volume:
                    {
                        int size = volumeMistakeList.Count;
                        if (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                if (volumeMistakeList[i].subType == mistake.subType && volumeMistakeList[i].volumeMistakeLongEnough)
                                {
                                    repeated++;
                                }
                            }
                        }
                        break;
                    }
                case Mistake.Type.cadence:
                    {
                        if (mistake.subType == Mistake.SubType.shortPause || mistake.subType == Mistake.SubType.shortSpeakingTime)
                        {
                            int size = shortMistakeList.Count;
                            if (size > 0){
                                if (mistake.shortMistake)
                                {
                                    for (int i = 0; i < size; i++)
                                    {
                                        if (shortMistakeList[i].subType == mistake.subType && shortMistakeList[i].shortMistake)
                                        {
                                            repeated++;
                                        }
                                    }
                                }   
                            }                                
                        }
                        else
                        {
                            int size = cadenceMistakeList.Count;
                            if (size > 0)
                            {
                                for (int i = 0; i < size; i++)
                                {
                                    if (cadenceMistakeList[i].subType == mistake.subType)
                                    {
                                        repeated++;
                                    }
                                }
                            }                            
                        }                        
                        
                        break;
                    }
                case Mistake.Type.posture:
                    {
                        if (postureMistakeList.Count > 0)
                        {
                            //every mistake is a bad mistake, therefore, every mistake in the mistake list should be added to repeated
                            repeated = postureMistakeList.Count;
                        }
                        break;
                    }
                case Mistake.Type.handMovement:
                    {
                        int size = handMovementMistakeList.Count;
                        if (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                if (handMovementMistakeList[i].subType == mistake.subType)
                                {
                                    repeated++;
                                }
                            }
                        }
                        break;
                    }
            }
            return repeated;
        }

        
        public void sendEvent(Mistake x)
        {
            if (!x.hasEnded)
            {
                switch (x.type)
                {
                    case Mistake.Type.volume:
                        {
                            volumeMistakeEvent(this, x);
                            break;
                        }
                    case Mistake.Type.cadence:
                        {
                            cadenceMistakeEvent(this, x);
                            break;
                        }
                    case Mistake.Type.posture:
                        {
                            postureMistakeEvent(this, x);
                            break;
                        }
                    case Mistake.Type.handMovement:
                        {
                            handMovementMistakeEvent(this, x);
                            break;
                        }
                }
            }
        }

        public Mistake.SubType getMostRepeatedMistake()
        {
            Mistake.SubType biggestSubType = Mistake.SubType.moduleVolume;
            int biggest = 0;
            int size = volumeMistakeList.Count;
            int loudR = 0;
            int softR = 0;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (volumeMistakeList[i].volumeMistakeLongEnough && volumeMistakeList[i].subType == Mistake.SubType.loud)                    
                        loudR++;
                    else if (volumeMistakeList[i].volumeMistakeLongEnough)                   
                        softR++;                    
                }
            }
            if (loudR > softR && loudR > biggest)
            {
                biggestSubType = Mistake.SubType.loud;
                biggest = loudR;
            }
            else if (softR > loudR && softR > biggest)
            {
                biggestSubType = Mistake.SubType.soft;
                biggest = softR;
            }

            size = cadenceMistakeList.Count;
            int longPR = 0;            
            int longSR = 0;            
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (cadenceMistakeList[i].subType == Mistake.SubType.longPause)
                        longPR++;                    
                    else if (cadenceMistakeList[i].subType == Mistake.SubType.longSpeakingTime)
                        longSR++;                    
                }
            }
            if (longPR > longSR && longPR > biggest)
            {
                biggestSubType = Mistake.SubType.longPause;
                biggest = longPR;
            }
            else if (longSR > longPR && longSR > biggest)
            {
                biggestSubType = Mistake.SubType.longSpeakingTime;
                biggest = longSR;
            }

            size = shortMistakeList.Count;
            int shortPR = 0;
            int shortSR = 0;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (shortMistakeList[i].shortMistake)
                    {
                        if (shortMistakeList[i].subType == Mistake.SubType.shortPause)
                            shortPR++;
                        else if (shortMistakeList[i].subType == Mistake.SubType.shortSpeakingTime)
                            shortSR++;
                    }                    
                }
            }
            if (shortPR > shortSR && shortPR > biggest)
            {
                biggestSubType = Mistake.SubType.shortPause;
                biggest = shortPR;
            }
            else if (shortSR > shortPR && shortSR > biggest)
            {
                biggestSubType = Mistake.SubType.shortSpeakingTime;
                biggest = shortSR;
            }

            size = postureMistakeList.Count;
            if (size > biggest)
            {
                biggestSubType = Mistake.SubType.badPosture;
                biggest = size;
            }

            size = handMovementMistakeList.Count;
            int noMovement = 0;
            int tooMuchmovement = 0;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (handMovementMistakeList[i].subType == Mistake.SubType.noHandMovement)
                        noMovement++;
                    else if (handMovementMistakeList[i].subType == Mistake.SubType.tooMuchHandMovement)
                        tooMuchmovement++;
                }
            }
            if (noMovement > tooMuchmovement && noMovement > biggest)
            {
                biggestSubType = Mistake.SubType.noHandMovement;
                biggest = noMovement;
            }
            else if (tooMuchmovement > noMovement && tooMuchmovement > biggest)
            {
                biggestSubType = Mistake.SubType.tooMuchHandMovement;
                biggest = tooMuchmovement;
            }

            repetitions = biggest;
            return biggestSubType;

        }

        public void makeReport()
        {
            nrOfVolumeMistakes = 0;
            for (int i = 0; i < volumeMistakeList.Count; i++)
            {
                if (volumeMistakeList[i].volumeMistakeLongEnough)
                {
                    nrOfVolumeMistakes++;
                }
            }
            //nrOfVolumeMistakes = volumeMistakeList.Count;
            nrOfCadenceMistakes = 0;
            for (int i = 0; i < cadenceMistakeList.Count; i++)
            {
                if (cadenceMistakeList[i].subType == Mistake.SubType.longPause || cadenceMistakeList[i].subType == Mistake.SubType.longSpeakingTime)
                {
                    nrOfCadenceMistakes++;
                }
            }
            //nrOfCadenceMistakes = cadenceMistakeList.Count;
            nrOfPostureMistakes = postureMistakeList.Count;
            nrOfHandMovementMistakes = handMovementMistakeList.Count;
            nrOfShortMistakes = 0;
            for (int i = 0; i < shortMistakeList.Count; i++)
            {
                if (shortMistakeList[i].shortMistake)
                {
                    nrOfShortMistakes++;
                }
            }
            //nrOfShortMistakes = shortMistakeList.Count;
            nrOfMistakes += nrOfVolumeMistakes + nrOfCadenceMistakes +
                nrOfPostureMistakes + nrOfHandMovementMistakes + nrOfShortMistakes;

            double biggest = 0;
            double x = 0;
            double sumOfMistakes = 0;
            for (int i =0; i<volumeMistakeList.Count; i++ ){
                if (volumeMistakeList[i].volumeMistakeLongEnough)
                {
                    x = getPointsOfMistake(volumeMistakeList[i]);
                    sumOfMistakes += x;
                    if (x > biggest)
                    {
                        biggest = x;
                        biggestOfAllMistakes = volumeMistakeList[i];
                    }
                }                
            }
            for (int i = 0; i < cadenceMistakeList.Count; i++)
            {
                if (cadenceMistakeList[i].subType == Mistake.SubType.longPause || cadenceMistakeList[i].subType == Mistake.SubType.longSpeakingTime)
                {
                    x = getPointsOfMistake(cadenceMistakeList[i]);
                    sumOfMistakes += x;
                    if (x > biggest)
                    {
                        biggest = x;
                        biggestOfAllMistakes = cadenceMistakeList[i];
                    }
                }                
            }
            for (int i = 0; i < postureMistakeList.Count; i++)
            {
                x = getPointsOfMistake(postureMistakeList[i]);
                sumOfMistakes += x;
                if (x > biggest)
                {
                    biggest = x;
                    biggestOfAllMistakes = postureMistakeList[i];
                }
            }
            for (int i = 0; i < handMovementMistakeList.Count; i++)
            {
                x = getPointsOfMistake(handMovementMistakeList[i]);
                sumOfMistakes += x;
                if (x > biggest)
                {
                    biggest = x;
                    biggestOfAllMistakes = handMovementMistakeList[i];
                }
            }
            for (int i = 0; i < shortMistakeList.Count; i++)
            {
                if (shortMistakeList[i].shortMistake)
                {
                    x = getPointsOfMistake(shortMistakeList[i]);
                    sumOfMistakes += x;
                    if (x > biggest)
                    {
                        biggest = x;
                        biggestOfAllMistakes = shortMistakeList[i];
                    }
                }                
            }
            averageMistakePoints = sumOfMistakes / nrOfMistakes;

            double sdSum = 0;
            for (int i = 0; i < volumeMistakeList.Count; i++)
            {
                if (volumeMistakeList[i].volumeMistakeLongEnough)
                    sdSum += Math.Pow((getPointsOfMistake(volumeMistakeList[i])-averageMistakePoints),2);
            }
            for (int i = 0; i < cadenceMistakeList.Count; i++)
            {
                if (cadenceMistakeList[i].subType == Mistake.SubType.longPause || cadenceMistakeList[i].subType == Mistake.SubType.longSpeakingTime)
                    sdSum += Math.Pow((getPointsOfMistake(cadenceMistakeList[i]) - averageMistakePoints), 2);
            }
            for (int i = 0; i < postureMistakeList.Count; i++)
            {
                sdSum += Math.Pow((getPointsOfMistake(postureMistakeList[i]) - averageMistakePoints), 2);
            }
            for (int i = 0; i < handMovementMistakeList.Count; i++)
            {
                sdSum += Math.Pow((getPointsOfMistake(handMovementMistakeList[i]) - averageMistakePoints), 2);
            }
            for (int i = 0; i < shortMistakeList.Count; i++)
            {
                if (shortMistakeList[i].shortMistake)
                    sdSum += Math.Pow((getPointsOfMistake(shortMistakeList[i]) - averageMistakePoints), 2);
            }
            standardDeviationOfMistakePoints = Math.Sqrt(sdSum / nrOfMistakes);

            mostRepeatedMistake = getMostRepeatedMistake(); //moduleVolume means it is null!! keep this in mind!!
            
            
        }

        public void makeLog()
        {  
            // Make sure to add everything together
            if (interruption)
            {
                clearReport();
                List<Mistake> tempV = new List<Mistake>();
                tempV.AddRange(volumeMistakeList);
                List<Mistake> tempC = new List<Mistake>();
                tempC.AddRange(cadenceMistakeList);
                List<Mistake> tempP = new List<Mistake>();
                tempP.AddRange(postureMistakeList);
                List<Mistake> tempH = new List<Mistake>();
                tempH.AddRange(handMovementMistakeList);
                List<Mistake> tempS = new List<Mistake>();
                tempS.AddRange(shortMistakeList);

                volumeMistakeList.AddRange(volumeMistakeListCopy);
                cadenceMistakeList.AddRange(cadenceMistakeListCopy);
                postureMistakeList.AddRange(postureMistakeListCopy);
                handMovementMistakeList.AddRange(handMovementMistakeListCopy);
                shortMistakeList.AddRange(shortMistakeListCopy);
                makeReport();

                volumeMistakeList = new List<Mistake>();
                volumeMistakeList.AddRange(tempV);
                cadenceMistakeList = new List<Mistake>();
                cadenceMistakeList.AddRange(tempC);
                postureMistakeList = new List<Mistake>();
                postureMistakeList.AddRange(tempP);
                handMovementMistakeList = new List<Mistake>();
                handMovementMistakeList.AddRange(tempH);
                shortMistakeList = new List<Mistake>();
                shortMistakeList.AddRange(tempS);
                
            }            

            String[] report = new String[19];
            report[0] = "Total number of mistakes: " + nrOfMistakes; 
            report[1] = "Volume Mistakes: " + nrOfVolumeMistakes;
            report[2] = "Cadence Mistakes: " + nrOfCadenceMistakes;
            report[3] = "Posture Mistakes: " + nrOfPostureMistakes;
            report[4] = "Hand Movement Mistakes: " + nrOfHandMovementMistakes;
            report[5] = "Short Mistakes: " + nrOfShortMistakes;
            report[6] = "Average Points of your Mistakes: " + averageMistakePoints;
            report[7] = "Standard deviation of your mistakes: " + standardDeviationOfMistakePoints;
            report[8] = "Your biggest Mistake: " + Mistake.getStringOfSubType(biggestOfAllMistakes.subType);
            report[9] = "Your most repeated Mistake: " + Mistake.getStringOfSubType(mostRepeatedMistake);
            report[10] = "The number of loud mistake interruptions: " + interruptions[0];
            report[11] = "The number of soft mistake interruptions: " + interruptions[1];
            report[12] = "The number of long speaking time mistake interruptions: " + interruptions[2];
            report[13] = "The number of loud pause mistake interruptions: " + interruptions[3];
            report[14] = "The number of short speaking time mistake interruptions: " + interruptions[4];
            report[15] = "The number of short pause mistake interruptions: " + interruptions[5];
            report[16] = "The number of bad posture interruptions: " + interruptions[6];
            report[17] = "The number of no hand movement mistake interruptions: " + interruptions[7];
            report[18] = "The number of too much hand movement interruptions: " + interruptions[8];
            

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Stage\TestResults";            
            string x =   DateTime.Now.TimeOfDay.ToString();
            string temp = x.Replace(":", "-").Remove(8);

            string fileName = @"\" + DateTime.Now.Date.ToLongDateString() + " " + temp + ".txt";           
            
            System.IO.File.WriteAllLines(mydocpath + fileName, report);
        }

        public void clearReport()
        {
            nrOfMistakes = 0;
            nrOfVolumeMistakes = 0;
            nrOfCadenceMistakes = 0;
            nrOfPostureMistakes = 0;
            nrOfHandMovementMistakes = 0;
            nrOfShortMistakes = 0;
            averageMistakePoints = 0;
            standardDeviationOfMistakePoints = 0;
            biggestOfAllMistakes = new Mistake(Mistake.Type.volume, Mistake.SubType.moduleVolume, Mistake.GravityType.bad);
            mostRepeatedMistake = Mistake.SubType.moduleVolume;
            repetitions = 0;
        }

        public void reset()
        {
            clearReport();
            volumeMistakeList = new List<Mistake>(); //int = 0
            cadenceMistakeList = new List<Mistake>(); //int = 1
            postureMistakeList = new List<Mistake>(); //int = 2
            handMovementMistakeList = new List<Mistake>(); //int = 3
            shortMistakeList = new List<Mistake>();

            feedBackList = new List<PresentationEvent>(); //first integer is number in the queue, the second integer stands for which mistake list the feedback belongs to
            fired = null;
            firedHasEnded = false;

            pauseStyleList = new List<Style>();
            postureStyleList = new List<Style>();
            speakingTimeStyleList = new List<Style>();


            volumeMistake = false;
            cadenceMistake = false;
            postureMistake = false;
            handMovementMistake = false;
            shortMistake = false;
        }

        public void resetForMistake(Mistake mistake)
        {
            switch (mistake.type)
            {
                case Mistake.Type.volume:{
                    volumeMistakeListCopy.AddRange(volumeMistakeList);
                    volumeMistakeList = new List<Mistake>();
                    break;
                }
                case Mistake.Type.cadence:{
                    if ((mistake.subType == Mistake.SubType.shortPause || mistake.subType == Mistake.SubType.shortSpeakingTime) && mistake.shortMistake)
                    {
                        shortMistakeListCopy.AddRange(shortMistakeList);
                        shortMistakeList = new List<Mistake>();
                    }
                    else
                    {
                        cadenceMistakeListCopy.AddRange(cadenceMistakeList);
                        cadenceMistakeList = new List<Mistake>();
                    }                    
                    break;
                }
                case Mistake.Type.posture:{
                    postureMistakeListCopy.AddRange(postureMistakeList);
                    postureMistakeList = new List<Mistake>();
                    break;
                }
                case Mistake.Type.handMovement:{
                    handMovementMistakeListCopy.AddRange(handMovementMistakeList);
                    handMovementMistakeList = new List<Mistake>();
                    break;
                }
            }
        }

        public void increaseInterruptions(Mistake.SubType subType)
        {
            switch (subType)
            {
                case Mistake.SubType.loud:
                    interruptions[0]++;
                    break;
                case Mistake.SubType.soft:
                    interruptions[1]++;
                    break;
                case Mistake.SubType.longSpeakingTime:
                    interruptions[2]++;
                    break;
                case Mistake.SubType.longPause:
                    interruptions[3]++;
                    break;
                case Mistake.SubType.shortSpeakingTime:
                    interruptions[4]++;
                    break;
                case Mistake.SubType.shortPause:
                    interruptions[5]++;
                    break;
                case Mistake.SubType.badPosture:
                    interruptions[6]++;
                    break;
                case Mistake.SubType.noHandMovement:
                    interruptions[7]++;
                    break;
                case Mistake.SubType.tooMuchHandMovement:
                    interruptions[8]++;
                    break;
            }
        }

        public Mistake.SubType intToSubType(int x)
        {
            Mistake.SubType temp = Mistake.SubType.moduleVolume;
            switch (x)
            {
                case 0:
                    temp = Mistake.SubType.loud;
                    break;
                case 1:
                    temp = Mistake.SubType.soft;
                    break;
                case 2:
                    temp = Mistake.SubType.longSpeakingTime;
                    break;
                case 3:
                    temp = Mistake.SubType.longPause;
                    break;
                case 4:
                    temp = Mistake.SubType.shortSpeakingTime;
                    break;
                case 5:
                    temp = Mistake.SubType.shortPause;
                    break;
                case 6:
                    temp = Mistake.SubType.badPosture;
                    break;
                case 7:
                    temp = Mistake.SubType.noHandMovement;
                    break;
                case 8:
                    temp = Mistake.SubType.tooMuchHandMovement;
                    break;                    
            }
            return temp;
        }
        //private void checkShortMistake()
        //{
        //    int size = cadenceMistakeList.Count;
        //    if (size > 1)
        //    {
        //        if (cadenceMistakeList[size - 2].subType == Mistake.SubType.shortPause && cadenceMistakeList[size - 2].shortMistake)
        //        {
        //            if (cadenceMistakeList[size - 1].subType == Mistake.SubType.shortSpeakingTime || cadenceMistakeList[size - 1].subType == Mistake.SubType.longSpeakingTime)
        //            {
        //                Mistake x = new Mistake(Mistake.Type.cadence, Mistake.SubType.shortPause, Mistake.GravityType.shortPause);
        //                x.shortMistake = true;
        //                shortMistakeList.Add(x);
        //                shortMistake = true;
        //                cadenceMistakeList.RemoveAt(size - 2);
        //            }
        //        }
        //        else if (cadenceMistakeList[size - 2].subType == Mistake.SubType.shortSpeakingTime && cadenceMistakeList[size - 2].shortMistake)
        //        {
        //            if (cadenceMistakeList[size - 1].subType == Mistake.SubType.shortPause || cadenceMistakeList[size - 1].subType == Mistake.SubType.longPause)
        //            {
        //                Mistake x = new Mistake(Mistake.Type.cadence, Mistake.SubType.shortSpeakingTime, Mistake.GravityType.shortSpeakingTime);
        //                x.shortMistake = true;
        //                shortMistakeList.Add(x);
        //                shortMistake = true;
        //                cadenceMistakeList.RemoveAt(size - 2);
        //            }
        //        }

        //    }
        //}

        
    }
}
