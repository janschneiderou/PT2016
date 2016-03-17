using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace PresentationTrainer
{
    
 public class PeriodicMovements
    {
     float minMovementThreshold = 0.016f;
     double averageTimeDiferenceThreshold = 200;
     float averageDistanceDiferenceThreshold = 0.02f;

     public string fired = "";

     List<PositionTimePair> maxZ;
     List<PositionTimePair> maxY;
     List<PositionTimePair> maxX;
     List<PositionTimePair> minZ;
     List<PositionTimePair> minY;
     List<PositionTimePair> minX;

     List<DisplacementTimePair> DispX;
     List<DisplacementTimePair> DispY;
     List<DisplacementTimePair> DispZ;
   

     enum Direction {increase, decrease,none};
     Direction directionX;
     Direction directionY;
     Direction directionZ;
     float currentMinX;
     float currentMaxX;
     float currentMinY;
     float currentMaxY;
     float currentMinZ;
     float currentMaxZ;
        float maxDisplacement;
        float minDislpacement;
        long maxMovementTime;
        bool isMoving=false;
      CameraSpacePoint hipNew ;
      CameraSpacePoint hipOld;
      DateTime startTime;
      TimeSpan time;
      int currentCycleX = 0;
      int currentCycleY = 0;
      int currentCycleZ = 0;

      public bool result=false;
     
       public PeriodicMovements()
       {
           directionX = Direction.none;
           directionY = Direction.none;
           directionZ = Direction.none;

           maxZ = new List<PositionTimePair>();
           maxY = new List<PositionTimePair>();
           maxX = new List<PositionTimePair>();
           minZ = new List<PositionTimePair>();
           minY = new List<PositionTimePair>();
           minX = new List<PositionTimePair>();
           DispX = new List<DisplacementTimePair>();
           DispY = new List<DisplacementTimePair>();
           DispZ = new List<DisplacementTimePair>();

           startTime = DateTime.Now;
       }
       public bool checPeriodicMovements(Body newBody)
       {
           

           time = DateTime.Now- startTime;
           if(time.TotalMilliseconds>=4000)
           {
               result = checkAnalisys();
               resetValues();
               startTime = DateTime.Now;
           }
           else
           {
               hipNew = newBody.Joints[JointType.HipRight].Position;
               if (BodyFramePreAnalysis.bodyOld.Count > 0)
               {
                   if(Math.Abs(hipNew.X-hipOld.X)>0.009 
                   ||  Math.Abs(hipNew.Y - hipOld.Y) > 0.009 ||
                       Math.Abs(hipNew.Z - hipOld.Z) > 0.009)
                   {
                       hipOld = BodyFramePreAnalysis.bodyOld[JointType.HipRight].Position;
                       storeValues();
                   }
                   
               }
               
           }

          
          


           
           return result;
       }
       bool checkAnalisys()
       {
           bool result=false;
           int countX = DispX.Count;
           if(countX>1)
           {
               //List<double> timesDiferences=new List<double>();
               //List<float> distanceDiferences = new List<float>();

               //int j = 0;
               //double averageTimeDiference = 0;
               //float averageDistanceDiference = 0;
               //for (int i = countX - 1; i > 0; i--)
               //{
               //    TimeSpan time = DispX[i].time - DispX[i - 1].time;
               //    float distanceDif = Math.Abs(DispX[i].displacement - DispX[i - 1].displacement);
               //    timesDiferences.Add(time.TotalMilliseconds);
               //    distanceDiferences.Add(distanceDif);
               //    averageTimeDiference = averageTimeDiference + time.Milliseconds;
               //    averageDistanceDiference = averageDistanceDiference + distanceDif;
               //    j++;
               //}

               //averageTimeDiference = averageTimeDiference/j;
               //averageDistanceDiference = Math.Abs(averageDistanceDiference/j);

               float averageDistance=0;
               for(int i=0;i<countX;i++)
               {
                   averageDistance = averageDistance + DispX[i].displacement;
               }
               averageDistance = averageDistance / countX;

               if(averageDistance>minMovementThreshold && countX>2)  
                   //&& 
                   //averageDistanceDiference<averageDistanceDiferenceThreshold
                   //)
               {
                   result = true;
                   fired = "X";
               }

           }
           int countY = DispY.Count;
           if (countY > 1)
           {

               float averageDistance = 0;
               for (int i = 0; i < countY; i++)
               {
                   averageDistance = averageDistance + DispY[i].displacement;
               }
               averageDistance = averageDistance / countY;

               if (averageDistance > minMovementThreshold && countY >3)
               {
       //            result = true;
               }

           }
           int countZ = DispZ.Count;
           if (countZ > 1)
           {

               float averageDistance = 0;
               for (int i = 0; i < countZ; i++)
               {
                   averageDistance = averageDistance + DispZ[i].displacement;
               }
               averageDistance = averageDistance / countZ;

               if (averageDistance > minMovementThreshold && countZ >= 2)
               {
                   result = true;
                   fired = "Z " + countZ + " "+ averageDistance;
               }

           }

           return result;
       }
     void resetValues()
     {
         maxZ = new List<PositionTimePair>();
         maxY = new List<PositionTimePair>();
         maxX = new List<PositionTimePair>();
         minZ = new List<PositionTimePair>();
         minY = new List<PositionTimePair>();
         minX = new List<PositionTimePair>();

         DispX = new List<DisplacementTimePair>();
         DispY = new List<DisplacementTimePair>();
         DispZ = new List<DisplacementTimePair>();

         directionX = Direction.none;
         directionY = Direction.none;
         directionZ = Direction.none;
     }
     void storeValues()
     {
         checDirections();
     }
     public void checDirections()
     {
         checkDirectionX();
         checkDirectionY();
         checkDirectionZ();
         
        
     }
     public void checkDirectionX()
     {
         if (hipNew.X >= hipOld.X)
         {
             if (directionX.Equals(Direction.decrease))
             {
                 minX.Add(new PositionTimePair(hipOld.X));
                 if (minX.Count == maxX.Count)
                 {
                     float max = Math.Abs(this.maxX[this.maxX.Count - 1].position);
                     float min = Math.Abs(this.minX[this.minX.Count - 1].position);
                     float displacement = Math.Abs(max - min);
                     if (displacement > minMovementThreshold)
                     {
                         DispX.Add(new DisplacementTimePair(displacement));
                     }

                 }
             }
             directionX = Direction.increase;
         }
         else
         {
             if (directionX.Equals(Direction.increase))
             {
                 maxX.Add(new PositionTimePair(hipOld.X));
                 if (minX.Count == maxX.Count)
                 {
                     float max = Math.Abs(this.maxX[this.maxX.Count - 1].position);
                     float min = Math.Abs(this.minX[this.minX.Count - 1].position);
                     float displacement = Math.Abs(max - min);
                     if (displacement > minMovementThreshold)
                     {
                         DispX.Add(new DisplacementTimePair(displacement));
                     }

                 }
             }
             directionX = Direction.decrease;
         }
     }
     public void checkDirectionY()
     {
         if (hipNew.Y >= hipOld.Y)
         {
             if (directionY.Equals(Direction.decrease))
             {
                 minY.Add(new PositionTimePair(hipOld.Y));
                 if (minY.Count == maxY.Count)
                 {
                     float max = Math.Abs(this.maxY[this.maxY.Count - 1].position);
                     float min = Math.Abs(this.minY[this.minY.Count - 1].position);
                     float displacement = Math.Abs(max - min);
                     if (displacement > minMovementThreshold)
                     {
                         DispY.Add(new DisplacementTimePair(displacement));
                     }

                 }
             }
             directionY = Direction.increase;
         }
         else
         {
             if (directionY.Equals(Direction.increase))
             {
                 maxY.Add(new PositionTimePair(hipOld.Y));
                 if (minY.Count == maxY.Count)
                 {
                     float max = Math.Abs(this.maxY[this.maxY.Count - 1].position);
                     float min = Math.Abs(this.minY[this.minY.Count - 1].position);
                     float displacement = Math.Abs(max - min);
                     if (displacement > minMovementThreshold)
                     {
                         DispY.Add(new DisplacementTimePair(displacement));
                     }

                 }
             }
             directionY = Direction.decrease;
         }
     }
     public void checkDirectionZ()
     {
         if (hipNew.Z >= hipOld.Z)
         {
             if (directionZ.Equals(Direction.decrease))
             {
                 minZ.Add(new PositionTimePair(hipOld.Z));
                 if (minZ.Count == maxZ.Count)
                 {
                     float max = Math.Abs(this.maxZ[this.maxZ.Count - 1].position);
                     float min = Math.Abs(this.minZ[this.minZ.Count - 1].position);
                     float displacement = Math.Abs(max - min);
                     if (displacement > minMovementThreshold)
                     {
                         DispZ.Add(new DisplacementTimePair(displacement));
                     }

                 }
             }
             directionZ = Direction.increase;
         }
         else
         {
             if (directionZ.Equals(Direction.increase))
             {
                 maxZ.Add(new PositionTimePair(hipOld.Z));
                 if (minZ.Count == maxZ.Count)
                 {
                     float max = Math.Abs(this.maxZ[this.maxZ.Count - 1].position);
                     float min = Math.Abs(this.minZ[this.minZ.Count - 1].position);
                     float displacement = Math.Abs(max - min);
                     if (displacement > minMovementThreshold)
                     {
                         DispZ.Add(new DisplacementTimePair(displacement));
                     }

                 }
             }
             directionZ = Direction.decrease;
         }
     }
    }
    
 public class PositionTimePair
 {
     public float position;
     public DateTime time;
     public PositionTimePair(float position)
     {
         this.position = position;
         this.time = DateTime.Now;
     }

 }
    public class DisplacementTimePair
    {
        public float displacement;
        public DateTime time;
        public DisplacementTimePair(float displacement)
        {
            this.displacement = displacement;
            this.time = DateTime.Now;
        }
    }
}
