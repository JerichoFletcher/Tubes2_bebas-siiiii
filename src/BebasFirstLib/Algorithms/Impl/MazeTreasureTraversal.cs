using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace BebasFirstLib.Algorithms.Impl {
    public class MazeTreasureTraversal<T> : IMazeTreasureAlgorithm where T : IMazeTreasureAlgorithm {
        public MazeTreasureMap Maze { get; private set; }
        public MazeTreasureMap.MapTile Start { get; set; }
        public List<MazeTreasureMap.MapTile> Ignore { get; set; }

        public T Algorithm { get; private set; }
        public int TreasureCount { get => treasureTiles.Count; }
        public bool ReturnToStart { get; private set; }

        private HashSet<MazeTreasureMap.MapTile> treasureTiles;

        public MazeTreasureTraversal(T algorithm, bool returnToStart = false) {
            Algorithm = algorithm;
            Maze = algorithm.Maze;
            Start = algorithm.Start;
            ReturnToStart = returnToStart;

            Ignore = new List<Map<MazeTreasureMap.MazeTileType>.MapTile>();
            treasureTiles = new HashSet<MazeTreasureMap.MapTile>();
            foreach(var pos in Maze.Treasures)
                treasureTiles.Add(Maze[pos]);
        }

        public Tree<MazeTreasureMap.MapTile>[] Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            var finalPath = new List<Tree<MazeTreasureMap.MapTile>>();
            var ignore = new List<MazeTreasureMap.MapTile>();
            foreach(var i in Ignore) ignore.Add(i);
            var currentStart = Start;

            while(TreasureCount > 0) {
                Algorithm.Start = currentStart;
                Algorithm.Ignore = ignore;
                var path = Algorithm.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.Treasure && treasureTiles.Contains(tile));

                if(path == null) {
                    var oldIgnore = ignore[0];
                    ignore.Clear();
                    foreach(var t in Algorithm.Maze.NeighborsOf(currentStart))
                        if(MazeTreasureMap.Walkable(t) && t != oldIgnore) ignore.Add(t);
                    Algorithm.Ignore = ignore;
                    path = Algorithm.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.Treasure && treasureTiles.Contains(tile));
                }

                if(path?.Length > 0) {
                    var reached = path[path.Length - 1];
                    treasureTiles.Remove(reached.Value);

                    if(finalPath.Count > 0) finalPath.RemoveAt(finalPath.Count - 1);
                    finalPath.AddRange(path);

                    currentStart = reached.Value;
                    ignore.Clear();
                    if(!reached.IsRoot()) ignore.Add(reached.Parent.Value);
                }
            }

            if(ReturnToStart) {
                Algorithm.Start = currentStart;
                var path = Algorithm.Search(tile => tile == Start);

                if(path?.Length > 0) {
                    var reached = path[path.Length - 1];

                    if(finalPath.Count > 0) finalPath.RemoveAt(finalPath.Count - 1);
                    finalPath.AddRange(path);
                }
            }

            return finalPath.ToArray();
        }
    }
}
