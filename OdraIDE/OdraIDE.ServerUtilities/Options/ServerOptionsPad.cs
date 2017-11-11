using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel;
using OdraIDE.Utilities;
using System.ComponentModel.Composition;

namespace OdraIDE.ServerUtilities
{
    [Export(CompositionPoints.Workbench.Options.ServerOptionsPad, typeof(ServerOptionsPad))]
    public class ServerOptionsPad : AbstractOptionsPad
    {
        public ServerOptionsPad()
        {
            Name = "ServerOptionsPad";
        }

        public override void Commit()
        {
            base.Commit();
            Properties.Settings.Default.Save();
        }

        #region "CentralServerLocation"

        public string CentralServerLocationEdit
        {
            get
            {
                return m_CentralServerLocationEdit;
            }
            set
            {
                if (m_CentralServerLocationEdit != value)
                {
                    m_CentralServerLocationEdit = value;
                    CommitActions.Add(
                        () =>
                        {
                            Properties.Settings.Default.CentralServerLocation = m_CentralServerLocationEdit;
                            NotifyPropertyChanged(m_CentralServerLocationEditArgs);
                            NotifyPropertyChanged(m_CentralServerLocationArgs);
                        }
                        );
                    CancelActions.Add(
                        () =>
                        {
                            m_CentralServerLocationEdit = Properties.Settings.Default.CentralServerLocation;
                            NotifyPropertyChanged(m_CentralServerLocationEditArgs);
                            NotifyPropertyChanged(m_CentralServerLocationArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_CentralServerLocationEditArgs);
                }
            }
        }
        private string m_CentralServerLocationEdit = Properties.Settings.Default.CentralServerLocation;
        static readonly PropertyChangedEventArgs m_CentralServerLocationEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<ServerOptionsPad>(o => o.CentralServerLocationEdit);

        public string CentralServerLocation
        {
            get
            {
                return Properties.Settings.Default.CentralServerLocation;
            }
        }
        static readonly PropertyChangedEventArgs m_CentralServerLocationArgs =
            NotifyPropertyChangedHelper.CreateArgs<ServerOptionsPad>(o => o.CentralServerLocation);
        #endregion

        #region "DataServerLocation"

        public string DataServerLocationEdit
        {
            get
            {
                return m_DataServerLocationEdit;
            }
            set
            {
                if (m_DataServerLocationEdit != value)
                {
                    m_DataServerLocationEdit = value;
                    CommitActions.Add(
                        () =>
                        {
                            Properties.Settings.Default.DataServerLocation = m_DataServerLocationEdit;
                            NotifyPropertyChanged(m_DataServerLocationEditArgs);
                            NotifyPropertyChanged(m_DataServerLocationArgs);
                        }
                        );
                    CancelActions.Add(
                        () =>
                        {
                            m_DataServerLocationEdit = Properties.Settings.Default.DataServerLocation;
                            NotifyPropertyChanged(m_DataServerLocationEditArgs);
                            NotifyPropertyChanged(m_DataServerLocationArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_DataServerLocationEditArgs);
                }
            }
        }
        private string m_DataServerLocationEdit = Properties.Settings.Default.DataServerLocation;
        static readonly PropertyChangedEventArgs m_DataServerLocationEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<ServerOptionsPad>(o => o.DataServerLocationEdit);

        public string DataServerLocation
        {
            get
            {
                return Properties.Settings.Default.DataServerLocation;
            }
        }
        static readonly PropertyChangedEventArgs m_DataServerLocationArgs =
            NotifyPropertyChangedHelper.CreateArgs<ServerOptionsPad>(o => o.DataServerLocation);
        #endregion
    }
}
