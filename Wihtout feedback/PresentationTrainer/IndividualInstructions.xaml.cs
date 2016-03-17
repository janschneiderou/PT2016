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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for IndividualInstructions.xaml
    /// </summary>
    public partial class IndividualInstructions : UserControl
    {

        public bool animationFinished = false;
        public bool stop = false;

        public IndividualInstructions()
        {
            InitializeComponent();
        }


        public void showStop()
        {
            stop = true;
            stopLabel.Visibility = Visibility.Visible;
        }

        public void startAnimation()
        {
            animationFinished = false;
            stop = false;

            instructionLabel4.Visibility = Visibility.Collapsed;
            instructionLabel2.Visibility = Visibility.Collapsed;
            instructionLabel3.Visibility = Visibility.Collapsed;


            instruction1Label.Visibility = Visibility.Visible;
            var animationTakeBreathOpacity = new DoubleAnimation();
            animationTakeBreathOpacity.From = 1.0;
            animationTakeBreathOpacity.To = 0.8;
            animationTakeBreathOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(900));

            Storyboard.SetTarget(animationTakeBreathOpacity, instruction1Label);
            Storyboard.SetTargetProperty(animationTakeBreathOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingBreath = new Storyboard();
            animatingBreath.Children.Add(animationTakeBreathOpacity);
            animatingBreath.Completed += animatingBreath_Completed;
            if(stop==false)
            {
                animatingBreath.Begin();
            }
            


        }

        void animatingBreath_Completed(object sender, EventArgs e)
        {
            instructionLabel2.Visibility = Visibility.Visible;
            var animationThinkContentOpacity = new DoubleAnimation();
            animationThinkContentOpacity.From = 1.0;
            animationThinkContentOpacity.To = 0.8;
            animationThinkContentOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(900));

            Storyboard.SetTarget(animationThinkContentOpacity, instructionLabel2);
            Storyboard.SetTargetProperty(animationThinkContentOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingThinkContent = new Storyboard();
            animatingThinkContent.Children.Add(animationThinkContentOpacity);
            animatingThinkContent.Completed += animatingThinkContent_Completed;
            

            if (stop == false)
            {
                animatingThinkContent.Begin();
            }
        }

        void animatingThinkContent_Completed(object sender, EventArgs e)
        {
            instructionLabel3.Visibility = Visibility.Visible;
            var animationThinkSkillOpacity = new DoubleAnimation();
            animationThinkSkillOpacity.From = 1.0;
            animationThinkSkillOpacity.To = 0.8;
            animationThinkSkillOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(900));

            Storyboard.SetTarget(animationThinkSkillOpacity, instructionLabel3);
            Storyboard.SetTargetProperty(animationThinkSkillOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingThinkSkill = new Storyboard();
            animatingThinkSkill.Children.Add(animationThinkSkillOpacity);
            animatingThinkSkill.Completed += animatingThinkSkill_Completed;
            
            if (stop == false)
            {
                animatingThinkSkill.Begin();
            }

        }

        void animatingThinkSkill_Completed(object sender, EventArgs e)
        {
            animationFinished = true;
            instructionLabel4.Visibility = Visibility.Visible;
        }
        public void setInvisible()
        {
            instruction1Label.Visibility = Visibility.Collapsed;
            instructionLabel2.Visibility = Visibility.Collapsed;
            instructionLabel3.Visibility = Visibility.Collapsed;
            instructionLabel4.Visibility = Visibility.Collapsed;
            stopLabel.Visibility = Visibility.Collapsed;
        }

    }
}
