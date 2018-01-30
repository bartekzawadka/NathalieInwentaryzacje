using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return GetTemplates(false);
        }

        public void CreateOrUpdateTemplate(TemplateInfo tInfo)
        {
            var path = Path.Combine(_templatesPath, tInfo.Name + ".xml");
            var templates = GetTemplates(true);

            if (string.IsNullOrEmpty(tInfo.Id))
            {
                if (templates.Any(x => string.Equals(x.Name.Trim().ToLower(), tInfo.Name.Trim().ToLower())))
                    throw new Exception("Szablon o takiej nazwie już istnieje");

                tInfo.Id = Guid.NewGuid().ToString();
            }
            else
            {
                var item = templates.SingleOrDefault(x => string.Equals(x.Id, tInfo.Id));
                if (item != null)
                {
                    File.Delete(Path.Combine(_templatesPath, item.Name + ".xml"));
                }
            }

            XmlFileSerializer.Serialize(tInfo, path);
        }

        private IEnumerable<TemplateInfo> GetTemplates(bool includeDisabled = false)
        {
            var files = Directory.GetFiles(_templatesPath);

            var templates = new List<TemplateInfo>();

            foreach (var file in files)
            {
                var template = XmlFileSerializer.Deserialize<TemplateInfo>(file);
                if(!includeDisabled && !template.IsEnabled)
                    continue;
                templates.Add(template);
            }

            return templates;
        }
    }
}
