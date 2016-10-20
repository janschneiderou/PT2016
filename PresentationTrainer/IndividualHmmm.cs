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
