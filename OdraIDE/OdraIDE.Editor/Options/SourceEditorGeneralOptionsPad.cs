using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using OdraIDE.Utilities;
using System.ComponentModel;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Windows.Media;

namespace OdraIDE.Editor
{
    [Export(CompositionPoints.Workbench.Options.SourceEditorGeneralOptionsPad, typeof(SourceEditorGeneralOptionsPad))]
    public class SourceEditorGeneralOptionsPad : AbstractOptionsPad
    {
        public SourceEditorGeneralOptionsPad()
        {
            Name = "SourceEditorGeneralOptionsPad";
        }

        public override void Commit()
        {
            base.Commit();
            Properties.Settings.Default.Save();
        }

        #region "ShowLineNumbers"

        public bool ShowLineNumbersEdit
        {
            get
            {
                return m_ShowLineNumbersEdit;
            }
            set
            {
                if (m_ShowLineNumbersEdit != value)
                {
                    m_ShowLineNumbersEdit = value;
                    CommitActions.Add(
                        () => 
                            {
                                Properties.Settings.Default.ShowLineNumbers = m_ShowLineNumbersEdit;
                                NotifyPropertyChanged(m_ShowLineNumbersEditArgs);
                                NotifyPropertyChanged(m_ShowLineNumbersArgs);
                            }
                        );
                    CancelActions.Add(
                        () =>
                        {
                            m_ShowLineNumbersEdit = Properties.Settings.Default.ShowLineNumbers;
                            NotifyPropertyChanged(m_ShowLineNumbersEditArgs);
                            NotifyPropertyChanged(m_ShowLineNumbersArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_ShowLineNumbersEditArgs);
                }
            }
        }
        private bool m_ShowLineNumbersEdit = Properties.Settings.Default.ShowLineNumbers;
        static readonly PropertyChangedEventArgs m_ShowLineNumbersEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.ShowLineNumbersEdit);

        public bool ShowLineNumbers
        {
            get
            {
                return Properties.Settings.Default.ShowLineNumbers;
            }
        }
        static readonly PropertyChangedEventArgs m_ShowLineNumbersArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.ShowLineNumbers);
        #endregion

        #region "WordWrap"

        public bool WordWrapEdit
        {
            get
            {
                return m_WordWrapEdit;
            }
            set
            {
                if (m_WordWrapEdit != value)
                {
                    m_WordWrapEdit = value;
                    CommitActions.Add(
                        () => 
                        {
                            Properties.Settings.Default.WordWrap = m_WordWrapEdit;
                            NotifyPropertyChanged(m_WordWrapEditArgs);
                            NotifyPropertyChanged(m_WordWrapArgs);
                        });
                    CancelActions.Add(
                        () =>
                        {
                            m_WordWrapEdit = Properties.Settings.Default.WordWrap;
                            NotifyPropertyChanged(m_WordWrapEditArgs);
                            NotifyPropertyChanged(m_WordWrapArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_WordWrapEditArgs);
                }
            }
        }
        private bool m_WordWrapEdit = Properties.Settings.Default.WordWrap;
        static readonly PropertyChangedEventArgs m_WordWrapEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.WordWrapEdit);

        public bool WordWrap
        {
            get
            {
                return Properties.Settings.Default.WordWrap;
            }
        }
        static readonly PropertyChangedEventArgs m_WordWrapArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.WordWrap);
        #endregion


        #region "FontFamily"

        public FontFamily FontFamilyEdit
        {
            get
            {
                return m_FontFamilyEdit;
            }
            set
            {
                if (m_FontFamilyEdit != value)
                {
                    m_FontFamilyEdit = value;
                    CommitActions.Add(
                        () =>
                        {
                            Properties.Settings.Default.FontFamily = m_FontFamilyEdit;
                            NotifyPropertyChanged(m_FontFamilyEditArgs);
                            NotifyPropertyChanged(m_FontFamilyArgs);
                        });
                    CancelActions.Add(
                        () =>
                        {
                            m_FontFamilyEdit = Properties.Settings.Default.FontFamily;
                            NotifyPropertyChanged(m_FontFamilyEditArgs);
                            NotifyPropertyChanged(m_FontFamilyArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_FontFamilyEditArgs);
                }
            }
        }
        private FontFamily m_FontFamilyEdit = Properties.Settings.Default.FontFamily;
        static readonly PropertyChangedEventArgs m_FontFamilyEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.FontFamilyEdit);

        public FontFamily FontFamily
        {
            get
            {
                return Properties.Settings.Default.FontFamily;
            }
        }

        static readonly PropertyChangedEventArgs m_FontFamilyArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.FontFamily);
        #endregion

        #region "FontSize"

        public double FontSizeEdit
        {
            get
            {
                return m_FontSizeEdit;
            }
            set
            {
                if (m_FontSizeEdit != value)
                {
                    m_FontSizeEdit = value;
                    CommitActions.Add(
                        () =>
                        {
                            Properties.Settings.Default.FontSize = m_FontSizeEdit;
                            NotifyPropertyChanged(m_FontSizeEditArgs);
                            NotifyPropertyChanged(m_FontSizeArgs);
                        });
                    CancelActions.Add(
                        () =>
                        {
                            m_FontSizeEdit = Properties.Settings.Default.FontSize;
                            NotifyPropertyChanged(m_FontSizeEditArgs);
                            NotifyPropertyChanged(m_FontSizeArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_FontSizeEditArgs);
                }
            }
        }
        private double m_FontSizeEdit = Properties.Settings.Default.FontSize;
        static readonly PropertyChangedEventArgs m_FontSizeEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.FontSizeEdit);

        public double FontSize
        {
            get
            {
                return Properties.Settings.Default.FontSize;
            }
        }

        static readonly PropertyChangedEventArgs m_FontSizeArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.FontSize);
        #endregion

        #region "HighlightingDefinition not used"

        public IHighlightingDefinition HighlightingDefinitionEdit
        {
            get
            {
                return m_HighlightingDefinitionEdit;
            }
            set
            {
                if (m_HighlightingDefinitionEdit != value)
                {
                    m_HighlightingDefinitionEdit = value;
                    CommitActions.Add(
                        () =>
                        {
                            m_HighlightingDefinition = m_HighlightingDefinitionEdit;
                            NotifyPropertyChanged(m_HighlightingDefinitionEditArgs);
                            NotifyPropertyChanged(m_HighlightingDefinitionArgs);
                        });
                    CancelActions.Add(
                        () =>
                        {
                            m_HighlightingDefinitionEdit = m_HighlightingDefinition;
                            NotifyPropertyChanged(m_HighlightingDefinitionEditArgs);
                            NotifyPropertyChanged(m_HighlightingDefinitionArgs);
                        });
                    NotifyOptionChanged();
                    NotifyPropertyChanged(m_HighlightingDefinitionEditArgs);
                }
            }
        }
        private IHighlightingDefinition m_HighlightingDefinitionEdit;

        static readonly PropertyChangedEventArgs m_HighlightingDefinitionEditArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.HighlightingDefinitionEdit);

        public IHighlightingDefinition HighlightingDefinition
        {
            get
            {
                return m_HighlightingDefinition;
            }
        }

        private IHighlightingDefinition m_HighlightingDefinition;

        static readonly PropertyChangedEventArgs m_HighlightingDefinitionArgs =
            NotifyPropertyChangedHelper.CreateArgs<SourceEditorGeneralOptionsPad>(o => o.HighlightingDefinition);
        #endregion
    }
}
