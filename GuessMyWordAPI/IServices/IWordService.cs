using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.IServices
{
    public interface IWordService
    {
        WordModel AddWord(WordModel word);
        WordModel GetWordById(long wordId);
        WordModel GetWordByGuid(Guid wordGuid);
        WordModel GetRandomWord(string language, int? length);
        WordModel SolveWord(long wordId, int tries);
    }
}
