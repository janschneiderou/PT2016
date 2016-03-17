using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.Runtime.InteropServices;
using System.IO;

namespace PresentationTrainer
{
    public class AudioHandler
    {
        public AudioPreAnalysis audioPreAnalysis;
        private KinectSensor kinectSensor;

      
        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        public SpeechRecognitionEngine speechEngine = null;

        /// <summary>
        /// Number of samples captured from Kinect audio stream each millisecond.
        /// </summary>
        private const int SamplesPerMillisecond = 16;

        /// <summary>
        /// Number of bytes in each Kinect audio stream sample (32-bit IEEE float).
        /// </summary>
        private const int BytesPerSample = sizeof(float);

        /// <summary>
        /// Number of audio samples represented by each column of pixels in wave bitmap.
        /// </summary>
        private const int SamplesPerColumn = 40;

        /// <summary>
        /// Minimum energy of audio to display (a negative number in dB value, where 0 dB is full scale)
        /// </summary>
        private const int MinEnergy = -90;

        /// <summary>
        /// Width of bitmap that stores audio stream energy data ready for visualization.
        /// </summary>
        private const int EnergyBitmapWidth = 300;

        /// <summary>
        /// Height of bitmap that stores audio stream energy data ready for visualization.
        /// </summary>
        private const int EnergyBitmapHeight = 300;

        /// <summary>
        /// Bitmap that contains constructed visualization for audio stream energy, ready to
        /// be displayed. It is a 2-color bitmap with white as background color and blue as
        /// foreground color.
        /// </summary>
        public WriteableBitmap energyBitmap;

        /// <summary>
        /// Rectangle representing the entire energy bitmap area. Used when drawing background
        /// for energy visualization.
        /// </summary>
        private readonly Int32Rect fullEnergyRect = new Int32Rect(0, 0, EnergyBitmapWidth, EnergyBitmapHeight);

        /// <summary>
        /// Array of background-color pixels corresponding to an area equal to the size of whole energy bitmap.
        /// </summary>
        private readonly byte[] backgroundPixels = new byte[EnergyBitmapWidth * EnergyBitmapHeight];
        private readonly byte[] lineVolumePixels = new byte[EnergyBitmapWidth * 3];
        private readonly byte[] lineVolumePixelsIsSpeaking = new byte[EnergyBitmapWidth * 3];
        private readonly byte[] lineVolumePixelsSoft = new byte[EnergyBitmapWidth * 3];
        private readonly byte[] lineVolumePixelsLoud = new byte[EnergyBitmapWidth * 3];


        /// <summary>
        /// Will be allocated a buffer to hold a single sub frame of audio data read from audio stream.
        /// </summary>
        public byte[] audioBuffer = null;

        /// <summary>
        /// Buffer used to store audio stream energy data as we read audio.
        /// We store 25% more energy values than we strictly need for visualization to allow for a smoother
        /// stream animation effect, since rendering happens on a different schedule with respect to audio
        /// capture.
        /// </summary>
      //  public readonly float[] energy = new float[(uint)(EnergyBitmapWidth * 1.25)];
        public readonly float[] energy = new float[(uint)(EnergyBitmapWidth * 1)];

        /// <summary>
        /// Object for locking energy buffer to synchronize threads.
        /// </summary>
        private readonly object energyLock = new object();

        /// <summary>
        /// Reader for audio frames
        /// </summary>
        public AudioBeamFrameReader reader = null;

        /// <summary>
        /// Array of foreground-color pixels corresponding to a line as long as the energy bitmap is tall.
        /// This gets re-used while constructing the energy visualization.
        /// </summary>
        private byte[] foregroundPixels;

        /// <summary>
        /// Sum of squares of audio samples being accumulated to compute the next energy value.
        /// </summary>
        private float accumulatedSquareSum;

        /// <summary>
        /// Number of audio samples accumulated so far to compute the next energy value.
        /// </summary>
        private int accumulatedSampleCount;

