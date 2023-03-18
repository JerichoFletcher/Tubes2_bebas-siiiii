using System;
using System.Collections.Generic;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Menyediakan metode-metode umum yang dimiliki struktur data berjenis peta grid.
    /// </summary>
    /// <typeparam name="TAxis">Tipe data yang merepresentasikan setiap sumbu dimensi pada grid peta.</typeparam>
    /// <typeparam name="TElement">Tipe data dari elemen dalam peta.</typeparam>
    public interface IGridMap<TAxis, TElement> : IGrid<TAxis>
        where TAxis : IComparable
        where TElement : IPosition<TAxis> {
        /// <summary>
        /// Mengakses elemen pada posisi yang diberikan.
        /// </summary>
        /// <param name="position">Posisi elemen yang ingin diakses.</param>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <returns>Elemen pada posisi yang diberikan.</returns>
        TElement this[IVector<TAxis> position] { get; set; }

        /// <summary>
        /// Mengumpulkan semua elemen yang bertetangga dengan <paramref name="tile"/>.
        /// </summary>
        /// <param name="tile">Elemen yang diperiksa.</param>
        /// <returns>Sebuah <see cref="List{T}"/> berisi elemen-elemen yang bertetangga dengan <paramref name="tile"/>.</returns>
        List<TElement> NeighborsOf(TElement tile);
    }
}
