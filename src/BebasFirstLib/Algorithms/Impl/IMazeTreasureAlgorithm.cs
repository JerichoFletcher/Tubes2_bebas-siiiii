using BebasFirstLib.Structs.Impl;
using BebasFirstLib.Structs;
using System.Collections.Generic;

namespace BebasFirstLib.Algorithms.Impl {
    public interface IMazeTreasureAlgorithm : ISearchAlgorithm<MazeTreasureMap.MapTile, Tree<MazeTreasureMap.MapTile>[]> {
        MazeTreasureMap Maze { get; }
        MazeTreasureMap.MapTile Start { get; set; }
        List<MazeTreasureMap.MapTile> Ignore { get; set; }
    }
}
