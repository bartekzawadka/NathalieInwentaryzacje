using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathalieInwentaryzacje.ViewModels.Common
{
    public abstract class ListScreen : ScreenBase
    {
        protected abstract void LoadData();
    }
}
