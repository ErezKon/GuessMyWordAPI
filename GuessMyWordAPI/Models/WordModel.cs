using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuessMyWordAPI.Models
{
    public class WordModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }
        [Required]
        public Guid Guid { get; set; }
        [Required]
        public string? Language { get; set; }
        [Required]
        public string? Word { get; set; }

        public List<WordMetadata> Metadata { get; set; }
    }
}
