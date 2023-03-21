using BebasFirstLib.Structs.Traits;
using BebasFirstLib.IO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Merepresentasikan sebuah peta terbatas dengan dimensi tertentu, menggunakan sistem koordinat bilangan bulat.
    /// </summary>
    /// <typeparam name="T">Tipe data dari nilai setiap sel dalam peta.</typeparam>
    public class Map<T> : IGridMap<int, Map<T>.MapTile> {
        /// <summary>Buffer internal berisi semua sel pada peta.</summary>
        protected MapTile[] _buffer;
        /// <summary>Banyak elemen dalam buffer.</summary>
        protected int _bufferLength = -1;
        /// <summary>Ukuran dari grid.</summary>
        protected IVector<int> _size;

        public int Dimension { get; protected set; }
        public IVector<int> Size {
            get {
                return _size;
            }
            set {
                _size = value;
                _bufferLength = -1;
            }
        }
        /// <summary>
        /// Banyak elemen dalam buffer.
        /// </summary>
        protected int BufferLength {
            get {
                if(_bufferLength >= 0) return _bufferLength;
                _bufferLength = 1;
                for(int i = 0; i < Dimension; i++)
                    _bufferLength *= Size[i];
                return _bufferLength;
            }
        }

        /// <summary>
        /// Membentuk sebuah objek <see cref="Map{T}"/> berukuran <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Ukuran dari peta.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Map(IVector<int> size) {
            int dimension = size.Dimension;
            if(dimension <= 0) throw new ArgumentOutOfRangeException();

            Dimension = dimension;
            Size = size;

            _buffer = new MapTile[BufferLength];
        }

        public virtual MapTile this[IVector<int> position] {
            get {
                if(!IsInBounds(position)) throw new IndexOutOfRangeException();
                return _buffer[VecToIndex(position)];
            }
            set {
                if(!IsInBounds(position)) throw new IndexOutOfRangeException();
                _buffer[VecToIndex(position)] = value;
            }
        }

        public bool DimEquals(IDimension other) => Dimension == other.Dimension;


        public bool IsInBounds(IVector<int> position) {
            if(!position.DimEquals(Size)) {
                return false;
            } else {
                for (int i = 0; i < position.Dimension; i++) {
                    int comp = position[i];
                    if (comp < 0 || comp >= Size[i]) {
                        return false;
                    }
                }
                return true;
            }
        }

        public List<MapTile> NeighborsOf(MapTile tile) {
            if(!DimEquals(tile)) throw new ArgumentException();

            List<MapTile> temp = new List<MapTile>();
            IVector<int> pos = tile.Position;
            for(int dim = 0; dim < Dimension; dim++) {
                int orig = pos[dim];
                pos[dim] = orig - 1; if(IsInBounds(pos)) temp.Add(this[pos]);
                pos[dim] = orig + 1; if(IsInBounds(pos)) temp.Add(this[pos]);
                pos[dim] = orig;
            }

            return temp;
        }

        /// <summary>
        /// Mengubah sebuah vektor koordinat elemen menjadi indeks elemen tersebut dalam buffer.
        /// </summary>
        /// <param name="vec">Vektor koordinat elemen yang dituju.</param>
        /// <returns>Indeks elemen yang dituju dalam buffer.</returns>
        /// <exception cref="ArgumentException"/>
        protected int VecToIndex(IVector<int> vec) {
            if(!DimEquals(vec)) throw new ArgumentException();

            int index = vec[0];
            int mult = 1;
            for(int i = 1; i < vec.Dimension; i++) {
                mult *= Size[i - 1];
                index += vec[i] * mult;
            }

            return index;
        }

        /// <summary>
        /// Merepresentasikan sebuah sel dalam <see cref="Map{T}"/> yang menyimpan sebuah nilai.
        /// </summary>
        public class MapTile : IPosition<int> {
            /// <summary>Nilai dari sel ini.</summary>
            public T Value { get ; set; }
            public IVector<int> Position { get; private set; }
            public int Dimension { get => Position.Dimension; }

            /// <summary>
            /// Membentuk sebuah objek <see cref="MapTile{T}"/> bernilai <paramref name="value"/> yang terletak pada <paramref name="value"/>.
            /// </summary>
            /// <param name="value">Nilai dari sel ini.</param>
            /// <param name="position">Posisi dari sel ini.</param>
            public MapTile(T value, IVector<int> position) {
                Value = value;
                Position = position;
            }

            public bool DimEquals(IDimension other) => Dimension == other.Dimension;

            public override string ToString() {
                return Position.ToString() + ": " + Value.ToString();
            }
        }
    }
}
