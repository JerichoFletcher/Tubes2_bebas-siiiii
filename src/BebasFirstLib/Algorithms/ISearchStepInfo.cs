namespace BebasFirstLib.Algorithms {
    public interface ISearchStepInfo<T> {
        T Value { get; }
        bool Found { get; }
    }
}
