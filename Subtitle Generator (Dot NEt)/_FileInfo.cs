using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitle_Generator__Dot_NEt_
{
    public class _FileInfo
    {

        public string Location { get; set; }
        public string FullName { get; set; }
        public string FileNameWithoutExtention
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(FullName);
            }
        }
        public override string ToString()
        {
            return FileNameWithoutExtention;
        }
    }
}
