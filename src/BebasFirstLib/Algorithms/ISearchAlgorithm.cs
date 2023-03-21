using System;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms {
    /// <summary>
    /// Antarmuka untuk algoritma-algoritma pencarian secara umum.
    /// </summary>
    /// <typeparam name="TPred">Tipe data elemen yang diperiksa oleh predikat berhenti.</typeparam>
    /// <typeparam name="TReturn">Tipe data luaran pencarian.</typeparam>
    /// <typeparam name="TInfo">Tipe data objek informasi langkah pencarian.</typeparam>
    public interface ISearchAlgorithm<TPred, TReturn, TInfo> where TInfo : ISearchStepInfo<TReturn> {
        /// <summary>
        /// Melakukan pencarian dengan algoritma tertentu.
        /// </summary>
        /// <param name="successPred">Kondisi yang harus dipenuhi oleh goal.</param>
        /// <returns>Sebuah <see cref="IEnumerable{T}"/> berisi semua langkah-langkah pencarian.</returns>
        IEnumerable<TInfo> Search(Predicate<TPred> successPred);
    }
}
