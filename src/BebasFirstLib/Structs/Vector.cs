using BebasFirstLib.Structs.Traits;
using System.Text;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Struktur data vektor yang terdiri dari sejumlah komponen bertipe <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tipe data dari komponen vektor.</typeparam>
    public class Vector<T> : IVector<T> {
        /// <summary>Larik komponen-komponen vektor.</summary>
        T[] elements;

        /// <summary>
        /// Membentuk sebuah objek <see cref="Vector{T}"/> dengan komponen sebanyak <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Banyak komponen vektor.</param>
        public Vector(int size) {
            elements = new T[size];
        }

        public int Dimension { get => elements.Length; }

        public bool DimEquals(IDimension other) => Dimension == other.Dimension;

        public T this[int index] {
            get => elements[index];
            set => elements[index] = value;
        }

        /// <summary>
        /// Membuat sebuah vektor dengan komponen-komponen yang diberikan.
        /// </summary>
        /// <param name="comps">Komponen-komponen dari vektor.</param>
        /// <returns>Vektor dengan komponen-komponen yang diberikan.</returns>
        public static Vector<T> From(params T[] comps) {
            Vector<T> vec = new Vector<T>(comps.Length);
            for(int i = 0; i < comps.Length; i++) {
                vec[i] = comps[i];
            }
            return vec;
        }

        /// <summary>
        /// Memeriksa apakah vektor ini sama dengan <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Objek lain yang diperiksa</param>
        /// <returns>
        ///     <see langword="true"/> jika kedua objek adalah <see cref="Vector{T}"/> yang tiap komponennya sama;
        ///     <see langword="false"/> sebaliknya.
        /// </returns>
        public override bool Equals(object other) {
            if(other is null) return false;
            if(other is Vector<T> otherVec) {
                if(!DimEquals(otherVec)) return false;
                for(int i = 0; i < elements.Length; i++)
                    if(!elements[i].Equals(otherVec.elements[i])) return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode() {
            return elements.GetHashCode();
        }

        public override string ToString() {
            StringBuilder str = new StringBuilder();
            str.Append("(");
            for(int i = 0; i < elements.Length; i++) {
                str.Append(elements[i].ToString());
                if(i < elements.Length - 1) str.Append(", ");
            }
            str.Append(")");
            return str.ToString();
        }
    }
}
