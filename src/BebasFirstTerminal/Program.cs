using System;
using System.IO;
using BebasFirstLib.Algorithms.Impl;
using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using BebasFirstTerminal.Algorithms;

namespace BebasFirstTerminal {
    internal class Program {
        static void Main(string[] args) {
            if(args.Length != 1) {
                Console.WriteLine("Tidak ada path yang diberikan!");
                return;
            }

            string path = args[0];
            FileStream fs = null;
            try {
                fs = File.OpenRead(path);
                var stream = new StreamReader(fs);

                var defsize = new Vector<int>(2);
                defsize[0] = 1; defsize[1] = 1;

                var map = new MazeTreasureMap(defsize);
                map.Read(stream);

                for(int i = 0; i < map.Size[0]; i++) {
                    for(int j = 0; j < map.Size[1]; j++) {
                        var pos = new Vector<int>(2);
                        pos[0] = i; pos[1] = j;
                        Console.Write(map[pos].ToString());
                        if(j < map.Size[1] - 1) Console.Write(" ");
                    }
                    Console.WriteLine();
                }

                var bfs = new MazeTreasureBFS(map);
                var dfs = new MazeTreasureDFS(map);
                var trv = new MazeTreasureTraversal<MazeTreasureBFS>(bfs, true);

                var treepath = trv.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.Treasure);
                foreach(var t in treepath) {
                    Console.WriteLine(t.Value.ToString());
                }
                
            } catch(FileNotFoundException e) {
                Console.WriteLine("File tidak ada!");
                Console.WriteLine(e.ToString());
            } catch(Exception e) {
                Console.WriteLine("Ah ngaco bet si lu:");
                Console.WriteLine(e.ToString());
            } finally {
                fs?.Close();
            }

            // PAUSE
            Console.Write("ENTER untuk melanjutkan...");
            Console.ReadLine();
        }
    }
}
