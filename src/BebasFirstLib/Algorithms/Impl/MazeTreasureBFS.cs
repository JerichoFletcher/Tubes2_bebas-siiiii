using BebasFirstLib.Structs.Impl;
using BebasFirstLib.Structs;
using System;
using System.Collections.Generic;
using static BebasFirstLib.Structs.Map<T>;

namespace BebasFirstLib.Algorithms.Impl {
    public class MazeTreasureBFS : ISearchAlgorithm<MazeTreasureMap.MapTile, Tree<MazeTreasureMap.MapTile>[]> {
        private MazeTreasureMap maze;
        private MazeTreasureMap.MapTile start;

        public MazeTreasureBFS(MazeTreasureMap maze, MazeTreasureMap.MapTile start = null) {
            if(maze == null) throw new ArgumentNullException();
            if(maze.StartPos == null) throw new ArgumentNullException();
            this.maze = maze;
            this.start = start ?? maze[maze.StartPos];
        }

        public Tree<MazeTreasureMap.MapTile>[] Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            var searchTree = new Tree<MazeTreasureMap.MapTile>(start);
            var searchQueue = new Queue<Tree<MazeTreasureMap.MapTile>>();
            var visited = new HashSet<MazeTreasureMap.MapTile>();

            searchQueue.Enqueue(searchTree);
            visited.Add(searchTree.Value);

            while(searchQueue.Count > 0) {
                searchTree = searchQueue.Dequeue();
                if(successPred(searchTree.Value)) return searchTree.TracePath();

                var neighbors = maze.NeighborsOf(searchTree.Value);
                foreach(var neighbor in neighbors) {
                    if(!visited.Contains(neighbor) && maze.Walkable(neighbor)) {
                        searchQueue.Enqueue((Tree<MazeTreasureMap.MapTile>)searchTree.AddChild(neighbor));
                        visited.Add(neighbor);
                    }
                }
            }

            return null;
        }
    }
}
