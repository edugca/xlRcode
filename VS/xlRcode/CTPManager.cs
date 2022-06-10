using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;

namespace xlRcode
{
    internal static class CTPManager
    {
        public static CustomTaskPane ctp;

        public static void ShowCTP()
        {
            if (ctp == null)
            {
                // Make a new one using ExcelDna.Integration.CustomUI.CustomTaskPaneFactory 
                ctp = CustomTaskPaneFactory.CreateCustomTaskPane(typeof(consoleControl), "xlRcode Excel console");
                ctp.Visible = true;
                ctp.DockPosition = MsoCTPDockPosition.msoCTPDockPositionBottom;
            }
            else
            {
                // Just show/hide it again
                ctp.Visible = !ctp.Visible;
            }
        }

    }
}
