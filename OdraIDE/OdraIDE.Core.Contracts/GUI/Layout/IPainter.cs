﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IPainter
    {
       // ITaskService TaskService { get; }
        IDocument Document { get; set; }

    }
}
