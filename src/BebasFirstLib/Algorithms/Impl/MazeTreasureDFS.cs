using BebasFirstLib.Structs.Impl;
using System;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms.Impl {
    public class MazeTreasureDFS : ISearchAlgorithm<MazeTreasureMap.MapTile, MazeTreasureMap.MapTile[]> {
        private MazeTreasureMap theMaze;
        public MazeTreasureDFS(MazeTreasureMap maze) {
            theMaze = maze;
        }

        public MazeTreasureMap.MapTile[] Search(Predicate<MazeTreasureMap.MapTile> successPred) {
            throw new NotImplementedException();
        }
    }
}
