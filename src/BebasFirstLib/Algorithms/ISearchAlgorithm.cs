namespace BebasFirstLib.Algorithms {
    /// <summary>
    /// Antarmuka untuk kelas-kelas algoritma pencarian secara umum.
    /// </summary>
    /// <typeparam name="T">Tipe data objek hasil pencarian.</typeparam>
    public interface ISearchAlgorithm<T> {
        /// <summary>
        /// Melakukan pencarian dengan algoritma tertentu.
        /// </summary>
        /// <returns>Objek hasil pencarian.</returns>
        T Search();
    }
}