        /// <summary>
        /// Index of next element available in audio energy buffer.
        /// </summary>
        public int energyIndex;

        /// <summary>
        /// Number of newly calculated audio stream energy values that have not yet been
        /// displayed.
        /// </summary>
        private int newEnergyAvailable;

        /// <summary>
        /// Error between time slice we wanted to display and time slice that we ended up
        /// displaying, given that we have to display in integer pixels.
        /// </summary>
        private float energyError;

        /// <summary>
        /// Last time energy visualization was rendered to screen.
        /// </summary>
        private DateTime? lastEnergyRefreshTime;

        /// <summary>
        /// Index of first energy element that has never (yet) been displayed to screen.
        /// </summary>
        private int energyRefreshIndex;

        /// <summary>
        /// Stream for 32b-16b conversion.
        /// </summary>
        private KinectAudioStream convertStream = null;

        public float isSpeakingValue = 0;
        public float speakingSoftValue = 0;
        public float speakingLoudValue = 0;

        private static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }


        public AudioHandler(KinectSensor kinectSensor)
        {
            audioPreAnalysis = new AudioPreAnalysis();
            this.kinectSensor = kinectSensor;
            // Get its audio source
            AudioSource audioSource = this.kinectSensor.AudioSource;

            // Allocate 1024 bytes to hold a single audio sub frame. Duration sub frame 
            // is 16 msec, the sample rate is 16khz, which means 256 samples per sub frame. 
            // With 4 bytes per sample, that gives us 1024 bytes.
            this.audioBuffer = new byte[audioSource.SubFrameLengthInBytes];

            // Open the reader for the audio frames
            this.reader = audioSource.OpenReader();

           // PixelFormats.Indexed1;
            this.energyBitmap = new WriteableBitmap(EnergyBitmapWidth, EnergyBitmapHeight, 96, 96, PixelFormats.Indexed4, new BitmapPalette(new List<Color> { Colors.White, Colors.Green, Colors.Red, Colors.LightBlue, Colors.Aquamarine, Colors.Pink, Colors.Orange}));

            // Initialize foreground pixels
            this.foregroundPixels = new byte[EnergyBitmapHeight];
            for (int i = 0; i < this.foregroundPixels.Length; ++i)
            {
                this.foregroundPixels[i] = 0xff;
            }
            for (int i = 0; i < this.lineVolumePixels.Length; ++i)
            {
                this.lineVolumePixels[i] = 0x55;
            }
            for (int i = 0; i < this.lineVolumePixelsIsSpeaking.Length; ++i)
            {
                this.lineVolumePixelsIsSpeaking[i] = 0x11;
            }
            for (int i = 0; i < this.lineVolumePixelsSoft.Length; ++i)
            {
                this.lineVolumePixelsSoft[i] = 0x22;
            }
            for (int i = 0; i < this.lineVolumePixelsLoud.Length; ++i)
            {
                this.lineVolumePixelsLoud[i] = 0x33;
            }

        //    this.kinectImage.Source = this.energyBitmap;
            CompositionTarget.Rendering += this.UpdateEnergy;
            
            

            //if (this.reader != null)
            //{
            //    // Subscribe to new audio frame arrived events
            //    this.reader.FrameArrived += this.Reader_FrameArrived;
            //}


            getSpeechthings();

        }
        public void getSpeechthings()
        {
            // SPEECH STUFF
            // grab the audio stream
            try
            {
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // create the convert stream
                this.convertStream = new KinectAudioStream(audioStream);
            }
            catch
            {

            }

            RecognizerInfo ri = TryGetKinectRecognizer();

            if (null != ri)
            {
             

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                /****************************************************************
                * 
                * Use this code to create grammar programmatically rather than from
                * a grammar file.
                * 
                * var directions = new Choices();
                * directions.Add(new SemanticResultValue("forward", "FORWARD"));
                * directions.Add(new SemanticResultValue("forwards", "FORWARD"));
                * directions.Add(new SemanticResultValue("straight", "FORWARD"));
                * directions.Add(new SemanticResultValue("backward", "BACKWARD"));
                * directions.Add(new SemanticResultValue("backwards", "BACKWARD"));
                * directions.Add(new SemanticResultValue("back", "BACKWARD"));
                * directions.Add(new SemanticResultValue("turn left", "LEFT"));
                * directions.Add(new SemanticResultValue("turn right", "RIGHT"));
                *
                * var gb = new GrammarBuilder { Culture = ri.Culture };
                * gb.Append(directions);
                *
                * var g = new Grammar(gb);
                * 
                ****************************************************************/

                // Create a grammar from grammar definition XML file.
                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                {
                    var g = new Grammar(memoryStream);
                    this.speechEngine.LoadGrammar(g);
                }

                this.speechEngine.SpeechRecognized += this.SpeechRecognized;
                this.speechEngine.SpeechRecognitionRejected += this.SpeechRejected;
          

                // let the convertStream know speech is going active
                this.convertStream.SpeechActive = true;

                // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
                // This will prevent recognition accuracy from degrading over time.
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
          
        }
        

        public void close()
        {
           // CompositionTarget.Rendering -= this.UpdateEnergy;

            if (this.reader != null)
            {
                // AudioBeamFrameReader is IDisposable
                this.reader.Dispose();
                this.reader = null;
            }

            if (null != this.speechEngine)
            {
                this.speechEngine.SpeechRecognized -= this.SpeechRecognized;
                this.speechEngine.SpeechRecognitionRejected -= this.SpeechRejected;
                this.speechEngine.RecognizeAsyncStop();
            }
        }

        /// <summary>
        /// Handles the audio frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        public void Reader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            AudioBeamFrameReference frameReference = e.FrameReference;

            try
            {
                AudioBeamFrameList frameList = frameReference.AcquireBeamFrames();

                if (frameList != null)
                {
                    // AudioBeamFrameList is IDisposable
                    using (frameList)
                    {
                        // Only one audio beam is supported. Get the sub frame list for this beam
                        IReadOnlyList<AudioBeamSubFrame> subFrameList = frameList[0].SubFrames;

                        // Loop over all sub frames, extract audio buffer and beam information
                        foreach (AudioBeamSubFrame subFrame in subFrameList)
                        {

                            subFrame.CopyFrameDataToArray(this.audioBuffer);
                           

                            for (int i = 0; i < this.audioBuffer.Length; i += BytesPerSample)
                            {
                                // Extract the 32-bit IEEE float sample from the byte array
                                float audioSample = BitConverter.ToSingle(this.audioBuffer, i);

                                this.accumulatedSquareSum += audioSample * audioSample;
                                ++this.accumulatedSampleCount;

                                if (this.accumulatedSampleCount < SamplesPerColumn)
                                {
                                    continue;
                                }

                                float meanSquare = this.accumulatedSquareSum / SamplesPerColumn;

                                if (meanSquare > 1.0f)
                                {
                                //    // A loud audio source right next to the sensor may result in mean square values
                                //    // greater than 1.0. Cap it at 1.0f for display purposes.
                                    meanSquare = 1.0f;
                                }

                                // Calculate energy in dB, in the range [MinEnergy, 0], where MinEnergy < 0
                                float energy = MinEnergy;

                                if (meanSquare > 0)
                                {
                                    energy = (float)(10.0 * Math.Log10(meanSquare));
                                }
                                
                                lock (this.energyLock)
                                {
                                    // Normalize values to the range [0, 1] for display
                                    this.energy[this.energyIndex] = (MinEnergy - energy) / MinEnergy;
                                  //  this.energy[this.energyIndex] = energy;
                                    this.energyIndex = (this.energyIndex + 1) % this.energy.Length;
                                    ++this.newEnergyAvailable;
                                }

                                this.accumulatedSquareSum = 0;
                                this.accumulatedSampleCount = 0;
                            }
                            audioPreAnalysis.analyzeAudio(this.audioBuffer, this.energy);
                        }

                        
                    }
                }
            }
            catch (Exception)
            {
                // Ignore if the frame is no longer available
            }
        }


