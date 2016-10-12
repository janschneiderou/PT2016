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

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for ReportControl.xaml
    /// </summary>
    public partial class ReportControl : UserControl
    {

        
        string myFileString;
        int totalTime;
        int startTime;
        int finishTime;

        int startTimeTemp;
        int finisTimeTemp;

        public ReportControl()
        {
            InitializeComponent();
        }

        public void doInit()
        {
            setLabels();
            doTimeline();
        }
        

        public void setLabels()
        {
            double ttime = (int)(MainWindow.presentationDuration / 1000);
            PTime.Content = PTime.Content + " " + ttime.ToString() + " Seconds";
            double pptm = (double)((int)(MainWindow.postureMistakeTime / MainWindow.presentationDuration*1000))/10;
            double crossptm =(double)((int)( MainWindow.armsCrossedMistakeTime / MainWindow.presentationDuration*1000))/10;
            double notVptm = (double)((int)(MainWindow.handsNotVisibleMistakeTime / MainWindow.presentationDuration*1000))/10;
            double hunchptm = (double)((int)(MainWindow.hunchMistakeTime / MainWindow.presentationDuration*1000))/10;
            double huderptm = (double)((int)(MainWindow.handsUnderhipsMistakeTime / MainWindow.presentationDuration*1000))/10;
            Posture.Content = Posture.Content +" " + MainWindow.totalPostureMistakes.ToString() + " % of time in Mistake " + pptm.ToString() +"%";
            Posture.Content = Posture.Content + " \n % of Arms crossed " + crossptm.ToString() + "%";
            Posture.Content = Posture.Content + "  % of Hunchback " + crossptm.ToString() + "%";
            Posture.Content = Posture.Content + " % of Hands not Visible " + crossptm.ToString() + "%";
            Posture.Content = Posture.Content + "  % of Hands under Hips " + crossptm.ToString() + "%";

            double volumeptm = (double)((int)(MainWindow.volumeMistakeTime / MainWindow.presentationDuration*1000))/10;
            double hiptm = (double)((int)(MainWindow.highVolumeMistakeTime / MainWindow.presentationDuration*1000))/10;
            double loptm = (double)((int)(MainWindow.lowVolumeMistakeTime / MainWindow.presentationDuration*1000))/10;
            Volume.Content = Volume.Content + MainWindow.totalVolumeMistakes.ToString() + " % of time in Mistake " + volumeptm.ToString() + "%";
            Volume.Content = Volume.Content + "\n  % of High Volume " + hiptm.ToString() + "%";
            Volume.Content = Volume.Content + "  % of Low Volume " + loptm.ToString() + "%";

            double gestureptm = (double)((int)(MainWindow.gestureMistakeTime / MainWindow.presentationDuration*1000))/10;
            Gesture.Content = Gesture.Content + MainWindow.totalGesturesMistakes.ToString() + " % of time in Mistake " + gestureptm.ToString() + "%";

            double cadenceptm = (double)((int)(MainWindow.cadenceMistakeTime / MainWindow.presentationDuration*1000))/10;
            Cadence.Content = Cadence.Content + MainWindow.totalCadenceMistakes.ToString() + " % of time in Mistake " + cadenceptm.ToString() + "%";

            double hmmptm = (double)((int)(MainWindow.hmmmMistakeTime / MainWindow.presentationDuration*1000))/10;
            PPauses.Content = PPauses.Content + MainWindow.totalHmmmMistakes.ToString() + " % of time in Mistake " + hmmptm.ToString() + "%";

            double dancingptm = (double)((int)(MainWindow.dancingMistakeTime / MainWindow.presentationDuration * 1000)) / 10;
            Dancing.Content = Dancing.Content + MainWindow.totalDancingMistakes.ToString() + " % of time in Mistake " + dancingptm.ToString() + "%";

            double seriousptm = (double)((int)(MainWindow.seriousMistakeTime / MainWindow.presentationDuration * 1000)) / 10;
            BlankFace.Content = BlankFace.Content + MainWindow.totalSeriousMistakes.ToString() + " % of time in Mistake " + seriousptm.ToString() + "%";

        }

        private void GoTimeLine_Click(object sender, RoutedEventArgs e)
        {
            if(scrollViewer.Visibility==Visibility.Visible)
            {
                scrollViewer.Visibility = Visibility.Collapsed;
                reportGrid.Visibility = Visibility.Visible;
            }
            else
            {
                scrollViewer.Visibility = Visibility.Visible;
                reportGrid.Visibility = Visibility.Collapsed;
            }
        }

        #region timeline

        public void doTimeline()
        {

            myFileString = MainWindow.logString;

            getTotalTime();
            getTimelineTimes();

            backCanvas.Width = totalTime * 20 + 200;
            ScrollCanvas.Width = totalTime * 20 + 200;
            PostureRect.Width = ScrollCanvas.Width;
            GestureRect.Width = ScrollCanvas.Width;
            VolumeRect.Width = ScrollCanvas.Width;
            CadenceRect.Width = ScrollCanvas.Width;
            PhoneticRect.Width = ScrollCanvas.Width;
            DancingRect.Width = ScrollCanvas.Width;

            try
            {
                getPostureMistakes();
            }
            catch
            {

            }


            try
            {
                getGestureMistakes();
            }
            catch
            {

            }
            try
            {
                getVolumeMistakes();
            }
            catch
            {

            }
            try
            {
                getCadenceMistakes();
            }
            catch
            {

            }
            try
            {
                getPhoneticMistakes();
            }
            catch
            {

            }
            try
            {
                getDancingMistakes();
            }
            catch
            {

            }

            try
            {
                getSeriousMistakes();
            }
            catch
            {

            }

            try
            {
                getSmiles();
            }
            catch
            {

            }

            try
            {
                getFeedbacks();
            }
            catch
            {

            }



        }

        public void getTotalTime()
        {
            int tempStart = myFileString.IndexOf("<Start Time>") + 12;
            int tempEnd = myFileString.IndexOf("</Start Time>");
            string temp = myFileString.Substring(tempStart, tempEnd - tempStart);
            startTime = (int)(Double.Parse(temp) / 1000);

            myFileString = myFileString.Substring(tempEnd + 13);

            tempStart = myFileString.IndexOf("<Finish Time>") + 13;
            tempEnd = myFileString.IndexOf("</FinishTime>");
            temp = myFileString.Substring(tempStart, tempEnd - tempStart);
            finishTime = (int)(Double.Parse(temp)) / 1000;

            totalTime = finishTime - startTime;


            myFileString = myFileString.Substring(tempEnd + 13);
        }


        public void getTimelineTimes()
        {
            int numbers = totalTime / 10;
            for (int i = 0; i <= numbers; i++)
            {
                Label top = new Label();
                Label bottom = new Label();
                top.FontSize = 20;
                bottom.FontSize = 20;
                Line l = new Line();

                string content = (i * 10) + "";
                top.Content = content;
                bottom.Content = content;
                backCanvas.Children.Add(top);
                Canvas.SetLeft(top, 200 + i * 200 - 15);
                Canvas.SetTop(top, -5);

                backCanvas.Children.Add(bottom);
                Canvas.SetLeft(bottom, 200 + i * 200 - 15);
                Canvas.SetTop(bottom, 710);

                l.X1 = 0;
                l.X2 = 0;
                l.Y1 = 0;
                l.Y2 = 700;

                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                dashes.Add(2);
                l.StrokeDashArray = dashes;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + i * 200);
                l.Stroke = Brushes.Black;

            }
        }

        #region mistakes


        public void getPostureMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Posture Mistakes>");
            int endPostureString = myFileString.IndexOf("</Posture Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 20);

                double top = 45 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;
            }
        }
        public void getGestureMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Gesture Mistakes>");
            int endPostureString = myFileString.IndexOf("</Gesture Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 120);

                double top = 145 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;

            }
        }
        public void getVolumeMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Volume Mistakes>");
            int endPostureString = myFileString.IndexOf("</Volume Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 220);

                double top = 245 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;

            }
        }


        private void getCadenceMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Cadence Mistakes>");
            int endPostureString = myFileString.IndexOf("</Cadence Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 320);

                double top = 345 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;

            }
        }

        private void getDancingMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Dancing Mistakes>");
            int endPostureString = myFileString.IndexOf("</Dancing Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 520);

                double top = 545 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;

            }
        }

        private void getPhoneticMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Phonetic pauses Mistakes>");
            int endPostureString = myFileString.IndexOf("</Phonetic pauses Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 420);

                double top = 445 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;

            }
        }

        private void getSeriousMistakes()
        {
            int startPostureString = myFileString.IndexOf("<Blank Face Mistakes>");
            int endPostureString = myFileString.IndexOf("</Blank Face Mistakes>");
            string postureMistakes = myFileString.Substring(startPostureString, endPostureString - startPostureString);
            myFileString = myFileString.Substring(endPostureString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (postureMistakes.IndexOf("<mistake>") != -1)
            {
                startMistakeType = postureMistakes.IndexOf("<mistake>") + 9;
                endMistakeType = postureMistakes.IndexOf(",") + 1;
                mistakeString = postureMistakes.Substring(startMistakeType, endMistakeType - startMistakeType);
                postureMistakes = postureMistakes.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf(","))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(postureMistakes.Substring(0, postureMistakes.IndexOf("<"))) / 1000 - startTime;
                postureMistakes = postureMistakes.Substring(postureMistakes.IndexOf("</mistake>") + 10);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Red;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 620);




                double top = 645 + (i % 3) * 10;

                Label l = new Label();
                l.FontSize = 12;
                l.Content = mistakeString;
                ScrollCanvas.Children.Add(l);
                Canvas.SetLeft(l, 200 + (startMistakeTime * 20));
                Canvas.SetTop(l, top);
                i++;

            }
        }

        #endregion

        #region Smiles

        private void getSmiles()
        {
            int startGoodiesString = myFileString.IndexOf("<Goodies>");
            int endGoodiesString = myFileString.IndexOf("</Goodies>");
            string Goodies = myFileString.Substring(startGoodiesString, endGoodiesString - startGoodiesString);
            myFileString = myFileString.Substring(endGoodiesString + 20);

            int startMistakeType;
            int endMistakeType;
            int startMistakeTime;
            int endMistakeTime;

            string mistakeString;

            int i = 0;
            while (Goodies.IndexOf("<SMILE") != -1)
            {
                startMistakeType = Goodies.IndexOf("<SMILE,") + 7;
                endMistakeType = Goodies.IndexOf(",") + 1;
                mistakeString = Goodies.Substring(startMistakeType, endMistakeType - startMistakeType);
                Goodies = Goodies.Substring(endMistakeType);

                startMistakeTime = (int)Double.Parse(Goodies.Substring(0, Goodies.IndexOf(","))) / 1000 - startTime;
                Goodies = Goodies.Substring(Goodies.IndexOf(",") + 1);

                endMistakeTime = (int)Double.Parse(Goodies.Substring(0, Goodies.IndexOf("/>"))) / 1000 - startTime;
                Goodies = Goodies.Substring(Goodies.IndexOf("/>") + 2);

                Rectangle gr = new Rectangle();
                gr.Height = 20;
                gr.Width = (endMistakeTime - startMistakeTime) * 20;
                gr.Fill = Brushes.Green;
                gr.Stroke = Brushes.Black;
                ScrollCanvas.Children.Add(gr);
                Canvas.SetLeft(gr, 200 + (startMistakeTime * 20));
                Canvas.SetTop(gr, 620);




                i++;

            }
        }

        #endregion


        #region Feedbacks

        public void getFeedbacks()
        {
            int startString = myFileString.IndexOf("<Freestyle Feedbacks>");
            int endString = myFileString.IndexOf("</Freestyle Feedbacks>");

            string feedbackStrings = myFileString.Substring(startString, endString - startString);


            int endFeedbackType;
            int startFeedback;
            try
            {
                while (feedbackStrings.IndexOf("<feedback>") != -1)
                {
                    startFeedback = feedbackStrings.IndexOf("<feedback>");

                    endFeedbackType = feedbackStrings.IndexOf("\r\n<time started>");
                    string feedbackType = feedbackStrings.Substring(startFeedback + 11, endFeedbackType - startFeedback - 11);

                    Uri uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_hands.png", UriKind.Relative);
                    double topImg = 0;


                    switch (feedbackType)
                    {
                        case "ARMSCROSSED":
                        case "LEFTHANDBEHINDBACK":
                        case "RIGHTHANDBEHINDBACK":
                        case "LEGSCROSSED":
                        case "RIGHTHANDUNDERHIP":
                        case "LEFTHANDUNDERHIP":
                        case "HUNCHBACK":
                        case "RIGHTLEAN":
                        case "LEFTLEAN":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_outline.png", UriKind.Relative);
                            topImg = 10;
                            break;
                        case "LOW_VOLUME":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_raiseVolume.png", UriKind.Relative);
                            topImg = 210;
                            break;
                        case "HIGH_VOLUME":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_raiseVolume.png", UriKind.Relative);
                            topImg = 210;
                            break;
                        case "LONG_PAUSE":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);
                            topImg = 310;
                            break;
                        case "LONG_TALK":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_pause.png", UriKind.Relative);
                            topImg = 310;
                            break;
                        case "HANDS_NOT_MOVING":
                        case "HANDS_MOVING_MUCH":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_hands.png", UriKind.Relative);
                            topImg = 110;
                            break;
                        case "HMMMM":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_outline.png", UriKind.Relative);
                            topImg = 410;
                            break;
                        case "DANCING":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_hands.png", UriKind.Relative);
                            topImg = 510;
                            break;
                        case "SERIOUS":
                            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_smile.png", UriKind.Relative);
                            topImg = 610;
                            break;

                    }

                    int startStartTime = feedbackStrings.IndexOf("<time started>") + 14;
                    int endStartTime = feedbackStrings.IndexOf("</time started>");
                    string startFeedbackTimeString = feedbackStrings.Substring(startStartTime
                        , endStartTime - startStartTime);

                    double startFeedbackTime = (int)Double.Parse(startFeedbackTimeString) / 1000 - startTime;


                    int startEndTime = feedbackStrings.IndexOf("<time finished>") + 15;
                    int endEndTime = feedbackStrings.IndexOf("</time finished>");
                    string endFeedbackTimeString = feedbackStrings.Substring(startEndTime
                        , endEndTime - startEndTime);

                    double endFeedbackTime = (int)Double.Parse(endFeedbackTimeString) / 1000 - startTime;

                    Image FeedbackIMG = new Image();
                    FeedbackIMG.Source = new BitmapImage(uriSource);
                    FeedbackIMG.Height = 80;
                    ScrollCanvas.Children.Add(FeedbackIMG);
                    Canvas.SetTop(FeedbackIMG, topImg);

                    Canvas.SetLeft(FeedbackIMG, 200 + (startFeedbackTime * 20));


                    feedbackStrings = feedbackStrings.Substring(feedbackStrings.IndexOf("</feedback>") + 11);
                }
            }
            catch
            {

            }
            
        }



        #endregion

        #endregion

    }
}
