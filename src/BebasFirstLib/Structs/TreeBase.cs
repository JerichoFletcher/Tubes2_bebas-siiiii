using System.Collections;
using System.Collections.Generic;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Merepresentasikan struktur data rekursif berjenis pohon.
    /// </summary>
    /// <typeparam name="T">Tipe data dari nilai (atau INFO) yang disimpan dalam setiap simpul.</typeparam>
    public abstract class TreeBase<T> : IEnumerable {
        /// <summary>Nilai (atau INFO) dari simpul ini.</summary>
        public virtual T Value { get; set; }

        /// <summary>Banyak simpul anak dari simpul ini.</summary>
        public abstract int ChildCount { get; }

        /// <summary>
        /// Menambahkan simpul baru dengan nilai <paramref name="value"/> sebagai simpul anak dari simpul ini.
        /// </summary>
        /// <param name="value">Nilai dari simpul baru yang akan dibuat.</param>
        /// <returns>Simpul baru yang dibuat.</returns>
        public abstract TreeBase<T> AddChild(T value);

        /// <summary>
        /// Mengakses objek <see cref="TreeBaseEnumerator"/> yang akan mengiterasi setiap simpul anak.
        /// </summary>
        /// <returns>Objek enumerator.</returns>
        public abstract TreeBaseEnumerator GetEnumerator();

        /// <summary>
        /// Mengembalikan apakah simpul ini merupakan simpul daun.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> jika simpul ini adalah daun;
        ///     <see langword="false"/> sebaliknya.
        /// </returns>
        public virtual bool IsLeaf() {
            return ChildCount == 0;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Enumerator dari objek <see cref="TreeBase{T}"/> yang memungkinkan enumerasi simpul-simpul anak menggunakan <c>foreach</c>.
        /// </summary>
        public abstract class TreeBaseEnumerator : IEnumerator<TreeBase<T>> {
            /// <summary>Elemen yang ditunjuk oleh enumerator sekarang.</summary>
            public abstract TreeBase<T> Current { get; }

            /// <summary>
            /// Maju ke elemen selanjutnya.
            /// </summary>
            /// <returns>
            ///     <see langword="true"/> jika simpul anak selanjutnya dicapai;
            ///     <see langword="false"/> jika semua simpul anak sudah dienumerasi.
            /// </returns>
            public abstract bool MoveNext();

            /// <summary>Memundurkan enumerator ke keadaan awal, yaitu menunjuk ke satu sebelum simpul pertama.</summary>
            public abstract void Reset();

            /// <summary>Membebaskan memori yang digunakan oleh enumerator.</summary>
            public virtual void Dispose() { }

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }
        }
    }
}
