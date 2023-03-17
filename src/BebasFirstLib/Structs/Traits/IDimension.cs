namespace BebasFirstLib.Structs.Traits {
    /// <summary>
    /// Menyediakan metode-metode untuk struktur data berdimensi.
    /// </summary>
    public interface IDimension {
        /// <summary>Dimensi dari objek ini.</summary>
        int Dimension { get; }

        /// <summary>
        /// Memeriksa apakah dimensi objek ini sama dengan dimensi <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Objek lain yang akan dibandingkan.</param>
        /// <returns>
        ///     <see langword="true"/> jika dimensi objek ini sama dengan dimensi <paramref name="other"/>;
        ///     <see langword="false"/> sebaliknya.
        /// </returns>
        bool DimEquals(IDimension other);
    }
}
