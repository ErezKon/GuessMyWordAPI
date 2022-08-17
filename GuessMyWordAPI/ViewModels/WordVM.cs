using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.ViewModels
{
    public class WordVM
    {
        public long? ID { get; set; }
        public Guid Guid { get; set; }
        public string Word { get; set; }
        public string Language { get; set; }

        public WordVM()
        {

        }

        public WordVM(WordModel word)
        {
            ID = word.ID;
            Guid = word.Guid;
            Word = word.Word;
            Language = word.Language;
        }
    }
}
