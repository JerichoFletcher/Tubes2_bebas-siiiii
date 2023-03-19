using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using System;
using System.Collections.Generic;
using System.Xml;
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
        private bool finished;

        public MazeTreasureTraversal(T algorithm, bool returnToStart = false) {
            Algorithm = algorithm;
            Maze = algorithm.Maze;
            Start = algorithm.Start;
            ReturnToStart = returnToStart;

            Ignore = new List<Map<MazeTreasureMap.MazeTileType>.MapTile>();
            treasureTiles = new HashSet<MazeTreasureMap.MapTile>();
            foreach(var pos in Maze.Treasures)
                treasureTiles.Add(Maze[pos]);
            finished = false;
        }

        public IEnumerable<MazeTreasureSearchStep> Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            var finalPath = new List<Tree<MazeTreasureMap.MapTile>>();
            var ignore = new List<MazeTreasureMap.MapTile>();
            foreach(var i in Ignore) ignore.Add(i);
            var currentStart = Start;

            while(TreasureCount > 0) {
                Algorithm.Start = currentStart;
                Algorithm.Ignore = ignore;
                Tree<MazeTreasureMap.MapTile>[] path = null;

                foreach(var i in Algorithm.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.Treasure && treasureTiles.Contains(tile))) {
                    path = i.Value;
                    if(!i.Found) {
                        var ret = i;
                        ret.Found = false;
                        yield return i;
                    }
                }

                if(path == null) {
                    var oldIgnore = ignore[0];
                    ignore.Clear();
                    foreach(var t in Algorithm.Maze.NeighborsOf(currentStart))
                        if(MazeTreasureMap.Walkable(t) && t != oldIgnore) ignore.Add(t);
                    Algorithm.Ignore = ignore;

                    foreach(var i in Algorithm.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.Treasure && treasureTiles.Contains(tile))) {
                        path = i.Value;
                        if(!i.Found) {
                            var ret = i;
                            ret.Found = false;
                            yield return i;
                        }
                    }

                    if(path == null) {
                        yield break;
                    }
                }

                if(path?.Length > 0) {
                    var reached = path[path.Length - 1];
                    treasureTiles.Remove(reached.Value);

                    if(finalPath.Count > 0) finalPath.RemoveAt(finalPath.Count - 1);
                    finalPath.AddRange(path);

                    currentStart = reached.Value;
                    ignore.Clear();
                    if(!reached.IsRoot()) ignore.Add(reached.Parent.Value);

                    finished = !ReturnToStart && TreasureCount == 0;
                    yield return new MazeTreasureSearchStep(finished ? finalPath.ToArray() : null, finished, reached.Value, null);
                }
            }

            if(ReturnToStart) {
                Algorithm.Start = currentStart;
                Algorithm.Ignore = ignore;
                Tree<MazeTreasureMap.MapTile>[] path = null;

                foreach(var i in Algorithm.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.KrustyKrabs)) {
                    path = i.Value;
                    if(!i.Found) {
                        var ret = i;
                        ret.Found = false;
                        yield return i;
                    }
                }

                if(path == null) {
                    var oldIgnore = ignore[0];
                    ignore.Clear();
                    foreach(var t in Algorithm.Maze.NeighborsOf(currentStart))
                        if(MazeTreasureMap.Walkable(t) && t != oldIgnore) ignore.Add(t);
                    Algorithm.Ignore = ignore;

                    foreach(var i in Algorithm.Search(tile => tile.Value == MazeTreasureMap.MazeTileType.KrustyKrabs)) {
                        path = i.Value;
                        if(!i.Found) {
                            var ret = i;
                            ret.Found = false;
                            yield return i;
                        }
                    }

                    if(path == null) {
                        yield break;
                    }
                }

                if(path?.Length > 0) {
                    var reached = path[path.Length - 1];

                    if(finalPath.Count > 0) finalPath.RemoveAt(finalPath.Count - 1);
                    finalPath.AddRange(path);

                    yield return new MazeTreasureSearchStep(finalPath.ToArray(), true, reached.Value, null);
                }
            }
        }
    }
}
