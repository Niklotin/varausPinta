namespace varausPinta.Models
{
    public class MeetingRoomBooking
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string BookedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
