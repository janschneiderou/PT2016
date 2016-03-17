using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    class IndividualHandMovements:IndividualTracker
    {

        public double ThresholdDefaultHandMovement = 1000;
        public double HandMovementFactor = 3770;

        public IndividualHandMovements(MainWindow parent)
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

        #endregion

        #region audioandMovementAnalysis

        private void handleSpeakingTime()
        {
            if (bfpa.body != null)
            {
                handleHandMovements();
            }


        }

        private void handleHandMovements()
        {

            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            CameraSpacePoint handLeft = bfpa.body.Joints[JointType.HandLeft].Position;
            CameraSpacePoint handRight = bfpa.body.Joints[JointType.HandRight].Position;
            CameraSpacePoint hipLeft = bfpa.body.Joints[JointType.HipLeft].Position;
            CameraSpacePoint hipRight = bfpa.body.Joints[JointType.HipRight].Position;

            double leftDistance = Math.Sqrt(
                (handLeft.X - hipLeft.X) * (handLeft.X - hipLeft.X) +
                (handLeft.Y - hipLeft.Y) * (handLeft.Y - hipLeft.Y) +
                (handLeft.Z - hipLeft.Z) * (handLeft.Z - hipLeft.Z));

            double rightDistance = Math.Sqrt(
                (handRight.X - hipRight.X) * (handRight.X - hipRight.X) +
                (handRight.Y - hipRight.Y) * (handRight.Y - hipRight.Y) +
                (handRight.Z - hipRight.Z) * (handRight.Z - hipRight.Z));

            if (myVoiceAndMovementObject.leftHandHipDistance == 0)
            {
                myVoiceAndMovementObject.leftHandHipDistance = leftDistance;
                myVoiceAndMovementObject.rightHandHipDistance = rightDistance;
            }

            double rightMovement = Math.Abs(myVoiceAndMovementObject.rightHandHipDistance - rightDistance);
            double leftMovement = Math.Abs(myVoiceAndMovementObject.leftHandHipDistance - leftDistance);

            myVoiceAndMovementObject.leftHandHipDistance = leftDistance;
            myVoiceAndMovementObject.rightHandHipDistance = rightDistance;

            double currentHandMovement = rightMovement;

            if (rightMovement > leftMovement)
            {
                myVoiceAndMovementObject.totalHandMovement = myVoiceAndMovementObject.totalHandMovement + rightMovement * HandMovementFactor;
            }
            else
            {
                currentHandMovement = leftMovement;
                myVoiceAndMovementObject.totalHandMovement = myVoiceAndMovementObject.totalHandMovement + leftMovement * HandMovementFactor;
            }

            double difTime = (currentTime - myVoiceAndMovementObject.timeStarted);
            if (difTime > 0)
            {
                myVoiceAndMovementObject.averageHandMovement = myVoiceAndMovementObject.totalHandMovement / difTime;
            }

            if (myVoiceAndMovementObject.averageHandMovement < 1)//&& currentHandMovement < 0.006)
            {
                PresentationAction pa = new PresentationAction(PresentationAction.MistakeType.HANDS_NOT_MOVING);
                pa.timeStarted = myVoiceAndMovementObject.timeStarted;
                pa.isVoiceAndMovementMistake = true;
                if (myVoiceAndMovementObject.firstImage != null)
                {
                    System.Windows.Media.ImageSource im = parent.videoHandler.kinectImage.Source;
                    //  pa.firstImage = myVoiceAndMovementObject.firstImage.CloneCurrentValue();
                    myVoiceAndMovementObject.lastImage = im.CloneCurrentValue();
                    //  pa.lastImage = im.CloneCurrentValue();
                }

                audioMovementMistakeTempList.Add(pa);
            }
            else if (difTime > 2000) //use for debugging
            {
                int aa = 0;
                aa++;
                //     myVoiceAndMovementObject.totalHandMovement = difTime + 1000;
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
