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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Media.Media3D;
using HelixToolkit;
using HelixToolkit.Wpf;
using System.Windows.Threading;
using NUnit.Framework;
using devDept.Eyeshot.Translators;
using System.Diagnostics;
using Microsoft.Win32;
using sysMedia = System.Windows.Media;


namespace Digitally_Inspired_Test_Task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public ObservableCollection<Task> Tasks { get; set; }
        public FileModelVisual3D fm;
        
        DispatcherTimer timer = new DispatcherTimer();
        int z;
        int a = 0;
        int i = 0;
       
        public MainWindow()
        {
            InitializeComponent();
            int zx = Convert.ToInt32(Zmax.Value);
            int zm = Convert.ToInt32(Zmin.Value);
            a = zx+zm;
            z = Convert.ToInt32(Zmin.Value);
            Tasks = new ObservableCollection<Task>
        {
            new Task {id=1, TaskHeader="Task 3", tb=Task3Body},
            new Task { id = 2, TaskHeader = "Task 4",tb=Task4Body},
            new Task { id = 3, TaskHeader = "Task 5",tb=Task5Body}

        };
            TasksList.ItemsSource = Tasks;


        }
        
       
        private void EXIT(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MINIMIZE(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MAX_RESTORE(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {this.WindowState = WindowState.Maximized;
              Mininwin_btn.Visibility = Visibility.Visible;               
                Max_btn.Visibility = Visibility.Hidden;  
                
            }
            else if(this.WindowState == WindowState.Maximized)
            { this.WindowState = WindowState.Normal;
                Max_btn.Visibility = Visibility.Visible;               
                Mininwin_btn.Visibility = Visibility.Hidden;
                }
        }       

        private void TasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  Task t = (Task)TasksList.SelectedItem;
           
          //t.tb();
            
        }       
       

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {try
            {
                Task t = (Task)TasksList.SelectedItem;

                t.tb();
            }
            catch
            {
                MessageBox.Show("Choose a Task!");
            }
            }
     
        public void Task4Body()
        {
            MessageBox.Show("This Task is haven't done. Try another one");
            //StartBtn.Visibility = Visibility.Hidden;
            //StopBtn.Visibility = Visibility.Hidden;
            //Zmax.Visibility = Visibility.Hidden;
            //Zmin.Visibility = Visibility.Hidden;
            //zmax.Visibility = Visibility.Hidden;
            //zmin.Visibility = Visibility.Hidden;

            //var d = new OpenFileDialog();
            //d.Filter = "Object files(*.obj)|*.obj";
            //if (!d.ShowDialog().Value)
            //{
            //    return;
            //}
            //string filename = d.FileName;
            //View1.Children.Clear();
            
            //fm = new FileModelVisual3D();
            //fm.Source = filename;
            //fm.Transform = new TranslateTransform3D(0, 0, 0);
            //View1.Children.Add(fm);
         

        }
        public void Task3Body()
        {
            
            StartBtn.Visibility = Visibility.Hidden;
            StopBtn.Visibility = Visibility.Hidden;
            Zmax.Visibility = Visibility.Hidden;
            Zmin.Visibility = Visibility.Hidden;
            zmax.Visibility = Visibility.Hidden;
            zmin.Visibility = Visibility.Hidden;
Open_object();
        }
        
        public void Task5Body()
        {
            
            var d = new OpenFileDialog();
            d.Filter = "Object files(*.obj)|*.obj";
            if (!d.ShowDialog().Value)
            {
                return;
            }
            string filename = d.FileName;
            View1.Children.Clear();
           
            fm = new FileModelVisual3D();
            fm.Source = filename;
            fm.Transform = new TranslateTransform3D(0, 0, 0);
            View1.Children.Add(fm);

            StartBtn.Visibility = Visibility.Visible;
            StopBtn.Visibility = Visibility.Visible;
            Zmax.Visibility = Visibility.Visible;
            Zmin.Visibility = Visibility.Visible;
            zmax.Visibility = Visibility.Visible;
            zmin.Visibility = Visibility.Visible;
            
        }
        
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            View1.Children.Clear();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        public void Open_object()
        {
            var d = new OpenFileDialog();
            d.Filter = "Object files(*.obj)|*.obj";
            if (!d.ShowDialog().Value)
            {
                return;
            }
            string filename = d.FileName;
            var loader = new ModelImporter();
            Model3D currentModel = loader.Load(filename, Dispatcher.CurrentDispatcher);
            
            View1.Children.Add(new ModelVisual3D { Content = currentModel });
        }

        private void Zmin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //fm.Transform = new TranslateTransform3D(0, 0, Zmin.Value);
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            

            if (z < Convert.ToInt32(Zmax.Value) && a!=0)
            {

                fm.Transform = new TranslateTransform3D(0, 0, z);
                z++;
                a--;
                

            }
            else if (a==0 && i < Convert.ToInt32(Zmax.Value - Zmin.Value))
            {
                fm.Transform = new TranslateTransform3D(0, 0, z);
                z--;
                i++;
            }
            else { a = Convert.ToInt32(Zmax.Value)- Convert.ToInt32(Zmin.Value);
                i = 0;
            }
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }

    public class Task
        {
        public int id { get; set; }
        public string TaskHeader { get; set; }

       
        public delegate void TaskBody();
        public TaskBody tb { get; set; }
        }
   

}
