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

        public void initializeB(MainWindow parent)
        {
            this.parent = parent;
            parent.videoHandler.multiFrameSourceReader.MultiSourceFrameArrived += multiFrameSourceReader_MultiSourceFrameArrivedB;
        }

        void multiFrameSourceReader_MultiSourceFrameArrivedB(object sender, Microsoft.Kinect.MultiSourceFrameArrivedEventArgs e)
        {
            parent.videoHandler.Reader_MultiSourceFrameArrived(sender, e);
            myImage.Source = parent.videoHandler.kinectImage.Source;
          //  parent.heroMode.heroAvatar.Source = parent.videoHandler.kinectImage.Source;

            //     writer.WriteVideoFrame(myImage);

            //      writer.Close();

            if (parent.videoHandler.kinectImage.Source != null)
            {
                int x = 1;
                x++;
           //     parent.heroMode.heroAvatar.Source = parent.videoHandler.kinectImage.Source;
            }
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
