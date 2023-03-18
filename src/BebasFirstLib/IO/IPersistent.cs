using System.IO;

namespace BebasFirstLib.IO {
    /// <summary>
    /// Menyediakan metode-metode bagi sebuah kelas untuk membentuk objek dari file dan menyimpannya ke dalam file.
    /// </summary>
    public interface IPersistent {
        /// <summary>
        /// Membaca data dari file dan membentuk objek ini.
        /// </summary>
        /// <param name="fs"><see cref="FileStream"/> berisi data yang akan dibaca.</param>
        void Read(FileStream fs);

        /// <summary>
        /// Menulis data objek ini ke dalam file.
        /// </summary>
        /// <param name="fs"><see cref="FileStream"/> dari file yang akan ditulis.</param>
        void Write(FileStream fs);
    }
}
