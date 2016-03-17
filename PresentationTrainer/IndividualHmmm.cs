using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    class IndividualHmmm:IndividualTracker
    {
        public IndividualHmmm(MainWindow parent)
        {
            myVoiceAndMovementObject = new PresentationAction();
            periodicMovements = new PeriodicMovements();

            tempMistakeList = new ArrayList();
            mistakeList = new ArrayList();
            voiceAndMovementsList = new ArrayList();
            audioMovementMistakeTempList = new ArrayList();
            bodyMistakes = new ArrayList();
            this.parent = parent;
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
            handleHmmm();
        }

        private void handleHmmm()
        {
            if (apa.foundHmmm == true)
            {
                PresentationAction pa = new PresentationAction(PresentationAction.MistakeType.HMMMM);
                pa.timeStarted = myVoiceAndMovementObject.timeStarted;
                pa.isVoiceAndMovementMistake = true;
                audioMovementMistakeTempList.Add(pa);
            }
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
