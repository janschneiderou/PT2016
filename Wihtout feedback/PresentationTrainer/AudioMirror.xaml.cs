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
    /// Interaction logic for AudioMirror.xaml
    /// </summary>
    public partial class AudioMirror : UserControl
    {
        MainWindow parent;

        public AudioMirror()
        {
            InitializeComponent();
        }
        public void initialize(MainWindow parent)
        {
            this.parent = parent;

            if (parent.audioHandler.reader != null)
            {
                // Subscribe to new audio frame arrived events
                parent.audioHandler.reader.FrameArrived += Reader_FrameArrived;
            }


                //    CompositionTarget.Rendering += this.UpdateEnergy;

            //if (this.reader != null)
            //{
            //    // Subscribe to new audio frame arrived events
            //    this.reader.FrameArrived += this.Reader_FrameArrived;
            //}

        }

        public void Reader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            parent.audioHandler.Reader_FrameArrived(sender, e);
            if(MainWindow.myState == MainWindow.States.freestyle)
            {
                var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_audio-feedback-1.png", UriKind.Relative);
                
                float currentVolume= parent.audioHandler.energy[parent.audioHandler.energyIndex];
                if( Math.Abs(currentVolume)<0.25)
                {
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_audio-feedback-1.png", UriKind.Relative);
                }
                else if (Math.Abs(currentVolume) < 0.5)
                {
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_audio-feedback-2.png", UriKind.Relative);
                }
                else if (Math.Abs(currentVolume) < 0.75)
                {
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_audio-feedback-3.png", UriKind.Relative);
                }
                else
                {
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_audio-feedback-4.png", UriKind.Relative);
                }
                
           
                MicroPhoneImage.Source = new BitmapImage(uriSource);
            }
            else
            {
                //  parent.audioHandler.UpdateEnergy(sender, null);
                kinectImage.Source = parent.audioHandler.energyBitmap;
            }
            

         
        }
    }
}
