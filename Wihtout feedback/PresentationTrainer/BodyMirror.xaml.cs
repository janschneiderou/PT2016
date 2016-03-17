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

//using AForge.Video;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for BodyMirror.xaml
    /// </summary>
    public partial class BodyMirror : UserControl
    {
        MainWindow parent;
       // FileSink filesink;
    //    VideoFileWriter writer;

        public BodyMirror()
        {
            InitializeComponent();
        }
        public void initialize(MainWindow parent)
        {
            this.parent = parent;

            // create instance of video writer
        //    writer = new VideoFileWriter();
            // create new video file
      //      writer.Open("test.avi", myImage.ActualWidth, myImage.ActualHeight, 25, VideoCodec.MPEG4);
            // create a bitmap to save into the video file

            parent.videoHandler.multiFrameSourceReader.MultiSourceFrameArrived += multiFrameSourceReader_MultiSourceFrameArrived;
            
        }

        void multiFrameSourceReader_MultiSourceFrameArrived(object sender, Microsoft.Kinect.MultiSourceFrameArrivedEventArgs e)
        {
            parent.videoHandler.Reader_MultiSourceFrameArrived( sender,  e);
            myImage.Source = parent.videoHandler.kinectImage.Source;


           
            
   

       //     writer.WriteVideoFrame(myImage);
         
      //      writer.Close();

        }
        public void close()
        {

        }
    }
}
