using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Subtitle_Generator__Dot_NEt_
{
    public class Util
    {


        public static string Time(double time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);

            //00:00:03.000

            string tem = "";
            tem += t.Hours.ToString().PadLeft(2, '0');
            tem += ":";
            tem += t.Minutes.ToString().PadLeft(2, '0');
            tem += ":";
            tem += t.Seconds.ToString().PadLeft(2, '0');
            tem += ".";
            tem += t.Milliseconds.ToString().PadLeft(3, '0');


            return tem;
        }

        public static string GetXAML(RichTextBox _sub)
        {
            TextRange tr = new TextRange(_sub.Document.ContentStart,
                                _sub.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Xaml);
            string xamlText = ASCIIEncoding.Default.GetString(ms.ToArray());
            xamlText = xamlText.Replace("\"", "");

            //Debug.WriteLine(Util.Combine(xamlText));

            ms.Close();
            return Util.Combine(xamlText);
        }


        public static string Time(string time)
        {
            TimeSpan t = TimeSpan.Parse(time);

            //00:00:03.000

            string tem = "";
            tem += t.Hours.ToString().PadLeft(2, '0');
            tem += ":";
            tem += t.Minutes.ToString().PadLeft(2, '0');
            tem += ":";
            tem += t.Seconds.ToString().PadLeft(2, '0');
            tem += ".";
            tem += t.Milliseconds.ToString().PadLeft(3, '0');


            return tem;
        }


        public static bool Formated(string time)
        {
            
            try
            {
                TimeSpan.Parse(time);
                
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        


        public static string[] GetParagraphs(string data)
        {
            //return new string[] { data };

            data = data.Substring(data.IndexOf(@"<Paragraph>"));
            data = data.Remove(data.IndexOf(@"</Section>"));



            data = data.Replace(@"<Paragraph>", "");
            data = data.Replace(@"</Paragraph>", "\n");

            //Debug.WriteLine(data);


            data = data.Replace(@"<Run>", @"<");
            //Debug.WriteLine(data);
            data = data.Replace(@"</Run>", @">");
            //Debug.WriteLine(data);
            data = data.Replace(@"<Run ", @"<");
            //Debug.WriteLine(data);
            data = data.Replace("FontWeight=", "");
            data = data.Replace("FontStyle=", "");
            data = data.Replace("TextDecorations=", "");
            //Debug.WriteLine(data);
            data = data.Replace("Underline>", "Underline---");
            data = data.Replace("Bold>", "Bold---");
            data = data.Replace("Italic>", "Italic---");
            //Debug.WriteLine(data);
            return data.Split('\n');
        }


        public static string Combine(string x)
        {
            string res = "";
            string[] xx = GetParagraphs(x);

            foreach (string y in xx)
            {
                res += Util.MidToSRT(y) + "\n";
            }
            return res;
        }

        public static string MidToSRT(string data)
        {
            string res = "";
            int len = data.Length;
            for(int i=0;i<len;i++)
            {
                if(data.ElementAt(i).Equals('<'))
                {
                    for(int j=i+1;j<len;j++)
                    {
                        if(data.ElementAt(j).Equals('>'))
                        {
                            string tem = data.Substring(i + 1, j - i - 1);
                            string[] t = tem.Split(new string[] { "---" }, StringSplitOptions.None);
                            if(t.Length>1)
                            {
                                string[] sty = t[0].Split(' ');
                                int ll = sty.Length;
                                for(int xx=0;xx<ll;xx++)
                                {
                                    res += Tag(sty[xx], true);
                                }
                                res += t[1];
                                for (int xx = 0; xx < ll; xx++)
                                {
                                    res += Tag(sty[xx], false);
                                }
                            }
                            else
                            {
                                res += tem;
                            }

                            i = j + 1;
                        }
                    }
                }
            }
            return res;
        }


        public static string Tag(string t,bool start)
        {
            if(start==true)
            {
                if (t.Equals("Bold"))
                    return @"<b>";
                else if (t.Equals("Italic"))
                    return @"<i>";
                else if (t.Equals("Underline"))
                    return @"<u>";
            }
            else
            {

                if (t.Equals("Bold"))
                    return @"</b>";
                else if (t.Equals("Italic"))
                    return @"</i>";
                else if (t.Equals("Underline"))
                    return @"</u>";
            }
            return "";
        }


        

    }
}
