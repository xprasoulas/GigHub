using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }

        [Key] //the combination of the GigId and AttendeeId uniquely represents an attendance.
        [Column(Order =1)]
        public int GigId { get; set; } //will save us from loading an entire gig or attendee object in order to add a new attendance to the database

        [Key]
        [Column(Order = 2)]
        public string AttendeeId { get; set; }
    }
}

//When you use data annotations for composite keys, we need to specify the order for your columns.