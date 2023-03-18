using BebasFirstLib.Structs.Traits;
using System;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Menyediakan metode-metode untuk kelas objek yang memiliki suatu posisi.
    /// </summary>
    /// <typeparam name="T">Tipe data yang merepresentasikan setiap sumbu dimensi pada posisi.</typeparam>
    public interface IPosition<T> : IDimension where T : IComparable {
        /// <summary>
        /// Posisi dari objek ini.
        /// </summary>
        IVector<T> Position { get; }
    }
}
