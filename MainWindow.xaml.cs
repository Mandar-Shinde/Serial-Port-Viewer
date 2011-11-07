using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;

namespace serialport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void expander1_Expanded(object sender, RoutedEventArgs e)
        {
            richTextBox1.Margin = new Thickness(0,110, 0, 0);
        }

        private void expander1_Collapsed(object sender, RoutedEventArgs e)
        {
            richTextBox1.Margin = new Thickness(0, 30, 0, 0);
        }
        int clrmon = 0;
        public void updat_monitor_ui(string dts)
        {
            clrmon++;
           
            richTextBox1.AppendText(dts);
            richTextBox1.ScrollToEnd();
            if (clrmon > 92)
                richTextBox1.Background = Brushes.LawnGreen;
            if (clrmon > 98)
            {
                richTextBox1.Background = Brushes.White;
                richTextBox1.Document.Blocks.Clear();
                clrmon = 0;
            }

        }


        SerialPort sp = new SerialPort();

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            sp.Handshake = Handshake.None;
            sp.ReadTimeout = 19000;// Timeout.Infinite;//300;
            sp.WriteTimeout = 20;
            sp.DtrEnable = true;
            sp.RtsEnable = true;
            sp.BaudRate = 115200;
            sp.PortName = textBox1.Text;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;

            Thread thr_SP_starter = new Thread(Start_SP);
            thr_SP_starter.IsBackground = true;
            thr_SP_starter.Name = "thr_SP_starter";
            thr_SP_starter.Start();
        }

        public void Start_SP()
        {
            sp.Open();
            LaunchMonitor();
        }

        public void LaunchMonitor()
        {
            String prn;
            while (true)
            {
                prn = (sp.ReadLine());
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<string>(updat_monitor_ui), prn);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            sp.WriteLine(textBox2.Text);
        }

    }
}
