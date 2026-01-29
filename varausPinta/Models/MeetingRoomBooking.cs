using System.ComponentModel.DataAnnotations;

namespace varausPinta.Models
{
    public class MeetingRoomBooking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Huoneen nimi on pakollinen tieto.")]
        public string RoomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Varaajan nimi on pakollinen tieto.")]
        public string BookedBy { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
