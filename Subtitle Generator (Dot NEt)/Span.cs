using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitle_Generator__Dot_NEt_
{
    public class Span
    {

        public static Dictionary<Span, string> store = new Dictionary<Span, string>();


        

        private float startingSecond;
        private float finishingSecond;
        public Span(float s,float f)
        {
            this.startingSecond = s;
            this.finishingSecond = f;
        }

        public static string GetContent(Dictionary<Span, string> s,double time)
        {

            int len = s.Count;
            for(int i=0;i<len;i++)
            {
                //Debug.WriteLine(s.Keys.ElementAt(i).startingSecond + " ^ " + s.Keys.ElementAt(i).finishingSecond + " ^ " + s.Values.ElementAt(i));
                
                if(s.Keys.ElementAt(i).startingSecond<time && time< s.Keys.ElementAt(i).finishingSecond)
                {
                    return s.Values.ElementAt(i);
                }
                
            }

            return "";
        }

    }
}
