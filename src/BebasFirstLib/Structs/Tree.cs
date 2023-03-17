using BebasFirstLib.Structs.Traits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BebasFirstLib.Structs {
    /// <inheritdoc cref="TreeBase{T}"/>
    /// <remarks>
    /// <see cref="Tree{T}"/> diimplementasi menggunakan <see cref="HashSet{T}"/> untuk mencegah duplikasi simpul anak.
    /// </remarks>
    public class Tree<T> : TreeBase<T>, IBacktrackable<Tree<T>> {
        /// <summary>Wadah untuk menyimpan simpul-simpul anak.</summary>
        protected HashSet<Tree<T>> children;

        /// <summary>
        /// Membentuk simpul baru dengan nilai <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        public Tree(T value) {
            Value = value;
            Parent = null;
            children = new HashSet<Tree<T>>();
        }

        public override int ChildCount { get => children.Count; }

        /// <summary>Simpul orangtua dari simpul ini.</summary>
        public Tree<T> Parent { get; private set; }

        public override TreeBase<T> AddChild(T value) {
            Tree<T> child = new Tree<T>(value) {
                Parent = this
            };
            children.Add(child);
            return child;
        }

        /// <summary>
        /// Membuang simpul anak <paramref name="node"/> dari simpul ini.
        /// </summary>
        /// <param name="node">Simpul yang dibuang.</param>
        /// <returns>
        ///     <see langword="true"/> jika <paramref name="node"/> berhasil dihapus;
        ///     <see langword="false"/> jika gagal atau <paramref name="node"/> bukan simpul anak dari simpul ini.
        /// </returns>
        public virtual bool RemoveChild(Tree<T> node) {
            bool removed = children.Remove(node);
            if(removed)Parent = null;
            return removed;
        }

        public override TreeBaseEnumerator GetEnumerator() {
            return new TreeEnumerator(this);
        }

        public virtual bool IsRoot() {
            return Parent == null;
        }

        public virtual Tree<T>[] TracePath() {
            List<Tree<T>> nodes = new List<Tree<T>>();

            Tree<T> node = this;
            for(; !node.IsRoot(); node = node.Parent) nodes.Add(node);
            nodes.Add(node);

            nodes.Reverse();
            return nodes.ToArray();
        }

        /// <remarks>
        /// Enumerator ini menunjuk ke objek bertipe <see cref="Tree{T}"/>.
        /// </remarks>
        /// <inheritdoc cref="TreeBase{T}.TreeBaseEnumerator"/>
        public class TreeEnumerator : TreeBaseEnumerator {
            protected List<Tree<T>> children;
            protected int currentIndex;

            /// <summary>
            /// Membentuk sebuah objek <see cref="TreeEnumerator"/> yang menunjuk ke simpul-simpul anak dari <paramref name="tree"/>.
            /// </summary>
            /// <param name="tree">Objek <see cref="Tree{T}"/> yang akan dienumerasi simpul-simpul anaknya.</param>
            public TreeEnumerator(Tree<T> tree) {
                children = tree.children.ToList();
                Reset();
            }

            /// <exception cref="InvalidOperationException"/>
            /// <inheritdoc cref="TreeBase{T}.TreeBaseEnumerator.Current"/>
            public override TreeBase<T> Current {
                get {
                    try {
                        return children[currentIndex];
                    } catch(IndexOutOfRangeException) {
                        throw new InvalidOperationException();
                    }
                }
            }

            public override bool MoveNext() {
                ++currentIndex;
                return currentIndex < children.Count;
            }

            public override void Reset() {
                currentIndex = -1;
            }
        }
    }
}