        /// <summary>
        /// Handler for recognized speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;
            

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                switch (e.Result.Semantics.Value.ToString())
               {
                    case "HMMMM":
                       audioPreAnalysis.foundHmmm = true;
                        break;
                }
            }
            else
            {
                audioPreAnalysis.foundHmmm = false;
            }
        }

        /// <summary>
        /// Handler for rejected speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

           
        }



        public void UpdateEnergy(object sender, EventArgs e)
        {
            if (MainWindow.myState == MainWindow.States.volumeCalibration)
            {
                lock (this.energyLock)
                {
                    // Calculate how many energy samples we need to advance since the last update in order to
                    // have a smooth animation effect
                    DateTime now = DateTime.UtcNow;
                    DateTime? previousRefreshTime = this.lastEnergyRefreshTime;
                    this.lastEnergyRefreshTime = now;

                    // No need to refresh if there is no new energy available to render
                    if (this.newEnergyAvailable <= 0)
                    {
                        return;
                    }

                    if (previousRefreshTime != null)
                    {
                        float energyToAdvance = this.energyError + (((float)(now - previousRefreshTime.Value).TotalMilliseconds * SamplesPerMillisecond) / SamplesPerColumn);
                        int energySamplesToAdvance = Math.Min(this.newEnergyAvailable, (int)Math.Round(energyToAdvance));
                        this.energyError = energyToAdvance - energySamplesToAdvance;
                        this.energyRefreshIndex = (this.energyRefreshIndex + energySamplesToAdvance) % this.energy.Length;
                        this.newEnergyAvailable -= energySamplesToAdvance;
                    }

                    // clear background of energy visualization area
                    this.energyBitmap.WritePixels(this.fullEnergyRect, this.backgroundPixels, EnergyBitmapWidth, 0);

                    // Draw each energy sample as a centered vertical bar, where the length of each bar is
                    // proportional to the amount of energy it represents.
                    // Time advances from left to right, with current time represented by the rightmost bar.
                    int baseIndex = (this.energyRefreshIndex + this.energy.Length - EnergyBitmapWidth) % this.energy.Length;

                    for (int i = 0; i < EnergyBitmapWidth; ++i)
                    {
                        const int HalfImageHeight = EnergyBitmapHeight / 2;

                        // Each bar has a minimum height of 1 (to get a steady signal down the middle) and a maximum height
                        // equal to the bitmap height.
                        int barHeight = (int)Math.Max(1.0, this.energy[(baseIndex + i) % this.energy.Length] * EnergyBitmapHeight);

                        // Center bar vertically on image
                        // var barRect = new Int32Rect(i, HalfImageHeight - (barHeight / 2), 1, barHeight);
                        var barRect = new Int32Rect(i, EnergyBitmapHeight - barHeight, 1, barHeight);

                        // Draw bar in foreground color
                        this.energyBitmap.WritePixels(barRect, this.foregroundPixels, 1, 0);
                    }
                    var loudRect = new Int32Rect(0, (int)(EnergyBitmapHeight * isSpeakingValue), EnergyBitmapWidth, 3);
                    var loudRect2 = new Int32Rect(0, (int)(EnergyBitmapHeight * speakingSoftValue), EnergyBitmapWidth, 3);
                    var loudRect3 = new Int32Rect(0, (int)(EnergyBitmapHeight * speakingLoudValue), EnergyBitmapWidth, 3);
                    this.energyBitmap.WritePixels(loudRect, this.lineVolumePixelsIsSpeaking, EnergyBitmapWidth, 0);
                    this.energyBitmap.WritePixels(loudRect2, this.lineVolumePixelsSoft, EnergyBitmapWidth, 0);
                    this.energyBitmap.WritePixels(loudRect3, this.lineVolumePixelsLoud, EnergyBitmapWidth, 0);
                    // this.energyBitmap.wr
                }
            }
            
            
        }
    }
}
