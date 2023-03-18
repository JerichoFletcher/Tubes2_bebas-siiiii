using BebasFirstLib.IO;
using System;
using System.IO;
using System.Collections.Generic;

namespace BebasFirstLib.Structs.Impl {
    /// <summary>
    /// Merepresentasikan sebuah peta terbatas dari persoalan Maze Treasure Hunt.
    /// </summary>
    public class MazeTreasureMap : Map<MazeTreasureMap.MazeTileType>, IPersistent {
        private const int MAZE_DIMENSION = 2;
        private const char
            CHAR_KRUSTY_KRAB = 'K',
            CHAR_TREASURE = 'T',
            CHAR_TRAVERSABLE = 'R',
            CHAR_OBSTACLE = 'X';
        private static char[] VALID_CHARS = { CHAR_TRAVERSABLE, CHAR_OBSTACLE, CHAR_TREASURE, CHAR_KRUSTY_KRAB };

        /// <summary>
        /// Membentuk sebuah objek <see cref="MazeTreasureMap{T}"/> berukuran <paramref name="size"/>.
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

        public static MazeTileType TypeOf(char c) {
            switch(c) {
                case CHAR_TRAVERSABLE: return MazeTileType.Walkable;
                case CHAR_OBSTACLE: return MazeTileType.Obstacle;
                case CHAR_TREASURE: return MazeTileType.Treasure;
                case CHAR_KRUSTY_KRAB: return MazeTileType.KrustyKrabs;
                default: throw new ArgumentException();
            }
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

        private bool IsValidChar(char c) {
            foreach(char v in VALID_CHARS)
                if(v == c) return true;
            return false;
        }

        /// <exception cref="InvalidDataException"/>
        /// <inheritdoc cref="IPersistent.Read(FileStream)"/>
        public void Read(StreamReader fs) {
            List<MapTile> temp = new List<MapTile>();
            Vector<int> size = new Vector<int>(MAZE_DIMENSION);

            int x = 0, y = 0;
            while(!fs.EndOfStream) {
                string line = fs.ReadLine();
                if(line == null) break;

                y = 0;
                string[] split = line.Split(' ');
                foreach(string s in split) {
                    if(s.Length != 1 || !IsValidChar(s[0])) throw new InvalidDataException();
                    Vector<int> pos = new Vector<int>(MAZE_DIMENSION);
                    pos[0] = x; pos[1] = y;
                    temp.Add(new MapTile(TypeOf(s[0]), pos));
                    y++;
                }
                x++;
            }

            size[0] = x; size[1] = y;
            Size = size;

            Array.Resize(ref _buffer, BufferLength);
            foreach(MapTile tile in temp)
                this[tile.Position] = tile;
        }

        public void Write(StreamWriter fs) {
            for(int i = 0; i < Size[0]; i++) {
                for(int j = 0; j < Size[1]; j++) {
                    Vector<int> pos = new Vector<int>(MAZE_DIMENSION);
                    pos[0] = i; pos[1] = j;
                    fs.Write(this[pos]);
                    if(j < Size[1] - 1) fs.Write(' ');
                }
                fs.WriteLine();
            }
        }

        public enum MazeTileType {
            Walkable = 0,
            Obstacle = 1,
            Treasure = 2,
            KrustyKrabs = 3
        }
    }
}
