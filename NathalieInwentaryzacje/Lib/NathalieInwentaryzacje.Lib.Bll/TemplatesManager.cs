using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll
{
    public class TemplatesManager : ITemplatesManager
    {
        private readonly string _templatesPath = Path.Combine(Path.GetFullPath("."), "Data", "Szablony");

        public TemplatesManager()
        {
            if (!Directory.Exists(_templatesPath))
                Directory.CreateDirectory(_templatesPath);
        }

        public IEnumerable<TemplateInfo> GetTemplates()
        {
            var files = Directory.GetFiles(_templatesPath);

            var templates = new List<TemplateInfo>();

            foreach (var file in files)
            {
                var template = XmlFileSerializer.Deserialize<TemplateInfo>(file);
                if(template.IsEnabled)
                    templates.Add(template);
            }

            return templates;
        }

        public void SaveTemplate(TemplateInfo tInfo)
        {
            var path = Path.Combine(_templatesPath, tInfo.Name + ".xml");
            XmlFileSerializer.Serialize(tInfo, path);
        }
    }
}
