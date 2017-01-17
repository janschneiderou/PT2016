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
using Microsoft.Kinect;
using System.IO.Ports;
using System.Collections;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
   

    public partial class MainWindow : Window
    {
        public FreestyleMode freestyleMode;
        public VolumeCalibration volumeCalibrationMode;
        public MainMenu mainMenu;
        public enum States { menu, freestyle, volumeCalibration, individual, hero};
        public static States myState;
        public Pausereport pauseReport;
        public GestureReport gestureReport;
        public PostureReport postureReport;
        public FutureImprovement futureImprovement;

        public static SerialPort hapticPort;
        public static bool stopGesture = false;
        public static string FileName = "";

        private KinectSensor kinectSensor;
        public InfraredFrameReader frameReader = null;
        public RulesAnalyzer rulesAnalyzer;
        public RulesAnalyzerFIFO rulesAnalyzerFIFO;


        public VideoHandler videoHandler;
        public AudioHandler audioHandler;
        public FaceFrameHandler faceFrameHandler;
        public BodyFrameHandler bodyFrameHandler;
        public IndividualSkills individualSkills;

        public ReportControl reportControl;

        public static string focusedString = "";
        public static string focusedPauses = "";
        public static string focusedGestures = "";
        public static string focusedPosture = "";

        public static string stringStartFinish = "";
        public static string stringGestures = "";
        public static string stringInterruptions = "";
        public static string stringFreestyleFeedbacks = "";
        public static string stringSpeakingTime = "";
        public static string stringPausingTime = "";
        public static string stringIndividualSkills = "";
        public static string stringIndividualFeedbacks = "";
        public static string stringMistakes = "";
        public static string stringReport="";
        public static string stringGoodies = "";

        public static ArrayList speakTimes;
        public static ArrayList gestureTimes;
        public static ImageSource[] gestureImages;
        public static ImageSource[] postureImages;

        public static int totalPostureMistakes = 0;
        public static int totalGesturesMistakes = 0;
        public static int totalVolumeMistakes = 0;
        public static int totalCadenceMistakes = 0;
        public static int totalHmmmMistakes = 0;
        public static int totalDancingMistakes = 0;
        public static int totalSeriousMistakes = 0;
        public static int totalMistakes = 0;

        public static int totalSmiles = 0;
        

        public static string timePostureMistakes = "";
        public static string timeGestureMistakes = "";
        public static string timeVolumeMistakes = "";
        public static string timeCadenceMistakes = "";
        public static string timeHmmmMistakes = "";
        public static string timeDancingMistakes = "";
        public static string timeSeriousMistakes = "";

        public static string timeSmiles = "";

        public static double presentationDuration;
        public static double postureMistakeTime;
        public static double armsCrossedMistakeTime;
        public static double hunchMistakeTime;
        public static double handsUnderhipsMistakeTime;
        public static double handsNotVisibleMistakeTime;
        public static double volumeMistakeTime;
        public static double highVolumeMistakeTime;
        public static double lowVolumeMistakeTime;
        public static double gestureMistakeTime;
        public static double cadenceMistakeTime;
        public static double dancingMistakeTime;
        public static double hmmmMistakeTime;
        public static double seriousMistakeTime;

        public static double smileTime;
        public static double totalgoodiesTime;

        public static double presentationStarted=0;
        public static double presentationFinished;

        public static string logString;

        public MainWindow()
        {
            InitializeComponent();
            myState = new States();
            myState = States.menu;
            loadMode();

            this.kinectSensor = KinectSensor.GetDefault();
            this.kinectSensor.Open();
            this.frameReader = this.kinectSensor.InfraredFrameSource.OpenReader();

           focusedString= System.IO.File.ReadAllText("focused.txt");
            rulesAnalyzer = new RulesAnalyzer(this);
            rulesAnalyzerFIFO = new RulesAnalyzerFIFO(this);
            videoHandler = new VideoHandler(this.kinectSensor);
            audioHandler = new AudioHandler(this.kinectSensor);
            bodyFrameHandler = new BodyFrameHandler(this.kinectSensor);
            faceFrameHandler = new FaceFrameHandler(this.kinectSensor);

            initializeHaptic();
        }

        private void initializeHaptic()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    hapticPort = new SerialPort(port, 9600);
                }
            }
            catch (Exception e)
            {
            }
        }

        #region controlStuff
        public void loadMode()
        {
            switch (myState)
            {
                case States.menu:
                    loadMainMenu();
                    break;
                case States.freestyle:
                    loadFreestyle();
                    break;
                case States.volumeCalibration:
                    loadVolumeCalibration();
                    break;
                case States.hero:
                  //  loadHeroMode();
                    break;

               
            }
        }

        private void loadHeroMode()
        {
            
        }

        void heroMode_exitEvent(object sender)
        {
            closeHeroMode();
            myState = States.menu;
            loadMode();
        }

        private void closeHeroMode()
        {
           
        }

        void heroMode_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        public void loadVolumeCalibration()
        {
            volumeCalibrationMode = new VolumeCalibration();
            volumeCalibrationMode.Height = this.ActualHeight;
            volumeCalibrationMode.Width = this.ActualWidth;
            MainCanvas.Children.Add(volumeCalibrationMode);
            Canvas.SetTop(volumeCalibrationMode, 0);
            Canvas.SetLeft(volumeCalibrationMode, 0);
            volumeCalibrationMode.Loaded += volumeCalibrationMode_Loaded;
        }

        void volumeCalibrationMode_Loaded(object sender, RoutedEventArgs e)
        {
            volumeCalibrationMode.parent = this;
            volumeCalibrationMode.loaded();
        }    

        public void loadMainMenu()
        {
            mainMenu = new MainMenu();
            MainCanvas.Children.Add(mainMenu);
            Canvas.SetTop(mainMenu, 0);
            Canvas.SetLeft(mainMenu, 0);
     
            mainMenu.Loaded += mainMenu_Loaded;
        }

        void mainMenu_Loaded(object sender, RoutedEventArgs e)
        {
            mainMenu.focusedLabel.Content = focusedString;
            mainMenu.FreestyleButton.Click += mainMenu_FreestyleClicked;
            mainMenu.volumeCalibrationButton.Click += volumeCalibrationButton_Click;
            mainMenu.Hero.Click += Hero_Click;
        }

        void Hero_Click(object sender, RoutedEventArgs e)
        {
            closeMainMenu();
            myState = States.hero;
            loadMode();
            //TODO GO BACK
           // volumeCalibrationMode.backButton.Click += volumeCalibrationMode_backButton_Click;
        }

        public void loadFreestyle()
        {

            if(freestyleMode==null)
            {
                freestyleMode = new FreestyleMode();
            }
            speakTimes = new ArrayList();
            gestureTimes = new ArrayList();
            gestureImages = new ImageSource[3];
            postureImages = new ImageSource[3];
            for (int i = 0; i < 3;i++ )
            {
                gestureImages[i] = null;
                postureImages[i] = null;
            }
            freestyleMode.Height = this.ActualHeight;
            freestyleMode.Width = this.ActualWidth;
            MainCanvas.Children.Add(freestyleMode);
            Canvas.SetTop(freestyleMode, 0);
            Canvas.SetLeft(freestyleMode, 0);

            freestyleMode.Loaded += freeStyle_Loaded;
        }

        public void loadIndividualSkills( PresentationAction pa)
        {
            
        
            individualSkills = new IndividualSkills(pa);
            individualSkills.parent = this;
            MainCanvas.Children.Add(individualSkills);
            individualSkills.Height = ActualHeight;
            individualSkills.Width = ActualWidth;
            Canvas.SetTop(individualSkills, 0);
            Canvas.SetLeft(individualSkills, 0);
            myState = States.individual;
            individualSkills.Loaded += individualSkills_Loaded;
            
        }

        void individualSkills_Loaded(object sender, RoutedEventArgs e)
        {
            individualSkills.finish.GoBackButton.Click += GoBackButton_Click;
        }

        void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            myState = States.freestyle;
            individualSkills.finish.GoBackButton.Click -= GoBackButton_Click;
            individualSkills.Visibility = Visibility.Collapsed;
            MainCanvas.Children.Remove(individualSkills);

            if(freestyleMode!=null)
            {
                freestyleMode.OrdinaryReturn_Click(null, null);
            }
            else
            {
                loadMode();
            }
            
            
        }

        void freeStyle_Loaded(object sender, RoutedEventArgs e)
        {
            freestyleMode.parent = this;
            freestyleMode.loaded();
        }

        void volumeCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            closeMainMenu();
            myState = States.volumeCalibration;
            loadMode();
            volumeCalibrationMode.backButton.Click += volumeCalibrationMode_backButton_Click;
        }

        private void volumeCalibrationMode_backButton_Click(object sender, RoutedEventArgs e)
        {
            closeVolumeCalibration();
            myState = States.menu;
            loadMode();
            // we might have to unsubscribe (-=) to the click event of the pressed button
        }

        private void closeVolumeCalibration()
        {
            MainCanvas.Children.Remove(volumeCalibrationMode);
        }

        private void mainMenu_FreestyleClicked(object sender, RoutedEventArgs e)
        {
            closeMainMenu();
            myState = States.freestyle;
          //  setInitialStrings();
            if (freestyleMode != null)
            {
                rulesAnalyzerFIFO.resetAll();
                freestyleMode.OrdinaryReturn_Click(null, null);
                freestyleMode.Visibility = Visibility.Visible;
            }
            else
            {
                loadMode();
            }
            
            freestyleMode.stopButton.Click += freeStyleMode_stopButton_Click;
            //freestyleMode.reportStopButton.Click += freeStyleMode_reportStopButton_Click;
            //freestyleMode.logStopButton.Click += freeStyleMode_logStopButton_Click;
        }

       
        
        public void freeStyleMode_reportStopButton_Click(object sender, RoutedEventArgs e)
        {
            //Stop everything, then report.
            //after these two things, close freeStyleMode
            closeFreeStyleMode();
            myState = States.menu;
            loadMode();
            // we might have to unsubscribe (-=) to the click event of the pressed button
        }

        public void freeStyleMode_logStopButton_Click(object sender, RoutedEventArgs e)
        {
            //Stop everything, then log it, then report it.
            //after these three things, close freeStyleMode
            closeFreeStyleMode();
            myState = States.menu;
            loadMode();
            // we might have to unsubscribe (-=) to the click event of the pressed button
        }
        
        public void freeStyleMode_stopButton_Click(object sender, RoutedEventArgs e)
        {
            closeFreeStyleMode();
            myState = States.menu;
            loadMode();
            // we might have to unsubscribe (-=) to the click event of the pressed button
        }

        #region selfreflection

        public void closeFreeStyleMode()
        {
           freestyleMode.setGhostMovingInvisible();
           freestyleMode.setOldTextInvisible();
           freestyleMode.setGhostInvisible();
           freestyleMode.setFeedbackTextInvisible();
           
          // MainCanvas.Children.Remove(freestyleMode);
           freestyleMode.Visibility = Visibility.Collapsed;

           doPausesesReflection();



           myState = States.menu;
            
          //  loadMode();
        }
        void doPausesesReflection()
        {
            pauseReport = new Pausereport();
            MainCanvas.Children.Add(pauseReport);


            Canvas.SetLeft(pauseReport, 20);
            Canvas.SetTop(pauseReport, 20);
            pauseReport.GoMainMenu.Click += GoMainMenu_Click_Pause;
        }

        void doGestureReflection()
        {
            
            gestureReport = new GestureReport();
            MainCanvas.Children.Add(gestureReport);


            Canvas.SetLeft(gestureReport, 20);
            Canvas.SetTop(gestureReport, 20);
            gestureReport.GoMainMenu.Click += GoMainMenu_Click_Gesture;
        }

        void doPostureReflection()
        {
            postureReport = new PostureReport();
            MainCanvas.Children.Add(postureReport);


            Canvas.SetLeft(postureReport, 20);
            Canvas.SetTop(postureReport, 20);
            postureReport.GoMainMenu.Click += GoMainMenu_Click_Posture;
        }
        void doTimelineReflection()
        {
            reportControl = new ReportControl();
            MainCanvas.Children.Add(reportControl);
            reportControl.doInit();
            Canvas.SetLeft(reportControl, 20);
            Canvas.SetTop(reportControl, 20);

            reportControl.GoMainMenu.Click += GoMainMenu_Click;
        }

        void doSelectionFutureReflection()
        {
            futureImprovement = new FutureImprovement();
            MainCanvas.Children.Add(futureImprovement);

            Canvas.SetLeft(futureImprovement, 20);
            Canvas.SetTop(futureImprovement, 20);

            futureImprovement.selectionEvent += futureImprovement_selectionEvent;
        }

        void futureImprovement_selectionEvent()
        {
            MainCanvas.Children.Remove(futureImprovement);

            try
            {
                freestyleMode.unload();
                MainCanvas.Children.Remove(freestyleMode);
                freestyleMode = null;
            }
           catch
            {

            }

            System.IO.File.WriteAllText(FileName, logString);
            System.IO.File.WriteAllText("focused.txt", focusedString);
            speakTimes = new ArrayList();
            gestureTimes = new ArrayList();
            loadMode();
        }

        void GoMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Remove(reportControl);
            reportControl = null;

            doSelectionFutureReflection();
            
            
            
        }

        private void GoMainMenu_Click_Pause(object sender, RoutedEventArgs e)
        {
            focusedPauses = pauseReport.answer1.Text;
            MainCanvas.Children.Remove(pauseReport);
            pauseReport = null;
            
            doGestureReflection();
        }

        private void GoMainMenu_Click_Gesture(object sender, RoutedEventArgs e)
        {
            focusedGestures = gestureReport.answerQuestion.Text;
            MainCanvas.Children.Remove(gestureReport);
            gestureReport = null;

            doPostureReflection();
        }

        private void GoMainMenu_Click_Posture(object sender, RoutedEventArgs e)
        {
            focusedPosture = postureReport.answerQuestion.Text;
            MainCanvas.Children.Remove(postureReport);
            postureReport = null;
            doTimelineReflection();
           
        }

        #endregion

        public void setInitialStrings()
        {
            stringStartFinish = "<Start Time>" + DateTime.Now.TimeOfDay.TotalMilliseconds + "</Start Time>" + System.Environment.NewLine;
            presentationStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
            stringGestures = "<Gestures>" + System.Environment.NewLine;
            stringInterruptions = "<Interruptions>" + System.Environment.NewLine;
            stringFreestyleFeedbacks = "<Freestyle Feedbacks>" + System.Environment.NewLine;
            stringSpeakingTime = "<Speaking time> " + System.Environment.NewLine;
            stringPausingTime = "<Pausing time>" + System.Environment.NewLine;
            stringIndividualSkills = "<Individual Skills>" + System.Environment.NewLine;
            stringIndividualFeedbacks = "<Individual Skills Feedback>" + System.Environment.NewLine;
            stringMistakes = "<Mistakes>" + System.Environment.NewLine;
        }
        public void setFinalStrings()
        {
            presentationFinished = DateTime.Now.TimeOfDay.TotalMilliseconds;
            presentationDuration = DateTime.Now.TimeOfDay.TotalMilliseconds - presentationStarted;
            stringStartFinish = stringStartFinish + System.Environment.NewLine + "<Finish Time>" + DateTime.Now.TimeOfDay.TotalMilliseconds + "</FinishTime>" + System.Environment.NewLine;
            stringGestures = stringGestures + System.Environment.NewLine + "</Gestures>" + System.Environment.NewLine;
            stringInterruptions = stringInterruptions + System.Environment.NewLine + "</Interruptions>" + System.Environment.NewLine;
            stringFreestyleFeedbacks = stringFreestyleFeedbacks + System.Environment.NewLine + "</Freestyle Feedbacks>" + System.Environment.NewLine;
            stringSpeakingTime = stringSpeakingTime + System.Environment.NewLine + "</Speaking time>" + System.Environment.NewLine;
            stringPausingTime = stringPausingTime + System.Environment.NewLine + "</Pausing time>" + System.Environment.NewLine;
            stringIndividualSkills = stringIndividualSkills + System.Environment.NewLine + "</Individual Skills>" + System.Environment.NewLine;
            stringIndividualFeedbacks = stringIndividualFeedbacks + System.Environment.NewLine + "</Individual Skills Feedback>" + System.Environment.NewLine;
            stringMistakes = stringMistakes + System.Environment.NewLine + "</Mistakes>" + System.Environment.NewLine;
            stringGoodies = "<Goodies>"+ stringGoodies + System.Environment.NewLine + "</Goodies>" + System.Environment.NewLine;
        }
       

        public void closeMainMenu()
        {
            MainCanvas.Children.Remove(mainMenu);
        }
        
        

        #endregion


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.frameReader != null)
            {
                this.frameReader.FrameArrived += frameReader_FrameArrived;
            }
        }
     

        void frameReader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
          //  rulesAnalyzer.AnalyseRules();
            switch(myState)
            {
                case States.freestyle:
                    rulesAnalyzerFIFO.AnalyseRules();
                    freestyleMode.checkPause();
                    break;
                case States.individual:
                    if(individualSkills.ready)
                    {
                        individualSkills.analyze();
                    }
                    break;
                case States.hero:
                   
                        

                    break;
            }
            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
          //  audioHandlerControl.close();
           // bodyFrameHandlerControl.close();
            videoHandler.close();

        }
    }
}
