using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public class PlaybackEventArgs : EventArgs
    {
        public Dictionary<JointType, Joint> joints;
        public Dictionary<JointType, System.Windows.Point> jointPoints;

        public PlaybackEventArgs(Dictionary<JointType, Joint> joints, Dictionary<JointType, System.Windows.Point> jointPoints)
        {
            this.joints = joints;
            this.jointPoints = jointPoints;
        }
    }

    class Playback
    {
        public event EventHandler<PlaybackEventArgs> OnNewFrame;

        public int frame = 0;
        public int frameCount;
        private List<Tuple<Dictionary<JointType, Joint>, Dictionary<JointType, Point>>> data;

        public Playback()
        {
            //OpenPlayback(fileName);
            System.Threading.Thread tick = new System.Threading.Thread(Tick);
            tick.Start();
        }

        private void Tick()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000 / 30);

                if (data != null && data.Count > 0)
                {
                    OnNewFrame?.Invoke(this, new PlaybackEventArgs(data[frame].Item1, data[frame].Item2));
                    frame = frame + 1 < data.Count ? frame + 1 : frame = 0;
                }
            }
        }

        public void OpenPlayback(string fileName)
        {
            string[] textData = File.ReadAllLines(fileName);
            data = new List<Tuple<Dictionary<JointType, Joint>, Dictionary<JointType, Point>>>();

            foreach (string line in textData)
            {
                data.Add(Parser.BuildJointsAndJointPoints(line));
            }
            frame = 0;
            frameCount = data.Count;
        }
    }
}
