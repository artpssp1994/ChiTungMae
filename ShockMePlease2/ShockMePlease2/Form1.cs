using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebaseSharp.Portable;
using System.IO.Ports;
using Newtonsoft.Json;

namespace ShockMePlease2
{
    public partial class Form1 : Form
    {
        private SerialPort port = new SerialPort("COM3");
        private DateTime lcStartime;
        private DateTime lcEndtime;
        public Form1() 
        {
            InitializeComponent();
            lcEndtime = new DateTime();
            lcStartime = new DateTime();
            DateTime timeset = new DateTime();
            port.Open();
            FirebaseApp app = new FirebaseApp(new Uri("https://shockmeplease-7ebf5.firebaseio.com/"));
            var scoresRef = app.Child("Test");
            scoresRef.OrderByValue()
             .On("value", (snapshot, child, context) => {
                 var test2 = snapshot.Value();
                 var time = JsonConvert.DeserializeObject<DateFirebase>(snapshot.Value());
                 //DateTime Starttime = Convert.ToDateTime(Convert.ToDouble( time.start));
                 //DateTime Endtime = Convert.ToDateTime(Convert.ToDouble(time.start));
                 DateTime Starttime = DateTime.ParseExact(time.start, "MM/dd/yyyy HH:mm ",
                                       System.Globalization.CultureInfo.InvariantCulture);
                 DateTime Endtime = DateTime.ParseExact(time.end, "MM/dd/yyyy HH:mm ",
                                       System.Globalization.CultureInfo.InvariantCulture);
                 DateTime currenttime = DateTime.Now;
                 if (Starttime > currenttime && timeset == Starttime && timeset == Endtime)
                 {
                     shutdown(0);
                 }
                 else if (Endtime < currenttime && timeset == Starttime && timeset == Endtime)
                 {
                     shutdown(0);
                 }
                 lcStartime = Starttime;
                 lcEndtime = Endtime;     
             });
            Task.Run(() => {
                  while (true)
                  {
                      if (DateTime.Now < lcStartime && timeset !=lcStartime)
                      {
                          SendData("pm");
                          shutdown(300);
                        break;
                      }
                      else if (DateTime.Now > lcEndtime && timeset !=lcEndtime)
                      {
                          SendData("pm");
                          shutdown(300);
                        break;
                      }
                      Task.Delay(2000);
                  }
              });
        }
        public void shutdown(int delay)
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t "+delay);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
        public void SendData(string text)
        {
            if(!port.IsOpen)
            {
                port.Open();
            }
            port.Write(text);
            Console.WriteLine(text);
        }

    }
}