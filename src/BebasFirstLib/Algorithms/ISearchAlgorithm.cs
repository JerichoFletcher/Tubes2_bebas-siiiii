using System;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms {
    /// <summary>
    /// Antarmuka untuk kelas-kelas algoritma pencarian secara umum.
    /// </summary>
    /// /// <typeparam name="TPred">Tipe data objek yang diperiksa.</typeparam>
    /// <typeparam name="TReturn">Tipe data objek hasil pencarian.</typeparam>
    public interface ISearchAlgorithm<TPred, TReturn, TInfo> where TInfo : ISearchStepInfo<TReturn> {
        /// <summary>
        /// Melakukan pencarian dengan algoritma tertentu.
        /// </summary>
        /// <param name="successPred">Kondisi yang harus dipenuhi oleh goal.</param>
        /// <returns>Objek hasil pencarian.</returns>
        IEnumerable<TInfo> Search(Predicate<TPred> successPred);
    }
}
