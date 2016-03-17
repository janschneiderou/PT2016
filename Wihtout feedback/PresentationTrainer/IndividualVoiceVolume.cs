using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    class IndividualVoiceVolume:IndividualTracker
    {
        public IndividualVoiceVolume(MainWindow parent)
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
            else if (myVoiceAndMovementObject.isSpeaking == true && apa.isSpeaking == false)
            {
                resetMyVoiceAndMovement();
            }

        }

        #region volumeAnalysis

        private void handleSpeakingTime()
        {
            handleVolume();

        }
        private void handleVolume()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (myVoiceAndMovementObject.minVolume == 0)
            {
                myVoiceAndMovementObject.minVolume = apa.averageVolume;
                myVoiceAndMovementObject.maxVolume = apa.averageVolume;
            }
            if (apa.averageVolume < myVoiceAndMovementObject.minVolume)
            {
                myVoiceAndMovementObject.minVolume = apa.averageVolume;
            }
            if (apa.averageVolume > myVoiceAndMovementObject.maxVolume)
            {
                myVoiceAndMovementObject.maxVolume = apa.averageVolume;
            }
            if (apa.averageVolume > apa.ThresholdIsSpeakingLoud)
            {
                LowVolume = null;
                if (highVolume == null)
                {
                    highVolume = new PresentationAction(PresentationAction.MistakeType.HIGH_VOLUME);
                    highVolume.isVoiceAndMovementMistake = true;
                }
                if (currentTime - highVolume.timeStarted > TIME_TO_CONSIDER_ACTION)
                {

                    audioMovementMistakeTempList.Add(highVolume);
                }
            }
            else if (apa.averageVolume < apa.ThresholdIsSpeakingSoft)
            {
                highVolume = null;

                if (LowVolume == null)
                {
                    LowVolume = new PresentationAction(PresentationAction.MistakeType.LOW_VOLUME);
                    LowVolume.isVoiceAndMovementMistake = true;
                }
                if (currentTime - LowVolume.timeStarted > TIME_TO_CONSIDER_ACTION)
                {
                    audioMovementMistakeTempList.Add(LowVolume);
                }
            }
            else
            {
                highVolume = null;
                LowVolume = null;
            }
            if (currentTime - myVoiceAndMovementObject.timeStarted > 3000 &&
                myVoiceAndMovementObject.maxVolume - myVoiceAndMovementObject.minVolume < 0.001)
            {
                PresentationAction pa = new PresentationAction(PresentationAction.MistakeType.LOW_MODULATION);
                {
                    pa.timeStarted = myVoiceAndMovementObject.timeStarted;
                    pa.isVoiceAndMovementMistake = true;
                    audioMovementMistakeTempList.Add(pa);
                }
            }


        }

        public void resetMyVoiceAndMovement()
        {
            myVoiceAndMovementObject.isSpeaking = apa.isSpeaking;
            myVoiceAndMovementObject.timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
            myVoiceAndMovementObject.interrupt = false;
            myVoiceAndMovementObject.totalHandMovement = ThresholdDefaultHandMovement;
            myVoiceAndMovementObject.minVolume = 0;
            myVoiceAndMovementObject.maxVolume = 0;
            myVoiceAndMovementObject.rightHandHipDistance = 0;
            myVoiceAndMovementObject.leftHandHipDistance = 0;
            myVoiceAndMovementObject.averageHandMovement = 1;
            audioMovementMistakeTempList = new ArrayList();
            if (parent.videoHandler.kinectImage.Source != null)
            {
                myVoiceAndMovementObject.firstImage = null;
                myVoiceAndMovementObject.firstImage = parent.videoHandler.kinectImage.Source.CloneCurrentValue();
            }

        }
        #endregion

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
