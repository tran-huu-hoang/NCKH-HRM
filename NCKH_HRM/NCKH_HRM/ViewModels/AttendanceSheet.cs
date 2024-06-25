namespace NCKH_HRM.ViewModels
{
    public class AttendanceSheet
    {
        public string StudentCode { get; set; }

        public string StudentName { get; set; }

        public List<int> ListBeginClass { get; set; }

        public List<int> ListEndClass { get; set; }

        public float NumberOfBeginClassesAttended { get; set; }
        public float NumberOfEndClassesAttended { get; set; }
        public float NumberOfBeginLate { get; set; }
        public float NumberOfEndLate { get; set; }
        public float CountDateLearn { get; set; }

    }
}