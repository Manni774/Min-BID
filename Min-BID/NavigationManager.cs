using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Min_BID
{
    internal class NavigationManager
    {
        private static Frame _mainFrame;

        public static void Initialize(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public static void Navigate(Page page)
        {
            _mainFrame?.Navigate(page);
        }
    }
}
