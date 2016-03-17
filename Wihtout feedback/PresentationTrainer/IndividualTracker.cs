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
