namespace BebasFirstLib.Structs.Traits {
    /// <summary>
    /// Menandai bahwa objek ini dapat dirunut balik.
    /// </summary>
    /// <typeparam name="T">Tipe data dari objek yang dirunut balik.</typeparam>
    public interface IBacktrackable<T> {
        /// <summary>Objek predesesor dari objek ini.</summary>
        T Parent { get; }

        /// <summary>
        /// Memeriksa apakah objek ini merupakan akar, yaitu tidak memiliki predesesor.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> jika objek ini tidak memiliki predesesor;
        ///     <see langword="false"/> sebaliknya.
        /// </returns>
        bool IsRoot();

        /// <summary>
        /// Membentuk lintasan dari objek akar menuju objek ini.
        /// </summary>
        /// <returns>Larik berisi lintasan dari akar menuju objek ini.</returns>
        T[] TracePath();
    }
}
