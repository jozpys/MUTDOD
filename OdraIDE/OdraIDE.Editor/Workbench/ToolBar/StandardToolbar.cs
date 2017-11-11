using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Windows.Input;

namespace OdraIDE.Editor
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarSeparator1 : AbstractToolBarSeparator
    {
        public StandardToolbarSeparator1()
        {
            ID = "ToolbarSeparator1";
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarCutItem : AbstractToolBarButton
    {
        public StandardToolbarCutItem()
        {
            ID = "ToolbarCut";
            InsertRelativeToID = "ToolbarSeparator1";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Cut";
            SetIconFromBitmap(Resources.Images.Cut);
            Command = ApplicationCommands.Cut;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarCopyItem : AbstractToolBarButton
    {
        public StandardToolbarCopyItem()
        {
            ID = "ToolbarCopy";
            InsertRelativeToID = "ToolbarCut";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Copy";
            SetIconFromBitmap(Resources.Images.Copy);
            Command = ApplicationCommands.Copy;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarPasteItem : AbstractToolBarButton
    {
        public StandardToolbarPasteItem()
        {
            ID = "ToolbarPaste";
            InsertRelativeToID = "ToolbarCopy";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Paste";
            SetIconFromBitmap(Resources.Images.Paste);
            Command = ApplicationCommands.Paste;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarSeparator2 : AbstractToolBarSeparator
    {
        public StandardToolbarSeparator2()
        {
            ID = "ToolbarSeparator2";
            InsertRelativeToID = "ToolbarPaste";
            BeforeOrAfter = RelativeDirection.After;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarUndoItem : AbstractToolBarButton
    {
        public StandardToolbarUndoItem()
        {
            ID = "ToolbarUndo";
            InsertRelativeToID = "ToolbarSeparator2";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Undo";
            SetIconFromBitmap(Resources.Images.Undo);
            Command = ApplicationCommands.Undo;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarRedoItem : AbstractToolBarButton
    {
        public StandardToolbarRedoItem()
        {
            ID = "ToolbarRedo";
            InsertRelativeToID = "ToolbarUndo";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Undo";
            SetIconFromBitmap(Resources.Images.Redo);
            Command = ApplicationCommands.Redo;
        }
    }
}
