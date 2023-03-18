using BebasFirstLib.Structs.Traits;
using BebasFirstLib.IO;
using System;

namespace BebasFirstLib.Structs {
    /// <summary>
    /// Merepresentasikan sebuah peta terbatas dengan dimensi tertentu, menggunakan sistem koordinat bilangan bulat.
    /// </summary>
    /// <typeparam name="T">Tipe data dari nilai setiap sel dalam peta.</typeparam>
    public class Map<T> : IGridMap<int, Map<T>.MapTile> {
        /// <summary>Buffer internal berisi semua sel pada peta.</summary>
        protected MapTile[] _buffer;

        public int Dimension { get; protected set; }
        public IVector<int> Size { get; protected set; }
        
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

        public MapTile this[IVector<int> position] {
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
                    if (comp < 0 || comp > Size[i] - 1) {
                        return false;
                    }
                }   
                return true;
            }   
        }

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

        protected int BufferLength {
            get {
                int bufferLength = 1;
                for(int i = 0; i < Dimension; i++)
                    bufferLength *= Size[i];
                return bufferLength;
            }
        }

        /// <summary>
        /// Merepresentasikan sebuah sel dalam <see cref="Map{T}"/> yang menyimpan sebuah nilai.
        /// </summary>
        public struct MapTile : IPosition<int> {
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
        }
    }
}
