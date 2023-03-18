using System;

namespace BebasFirstLib.Algorithms {
    /// <summary>
    /// Antarmuka untuk kelas-kelas algoritma pencarian secara umum.
    /// </summary>
    public interface ISearchAlgorithm {
        /// <summary>
        /// Melakukan pencarian dengan algoritma tertentu.
        /// </summary>
        /// <typeparam name="TPred">Tipe data objek yang diperiksa oleh <paramref name="successPred"/>.</typeparam>
        /// <typeparam name="TReturn">Tipe data objek hasil pencarian.</typeparam>
        /// /// <param name="successPred">Kondisi yang harus dipenuhi oleh goal.</param>
        /// <returns>Objek hasil pencarian.</returns>
        TReturn Search<TReturn, TPred>(Predicate<TPred> successPred);
    }
}
