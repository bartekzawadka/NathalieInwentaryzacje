using System;
using System.IO;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Bll.Mappers;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Appendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class AppendixManager : ManagerBase, IAppendixManager
    {
        public AppendixManager(DataLocationInfo paths) : base(paths)
        {
        }

        public RecordAppendixInfo GetAppendix(DateTime? recordDate)
        {
            var isRecord = recordDate != null && recordDate.Value != new DateTime();
            var path = !isRecord
                ? Path.Combine(Paths.TemplatesPath, "Appendix.xml")
                : Path.Combine(Paths.RecordsPath, recordDate.Value.ToRecordDateString(), "Appendix.xml");

            if (!File.Exists(path))
            {
                if (!isRecord)
                    XmlFileSerializer.Serialize(AppendixMapper.ToAppendix(new RecordAppendixInfo()),
                        path);
                else
                {
                    var appendixTemplatePath = Path.Combine(Paths.TemplatesPath, "Appendix.xml");
                    if (!File.Exists(appendixTemplatePath))
                    {
                        var rai = new RecordAppendixInfo();
                        XmlFileSerializer.Serialize(AppendixMapper.ToAppendix(rai),
                            appendixTemplatePath);
                    }
                    File.Copy(appendixTemplatePath, path);
                }
            }

            var info = AppendixMapper.ToAppendixInfo(XmlFileSerializer.Deserialize<Appendix>(path));
            if (recordDate != null && recordDate != new DateTime())
                info.RecordDate = recordDate.Value;

            return info;
        }

        public void SaveAppendix(RecordAppendixInfo appendix)
        {
            if (appendix == null)
                return;

            var path = appendix.RecordDate != new DateTime()
                ? Path.Combine(Paths.RecordsPath, appendix.RecordDate.ToRecordDateString(), "Appendix.xml")
                : Path.Combine(Paths.TemplatesPath, "Appendix.xml");

            XmlFileSerializer.Serialize(AppendixMapper.ToAppendix(appendix), path);
        }
    }
}
