using BebasFirstLib.Algorithms;
using BebasFirstLib.Structs;
using System;

namespace BebasFirstTerminal.Algorithms {
    internal class TreeFind<T> : ISearchAlgorithm<T, TreeBase<T>> where T : IComparable {
        TreeBase<T> tree;

        public TreeFind(TreeBase<T> tree) {
            this.tree = tree;
        }

        public TreeBase<T> Search(Predicate<T> successPred) {
            if(successPred(tree.Value)) return tree;
            foreach(TreeBase<T> t in tree) {
                TreeFind<T> subsearch = new TreeFind<T>(t);
                try {
                    TreeBase<T> res = subsearch.Search(successPred);
                    return res;
                } catch(NotFoundException) {
                    
                }
            }
            throw new NotFoundException();
        }

        public class NotFoundException : Exception { }
    }
}
