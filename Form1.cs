using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cam2 {
    public partial class Form1 : Form {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        public Form1() {
            InitializeComponent();
            {
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach(FilterInfo VideoCaptureDevice in VideoCaptureDevices) {
                    comboBox1.Items.Add(VideoCaptureDevice.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
        }

        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, Byte> img1 = video.ToImage<Bgr, byte>();
            Rectangle[] rec = cascadeClassifier.DetectMultiScale(img1, 1.2, 1);
            foreach(Rectangle r in rec) {
                using(Graphics graphics = Graphics.FromImage(video)) {
                    using(Pen pen = new Pen(Color.Red, 1)) {
                        graphics.DrawRectangle(pen, r);
                    }
                }
            }
            pictureBox1.Image = video;
        }

        public static byte[] ImageToByte(Image img) {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static byte[] ImageToByte2(Image img) {
            using(var stream = new MemoryStream()) {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
        }

        private void button2_Click(object sender, EventArgs e) {
            FinalVideo.Stop();
        }

        private void label1_Click(object sender, EventArgs e) {

        }
    }
}
