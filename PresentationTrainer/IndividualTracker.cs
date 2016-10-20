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

    public class IndividualTracker
    {

        public ArrayList tempMistakeList;
        public ArrayList mistakeList;
        public ArrayList bodyMistakes;
        public ArrayList tempGoodiesList;
        public ArrayList GoodiesList;
        public ArrayList voiceAndMovementsList;
        public ArrayList audioMovementMistakeTempList;

        public double TIME_TO_CONSIDER_ACTION = 300;
        public double TIME_TO_CONSIDER_CORRECTION = 100;
        public double TIME_TO_CONSIDER_INTERRUPTION = 6000;

        public double ThresholdDefaultHandMovement = 1000;
        public double HandMovementFactor = 3770;
        public double bufferTime = 400;

        public PeriodicMovements periodicMovements;
        public PresentationAction myVoiceAndMovementObject;

        public int[] nolongerBodyErrors;

        public BodyFramePreAnalysis bfpa;
        public AudioPreAnalysis apa;
        public Body oldBody;

      
        public PresentationAction highVolume = null;
        public PresentationAction LowVolume = null;

        public MainWindow parent;

        public IndividualTracker()
        {

        }
        public IndividualTracker(MainWindow parent)
        {
            this.parent = parent;
        }
        public virtual void analyze()
        {

        }

        public void clearLists()
        {
            tempMistakeList.Clear();
            mistakeList.Clear();
            voiceAndMovementsList.Clear();
            audioMovementMistakeTempList.Clear();
            bodyMistakes.Clear();
        }
    }
}
