using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    class IndividualPosture:IndividualTracker
    {
        public IndividualPosture(MainWindow parent)
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
            bfpa = this.parent.bodyFrameHandler.bodyFramePreAnalysis;

            searchMistakes();

            mistakeList = new ArrayList(bodyMistakes);
        }


        #region HandleList
        private void searchMistakes()
        {
            addBodyMistakes();
            deleteBodyMistakes();

            
        }

        #region addMistakes

        private void addBodyMistakes()
        {
            addNewMistakesToTemp();
            findMistakesInTempList();
        }
        private void addNewMistakesToTemp()
        {

            foreach (PresentationAction fb in bfpa.currentMistakes)
            {
                int x = 0;
                foreach (PresentationAction fa in tempMistakeList)
                {
                    if (fa.myMistake == fb.myMistake)
                    {
                        x = 1;
                        break;
                    }
                }
                if (x == 0)
                {
                    fb.timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    tempMistakeList.Add(fb);
                }
            }
        }

        private void findMistakesInTempList()
        {
            foreach (PresentationAction fa in tempMistakeList)
            {
                foreach (PresentationAction fb in bfpa.currentMistakes)
                {
                    if (fa.myMistake == fb.myMistake)
                    {
                        if (findMistakeInMistakeList(fb) == false)
                        {
                            if (checkTimeToPutMistake(fa))
                            {

                                bodyMistakes.Add(fa);


                                fa.firstImage = parent.videoHandler.kinectImage.Source.CloneCurrentValue();
                            }
                        }

                    }
                }
            }
        }



        private bool findMistakeInMistakeList(PresentationAction fb)
        {
            bool found = false;
            foreach (PresentationAction fa in mistakeList)
            {
                if (fb.myMistake == fa.myMistake)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private bool checkTimeToPutMistake(PresentationAction fa)
        {
            bool result = false;
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (currentTime - fa.timeStarted > TIME_TO_CONSIDER_ACTION)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region deletingBodyMistakes
        private void deleteBodyMistakes()
        {
            findNoMistakesInBodyTempList();
            removeMistakesBodyTemp();
            removeBodyMistakes();
        }

        private void findNoMistakesInBodyTempList()
        {
            nolongerBodyErrors = new int[tempMistakeList.Count];
            int i = 0;
            foreach (PresentationAction fa in tempMistakeList)
            {
                int x = 0;
                foreach (PresentationAction fb in bfpa.currentMistakes)
                {
                    if (fa.myMistake == fb.myMistake)
                    {
                        x = 1;
                        fa.timeFinished = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        break;
                    }
                }
                if (x == 0)
                {
                    double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    //      if (currentTime - fa.timeFinished > TIME_TO_CONSIDER_CORRECTION)
                    //      {
                    nolongerBodyErrors[i] = 1;
                    //     }
                }
                i++;
            }
        }

        private void removeMistakesBodyTemp()
        {
            for (int i = tempMistakeList.Count; i > 0; i--)
            {
                if (nolongerBodyErrors[i - 1] == 1)
                {
                    tempMistakeList.RemoveAt(i - 1);
                }
            }
        }

        private void removeBodyMistakes()
        {
            int[] nolongerMistakes = new int[bodyMistakes.Count];
            int i = 0;
            foreach (PresentationAction fa in bodyMistakes)
            {
                for (int j = tempMistakeList.Count; j > 0; j--)
                {
                    if (((PresentationAction)tempMistakeList[j - 1]).myMistake == fa.myMistake)
                    {
                        nolongerMistakes[i] = 1;
                    }
                }
                i++;
            }
            for (int ii = bodyMistakes.Count; ii > 0; ii--)
            {
                if (nolongerMistakes[ii - 1] == 0)
                {
                    bodyMistakes.RemoveAt(ii - 1);
                }
            }
        }

        #endregion

        #endregion
    }
}
