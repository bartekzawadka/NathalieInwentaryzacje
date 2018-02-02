using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NathalieInwentaryzacje.Lib.Bll.Managers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace Bll
{
    [TestClass]
    public class TemplatesManagerTests
    {
        private ITemplatesManager _templatesManager = new TemplatesManager();

        [TestMethod]
        public void CreateTemplate()
        {
            var templateInfo = new TemplateInfo
            {
                Name = "Srebro",
                Columns = new[]
                {
                    new TemplateColumn {Name = "Kolumienka jeden"},
                    new TemplateColumn {Name = "Kolumienka dwa"}
                }
            };

            _templatesManager.CreateOrUpdateTemplate(templateInfo);
            Assert.IsNotNull(templateInfo);
        }


        [TestMethod]
        public void CreateExistingTemplate()
        {
            var templateInfo = new TemplateInfo
            {
                Name = "Srebro",
                Columns = new[]
                {
                    new TemplateColumn {Name = "Kolumienka jeden"},
                    new TemplateColumn {Name = "Kolumienka dwa"}
                }
            };


            Assert.ThrowsException<Exception>(() => _templatesManager.CreateOrUpdateTemplate(templateInfo));
        }

        [TestMethod]
        public void UpdateTemplate()
        {
            var templateInfo = new TemplateInfo
            {
                Id = "897f5b3d-8294-4d2f-b0b8-4515b2180d9e",
                Name = "Sreberuszko dupuszko",
                Columns = new[]
                {
                    new TemplateColumn {Name = "Kolumienka jeden"},
                    new TemplateColumn {Name = "Kolumienka dwa"}
                }
            };

            _templatesManager.CreateOrUpdateTemplate(templateInfo);
            Assert.IsNotNull(templateInfo);
        }
    }
}
