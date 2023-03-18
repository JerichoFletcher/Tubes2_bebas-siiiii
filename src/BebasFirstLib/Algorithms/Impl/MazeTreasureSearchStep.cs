using BebasFirstLib.Structs;
using BebasFirstLib.Structs.Impl;

namespace BebasFirstLib.Algorithms.Impl {
    public struct MazeTreasureSearchStep : ISearchStepInfo<Tree<MazeTreasureMap.MapTile>[]> {
        public Tree<MazeTreasureMap.MapTile>[] Value { get; private set; }
        public bool Found { get; set; }

        public MazeTreasureMap.MapTile Visited { get; private set; }
        public Tree<MazeTreasureMap.MapTile> SearchTree { get; private set; }

        public MazeTreasureSearchStep(Tree<MazeTreasureMap.MapTile>[] value, bool found, MazeTreasureMap.MapTile visited, Tree<MazeTreasureMap.MapTile> searchTree) {
            Value = value;
            Found = found;
            Visited = visited;
            SearchTree = searchTree;
        }
    }
}
