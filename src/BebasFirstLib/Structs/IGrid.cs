using System;
using BebasFirstLib.Structs.Traits;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Menyediakan metode-metode umum untuk tipe data yang merepresentasikan sebuah grid.
    /// </summary>
    /// <typeparam name="T">Tipe data yang merepresentasikan setiap sumbu dimensi pada grid.</typeparam>
    public interface IGrid<T> : IDimension where T : IComparable {
        /// <summary>Ukuran dari grid.</summary>
        IVector<T> Size { get; }

        /// <summary>
        /// Memeriksa apakah posisi yang diberikan berada di dalam batasan grid.
        /// </summary>
        /// <param name="position">Posisi yang akan diperiksa.</param>
        /// <returns>
        ///     <see langword="true"/> jika posisi berada dalam batasan grid;
        ///     <see langword="false"/> sebaliknya.
        /// </returns>
        bool IsInBounds(IVector<T> position);
    }
}
