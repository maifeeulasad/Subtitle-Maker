using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;
using System.Xml.Linq;

namespace Subtitle_Generator__Dot_NEt_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string _stp = "00:00:00.000";
        public static string _ftp = "00:00:00.000";

        string pathM = "";

        static double ppos = 0;

        static string eend = "";

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            _media.Width = this.ActualWidth - 10;
            _media.Height = this.ActualHeight - 20;
        }


        bool b = false;
        string _SelectedFilePath = null;

        

        public MainWindow()
        {
            InitializeComponent();

            //Span.store.Add(new Span(0, 1000), "asdasdasd");
            //Span.store.Add(new Span(1500, 2000), "as6465asd");
            //Span.store.Add(new Span(3000, 5000), "465464");

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Video|*.avi;*.mpg;*.wmv;*.mp4;*.srt";
            dlg.Multiselect = false;
            if (dlg.ShowDialog().Value == true)
            {
                if (dlg.FileNames.Count() == 0)
                    return;

                foreach (string filename in dlg.FileNames)
                {

                    string path = System.IO.Path.GetFullPath(dlg.FileName);
                    path = path.Substring(0, path.LastIndexOf('\\'));
                    pathM = path;
                    _FileInfo NewFile = new _FileInfo { FullName = filename ,Location = path};
                    b = false;
                    _SelectedFilePath = NewFile.FullName;
                    this.Title = NewFile.ToString();
                    //Debug.WriteLine("path &&&" + pathM);
                }
                Func();
                b = true;
            }
        }

        void mtl_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            //_subPPPP.Text =  Span.GetContent(Span.store, _media.Position.TotalSeconds);
            _Time.Text = Util.Time(_media.Position.TotalSeconds);
            _positionProgress.Value = _media.Position.TotalSeconds;
        }
        private void PlayStop_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Style content = _playButton.Style;

            if (content == this.Resources["pauseBtnStyle"] as System.Windows.Style)
            {
                _media.Clock.Controller.Pause();
                _playButton.Style = this.Resources["playBtnStyle"] as System.Windows.Style;
            }
            if (content == this.Resources["playBtnStyle"] as System.Windows.Style)
            {
                _media.Clock.Controller.Resume();
                _playButton.Style = this.Resources["pauseBtnStyle"] as System.Windows.Style;
            }
        }

        private void Progress_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double pos = _media.NaturalDuration.TimeSpan.TotalSeconds * e.GetPosition(_positionProgress).X / _positionProgress.RenderSize.Width;

            DoubleAnimation progressAnimation = new DoubleAnimation();
            progressAnimation.From = _media.Position.TotalSeconds;
            progressAnimation.To = pos;
            progressAnimation.FillBehavior = FillBehavior.Stop;
            progressAnimation.Duration = TimeSpan.FromMilliseconds(150);
            _positionProgress.BeginAnimation(ProgressBar.ValueProperty, progressAnimation);

            _media.Clock.Controller.Seek(TimeSpan.FromSeconds((int)pos), TimeSeekOrigin.BeginTime);
            
        }

        private void _mediaOpened(object sender, RoutedEventArgs e)
        {
            _positionProgress.Maximum = _media.NaturalDuration.TimeSpan.TotalSeconds;
            _playButton.Style = this.Resources["pauseBtnStyle"] as System.Windows.Style;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null)
                return;

            foreach (string filename in files)
            {
                _FileInfo NewFile = new _FileInfo { FullName = filename };
                b = false;
                _SelectedFilePath = NewFile.FullName;
                this.Title = NewFile.ToString();


                string path = System.IO.Path.GetFullPath(filename);
                path = path.Substring(0, path.LastIndexOf('\\'));
                pathM = path;

            }
            Func();
            b = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_media != null)
                _media.Volume = _Slider.Value * 0.1;
        }



        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (b)
                if (_positionProgress.Value < 5)
                    _media.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
                else
                    _media.Clock.Controller.Seek(TimeSpan.FromSeconds(_positionProgress.Value - 5), TimeSeekOrigin.BeginTime);
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
            if (b && _positionProgress.Value < _positionProgress.Maximum - 5)
                _media.Clock.Controller.Seek(TimeSpan.FromSeconds(_positionProgress.Value + 5), TimeSeekOrigin.BeginTime);
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            if (b)
                _media.Clock.Controller.Stop();
        }

        public void Func()
        {
            MediaTimeline mtl = new MediaTimeline(new Uri(_SelectedFilePath));
            mtl.CurrentTimeInvalidated += mtl_CurrentTimeInvalidated;
            _media.Clock = mtl.CreateClock();
            _media.Clock.Controller.Resume();
        }

        private void Set_Button_Click(object sender, RoutedEventArgs e)
        {
            string s = Util.GetXAML(_sub);

            TimeSRT.Add(_st.Text, _ft.Text, s);
        }

        
        private void _st_TextChanged(object sender, TextChangedEventArgs e)
        {

            if(!Util.Formated(_st.Text))
            {
                _st.Foreground = Brushes.Red;
            }
            else
            {
                _st.Foreground = Brushes.Green;
            }
        }

        private void _ft_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Util.Formated(_ft.Text))
            {
                _ft.Foreground = Brushes.Red;
            }
            else
            {
                _ft.Foreground = Brushes.Green;
            }
        }

        private void _save_Click(object sender, RoutedEventArgs e)
        {
            int cou = 0;
            string srt = "";
            /*
             * 1
00:00:05,000 --> 00:00:15,000
Created and Encoded by --  Bokutox -- of  www.YIFY-TORRENTS.com. The Best 720p/1080p/3d movies with the lowest file size on the internet.


             * 
             * 
             */
            Dictionary<TimeSRT, string> x = TimeSRT.srt;
            foreach (KeyValuePair<TimeSRT, string> k in x)
            {
                srt += (++cou).ToString() + "\n";
                srt += k.Key.start + " --> " + k.Key.finish + "\n";
                srt += k.Value + "\n";
                //Debug.WriteLine(k.Key.start + " ^^^ " + k.Key.finish + " ^^^ " + k.Value);
            }
            //Debug.Write(srt);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Sub-title|*.srt";

            dlg.FileName = this.Title;
            dlg.InitialDirectory = pathM;

            //Debug.WriteLine("path &&&^^^" + pathM);

            if (dlg.ShowDialog().Value == true)
            {
                string path = System.IO.Path.GetFullPath(dlg.FileName);
                //Debug.WriteLine();
                File.WriteAllLines(path, new string[] { srt });
            }

        }




        private void Preview_Button_Click(object sender, RoutedEventArgs e)
        {
            _media.Clock.Controller.Resume();
            _media.Clock.Controller.Seek(TimeSpan.FromSeconds(TimeSRT.GetSeconds(_st.Text)), TimeSeekOrigin.BeginTime);
            //while(_media.Clock.Controller.)
            Thread t2 = new Thread(t);
            t2.Start();
        }

        void t()
        {
            while (true)
            {

                this.Dispatcher.Invoke(
                    new Action(() =>
                    ppos = _media.Position.TotalSeconds));
                this.Dispatcher.Invoke(
                    new Action(() =>
                    eend = _ft.Text));

                if (ppos > TimeSRT.GetSeconds(eend))
                {

                    this.Dispatcher.Invoke(
                        new Action(() =>
                        _media.Clock.Controller.Pause()));
                    
                    return;
                }
            }
        }




    }
}
