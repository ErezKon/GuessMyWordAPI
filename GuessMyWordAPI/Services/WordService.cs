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
        private readonly int maxTries = 6;
        public WordService(IConfiguration config)
        {
            _wordDB = new WordContext(config);
            _random = new Random();
        }
        public WordModel AddWord(string language, string word)
        {
            if (string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentNullException("Missing data, Language/Word");
            }
            var existingWord = _wordDB.Words
                .FirstOrDefault(w => w.Language.Equals(language) && w.Word.Equals(word));
            if(existingWord != null)
            {
                return existingWord;
            }
            var wordModel = new WordModel
            {
                Guid = Guid.NewGuid(),
                Language = language.ToLower(),
                Word = word.ToUpper()
            };
            _wordDB.Add(wordModel);
            _wordDB.SaveChanges();
            for (int i = 0; i <= maxTries; i++)
            {
                wordModel.Metadata.Add(new WordMetadata
                    {
                        WordId = wordModel.ID,
                        SolveCount = 0,
                        SolveIndex = i,
                    });
            }
            _wordDB.SaveChanges();
            return wordModel;
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
            var word = _wordDB
                .Words
                .Include(w => w.Metadata)
                .FirstOrDefault(w => w.ID == wordId);
            if(word != null)
            {
                var meta = word.Metadata
                    .FirstOrDefault(m => m.SolveIndex == tries);
                if(meta != null)
                {
                    meta.SolveCount++;
                    _wordDB.SaveChanges();
                }
            }
            return word;
        }
    }
}
