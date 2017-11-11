using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace OdraIDE.SolutionExplorer
{
    //[Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.NewMenu, typeof(IMenuItem))]
    //class FileMenuNewSolution : AbstractMenuItem, IPartImportsSatisfiedNotification
    //{
    //    [Import(CompositionPoints.Workbench.Commands.NewSolution, typeof(ICustomCommand))]
    //    private ICustomCommand command { get; set; }

    //    public FileMenuNewSolution()
    //    {
    //        ID = "NewSolution";
    //        Header = Resources.Strings.Workbench_MainMenu_File_New_Solution;
    //        SetIconFromBitmap(Resources.Images.NewSolution);
    //    }

    //    #region IPartImportsSatisfiedNotification Members

    //    public void OnImportsSatisfied()
    //    {
    //        Command = command;
    //    }

    //    #endregion
    //}

    //[Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.OpenMenu, typeof(IMenuItem))]
    //class FileMenuOpenSolution : AbstractMenuItem, IPartImportsSatisfiedNotification
    //{
    //    [Import(CompositionPoints.Workbench.Commands.OpenSolution, typeof(ICustomCommand))]
    //    private ICustomCommand command { get; set; }

    //    public FileMenuOpenSolution()
    //    {
    //        ID = "OpenSolution";
    //        Header = Resources.Strings.Workbench_MainMenu_File_Open;
    //        SetIconFromBitmap(Resources.Images.Open);
    //    }

    //    public void OnImportsSatisfied()
    //    {
    //        Command = command;
    //    }
    //}
}
