using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.ViewModels
{
    public class WordVM
    {
        public long? ID { get; set; }
        public Guid Guid { get; set; }
        public string Word { get; set; }
        public string Language { get; set; }
        public string? Description { get; set; }
        public List<SolveWordVM> Solved { get; set; }

        public WordVM()
        {

        }

        public WordVM(WordModel word)
        {
            ID = word.ID;
            Guid = word.Guid;
            Word = word.Word;
            Language = word.Language;
            Solved = new List<SolveWordVM>();
            if(word.Metadata != null && word.Metadata.Count > 0)
            {
                Solved = word.Metadata
                    .Select(m => new SolveWordVM(m))
                    .ToList();
            }
        }
    }
}
