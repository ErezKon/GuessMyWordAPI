using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.ViewModels
{
    public class SolveWordVM
    {
        public long ID { get; set; }
        public long WordId { get; set; }
        public int SolveIndex { get; set; }
        public int SolveCount { get; set; }

        public SolveWordVM()
        {

        }

        public SolveWordVM(WordMetadata metadata)
        {
            ID = metadata.ID;
            WordId = metadata.WordId;
            SolveIndex = metadata.SolveIndex;
            SolveCount = metadata.SolveCount;
        }
    }
}
