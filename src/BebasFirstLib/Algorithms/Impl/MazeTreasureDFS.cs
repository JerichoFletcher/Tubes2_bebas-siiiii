using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;
using System;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms.Impl {
    public class MazeTreasureDFS : ISearchAlgorithm<MazeTreasureMap.MapTile, Tree<MazeTreasureMap.MapTile>[]> {
        private MazeTreasureMap maze;
        private MazeTreasureMap.MapTile start;

        public MazeTreasureDFS(MazeTreasureMap maze, MazeTreasureMap.MapTile start = null) {
            if(maze == null) throw new ArgumentNullException();
            if(maze.StartPos == null) throw new ArgumentNullException();
            this.maze = maze;
            this.start = start ?? maze[maze.StartPos];
        }

        public Tree<MazeTreasureMap.MapTile>[] Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            var searchTree = new Tree<MazeTreasureMap.MapTile>(start);
            var searchStack = new Stack<Tree<MazeTreasureMap.MapTile>>();
            var visited = new HashSet<MazeTreasureMap.MapTile>();

            searchStack.Push(searchTree);
            visited.Add(searchTree.Value);

            while(searchStack.Count > 0) {
                searchTree = searchStack.Pop();
                if(successPred(searchTree.Value)) return searchTree.TracePath();

                var neighbors = maze.NeighborsOf(searchTree.Value);
                foreach(var neighbor in neighbors) {
                    if(!visited.Contains(neighbor) && maze.Walkable(neighbor)) {
                        searchStack.Push((Tree<MazeTreasureMap.MapTile>)searchTree.AddChild(neighbor));
                        visited.Add(neighbor);
                    }
                }
            }

            return null;
        }
    }
}
