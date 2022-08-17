using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuessMyWordAPI.Models
{
    public class WordMetadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }
        public long WordId { get; set; }
        public int SolveIndex { get; set; }
        public int SolveCount { get; set; }

        public WordModel Word { get; set; }
    }
}
