using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NathalieInwentaryzacje.Lib.Bll.Mappers;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class TemplatesManager : ManagerBase, ITemplatesManager
    {
        public TemplatesManager(DataLocationInfo paths) : base(paths)
        {
            if (!Directory.Exists(Paths.TemplatesPath))
                Directory.CreateDirectory(Paths.TemplatesPath);
        }

        public IEnumerable<TemplateInfo> GetTemplates(bool includeDisabled = false)
        {
            var files = Directory.GetFiles(Paths.TemplatesPath, "*.xml");

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
            var path = Path.Combine(Paths.TemplatesPath, Path.GetFileNameWithoutExtension(t.TemplateFilePath) + ".xml");
            var templates = GetTemplates(true);

            if (string.IsNullOrEmpty(t.Id))
            {
                if (File.Exists(path) || File.Exists(Path.Combine(Paths.TemplatesPath,
                        Path.GetFileName(t.TemplateFilePath) ?? throw new InvalidOperationException())))
                {
                    throw new Exception("Szablon o takiej nazwie już istnieje");
                }

                t.Id = Guid.NewGuid().ToString();
                var fName = Path.GetFileName(t.TemplateFilePath);
                File.Copy(t.TemplateFilePath, Path.Combine(Paths.TemplatesPath, fName));
                t.TemplateFilePath = fName;
            }
            else
            {
                var item = templates.SingleOrDefault(x => string.Equals(x.Id, t.Id));
                if (item != null)
                {
                    File.Delete(Path.Combine(Paths.TemplatesPath, item.Name + ".xml"));
                }
            }

            XmlFileSerializer.Serialize(TemplateMapper.ToTemplate(t), path);
        }
    }
}
