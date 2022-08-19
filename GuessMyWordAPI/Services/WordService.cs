using GuessMyWordAPI.DataLayer;
using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Models;
using Microsoft.EntityFrameworkCore;

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
            if (string.IsNullOrWhiteSpace(word.Language) || string.IsNullOrWhiteSpace(word.Word))
            {
                throw new ArgumentNullException("Missing data, Language/Word");
            }
            var existingWord = _wordDB.Words
                .FirstOrDefault(w => w.Language.Equals(word.Language) && w.Word.Equals(word.Word));
            if(existingWord != null)
            {
                return existingWord;
            }
            word.Guid = Guid.NewGuid();
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

        public WordModel GetWordByGuid(Guid wordGuid, bool withMetada = false)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return withMetada ?
                _wordDB
                .Words
                .Include(w => w.Metadata)
                .FirstOrDefault(w => w.Guid.Equals(wordGuid)) :
                _wordDB
                .Words
                .FirstOrDefault(w => w.Guid.Equals(wordGuid));
#pragma warning restore CS8603 // Possible null reference return.
        }

        public WordModel GetWordById(long wordId, bool withMetada = false)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return withMetada ?
                _wordDB
                .Words
                .Include(w => w.Metadata)
                .FirstOrDefault(w => w.ID == wordId) :
                _wordDB
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
