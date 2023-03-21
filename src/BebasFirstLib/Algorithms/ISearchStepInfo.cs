namespace BebasFirstLib.Algorithms {
    /// <summary>
    /// Menyediakan informasi mengenai sebuah langkah dalam algoritma pencarian.
    /// </summary>
    /// <typeparam name="T">Tipe data luaran pencarian.</typeparam>
    public interface ISearchStepInfo<T> {
        /// <summary>
        /// Nilai dari hasil pencarian pada langkah ini.
        /// </summary>
        T Value { get; }
        /// <summary>
        /// Apakah pencarian sudah selesai pada langkah ini?
        /// </summary>
        bool Found { get; }
    }
}
