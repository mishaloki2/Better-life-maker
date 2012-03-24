using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace BetterLifeMaker
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
        }

        private enum Fatals
        {
            BlueScreen = 0,
            Rollback = 1
        }

        private enum Part
        {
            Info = 0,
            Health = 20,
            Money = 40,
            Private = 60,
            Common = 80
        }

        private Dictionary<Part,string []> Status = new Dictionary<Part, string[]>(){
        {Part.Info,new string[1]{"getting personal info"}},
        {Part.Health,new string[3]{"normalizing blood pressure","eliminating genetic diseases","strengthening immunity"}},
        {Part.Money,new string[3]{"increasing a salary","paying the debts","filling the purse with money"}},
        {Part.Private,new string[3]{"searching for a partner","increasing a sexual activity","uniting the family"}},
        {Part.Common,new string[3]{"killing the enemies","making the sun brighter","solving the problems"}},
        };

        private Fatals currentFatal;

        private bool isProgress = false;

        private Timer timer;

        private int finalValue;

        private int maxValue;

        private void makeBtn_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            ShowBgImage();
            makeBtn.IsEnabled = false;
        }

        private void ShowBgImage()
        {
            timer = new Timer(UpdateUI,null,0,200);
        }

        private void UpdateUI(object obj)
        {
            Deployment.Current.Dispatcher.BeginInvoke(()=>
                                                          {


                                                              if (lifeLine.Value >= finalValue)
                                                              {

                                                                  if (currentFatal==Fatals.BlueScreen)
                                                                  {
                                                                      timer.Dispose();                                                                     
                                                                      SetBlueScreen();
                                                                      return;
                                                                  }
                                                                  
                                                                  if (lifeLine.Value==finalValue)
                                                                  {
                                                                      currentImprovement.Text = string.Empty;
                                                                      dots.Text = string.Empty;
                                                                      maxValue = finalValue;
                                                                      finalValue = 0;
                                                                      MessageBox.Show("Your life will be made worse. Would you like to continue?",
                                                                                      "System error",
                                                                                      MessageBoxButton.OK);
                                                                  }                                                                 
                                                                  
                                                                  RollBack();

                                                              }
                                                              else
                                                              {
                                                                  bgImage.Opacity += 0.01;

                                                                  lifeLine.Value += 1;
                                                                  progressText.Text = "Progress: " + lifeLine.Value + "%";

                                                                  if (lifeLine.Value == 80)
                                                                  {
                                                                      Random random = new Random();
                                                                      currentImprovement.Text = Status[Part.Common][random.Next(Status[Part.Common].Length)];
                                                                  }
                                                                  else
                                                                  {
                                                                      if (lifeLine.Value == 60)
                                                                      {
                                                                          Random random = new Random();
                                                                          currentImprovement.Text = Status[Part.Private][random.Next(Status[Part.Private].Length)];
                                                                      }
                                                                      else
                                                                      {
                                                                          if (lifeLine.Value == 40)
                                                                          {
                                                                              Random random = new Random();
                                                                              currentImprovement.Text = Status[Part.Money][random.Next(Status[Part.Money].Length)];
                                                                          }
                                                                          else
                                                                          {
                                                                              if (lifeLine.Value == 20)
                                                                              {
                                                                                  Random random = new Random();
                                                                                  currentImprovement.Text = Status[Part.Health][random.Next(Status[Part.Health].Length)];
                                                                              }
                                                                              else
                                                                              {
                                                                                  if (lifeLine.Value < 20)
                                                                                  {
                                                                                      currentImprovement.Text = Status[Part.Info][0];
                                                                                  }
                                                                                  
                                                                              }
                                                                          }
                                                                      }
                                                                  }
                                                                 
                                                                  
                                                                  if (dots.Text.IndexOf("...") > -1)
                                                                  {
                                                                      dots.Text = dots.Text.Remove(
                                                                          dots.Text.Length - 3);
                                                                  }
                                                                  if (lifeLine.Value % 3 == 0)
                                                                  {
                                                                      dots.Text += ".";
                                                                  }

                                                                  if (bgImage.Opacity >= 1 || lifeLine.Value >= 100)
                                                                  {
                                                                      timer.Dispose();
                                                                      currentImprovement.Text = "Operations completed";
                                                                      MessageBox.Show("Your life is now better!",
                                                                                      "Congratulations!!!",
                                                                                      MessageBoxButton.OK);
                                                                      makeBtn.IsEnabled = true;

                                                                  }
                                                              }

                                                          });

        }

        private void GetFinalValue()
        {
            Random random = new Random();
            finalValue = random.Next(100);
            if (finalValue<30)
            {
                finalValue = 100 - finalValue;
            }
            if (finalValue%12==0)
            {
                finalValue = 101;
            }
            currentFatal = (finalValue%2 == 0) ? Fatals.BlueScreen : Fatals.Rollback;
        }

        private void Reset()
        {
            GetFinalValue();
            progressText.Text = string.Empty;
            lifeLine.Value = 0;
            bgImage.Opacity = 0;
            BitmapImage bg = new BitmapImage(new Uri("/Image/bgImage.jpg", UriKind.Relative));
            bgImage.Source = bg;
            makeBtn.Visibility = System.Windows.Visibility.Visible;
            lifeLine.Visibility = System.Windows.Visibility.Visible;
            worseImage.Visibility = System.Windows.Visibility.Visible;
            betterImage.Visibility = System.Windows.Visibility.Visible;
            progressText.Visibility = System.Windows.Visibility.Visible;
            currentImprovement.Visibility = System.Windows.Visibility.Visible;
            dots.Visibility = System.Windows.Visibility.Visible;
            currentImprovement.Text = string.Empty;
            dots.Text = string.Empty;
            makeBtn.IsEnabled = true;
        }

        private void SetBlueScreen()
        {
            BitmapImage blueScreen = new BitmapImage(new Uri("/Image/bluescreen.png",UriKind.Relative));
            bgImage.Source = blueScreen;
            bgImage.Opacity = 1;
            makeBtn.Visibility = System.Windows.Visibility.Collapsed;
            lifeLine.Visibility = System.Windows.Visibility.Collapsed;
            worseImage.Visibility = System.Windows.Visibility.Collapsed;
            betterImage.Visibility = System.Windows.Visibility.Collapsed;
            progressText.Visibility = System.Windows.Visibility.Collapsed;
            currentImprovement.Visibility = System.Windows.Visibility.Collapsed;
            dots.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void RollBack()
        {
            bgImage.Opacity -= 0.02;

            lifeLine.Value -= 2;
            progressText.Text = "Rollback: " + (100-Math.Round(lifeLine.Value*100/maxValue)) + "%";

            if (bgImage.Opacity <=0 || lifeLine.Value <= 0)
            {
                timer.Dispose();
                MessageBox.Show("Your life is now worse.",
                                "Rollback is finished",
                                MessageBoxButton.OK);
                Reset();

            }
        }

        private void bgImage_Tap(object sender, GestureEventArgs e)
        {
            if (makeBtn.Visibility == System.Windows.Visibility.Collapsed)
            {
                Reset();
            }
        }
    }
}