using System.Windows;
using System.Windows.Controls;

namespace _1712907
{
    internal class Bitmap : Image
    {
        private Image imgToResize;
        private Size size;

        public Bitmap(Image imgToResize, Size size)
        {
            this.imgToResize = imgToResize;
            this.size = size;
        }
    }
}