﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;

namespace OdraIDE.Tasks
{
    /// <summary>
    /// Interaction logic for GridTasksPadView.xaml
    /// </summary>
    [Export(OdraIDE.Core.ExtensionPoints.Host.Views, typeof(ResourceDictionary))]
    public partial class GridTasksPadView : ResourceDictionary
    {
        public GridTasksPadView()
        {
            InitializeComponent();
        }
    }
}
