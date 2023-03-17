using System;
using BebasFirstLib.Structs.Traits;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Menyediakan metode-metode yang umum ada pada struktur data berjenis vektor.
    /// </summary>
    /// <typeparam name="T">Tipe data dari setiap komponen vektor.</typeparam>
    public interface IVector<T> : IDimension {
        /// <summary>
        /// Mengakses elemen pada indeks yang diberikan.
        /// </summary>
        /// <param name="index">Indeks elemen yang ingin diakses.</param>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <returns>Elemen pada indeks yang diberikan.</returns>
        T this[int index] { get; set; }
    }
}
