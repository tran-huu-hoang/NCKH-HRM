namespace NCKH_HRM.ViewModels
{
    public class StaffIndex
    {
        public long DetailTermId { get; set; }
        public string? StaffCode { get; set; }
        public string? StaffName { get; set; }
        public string? SubjectName { get; set; }
        public string? TermName { get; set; }
        public string? TermCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Room { get; set; }
        public int? CollegeCredit { get; set; }
    }
}