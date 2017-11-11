using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using OdraIDE.Utilities;
using AvalonDock;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace OdraIDE.Results
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(OdraIDE.Results.CompositionPoints.Workbench.Pads.StringResultsPad, typeof(StringResultsPad))]
    [Pad(Name = StringResultsPad.RP_NAME)]
    public class StringResultsPad : AbstractPad
    {
        public const string RP_NAME = "StringResultsPad";

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        public StringResultsPad()
        {
            Name = RP_NAME;
            Title = "Results";
            Location = PadLocation.Bottom;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.StringResults);
        }

        #region TextBlock

        public string Result
        {
            get
            {
                return m_Result;
            }

            private set
            {
                if (m_Result != value)
                {
                    m_Result = value;
                    NotifyPropertyChanged(m_ResultArgs);
                }
            }
        }

        private string m_Result;
        static readonly PropertyChangedEventArgs m_ResultArgs = NotifyPropertyChangedHelper.CreateArgs<StringResultsPad>(o => o.Result);

        #endregion


        public void ShowResult(string result)
        {
            Result = result;
            layoutManager.Value.ShowPad(this);
        }

        public void Clear()
        {
            Result = "";
        }
    }


}
