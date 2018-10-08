using System;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;

namespace FFImageLoading.Forms
{
    public class KeyStreamImageSource : StreamImageSource,IKeyImageSource
    {
        public string Key { get; set; }
    }
}
