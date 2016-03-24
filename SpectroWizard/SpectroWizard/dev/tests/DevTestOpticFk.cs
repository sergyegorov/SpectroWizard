using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;

namespace SpectroWizard.dev.tests
{
    public class DevTestOpticFk : DevTest
    {
        public DevTestOpticFk()
        {
        }

        public override void Apply()
        {
            return;
        }

        public override string Results()
        {
            return "";
        }

        public override Spectr[] GetSpectrResults(out string[] names,out int view_type)
        {
            view_type = 0;
            names = null;
            return null;
        }

        public override bool RunProc()
        {
            return true;
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst,"Корректировка параметров оптики");
        }

        public override bool GetDefaultState()
        {
            return true;
        }
    }
}
