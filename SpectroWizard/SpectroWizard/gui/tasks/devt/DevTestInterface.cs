using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectroWizard.gui.tasks.devt
{
    interface DevTestInterface
    {
        bool Run();
        string GetReport();
    }
}
