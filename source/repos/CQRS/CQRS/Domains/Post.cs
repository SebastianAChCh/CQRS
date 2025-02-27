using System.ComponentModel.DataAnnotations;

namespace CQRS.Domains
{
    public class Post
    {
        [Key]
        public int id_key { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string author { get; set; }

    }
}
