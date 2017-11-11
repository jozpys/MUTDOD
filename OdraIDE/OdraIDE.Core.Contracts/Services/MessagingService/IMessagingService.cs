using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OdraIDE.Core
{
    public interface IMessagingService
    {
        void ShowMessage(string message, string title);
        DialogResult ShowDialog(string message, string title, MessageBoxButtons buttons);
    }
}
