using System;
using System.Data;
using System.IO;
using BebasFirstLib.Algorithms.Impl;
using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;

namespace BebasFirstTerminal {
    internal class Program {
        static void PrintMap(MazeTreasureMap map) {
            for(int i = 0; i < map.Size[0]; i++) {
                for(int j = 0; j < map.Size[1]; j++) {
                    var pos = new Vector<int>(2);
                    pos[0] = i; pos[1] = j;

                    var t = map[pos];
                    switch(t.Value) {
                        case MazeTreasureMap.MazeTileType.Walkable:
                            Console.Write(' ');
                            break;
                        case MazeTreasureMap.MazeTileType.Obstacle:
                            Console.Write('X');
                            break;
                        case MazeTreasureMap.MazeTileType.Treasure:
                            Console.Write('T');
                            break;
                        case MazeTreasureMap.MazeTileType.KrustyKrabs:
                            Console.Write('K');
                            break;
                    }
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

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

                PrintMap(map);

                var bfs = new MazeTreasureBFS(map);
                var dfs = new MazeTreasureDFS(map);
                var trv = new MazeTreasureTraversal<MazeTreasureDFS>(dfs, true);

                int d = 1;
                Tree<MazeTreasureMap.MapTile>[] treepath = null;
                foreach(var i in trv.Search(_ => true)) {
                    if(i.Found) treepath = i.Value;
                    Console.WriteLine($"[{d++}] Visited {i.Visited}, found: {i.Found}");
                }

                if(treepath != null) {
                    foreach(var t in treepath) {
                        Console.WriteLine(t.Value);
                    }
                } else {
                    Console.WriteLine("Tidak ditemukan:(");
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
