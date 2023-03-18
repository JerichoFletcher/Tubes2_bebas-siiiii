using BebasFirstLib.Structs;

namespace BebasFirstVisualize {
    internal static class Helper {
        public static Vector<T> VectorFrom<T>(params T[] values) {
            Vector<T> v = new Vector<T>(values.Length);
            for(int i = 0; i < values.Length; i++) {
                v[i] = values[i];
            }
            return v;
        }
    }
}
