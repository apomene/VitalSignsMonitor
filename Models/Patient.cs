namespace VitalSignsMonitor.Models
{
 
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string RoomNumber { get; set; } = string.Empty;

        public ICollection<VitalSign> VitalSigns { get; set; } = new List<VitalSign>();
    }

}
