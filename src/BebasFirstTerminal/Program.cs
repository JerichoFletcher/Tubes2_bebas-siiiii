using System;
using BebasFirstLib.Structs;

namespace BebasFirstTerminal {
    internal class Program {
        static void PrintRecurseTree<T>(TreeBase<T> tree) {
            Console.Write("(" + tree.Value.ToString());
            foreach(var child in tree) {
                PrintRecurseTree(child);
            }
            Console.Write(")");
        }

        static void Main(string[] args) {
            TreeBase<int>
                tree1 = new Tree<int>(1),
                tree2 = tree1.AddChild(2),
                tree3 = tree1.AddChild(3),
                tree4 = tree2.AddChild(4),
                tree5 = tree3.AddChild(5),
                tree6 = tree5.AddChild(6),
                tree7 = tree2.AddChild(7);

            PrintRecurseTree(tree1);
            Console.WriteLine();
            Console.ReadLine();
            // Lakukan test pencarian di sini, output ke terminal
        }
    }
}
