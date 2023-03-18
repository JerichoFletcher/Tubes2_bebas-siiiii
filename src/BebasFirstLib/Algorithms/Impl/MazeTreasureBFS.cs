using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using System;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms.Impl {
    public class MazeTreasureBFS : IMazeTreasureAlgorithm {
        public MazeTreasureMap Maze { get; private set; }
        public MazeTreasureMap.MapTile Start { get; set; }
        public List<MazeTreasureMap.MapTile> Ignore { get; set; }

        public MazeTreasureBFS(MazeTreasureMap maze, MazeTreasureMap.MapTile start = null) {
            if(maze == null) throw new ArgumentNullException();
            if(maze.StartPos == null) throw new ArgumentNullException();
            Maze = maze;
            Start = start ?? maze[maze.StartPos];
            Ignore = new List<Map<MazeTreasureMap.MazeTileType>.MapTile>();
        }

        public IEnumerable<MazeTreasureSearchStep> Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            var searchTree = new Tree<MazeTreasureMap.MapTile>(Start);
            var searchQueue = new Queue<Tree<MazeTreasureMap.MapTile>>();
            var visited = new HashSet<MazeTreasureMap.MapTile>();
            foreach(var i in Ignore) visited.Add(i);

            var searchRoot = searchTree;

            searchQueue.Enqueue(searchTree);
            visited.Add(searchTree.Value);

            while(searchQueue.Count > 0) {
                searchTree = searchQueue.Dequeue();
                if(successPred(searchTree.Value)) {
                    var path = searchTree.TracePath();
                    yield return new MazeTreasureSearchStep(path, true, searchTree.Value, searchRoot);
                    break;
                }
                yield return new MazeTreasureSearchStep(null, false, searchTree.Value, searchRoot);

                var neighbors = Maze.NeighborsOf(searchTree.Value);
                foreach(var neighbor in neighbors) {
                    if(!visited.Contains(neighbor) && MazeTreasureMap.Walkable(neighbor)) {
                        searchQueue.Enqueue((Tree<MazeTreasureMap.MapTile>)searchTree.AddChild(neighbor));
                        visited.Add(neighbor);
                    }
                }
            }
        }
    }
}
