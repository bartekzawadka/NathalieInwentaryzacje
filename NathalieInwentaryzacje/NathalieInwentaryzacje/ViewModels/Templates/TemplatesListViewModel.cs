using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Templates
{
    public class TemplatesListViewModel : ListScreen<TemplateInfo>
    {
        private readonly ITemplatesManager _templatesManager = IoC.Get<ITemplatesManager>();

        public TemplatesListViewModel()
        {
            LoadData();
        }

        public override void LoadData()
        {
            Context = _templatesManager.GetTemplates();
        }
    }
}
