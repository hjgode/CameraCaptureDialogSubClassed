using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.WindowsMobile.Forms;
using StartButtonWM65;
using SHELLAPI;

namespace CameraCaptureDialogSubClassed
{
    public partial class Form1 : Form
    {
        private string m_sPos = "";

        public Form1(string sPosition)
        {
            InitializeComponent();
            m_sPos = sPosition;
        }
        hwndutils subClassUtils;
        private void ShowCamera()
        {
            //Hide Start and Done/Close button symbol
            bool bOldStart = StartAndCloseButton.showStartButton(false);
            bool bOldDone = StartAndCloseButton.showCloseButton(false);

            CameraCaptureDialog cdlg = new CameraCaptureDialog();
            cdlg.DefaultFileName = "picture.jpg";
            cdlg.InitialDirectory = "\\My Documents";
            cdlg.Mode = CameraCaptureMode.Still;
            cdlg.Owner = this.pictureBox1;
            cdlg.StillQuality = CameraCaptureStillQuality.High;
            cdlg.Title = "Take a picture and Select";
            cdlg.Resolution = new Size(240, 320);
            cdlg.VideoTypes = CameraCaptureVideoTypes.All;

            //subclass main window with delay
            subClassUtils = new hwndutils();
            System.Threading.Thread threadTick = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
            threadTick.Start();


            DialogResult dRes = cdlg.ShowDialog();
            if (dRes == DialogResult.OK)
            {
                //load image
                try
                {
                    loadImage(cdlg.FileName);
                }
                catch (SystemException sx)
                {
                    System.Diagnostics.Debug.WriteLine(sx.Message);
                }
            }
            subClassUtils.Dispose();
            cdlg.Dispose();
            //restore Start and Done/Close button display
            StartAndCloseButton.showStartButton(bOldStart);
            StartAndCloseButton.showCloseButton(bOldDone);
            if (threadTick.Join(1000))
            {
                threadTick.Abort();
                threadTick = null;
            }
        }

        void thread()
        {
            System.Diagnostics.Debug.WriteLine("Thread started");
            int maxSleep = 1000;
            int sleepCount = 0;
            IntPtr hwnd = IntPtr.Zero;
            try
            {
                do
                {
                    System.Threading.Thread.Sleep(100);
                    sleepCount += 100;
                    //wait for class name...
                    hwnd = hwndutils.FindWindow(subClassUtils.winClassName, IntPtr.Zero);
                }
                while (sleepCount < maxSleep || hwnd == IntPtr.Zero);
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                System.Diagnostics.Debug.WriteLine("Thread aborted: " + ex.Message);
            }
            if (hwnd != IntPtr.Zero)
            {
            System.Threading.Thread.Sleep(200);
                System.Diagnostics.Debug.WriteLine("Camera View window found");
                subClassUtils.winClassName = "Camera View";
                //subClassUtils.CloseButtonDisabled = true;
                //subClassUtils.StartButtonDisabled = true;
                subClassUtils.MenuButtonDisabled = true;
            }
            System.Diagnostics.Debug.WriteLine("Thread ended");
        }

        private void loadImage(string s)
        {
            Image i = new Bitmap(s);
            Graphics g = Graphics.FromImage(i);

            // Create font and brush.
            Font drawFont = new Font("Arial", 10, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.GreenYellow);

            // Create point for upper-left corner of drawing.
            float x = 0.0F;
            float y = 0.0F;

            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.NoClip;

            //            g.DrawImage(i,0,0);
            string sDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            g.DrawString(sDateTime + " at " + this.m_sPos, drawFont, drawBrush, x, y, drawFormat);
            pictureBox1.Image = i; // new Bitmap(cdlg.FileName);

            pictureBox1.Update();
            string sImageFile;
            sImageFile = ShellAPI.getMyPicturesFolder() + "\\SnapShot" + sDateTime + ".jpg";
            pictureBox1.Image.Save(sImageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        private void mnuCamera_Click_1(object sender, EventArgs e)
        {
            ShowCamera();
        }

        private void mnuBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}