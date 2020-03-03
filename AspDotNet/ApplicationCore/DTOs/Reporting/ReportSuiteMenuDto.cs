using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class ReportSuiteMenuDTO
    {
        public ReportSuiteMenuDTO()
        {
            Childs = new List<ReportSuiteMenuDTO>();
        }

        public int ReportId { get; set; }
        public int? Parent_Key { get; set; }
        public string Node_Text { get; set; }
        public string Report_Name { get; set; }
        public string Report_Path_Name { get; set; }
        public string Report_Sql { get; set; }
        public int SeqNo { get; set; }
        public bool IsVisible { get; set; }
        public bool HasDefaultValue { get; set; }
        public bool IsMultipleSelection { get; set; }
        public int ApplicationId { get; set; }
        public bool HasExternalReport { get; set; }
        public bool IsSessionUse { get; set; }
        public List<ReportSuiteMenuDTO> Childs { get; set; }
    }
}
