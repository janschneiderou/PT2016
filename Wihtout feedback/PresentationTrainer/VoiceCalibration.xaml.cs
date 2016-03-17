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

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for VolumeCalibration.xaml
    /// </summary>
    public partial class VolumeCalibration : UserControl
    {
        public MainWindow parent;

        private float ThresholdIsSpeaking = 0.2f;
        private float ThresholdIsSpeakingLoud = 0.5f;
        private float ThresholdIsSpeakingVeryLoud = 0.6f;
        private float ThresholdIsSpeakingSoft = 0.4f;
        private float ThresholdIsSpeakingVerySoft = 0.3f;
        

        public VolumeCalibration()
        {
            InitializeComponent();
        }

        public void loaded()
        {
            myAudio.initialize(parent);
            ThresholdIsSpeaking  = this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeaking;
            ThresholdIsSpeakingSoft = this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingSoft;
            ThresholdIsSpeakingVerySoft = this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingVerySoft;
            ThresholdIsSpeakingLoud = this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingLoud;
            ThresholdIsSpeakingVeryLoud = this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingVeryLoud;

            ShortPauseSlider.Value = this.parent.audioHandler.audioPreAnalysis.ThresholdShortPause;
            LongPauseSlider.Value = this.parent.audioHandler.audioPreAnalysis.ThresholdIsLongPauseTime;
            ShortSpeakSlider.Value = this.parent.audioHandler.audioPreAnalysis.ThresholdShortSpeakingTime;
            LongSpeakSlider.Value = this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingLongTime;

            ShortPauseLabel.Text = ShortPauseSlider.Value + "";
            LongPauseLabel.Text = LongPauseSlider.Value + "";
            ShortSpeakingLabel.Text = ShortSpeakSlider.Value + "";
            LongSpeakingLabel.Text = LongSpeakSlider.Value + "";

            isSpeakingSlider.Value = 1 - ThresholdIsSpeaking;
            speakingSoftSlider.Value = 1 - ThresholdIsSpeakingSoft;
            speakingLoudSlider.Value = 1 - ThresholdIsSpeakingLoud;

            isSpeakingLabel.Content = ThresholdIsSpeaking;
            speakingSoftLabel.Content = ThresholdIsSpeakingSoft;
            speakingLoudLabel.Content = ThresholdIsSpeakingLoud;

            this.parent.audioHandler.isSpeakingValue = (float)isSpeakingSlider.Value;
            this.parent.audioHandler.speakingSoftValue =  (float)speakingSoftSlider.Value;
            this.parent.audioHandler.speakingLoudValue =  (float)speakingLoudSlider.Value;
        }

        public void closing()
        {

        }

        private void isSpeakingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {               
            isSpeakingLabel.Content = ((int) ((1 - isSpeakingSlider.Value)*1000))/1000.0;
            this.parent.audioHandler.isSpeakingValue = (float)isSpeakingSlider.Value;
            
        }

        private void speakingSoftSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speakingSoftLabel.Content = ((int)((1 - speakingSoftSlider.Value) * 1000)) / 1000.0;
            this.parent.audioHandler.speakingSoftValue = (float)speakingSoftSlider.Value;
        }

        private void speakingLoudSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speakingLoudLabel.Content = ((int)((1 - speakingLoudSlider.Value) * 1000)) / 1000.0;
            this.parent.audioHandler.speakingLoudValue = (float)speakingLoudSlider.Value;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            ThresholdIsSpeaking = 1 - (float)isSpeakingSlider.Value;
            ThresholdIsSpeakingSoft = 1 - (float)speakingSoftSlider.Value;
            ThresholdIsSpeakingLoud = 1 - (float)speakingLoudSlider.Value;

            ThresholdIsSpeakingVerySoft = ThresholdIsSpeakingSoft - 0.1f;
            ThresholdIsSpeakingVeryLoud = ThresholdIsSpeakingLoud + 0.1f;

            this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeaking= ThresholdIsSpeaking;
            this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingSoft = ThresholdIsSpeakingSoft;
            this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingVerySoft = ThresholdIsSpeakingVerySoft;
            this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingLoud = ThresholdIsSpeakingLoud;
            this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingVeryLoud = ThresholdIsSpeakingVeryLoud;


            this.parent.audioHandler.audioPreAnalysis.ThresholdShortPause = float.Parse(ShortPauseLabel.Text);
            this.parent.audioHandler.audioPreAnalysis.ThresholdIsLongPauseTime = float.Parse(LongPauseLabel.Text);
            this.parent.audioHandler.audioPreAnalysis.ThresholdShortSpeakingTime = float.Parse(ShortSpeakingLabel.Text);
            this.parent.audioHandler.audioPreAnalysis.ThresholdIsSpeakingLongTime = float.Parse(LongSpeakingLabel.Text);

            
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShortPauseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShortPauseLabel.Text = ShortPauseSlider.Value +"";
          
        }

        private void LongPauseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            LongPauseLabel.Text = LongPauseSlider.Value + "";
            
        }

        private void ShortSpeakSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            ShortSpeakingLabel.Text = ShortSpeakSlider.Value + "";
            
        }

        private void LongSpeakSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            LongSpeakingLabel.Text = LongSpeakSlider.Value + "";
        }

    }
}
