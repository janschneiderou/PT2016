    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Media.Media3D;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Face;

namespace PresentationTrainer
{
    public class FaceFramePreAnalisys
    {
        public FaceFrameResult myFace =null;

        public bool smiling = false;
        public bool smilingTemp = false;

        /// <summary>
        /// Face rotation display angle increment in degrees
        /// </summary>
        private const double FaceRotationIncrementInDegrees = 5.0;

        public  double yaw; // panning angle 
        public double pitch;
        public double roll;
       // public FaceFrame faceFrame[];

        public double yawMin;
        public double yawMax;

        public FaceFramePreAnalisys(FaceFrameResult face)
        {

            this.myFace = face;
            
        }
        public void facePreAnalysis()
        {
            

            
        }
        public void Analyze()
        {
            string faceText = string.Empty;

            ExtractFaceRotationInDegrees(myFace.FaceRotationQuaternion);

            // extract each face property information and store it in faceText
            if (myFace.FaceProperties != null)
            {
                foreach (var item in myFace.FaceProperties)
                {
                    faceText += item.Key.ToString() + " : ";

                    // consider a "maybe" as a "no" to restrict 
                    // the detection result refresh rate
                    if (item.Value == DetectionResult.Maybe)
                    {
                        faceText += DetectionResult.No + "\n";
                    }
                    else
                    {
                        faceText += item.Value.ToString() + "\n";
                    }
                    if(item.Key.ToString().Equals("Happy"))
                    {
                        if (item.Value == DetectionResult.Yes || item.Value == DetectionResult.Maybe)
                        {
                            smilingTemp = true;
                        }
                        else
                        {
                            smilingTemp = false;
                        }
                    
                    }
                }
            }
            
            if(smilingTemp!=smiling)
            {
                if(smiling) //Stop Smiling
                {
                    //Do loggin stuf
                    smiling = smilingTemp;

                }
                else //start smilling
                {
                    smiling = smilingTemp;
                }
            }
            
        }

        private  void ExtractFaceRotationInDegrees(Vector4 rotQuaternion)
        {
            double x = rotQuaternion.X;
            double y = rotQuaternion.Y;
            double z = rotQuaternion.Z;
            double w = rotQuaternion.W;

            // convert face rotation quaternion to Euler angles in degrees
            double yawD, pitchD, rollD;
            pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
            yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
            rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

            pitch = pitchD;
            yaw = yawD;
            roll = rollD;

            //// clamp the values to a multiple of the specified increment to control the refresh rate
            //double increment = FaceRotationIncrementInDegrees;
            //pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
            //yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
            //roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);
        }
    }


}
