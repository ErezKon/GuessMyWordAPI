using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.IServices
{
    public interface IWordService
    {
        WordModel AddWord(WordModel word);
        WordModel GetWordById(long wordId, bool withMetada = false);
        WordModel GetWordByGuid(Guid wordGuid, bool withMetada = false);
        WordModel GetRandomWord(string language, int? length);
        WordModel SolveWord(long wordId, int tries);
    }
}
