using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitle_Generator__Dot_NEt_
{
    public class TimeSRT
    {
        public static Dictionary<TimeSRT, string> srt = new Dictionary<TimeSRT, string>();

        public string start;
        public string finish;


        public static void Add(string s, string f,string co)
        {
            try
            {
                //Debug.WriteLine(s);
                //Debug.WriteLine(f);
                //Debug.WriteLine(co);
                if(TimeSpan.Compare(TimeSpan.Parse(s), TimeSpan.Parse(f))==-1)
                    srt.Add(new TimeSRT(s, f), co);
            }
            catch(Exception exx)
            {

            }
        }

        public TimeSRT(string s,string f)
        {
            this.start = Util.Time(s);
            this.finish = Util.Time(f);
            //Debug.WriteLine(start.ToString());
            //Debug.WriteLine(finish.ToString());
        }

        public static double GetSeconds(string s)
        {
            TimeSpan t = TimeSpan.Parse(s);
            return t.TotalSeconds;
        }


    }
}
