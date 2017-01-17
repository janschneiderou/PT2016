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
using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    public class RulesAnalyzerFIFO
    {
        MainWindow parent;

        bool noMistake = true;
        bool freeStyle = true;
        bool interruptionNeeded = false;

        public bool interruption = true;
        public bool interrupted = false;

        BodyFramePreAnalysis bfpa;
        AudioPreAnalysis apa;
        Body oldBody;
        public JudgementMaker myJudgementMaker;
        PresentationAction previousAction;

        public delegate void FeedBackEvent(object sender, PresentationAction x);
        public event FeedBackEvent feedBackEvent;

        public delegate void CorrectionEvent(object sender, PresentationAction x);
        public event CorrectionEvent correctionEvent;

        public delegate void InterruptionEvent(object sender, PresentationAction[] x);
        public event InterruptionEvent myInterruptionEvent;

        ArrayList mistakes;
        ArrayList mistakesL;

        ArrayList goodiesL;

        PresentationAction[] sentMistakes;
        PresentationAction[] crossedArms;
        PresentationAction[] handsUnderHips;
        PresentationAction[] handsBehindBack;
        PresentationAction[] hunchPosture;
        PresentationAction[] leaningPosture;
        PresentationAction[] highVolumes;
        PresentationAction[] lowVolumes;
        PresentationAction[] longPauses;
        PresentationAction[] longTalks;
        PresentationAction[] periodicMovements;
        PresentationAction[] hmmms;
        PresentationAction[] legsCrossed;
        PresentationAction[] handsNotMoving;
        PresentationAction[] handsMovingMuch;
        PresentationAction[] noModulation;
        PresentationAction[] serious;

       int crossedArmsMistakes =0;
       int handsUnderHipsMistakes = 0;
       int handsBehindBackMistakes = 0;
       int hunchPostureMistakes = 0;
       int leaningPostureMistakes = 0;
       int highVolumesMistakes = 0;
       int lowVolumesMistakes = 0;
       int longPausesMistakes = 0;
       int longTalksMistakes = 0;
       int periodicMovementsMistakes = 0;
       int hmmmsMistakes = 0;
       int legsCrossedMistakes = 0;
       int handsNotMovingMistakes = 0;
       int handsMovingMuchMistakes = 0;
       int noModulationMistakes = 0;
       int seriousMistakes = 0;
       public double lastFeedbackTime = 0;
       public double timeBetweenFeedbacks = 3500;
       public bool noInterrupt = true;

       int interruptNumber = 9999999 ;//4;
 
        public RulesAnalyzerFIFO(MainWindow parent)
        {
            this.parent = parent;
            myJudgementMaker = new JudgementMaker(parent);
            mistakes = new ArrayList();
            mistakesL = new ArrayList();
            goodiesL = new ArrayList();
            crossedArms = new PresentationAction[interruptNumber];
            handsUnderHips = new PresentationAction[interruptNumber];
            handsBehindBack = new PresentationAction[interruptNumber];
            hunchPosture = new PresentationAction[interruptNumber];
            leaningPosture = new PresentationAction[interruptNumber];
            highVolumes = new PresentationAction[interruptNumber];
            lowVolumes = new PresentationAction[interruptNumber];
            longPauses = new PresentationAction[interruptNumber];
            longTalks = new PresentationAction[interruptNumber];
            periodicMovements = new PresentationAction[interruptNumber];
            hmmms = new PresentationAction[interruptNumber];
            legsCrossed = new PresentationAction[interruptNumber];
            handsNotMoving = new PresentationAction[interruptNumber];
            handsMovingMuch = new PresentationAction[interruptNumber];
            noModulation = new PresentationAction[interruptNumber];
            serious = new PresentationAction[interruptNumber];
            previousAction = new PresentationAction();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
        }

        public void setFreeStyle(bool fs)
        {
            freeStyle = fs;
        }

        #region analysisCycle

        public void AnalyseRules()
        {
            if (parent.freestyleMode != null)
            {
                /*
                if(parent.faceFrameHandler.faceFramePreAnalysis.smiling)
                {
                    parent.freestyleMode.smiley.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    parent.freestyleMode.smiley.Visibility = System.Windows.Visibility.Collapsed;
                }
                // */
                if (parent.freestyleMode.myState == PresentationTrainer.FreestyleMode.currentState.play )
                {
                    myJudgementMaker.analyze();
                    if(checkTimeToStartAnalysing()==true)
                    {
                      //  myJudgementMaker.analyze();

                         /*
                        parent.freestyleMode.debugLabel.Content = "right " +(int)myJudgementMaker.armMovementsCalc.rightArmAngleChange +
                            "\n left " + (int)myJudgementMaker.armMovementsCalc.leftArmAngleChange +
                            "\n gesture " + myJudgementMaker.armMovementsCalc.currentGesture.ToString()
                            + "\n time between gestures " + (int)myJudgementMaker.timeBetweenGestures
                            + " change " + (int)myJudgementMaker.armMovementsCalc.leftArmAngleChange; 
                        // */
                    }

                    doListThingsForLogging();

                    if (checkTimeToGiveFeedback()==true)
                    {
                        bool didIGiveFeedback = false;

                        

                        //mistake was corrected?

                        //updateLists
                        if (myJudgementMaker.mistakeList.Count > 0)
                        {

                            updateMistakeList();
                          //  logMistakes();
                            
                        }
                        else
                        {
                            mistakes.Clear();
                        }


                        //give correction when no mistakes and previous mistake is not NOMISTAKE
                        if (mistakes.Count == 0 && previousAction.myMistake != PresentationAction.MistakeType.NOMISTAKE)
                        {
                            doGoodEventStuff();
                            
                            //put previous mistake to no Mistake
                            //start timer
                        }

                        if (mistakes.Count > 0)
                        {
                            if (((PresentationAction)mistakes[0]).myMistake != previousAction.myMistake &&
                                 previousAction.myMistake != PresentationAction.MistakeType.NOMISTAKE)
                            {
                                doGoodEventStuff();
                                
                                // mistakes.Clear();
                                didIGiveFeedback = true;
                            }
                            if (didIGiveFeedback == false)
                            {
                                doFeedbackEventStuff();
                            }
                        }

                       
                    }
                    }
                   
 

            }
        }

        

        public bool checkTimeToStartAnalysing()
        {
            bool startAnalysis = false;
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (currentTime - lastFeedbackTime + 600 > timeBetweenFeedbacks)
            {
                startAnalysis = true;
            }

            return startAnalysis;
        }

        public bool checkTimeToGiveFeedback()
        {
            bool giveFeedback = false;

             double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

             if (currentTime - lastFeedbackTime > timeBetweenFeedbacks)
             {
                 giveFeedback=true;
             }

            return giveFeedback;
        }

        #endregion

        #region loggingStuff

        void doListThingsForLogging()
        {
            if (myJudgementMaker.mistakeList.Count > 0)
            {

                updateMistakeListL();
                logMistakes();

            }
            else
            {
                logRemainingMistakes();
                mistakesL.Clear();
            }
            logGoodies();
        }

        #region mistakesLLists

        private void updateMistakeListL()
        {
            if (mistakes.Count > 0)
            {
                deleteNoLongerMistakesL();
            }

            addNewMistakesL();
        }

        private void addNewMistakesL()
        {
            foreach (PresentationAction ba in myJudgementMaker.mistakeList)
            {
                bool isAlreadyThere = false;
                foreach (PresentationAction a in mistakesL)
                {
                    if (ba.myMistake == a.myMistake)
                    {
                        isAlreadyThere = true;
                        break;
                    }
                }
                if (isAlreadyThere == false)
                {
                    if (ba.interrupt)
                    {
                        mistakesL.Insert(0, ba);
                    }
                    else
                    {
                        mistakesL.Add(ba);

                    }
                    MainWindow.totalMistakes++;
                    logMistakeNumberType(ba);

                }
            }
        }

        private void deleteNoLongerMistakesL()
        {
            int[] nolongerErrors = new int[mistakesL.Count];
            int i = 0;

            foreach (PresentationAction a in mistakesL)
            {

                foreach (PresentationAction ba in myJudgementMaker.mistakeList)//judgementMaker.mistakeList
                {
                    if (a.myMistake == ba.myMistake)
                    {
                        nolongerErrors[i] = 1;
                        break;
                    }
                }
                i++;
            }
            for (int j = nolongerErrors.Length; j > 0; j--)
            {
                if (nolongerErrors[j - 1] == 0)
                {
                    logTimeMistakes(j - 1);
                    mistakesL.RemoveAt(j - 1);
                }
            }
        }


        #endregion

        #region goodieStuff
        private void logGoodies()
        {
            deleteNoLongerGoodies();
            findGoodies();
        }

        private void deleteNoLongerGoodies()
        {
            int[] nolongerGoodies = new int[goodiesL.Count];
            int i = 0;

            foreach (PresentationAction a in goodiesL)
            {

                foreach (PresentationAction ba in myJudgementMaker.GoodiesList)
                {
                    if (a.myGoodie == ba.myGoodie)
                    {
                        nolongerGoodies[i] = 1;
                        break;
                    }
                }
                i++;
            }
            for (int j = nolongerGoodies.Length; j > 0; j--)
            {
                if (nolongerGoodies[j - 1] == 0)
                {
                    goodiesL.RemoveAt(j - 1);
                    //do end goodie logging 
                    double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    MainWindow.totalgoodiesTime = MainWindow.totalgoodiesTime+ currentTime - MainWindow.smileTime;
                    MainWindow.stringGoodies = MainWindow.stringGoodies + currentTime+"/>";
                }
            }
        }
        private void findGoodies()
        {
            int x = 0;

            foreach (PresentationAction ba in myJudgementMaker.GoodiesList)
            {
                bool isAlreadyThere = false;
                foreach (PresentationAction a in goodiesL)
                {
                    if (ba.myGoodie == a.myGoodie)
                    {
                        isAlreadyThere = true;
                        break;
                    }
                }
                if (isAlreadyThere == false)
                {
                    goodiesL.Add(ba);
                    double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    MainWindow.smileTime = currentTime;
                    MainWindow.stringGoodies = MainWindow.stringGoodies + "<" + ba.myGoodie.ToString() + "," + currentTime + ",";
                }
            }  

        }
        #endregion
        private void logMistakes()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            MainWindow.stringMistakes = MainWindow.stringMistakes +
                System.Environment.NewLine + "<Mistakes>" + currentTime;
            foreach (PresentationAction a in mistakes)
            {
                MainWindow.stringMistakes = MainWindow.stringMistakes +
                    System.Environment.NewLine + "" + a.myMistake.ToString();
            }
            MainWindow.stringMistakes = MainWindow.stringMistakes +
                System.Environment.NewLine + "</Mistakes>";
        }

        private void logMistakeNumberType(PresentationAction ba)
        {
            switch (ba.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEGSCROSSED:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.HUNCHBACK:
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:

                    MainWindow.totalPostureMistakes++;
                    break;
                case PresentationAction.MistakeType.DANCING:

                    MainWindow.totalDancingMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:

                    MainWindow.totalGesturesMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:

                    MainWindow.totalGesturesMistakes++;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                case PresentationAction.MistakeType.LOW_VOLUME:
                case PresentationAction.MistakeType.LOW_MODULATION:

                    MainWindow.totalVolumeMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                case PresentationAction.MistakeType.LONG_TALK:

                    MainWindow.totalCadenceMistakes++;
                    break;
                case PresentationAction.MistakeType.HMMMM:

                    MainWindow.totalHmmmMistakes++;
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    MainWindow.totalSeriousMistakes++;
                    break;
            }
        }

        void logTimeMistakes(int j)
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double mistakeTime = (currentTime - ((PresentationAction)mistakesL[j]).timeStarted);

            switch (((PresentationAction)mistakesL[j]).myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                    MainWindow.armsCrossedMistakeTime = MainWindow.armsCrossedMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    MainWindow.hunchMistakeTime = MainWindow.hunchMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                    MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.LEFTHANDNOTVISIBLE:
                    MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    MainWindow.handsUnderhipsMistakeTime = MainWindow.handsUnderhipsMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.LEFTLEAN:
                    MainWindow.hunchMistakeTime = MainWindow.hunchMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.RIGHTHANDNOTVISIBLE:
                    MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                    MainWindow.handsUnderhipsMistakeTime = MainWindow.handsUnderhipsMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                     MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                    MainWindow.hunchMistakeTime = MainWindow.hunchMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                    MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;

                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    MainWindow.gestureMistakeTime = MainWindow.gestureMistakeTime + mistakeTime-5000;
                    MainWindow.timeGestureMistakes = MainWindow.timeGestureMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;

                case PresentationAction.MistakeType.HIGH_VOLUME:
                    MainWindow.highVolumeMistakeTime = MainWindow.highVolumeMistakeTime + mistakeTime;
                    MainWindow.volumeMistakeTime = MainWindow.volumeMistakeTime + mistakeTime;
                    MainWindow.timeVolumeMistakes = MainWindow.timeVolumeMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;

                case PresentationAction.MistakeType.LOW_VOLUME:
                     MainWindow.lowVolumeMistakeTime = MainWindow.lowVolumeMistakeTime + mistakeTime;
                    MainWindow.volumeMistakeTime = MainWindow.volumeMistakeTime + mistakeTime;
                    MainWindow.timeVolumeMistakes = MainWindow.timeVolumeMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    MainWindow.timeVolumeMistakes = MainWindow.timeVolumeMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;

                case PresentationAction.MistakeType.LONG_PAUSE:
                case PresentationAction.MistakeType.LONG_TALK:
                    MainWindow.cadenceMistakeTime = MainWindow.cadenceMistakeTime + mistakeTime;//-10000;
                    MainWindow.timeCadenceMistakes = MainWindow.timeCadenceMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;

                case PresentationAction.MistakeType.HMMMM:
                    MainWindow.hmmmMistakeTime = MainWindow.hmmmMistakeTime + mistakeTime;
                    MainWindow.timeHmmmMistakes = MainWindow.timeHmmmMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;

                case PresentationAction.MistakeType.DANCING:
                    MainWindow.dancingMistakeTime = MainWindow.dancingMistakeTime + mistakeTime;
                    MainWindow.timeDancingMistakes = MainWindow.timeDancingMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    MainWindow.seriousMistakeTime = MainWindow.seriousMistakeTime + mistakeTime;
                    MainWindow.timeSeriousMistakes = MainWindow.timeSeriousMistakes +
                    System.Environment.NewLine + "<mistake>" + ((PresentationAction)mistakesL[j]).myMistake.ToString() + ","
                    + ((PresentationAction)mistakesL[j]).timeStarted +
                    "," + currentTime + "</mistake>";
                    break;
            }
        }

        public void logRemainingMistakes()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            
            foreach (PresentationAction pa in mistakesL)
            {
                double mistakeTime = (currentTime - pa.timeStarted);
                switch (pa.myMistake)
                {
                    case PresentationAction.MistakeType.ARMSCROSSED:
                        MainWindow.armsCrossedMistakeTime = MainWindow.armsCrossedMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.HUNCHBACK:
                        MainWindow.hunchMistakeTime = MainWindow.hunchMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                        MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LEFTHANDNOTVISIBLE:
                        MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                        MainWindow.handsUnderhipsMistakeTime = MainWindow.handsUnderhipsMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LEFTLEAN:
                        MainWindow.hunchMistakeTime = MainWindow.hunchMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LEGSCROSSED:
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                        MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.RIGHTHANDNOTVISIBLE:
                        MainWindow.handsNotVisibleMistakeTime = MainWindow.handsNotVisibleMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                        MainWindow.handsUnderhipsMistakeTime = MainWindow.handsUnderhipsMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.RIGHTLEAN:
                        MainWindow.hunchMistakeTime = MainWindow.hunchMistakeTime + mistakeTime;
                    MainWindow.postureMistakeTime = MainWindow.postureMistakeTime + mistakeTime;
                        MainWindow.timePostureMistakes = MainWindow.timePostureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;

                    case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                        
                    MainWindow.gestureMistakeTime = MainWindow.gestureMistakeTime + mistakeTime - 5000;
                        MainWindow.timeGestureMistakes = MainWindow.timeGestureMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;

                    case PresentationAction.MistakeType.HIGH_VOLUME:
                        MainWindow.highVolumeMistakeTime = MainWindow.highVolumeMistakeTime + mistakeTime;
                    MainWindow.volumeMistakeTime = MainWindow.volumeMistakeTime + mistakeTime;
                        MainWindow.timeVolumeMistakes = MainWindow.timeVolumeMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LOW_VOLUME:
                        MainWindow.lowVolumeMistakeTime = MainWindow.lowVolumeMistakeTime + mistakeTime;
                    MainWindow.volumeMistakeTime = MainWindow.volumeMistakeTime + mistakeTime;
                        MainWindow.timeVolumeMistakes = MainWindow.timeVolumeMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.LOW_MODULATION:
                        MainWindow.timeVolumeMistakes = MainWindow.timeVolumeMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;

                    case PresentationAction.MistakeType.LONG_PAUSE:
                    case PresentationAction.MistakeType.LONG_TALK:
                        MainWindow.cadenceMistakeTime = MainWindow.cadenceMistakeTime + mistakeTime;//-10000;
                        MainWindow.timeCadenceMistakes = MainWindow.timeCadenceMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;

                    case PresentationAction.MistakeType.HMMMM:
                        MainWindow.hmmmMistakeTime = MainWindow.hmmmMistakeTime + mistakeTime;
                        MainWindow.timeHmmmMistakes = MainWindow.timeHmmmMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;

                    case PresentationAction.MistakeType.DANCING:
                        MainWindow.dancingMistakeTime = MainWindow.dancingMistakeTime + mistakeTime;
                        MainWindow.timeDancingMistakes = MainWindow.timeDancingMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                    case PresentationAction.MistakeType.SERIOUS:
                        MainWindow.seriousMistakeTime = MainWindow.seriousMistakeTime + mistakeTime;
                        MainWindow.timeSeriousMistakes = MainWindow.timeSeriousMistakes +
                        System.Environment.NewLine + "<mistake>" + pa.myMistake.ToString() + ","
                        + pa.timeStarted +
                        "," + currentTime + "</mistake>";
                        break;
                }
            }
        }

        #endregion

        // update, lists add mistakes, delete no longer mistakes
        #region manageLists 

        private void updateMistakeList()
        {
            if (mistakes.Count > 0)
            {
                deleteNoLongerMistakes();
            }

            addNewMistakes();
        }

        private void addNewMistakes()
        {
            foreach (PresentationAction ba in myJudgementMaker.mistakeList)
            {
                bool isAlreadyThere = false;
                foreach (PresentationAction a in mistakes)
                {
                    if (ba.myMistake == a.myMistake)
                    {
                        isAlreadyThere = true;
                        break;
                    }
                }
                if (isAlreadyThere == false)
                {
                    if(ba.interrupt)
                    {
                        mistakes.Insert(0, ba);
                    }
                    else
                    {
                        mistakes.Add(ba);
                    }
                    
                }
            }
        }

        private void deleteNoLongerMistakes()
        {
            int[] nolongerErrors = new int[mistakes.Count];
            int i = 0;

            foreach (PresentationAction a in mistakes)
            {

                foreach (PresentationAction ba in myJudgementMaker.mistakeList)//judgementMaker.mistakeList
                {
                    if (a.myMistake == ba.myMistake)
                    {
                        nolongerErrors[i] = 1;
                        break;
                    }
                }
                i++;
            }
            for (int j = nolongerErrors.Length; j > 0; j--)
            {
                if (nolongerErrors[j - 1] == 0)
                {
                    mistakes.RemoveAt(j - 1);
                }
            }
        }

        #endregion

        //sending feedbacks, corrections, and interruptions
        #region sendFeedbacks

        private void doGoodEventStuff()
        {

            correctionEvent(this, previousAction);

            lastFeedbackTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            MainWindow.stringFreestyleFeedbacks = MainWindow.stringFreestyleFeedbacks + "<time finished>" +
                lastFeedbackTime + "</time finished>" + System.Environment.NewLine +
                "</feedback>";

            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            myJudgementMaker.clearLists();
            //if(previousAction!=null)
            //{
            //    double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            //    if (parent.freestyleMode.myState == PresentationTrainer.FreestyleMode.currentState.play)
            //    {
            //        correctionEvent(this, previousAction);

            //        lastFeedbackTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            //        previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            //    }
                
            //}
            //else
            //{
                
            //   // correctionEvent(this, previousAction);
            //}
            


        }

        private void doFeedbackEventStuff()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            ((PresentationAction)mistakes[0]).timeFinished = currentTime;


            if (previousAction.myMistake != ((PresentationAction)mistakes[0]).myMistake && ((PresentationAction)mistakes[0]).interrupt == false)
            {
                if (((PresentationAction)mistakes[0]).myMistake == PresentationAction.MistakeType.HANDS_NOT_MOVING)
                {
                    ((PresentationAction)mistakes[0]).firstImage = myJudgementMaker.myVoiceAndMovementObject.firstImage;
                    ((PresentationAction)mistakes[0]).lastImage = myJudgementMaker.myVoiceAndMovementObject.lastImage;
                }
                feedBackEvent(this, (PresentationAction)mistakes[0]);

                MainWindow.stringFreestyleFeedbacks = MainWindow.stringFreestyleFeedbacks + System.Environment.NewLine +
                    "<feedback> " + ((PresentationAction)mistakes[0]).myMistake.ToString() + System.Environment.NewLine +
                    "<time started>" + currentTime + "</time started>" + System.Environment.NewLine;

                checkWhereToPutMistake();
                if(noInterrupt==true)
                {
                    previousAction = (PresentationAction)mistakes[0];
                }
                
            }
            else if (((PresentationAction)mistakes[0]).interrupt == true)
            {
              

                checkWhereToPutMistake();
                bigMistakeInterruption();

            }

        }

        public void doInterruptionThing()
        {
            noInterrupt = false;

            MainWindow.stringInterruptions = MainWindow.stringInterruptions + System.Environment.NewLine +
                  "<interruption>" + ((PresentationAction)mistakes[0]).myMistake.ToString() +
                  System.Environment.NewLine + DateTime.Now.TimeOfDay.TotalMilliseconds + "</interruption>";
            MainWindow.speakTimes.Add(DateTime.Now.TimeOfDay.TotalMilliseconds);
            myInterruptionEvent(this, sentMistakes);
            
           // doGoodEventStuff();
            
         //   myJudgementMaker.mistakeList = new ArrayList();
         //   myJudgementMaker.tempMistakeList = new ArrayList();
            // // myJudgementMaker.audioMovementMistakeTempList = new ArrayList();
        }
        public void resetAfterPause()
        {
            mistakes = new ArrayList();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            myJudgementMaker = new JudgementMaker(parent);
            noInterrupt = true;
        }
        public void resetAfterInterruption()
        {
            resetMistakeArray();
            mistakes = new ArrayList();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            myJudgementMaker = new JudgementMaker(parent);
            noInterrupt = true;
        }
        public void resetAll()
        {

           mistakes = new ArrayList();
            crossedArms = new PresentationAction[interruptNumber];
            handsUnderHips = new PresentationAction[interruptNumber];
            handsBehindBack = new PresentationAction[interruptNumber];
            hunchPosture = new PresentationAction[interruptNumber];
            leaningPosture = new PresentationAction[interruptNumber];
            highVolumes = new PresentationAction[interruptNumber];
            lowVolumes = new PresentationAction[interruptNumber];
            longPauses = new PresentationAction[interruptNumber];
            longTalks = new PresentationAction[interruptNumber];
            periodicMovements = new PresentationAction[interruptNumber];
            hmmms = new PresentationAction[interruptNumber];
            legsCrossed = new PresentationAction[interruptNumber];
            handsNotMoving = new PresentationAction[interruptNumber];
            handsMovingMuch = new PresentationAction[interruptNumber];
            noModulation = new PresentationAction[interruptNumber];
            serious = new PresentationAction[interruptNumber];
            previousAction = new PresentationAction();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
  

          
           
            myJudgementMaker = new JudgementMaker(parent);
            noInterrupt = true;
        }

        #endregion

        #region handleInterruptions

        private void bigMistakeInterruption()
        {
            switch (((PresentationAction)mistakes[0]).myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:

                    sentMistakes = crossedArms;
                    break;
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:

                    sentMistakes = handsBehindBack;
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:

                    sentMistakes = legsCrossed;
                    break;

                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:

                    sentMistakes = handsUnderHips;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:

                    sentMistakes = hunchPosture;
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:

                    sentMistakes = leaningPosture;
                    break;
                case PresentationAction.MistakeType.DANCING:

                    sentMistakes = periodicMovements;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:

                    sentMistakes = handsNotMoving;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:

                    sentMistakes = handsMovingMuch;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:

                    sentMistakes = highVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:

                    sentMistakes = lowVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:

                    sentMistakes = noModulation;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:

                    sentMistakes = longPauses;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:

                    sentMistakes = longTalks;
                    break;
                case PresentationAction.MistakeType.HMMMM:

                    sentMistakes = hmmms;
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    sentMistakes = serious;
                    break;
            }
            doInterruptionThing();
        }



        private void resetMistakeArray()
        {
            switch (((PresentationAction)mistakes[0]).myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossedArms = new PresentationAction[interruptNumber];
                    crossedArmsMistakes++;
                    break;
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBack = new PresentationAction[interruptNumber];
                    handsBehindBackMistakes++;

                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    legsCrossed = new PresentationAction[interruptNumber];
                    legsCrossedMistakes++;
                    break;

                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    handsUnderHips = new PresentationAction[interruptNumber];
                    handsUnderHipsMistakes++;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPosture = new PresentationAction[interruptNumber];
                    hunchPostureMistakes++;
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    leaningPosture = new PresentationAction[interruptNumber];
                    leaningPostureMistakes++;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    periodicMovements = new PresentationAction[interruptNumber];
                    periodicMovementsMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMoving = new PresentationAction[interruptNumber];
                    handsNotMovingMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuch = new PresentationAction[interruptNumber];
                    handsMovingMuchMistakes++;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumes = new PresentationAction[interruptNumber];
                    highVolumesMistakes++;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumes = new PresentationAction[interruptNumber];
                    lowVolumesMistakes++;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulation = new PresentationAction[interruptNumber];
                    noModulationMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPauses = new PresentationAction[interruptNumber];
                    longPausesMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalks = new PresentationAction[interruptNumber];
                    longTalksMistakes++;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmms = new PresentationAction[interruptNumber];
                    hmmmsMistakes++;
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    serious = new PresentationAction[interruptNumber];
                    seriousMistakes++;
                    break;
            }
        }


        private void checkWhereToPutMistake()
        {
            int i = 0;
            int x = 0;
            // sentMistakes;
            PresentationAction temp = new PresentationAction();
            temp = ((PresentationAction)mistakes[0]).Clone();

            switch (temp.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossedArmsMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (crossedArms[i] == null)
                        {
                            crossedArms[i] = temp;
                            crossedArms[i].myMistake = PresentationAction.MistakeType.ARMSCROSSED;
                            break;
                        }
                    }
                    sentMistakes = crossedArms;
                    break;
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBackMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (handsBehindBack[i] == null)
                        {
                            handsBehindBack[i] = temp;
                            handsBehindBack[i].myMistake = PresentationAction.MistakeType.RIGHTHANDBEHINDBACK;
                            break;
                        }
                    }
                    sentMistakes = handsBehindBack;
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    legsCrossedMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (legsCrossed[i] == null)
                        {
                            legsCrossed[i] = temp;
                            legsCrossed[i].myMistake = PresentationAction.MistakeType.LEGSCROSSED;
                            break;
                        }
                    }
                    sentMistakes = legsCrossed;
                    break;

                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    handsUnderHipsMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (handsUnderHips[i] == null)
                        {
                            handsUnderHips[i] = temp;
                            handsUnderHips[i].myMistake = PresentationAction.MistakeType.RIGHTHANDUNDERHIP;
                            break;
                        }
                    }
                    sentMistakes = handsUnderHips;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPostureMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (hunchPosture[i] == null)
                        {
                            hunchPosture[i] = temp;
                            hunchPosture[i].myMistake = PresentationAction.MistakeType.HUNCHBACK;
                            break;
                        }
                    }
                    sentMistakes = hunchPosture;
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    leaningPostureMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (leaningPosture[i] == null)
                        {
                            leaningPosture[i] = temp;
                            leaningPosture[i].myMistake = PresentationAction.MistakeType.RIGHTLEAN;
                            break;
                        }
                    }
                    sentMistakes = leaningPosture;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    periodicMovementsMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (periodicMovements[i] == null)
                        {
                            periodicMovements[i] = temp;
                            periodicMovements[i].myMistake = PresentationAction.MistakeType.DANCING;
                            break;
                        }
                    }
                    sentMistakes = periodicMovements;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMovingMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (handsNotMoving[i] == null)
                        {
                           // System.Windows.Media.ImageSource im = parent.videoHandler.kinectImage.Source;  
                            handsNotMoving[i] = temp;
                            handsNotMoving[i].myMistake = PresentationAction.MistakeType.HANDS_NOT_MOVING;
                           // handsNotMoving[i].lastImage = im.CloneCurrentValue();

                            break;
                        }
                    }
                    sentMistakes = handsNotMoving;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuchMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (handsMovingMuch[i] == null)
                        {
                            handsMovingMuch[i] = temp;
                            handsMovingMuch[i].myMistake = PresentationAction.MistakeType.HANDS_MOVING_MUCH;
                            
                            break;
                        }
                    }
                    sentMistakes = handsMovingMuch;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumesMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (highVolumes[i] == null)
                        {
                            highVolumes[i] = temp;
                            highVolumes[i].myMistake = PresentationAction.MistakeType.HIGH_VOLUME;
                            break;
                        }
                    }
                    sentMistakes = highVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumesMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (lowVolumes[i] == null)
                        {
                            lowVolumes[i] = temp;
                            lowVolumes[i].myMistake = PresentationAction.MistakeType.LOW_VOLUME;
                            break;
                        }
                    }
                    sentMistakes = lowVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulationMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (noModulation[i] == null)
                        {
                            noModulation[i] = temp;
                            noModulation[i].myMistake = PresentationAction.MistakeType.LOW_MODULATION;
                            break;
                        }
                    }
                    sentMistakes = noModulation;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPausesMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (longPauses[i] == null)
                        {
                            longPauses[i] = temp;
                            longPauses[i].myMistake = PresentationAction.MistakeType.LONG_PAUSE;
                            break;
                        }
                    }
                    sentMistakes = longPauses;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalksMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (longTalks[i] == null)
                        {
                            longTalks[i] = temp;
                            longTalks[i].myMistake = PresentationAction.MistakeType.LONG_TALK;
                            break;
                        }
                    }
                    sentMistakes = longTalks;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmsMistakes++;
                    for (i = 0; i < interruptNumber; i++)
                    {
                        if (hmmms[i] == null)
                        {
                            hmmms[i] = temp;
                            hmmms[i].myMistake = PresentationAction.MistakeType.HMMMM;
                            break;
                        }
                    }
                    sentMistakes = hmmms;
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    seriousMistakes++;
                    for (i = 0; i < interruptNumber; i++ ) // TODO
                    {
                        if (serious[i] == null)
                        {
                            serious[i] = temp;
                            serious[i].myMistake = PresentationAction.MistakeType.SERIOUS;
                            break;
                        }
                    }
                        break;
            }
            if (i == interruptNumber)
            {
                doInterruptionThing();
            }
        }



    }

        #endregion

      
}
