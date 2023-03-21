using BebasFirstLib.IO;
using System;
using System.IO;
using System.Collections.Generic;
using System.CodeDom;

namespace BebasFirstLib.Structs.Impl {
    /// <summary>
    /// Merepresentasikan sebuah peta terbatas dari persoalan Maze Treasure Hunt.
    /// </summary>
    public class MazeTreasureMap : Map<MazeTreasureMap.MazeTileType>, IPersistent {
        /// <summary>Dimensi dari peta.</summary>
        private const int MAZE_DIMENSION = 2;

        /// <summary>
        /// Membentuk sebuah objek <see cref="MazeTreasureMap"> kosong.
        /// </summary>
        /// <inheritdoc cref="Map{T}.Map(IVector{int})"/>
        public MazeTreasureMap() : this(Vector<int>.From(0, 0)) { }

        /// <summary>
        /// Membentuk sebuah objek <see cref="MazeTreasureMap"/> berukuran <paramref name="size"/>.
        /// </summary>
        /// <inheritdoc cref="Map{T}.Map(IVector{int})"/>
        public MazeTreasureMap(IVector<int> size) : base(size) {
            if(size.Dimension != MAZE_DIMENSION) throw new ArgumentException();
            StartPos = null;
            Treasures = new List<IVector<int>>();
        }

        public IVector<int> StartPos { get; private set; }
        public List<IVector<int>> Treasures { get; private set; }

        public static bool Walkable(MapTile tile) {
            return tile.Value != MazeTileType.Obstacle;
        }

        /// <summary>
        /// Mengakses sel peta pada posisi yang diberikan.
        /// </summary>
        /// <param name="position">Posisi sel yang ingin diakses.</param>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <returns>Sel pada posisi yang diberikan.</returns>
        public override MapTile this[IVector<int> position] {
            get => base[position];
            set {
                if(value.Value == MazeTileType.KrustyKrabs && StartPos != null) throw new InvalidOperationException();

                if(value.Value == MazeTileType.KrustyKrabs) StartPos = position;
                if(value.Value == MazeTileType.Treasure) Treasures.Add(position);
                base[position] = value;
            }
        }

        /// <exception cref="InvalidDataException"/>
        /// <inheritdoc cref="IPersistent.Read(FileStream)"/>
        public void Read(StreamReader fs) {
            StartPos = null;
            Treasures.Clear();
            List<MapTile> temp = new List<MapTile>();

            int x = 0, y = 0;
            while(!fs.EndOfStream) {
                string line = fs.ReadLine();
                if(line == null) break;

                y = 0;
                string[] split = line.Split(' ');
                foreach(string s in split) {
                    if(s.Length != 1) throw new InvalidDataException();
                    Vector<int> pos = Vector<int>.From(x, y);
                    temp.Add(new MapTile(MazeTileType.From(s[0]) ?? throw new InvalidDataException(), pos));
                    y++;
                }
                x++;
            }

            Size = Vector<int>.From(x, y);

            try {
                Array.Resize(ref _buffer, BufferLength);
                foreach(MapTile tile in temp)
                    this[tile.Position] = tile;
            } catch(IndexOutOfRangeException) {
                throw new InvalidDataException();
            }

            if(StartPos == null) throw new InvalidCastException();
        }

        public void Write(StreamWriter fs) {
            for(int i = 0; i < Size[0]; i++) {
                for(int j = 0; j < Size[1]; j++) {
                    Vector<int> pos = Vector<int>.From(i, j);
                    fs.Write(this[pos]);
                    if(j < Size[1] - 1) fs.Write(' ');
                }
                fs.WriteLine();
            }
        }

        /// <summary>
        /// Merepresentasikan tipe sel dalam peta.
        /// </summary>
        public struct MazeTileType {
            public static readonly MazeTileType
                Walkable = new MazeTileType('R'),
                Obstacle = new MazeTileType('X'),
                Treasure = new MazeTileType('T'),
                KrustyKrabs = new MazeTileType('K');
            public static readonly MazeTileType[] Values = new MazeTileType[] { Walkable, Obstacle, Treasure, KrustyKrabs };

            /// <summary>Nilai karakter dari tipe sel ini.</summary>
            public char Char { get; private set; }

            private MazeTileType(char ch) {
                Char = ch;
            }

            /// <summary>
            /// Memetakan sebuah karakter ke <see cref="MazeTileType"/> yang bersesuaian.
            /// </summary>
            /// <param name="ch">Karakter yang dipetakan.</param>
            /// <returns>
            ///     Objek <see cref="MazeTileType"/> yang bernilai <paramref name="ch"/>;
            ///     <see langword="null"/> jika tidak ada.
            /// </returns>
            public static MazeTileType? From(char ch) {
                switch(ch) {
                    case 'R': return Walkable;
                    case 'X': return Obstacle;
                    case 'T': return Treasure;
                    case 'K': return KrustyKrabs;
                    default: return null;
                }
            }

            public override bool Equals(object obj) {
                return obj is MazeTileType type && Char.Equals(type.Char);
            }

            public override int GetHashCode() {
                return Char.GetHashCode();
            }

            public override string ToString() {
                switch(Char) {
                    case 'R': return "Walkable";
                    case 'X': return "Obstacle";
                    case 'T': return "Treasure";
                    default: return "KrustyKrabs"; // Char == 'K'
                }
            }

            public static bool operator ==(MazeTileType t1, MazeTileType t2) {
                return t1.Char == t2.Char;
            }

            public static bool operator !=(MazeTileType t1, MazeTileType t2) {
                return t1.Char != t2.Char;
            }
        }
    }
}
