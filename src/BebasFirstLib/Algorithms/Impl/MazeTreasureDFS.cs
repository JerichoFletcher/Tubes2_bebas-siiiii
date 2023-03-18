using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using System;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms.Impl {
    public class MazeTreasureDFS : IMazeTreasureAlgorithm {
        public MazeTreasureMap Maze { get; private set; }
        public MazeTreasureMap.MapTile Start { get; set; }
        public List<MazeTreasureMap.MapTile> Ignore { get; set; }

        public MazeTreasureDFS(MazeTreasureMap maze, MazeTreasureMap.MapTile start = null) {
            if(maze == null) throw new ArgumentNullException();
            if(maze.StartPos == null) throw new ArgumentNullException();
            Maze = maze;
            Start = start ?? maze[maze.StartPos];
            Ignore = new List<Map<MazeTreasureMap.MazeTileType>.MapTile>();
        }

        public IEnumerable<MazeTreasureSearchStep> Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            var searchTree = new Tree<MazeTreasureMap.MapTile>(Start);
            var searchStack = new Stack<Tree<MazeTreasureMap.MapTile>>();
            var visited = new HashSet<MazeTreasureMap.MapTile>();
            foreach(var i in Ignore) visited.Add(i);

            var searchRoot = searchTree;

            searchStack.Push(searchTree);
            visited.Add(searchTree.Value);

            while(searchStack.Count > 0) {
                searchTree = searchStack.Pop();
                if(successPred(searchTree.Value)) {
                    var path = searchTree.TracePath();
                    yield return new MazeTreasureSearchStep(path, true, searchTree.Value, searchRoot);
                    break;
                }
                yield return new MazeTreasureSearchStep(null, false, searchTree.Value, searchRoot);

                var neighbors = Maze.NeighborsOf(searchTree.Value);
                foreach(var neighbor in neighbors) {
                    if(!visited.Contains(neighbor) && MazeTreasureMap.Walkable(neighbor)) {
                        searchStack.Push((Tree<MazeTreasureMap.MapTile>)searchTree.AddChild(neighbor));
                        visited.Add(neighbor);
                    }
                }
            }
        }
    }
}
