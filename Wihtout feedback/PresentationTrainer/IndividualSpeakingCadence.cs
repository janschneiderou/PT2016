using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    class IndividualSpeakingCadence: IndividualTracker
    {
        public IndividualSpeakingCadence(MainWindow parent)
        {
            this.parent = parent;
            myVoiceAndMovementObject = new PresentationAction();
            periodicMovements = new PeriodicMovements();

            tempMistakeList = new ArrayList();
            mistakeList = new ArrayList();
            voiceAndMovementsList = new ArrayList();
            audioMovementMistakeTempList = new ArrayList();
            bodyMistakes = new ArrayList();
        }
        public override void analyze()
        {
            apa = this.parent.audioHandler.audioPreAnalysis;
            searchMistakes();
            mistakeList = new ArrayList(audioMovementMistakeTempList);
        }
        private void searchMistakes()
        {
            findMistakesInVoiceAndMovement();
            deleteVoiceAndMovementsMistakes();
        }

        #region handleLists

        public void findMistakesInVoiceAndMovement()
        {
            audioMovementMistakeTempList = new ArrayList();

            if (myVoiceAndMovementObject.timeStarted == 0)
            {
                resetMyVoiceAndMovement();
            }
            if (myVoiceAndMovementObject.isSpeaking == true && apa.isSpeaking == true)
            {
                handleSpeakingTime();
            }
            else if (myVoiceAndMovementObject.isSpeaking == false && apa.isSpeaking == true)
            {
                resetMyVoiceAndMovement();
                handleSpeakingTime();

            }
            else if (myVoiceAndMovementObject.isSpeaking == false && apa.isSpeaking == false)
            {
                handlePauses();
            }
            else if (myVoiceAndMovementObject.isSpeaking == true && apa.isSpeaking == false)
            {
                resetMyVoiceAndMovement();
                handlePauses();
            }

        }

        #endregion

        #region audioAnalysis

        private void handleSpeakingTime()
        {
            handleTimeSpeaking();
        }

        private void handleTimeSpeaking()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (currentTime - myVoiceAndMovementObject.timeStarted > apa.ThresholdIsSpeakingLongTime)
            {
                PresentationAction pa = new PresentationAction(PresentationAction.MistakeType.LONG_TALK);
                pa.timeStarted = myVoiceAndMovementObject.timeStarted;
                pa.isVoiceAndMovementMistake = true;
                if (currentTime - myVoiceAndMovementObject.timeStarted > apa.ThresholdIsSpeakingVeryLongTime)
                {
                    pa.interrupt = true;
                }

                audioMovementMistakeTempList.Add(pa);
            }
        }

        public void handlePauses()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (currentTime - myVoiceAndMovementObject.timeStarted > apa.ThresholdIsLongPauseTime)
            {
                PresentationAction pa = new PresentationAction(PresentationAction.MistakeType.LONG_PAUSE);
                pa.timeStarted = myVoiceAndMovementObject.timeStarted;
                pa.isVoiceAndMovementMistake = true;
                audioMovementMistakeTempList.Add(pa);
            }
        }

        public void resetMyVoiceAndMovement()
        {
            myVoiceAndMovementObject.isSpeaking = apa.isSpeaking;
            myVoiceAndMovementObject.timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;

            audioMovementMistakeTempList = new ArrayList();


        }

        #endregion

        #region deletingVoiceandMovements

        private void deleteVoiceAndMovementsMistakes()
        {
            int x = 0;
            int[] mistakesToDelete = new int[mistakeList.Count];
            foreach (PresentationAction pa in mistakeList)
            {
                foreach (PresentationAction pb in audioMovementMistakeTempList)
                {
                    if (pb.myMistake == pa.myMistake)
                    {
                        mistakesToDelete[x] = 1;
                        break;
                    }
                }
                x++;
            }
            for (int j = mistakeList.Count; j > 0; j--)
            {
                if (mistakesToDelete[j - 1] == 1 && ((PresentationAction)mistakeList[j - 1]).isVoiceAndMovementMistake)
                {
                    mistakeList.RemoveAt(j - 1);
                }
            }
        }
        #endregion
    }
}
