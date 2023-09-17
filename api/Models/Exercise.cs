using System.ComponentModel.DataAnnotations;

namespace api
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string possible_answers { get; set; }
        public string img { get; set; }
        public string topic { get; set; }
    }
}