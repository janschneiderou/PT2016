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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    public class AudioPreAnalysis
    {
        public float ThresholdIsSpeaking = 0.2f;
        public float ThresholdIsSpeakingLoud= 0.5f;
        public float ThresholdIsSpeakingVeryLoud= 0.6f;
        public float ThresholdIsSpeakingSoft=0.32f;
        public float ThresholdIsSpeakingVerySoft=0.25f;

        public float ThresholdPauseStartTime = 250f;
        public float ThresholdIsLongPauseTime=3000f;
        public float ThresholdIsVeryLongPauseTime=6000f;
        public float ThresholdIsSpeakingLongTime=12000f;
        public float ThresholdIsSpeakingVeryLongTime=15000f;
        public float ThresholdShortPause=1000f;
        public float ThresholdActuallySpeaking = 500f;
        public float ThresholdShortSpeakingTime = 1000f;
        

        public DateTime startedSpeaking;
        public DateTime startedPause;
        public DateTime stoppedSpeaking;

        public bool isNotSpeaking = false;
        public bool isSpeaking = false;

        public Cadence previousValue;

        public bool loud = false;
        public byte[] audioBuffer = null;
        public  float[] energy = null;
        public float averageVolume;
        public bool foundHmmm = false;
        public bool foundHmmmHandled = false;

        public enum Volume { verySoft, soft, good, loud, veryLoud };
        public enum Cadence { veryLongPause, longPause, shortPause, goodPause, shortSpeakingTime, goodSpeakingTime, longSpeakingTime, veryLongSpeakingTime };        

        public Volume voiceVolume;
        public Cadence voiceCadence;

        public AudioPreAnalysis()
        {

        }

        public void analyzeAudio(byte[] audioBuffer, float[] energy)
        {
            this.audioBuffer = audioBuffer;
            this.energy = energy;
            this.averageVolume = 0;

            calcAverageVolume();
            calcIsSpeaking();
            if(isSpeaking)
            {
                TimeSpan speaking = new TimeSpan(startedSpeaking.Ticks);
                TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                if (now.TotalMilliseconds - speaking.TotalMilliseconds > ThresholdActuallySpeaking)
                {
                    assignSpeakingVolume();
                    calcSpeakingTime();
                }
                else
                {
                    voiceVolume = Volume.good;
                    calcPausingTime();
                }
            }
            else
            {
                voiceVolume = Volume.good;
                calcPausingTime();
            }

        }

        //probably should be called from rulesAnalizer
        public void calcSpeakingTime()
        {
            TimeSpan speaking = new TimeSpan(startedSpeaking.Ticks);
            TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
            
            if (now.TotalMilliseconds - speaking.TotalMilliseconds < ThresholdShortSpeakingTime)
            {
                voiceCadence = Cadence.shortSpeakingTime;
            }
            else if(now.TotalMilliseconds-speaking.TotalMilliseconds<ThresholdIsSpeakingLongTime)
            {
                voiceCadence = Cadence.goodSpeakingTime;
            }
            else if(now.TotalMilliseconds-speaking.TotalMilliseconds<ThresholdIsSpeakingVeryLongTime)
            {
                voiceCadence = Cadence.longSpeakingTime;
            }
            else 
            {
                voiceCadence = Cadence.veryLongSpeakingTime;
            }
        }

        //probably should be called from rulesAnalizer
        public void calcPausingTime()
        {
            TimeSpan pausing = new TimeSpan(startedPause.Ticks);
            TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
            if (now.TotalMilliseconds - pausing.TotalMilliseconds < ThresholdShortPause)
            {
                voiceCadence = Cadence.shortPause;
            }
            else if (now.TotalMilliseconds - pausing.TotalMilliseconds < ThresholdIsLongPauseTime)
            {
                voiceCadence = Cadence.goodPause;
            }
            else if (now.TotalMilliseconds - pausing.TotalMilliseconds < ThresholdIsVeryLongPauseTime)
            {
                voiceCadence = Cadence.longPause;
            }
            else
            {
                voiceCadence = Cadence.veryLongPause;
            }
        }

        public void calcAverageVolume()
        {
            int x=0;
            for (int i = 0; i < energy.Length; i++)
            {
                if(energy[i]>ThresholdIsSpeaking)
                {
                    averageVolume = averageVolume + energy[i];
                    x++;
                }
                
            }
            averageVolume = averageVolume / x;
        }

        public void calcIsSpeaking()
        {
            if (Math.Abs(averageVolume) > ThresholdIsSpeaking)
            {
                if(isSpeaking==false)
                {
                    startedSpeaking = DateTime.Now;
                    previousValue = voiceCadence;
                }
                isSpeaking = true;
                isNotSpeaking=false;
            }
            else
            {
                if(isSpeaking)
                {
                    if(isNotSpeaking==false)
                    {
                        stoppedSpeaking = DateTime.Now;
                        isNotSpeaking=true;
                    }
                    TimeSpan pausedtime = new TimeSpan(stoppedSpeaking.Ticks);
                    TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                    if (now.TotalMilliseconds - pausedtime.TotalMilliseconds > ThresholdPauseStartTime)
                    {
                        startedPause = stoppedSpeaking;
                        isSpeaking = false;
                        previousValue = voiceCadence;
                    }
                }
                
            }
        }

        public void assignSpeakingVolume()
        {
            
            if (averageVolume < ThresholdIsSpeakingVerySoft)
            {
                voiceVolume = Volume.verySoft;
            }
            else if (averageVolume < ThresholdIsSpeakingSoft)
            {
                voiceVolume = Volume.soft;
            }
            else if (averageVolume < ThresholdIsSpeakingLoud)
            {
                voiceVolume = Volume.good;
            }
            else if (averageVolume < ThresholdIsSpeakingVeryLoud)
            {
                voiceVolume = Volume.loud;
            }
            else
            {
                voiceVolume = Volume.veryLoud;

            }                       
        }
    }
}
