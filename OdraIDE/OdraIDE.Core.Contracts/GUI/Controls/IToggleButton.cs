﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IToggleButton : IButton
    {
        bool IsChecked { get; }
    }
}
