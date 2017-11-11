using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using OdraIDE.Utilities;
using System.ComponentModel;

namespace OdraIDE.ServerUtilities
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(OdraIDE.ServerUtilities.CompositionPoints.Workbench.Pads.ServerConsolePad, typeof(ServerConsolePad))]
    [Pad(Name = ServerConsolePad.SCP_NAME)]
    public class ServerConsolePad: AbstractPad
    {
        public const string SCP_NAME = "ServerConsolePad";

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        public ServerConsolePad()
        {
            Name = SCP_NAME;
            Title = "Server Console";
            Location = PadLocation.Bottom;
            Icon = ImageHelper.GetImageFromResources(Images.CommandPrompt);
        }

        #region TextBlock

        public string Output
        {
            get
            {
                return m_Output;
            }

            private set
            {
                if (m_Output != value)
                {
                    m_Output = value;
                    NotifyPropertyChanged(m_ResultArgs);
                }
            }
        }

        private string m_Output;
        static readonly PropertyChangedEventArgs m_ResultArgs = NotifyPropertyChangedHelper.CreateArgs<ServerConsolePad>(o => o.Output);

        #endregion


        public void AttachOutput(string result)
        {
            Output += "\n" + result;
        }

        public void Clear()
        {
            Output = "";
        }
    }


}
