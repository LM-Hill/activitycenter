using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class Outing
    {
        [Key]
        public int OutingId {get;set;}

        public int UserId {get;set;}
        public int JubileeId {get;set;}
        public User Participant {get;set;}
        public Jubilee SportingEvent {get;set;}
    }
}