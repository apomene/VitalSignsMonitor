namespace VitalSignsMonitor.Models
{
   
    public class VitalSign
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        public DateTime Timestamp { get; set; }

        public int HeartRate { get; set; }          
        public int SystolicBP { get; set; }         
        public int DiastolicBP { get; set; }       
        public int OxygenSaturation { get; set; }   // percentage

        public Patient Patient { get; set; } = null!;
    }

}
