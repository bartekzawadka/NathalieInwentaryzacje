using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NathalieInwentaryzacje.Lib.Bll.Mappers;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class TemplatesManager : ITemplatesManager
    {
        private readonly string _templatesPath = Path.Combine(Path.GetFullPath("."), "Data", "Szablony");

        public TemplatesManager()
        {
            if (!Directory.Exists(_templatesPath))
                Directory.CreateDirectory(_templatesPath);
        }

        public IEnumerable<TemplateInfo> GetTemplates(bool includeDisabled = false)
        {
            var files = Directory.GetFiles(_templatesPath, "*.xml");

            var templates = new List<TemplateInfo>();

            foreach (var file in files)
            {
                var template = XmlFileSerializer.Deserialize<Template>(file);
                if (!includeDisabled && !template.IsEnabled)
                    continue;
                templates.Add(TemplateMapper.ToTemplateInfo(template));
            }

            return templates;
        }

        public TemplateInfo GetTemplate(string id)
        {
            return GetTemplates(true).Single(x => string.Equals(x.Id, id));
        }

        public void CreateOrUpdateTemplate(TemplateInfo t)
        {
            var path = Path.Combine(_templatesPath, Path.GetFileNameWithoutExtension(t.TemplateFilePath) + ".xml");
            var templates = GetTemplates(true);

            if (string.IsNullOrEmpty(t.Id))
            {
                //                if (templates.Any(x => string.Equals(x.Name.Trim().ToLower(), t.Name.Trim().ToLower())))
                //                    throw new Exception("Szablon o takiej nazwie już istnieje");

                if (File.Exists(path) || File.Exists(Path.Combine(_templatesPath,
                        Path.GetFileName(t.TemplateFilePath) ?? throw new InvalidOperationException())))
                {
                    throw new Exception("Szablon o takiej nazwie już istnieje");
                }

                t.Id = Guid.NewGuid().ToString();
                var fName = Path.GetFileName(t.TemplateFilePath);
                File.Copy(t.TemplateFilePath, Path.Combine(_templatesPath, fName));
                t.TemplateFilePath = fName;
            }
            else
            {
                var item = templates.SingleOrDefault(x => string.Equals(x.Id, t.Id));
                if (item != null)
                {
                    File.Delete(Path.Combine(_templatesPath, item.Name + ".xml"));
                }
            }

            XmlFileSerializer.Serialize(TemplateMapper.ToTemplate(t), path);
        }
    }
}
