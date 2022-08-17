using GuessMyWordAPI.DataLayer;
using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.Services
{
    public class WordService : IWordService
    {
        private readonly WordContext _wordDB;
        private readonly Random _random;
        public WordService(IConfiguration config)
        {
            _wordDB = new WordContext(config);
            _random = new Random();
        }
        public WordModel AddWord(WordModel word)
        {
            word.Guid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(word.Language) || string.IsNullOrWhiteSpace(word.Word))
            {
                throw new ArgumentNullException("Missing data, Language/Word");
            }
            word.Language = word.Language.ToLower();
            word.Word = word.Word.ToUpper();
            _wordDB.Add(word);
            _wordDB.SaveChanges();
            return word;
        }

        public WordModel GetRandomWord(string language, int? length)
        {
            var lang = language.ToLower();
            var query = _wordDB.Words
                .Where(w => w.Language.Equals(lang));
            if(length.HasValue && length.Value > 0)
            {
                query = query
                    .Where(w => w.Word.Length == length.Value);
            }

            var lst = query.ToList();
            return lst[_random.Next(lst.Count)];
        }

        public WordModel GetWordByGuid(Guid wordGuid)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _wordDB
                .Words
                .FirstOrDefault(w => w.Guid.Equals(wordGuid));
#pragma warning restore CS8603 // Possible null reference return.
        }

        public WordModel GetWordById(long wordId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _wordDB
                .Words
                .FirstOrDefault(w => w.ID == wordId);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public WordModel SolveWord(long wordId, int tries)
        {
            throw new NotImplementedException();
        }
    }
}
