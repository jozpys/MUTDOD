using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace OdraIDE.Core.Messaging
{
    [Export(Services.Messaging.MessagingService, typeof(IMessagingService))]
    class MessagingService : IMessagingService
    {
        [Import(Services.Logging.LoggingService, typeof(ILoggingService))]
        private Lazy<ILoggingService> loggingService { get; set; }

        #region IMessagingService Members

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(
                message, title);
            loggingService.Value.Info(message);
        }

        public DialogResult ShowDialog(string message, string title, MessageBoxButtons buttons)
        {
            DialogResult result = MessageBox.Show(
                message, title, buttons);
            loggingService.Value.Info(message + "; result = " + result.ToString());
            return result;
        }

        #endregion
    }
}
