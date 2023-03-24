# Tugas Besar II bebas-siiiii
Tugas Besar II <br> IF2211 Strategi Algoritma Semester II Tahun 2022/2023 <br>Implementasi BFS dan DFS dalam Maze Tree

## Daftar Isi
* [Penjelasan Ringkas Program](#penjelasan-ringkas-program)
* [Penjelasam Algoritma](#penjelasan-algoritma)
* [Requirement](#requirement)
* [Cara Menjalankan Program](#cara-menjalankan-program)
* [Struktur Program](#struktur-program)
* [Kontributor](#kontributor)

## Penjelasan Ringkas Program
Implementasi BFS dan DFS dalam Maze Treasure Hunt. Program ini dibuat untuk mencari sebuah treasure dalam sebuah maze tentu saja dengan menerapkan BFS, DFS, serta TSP untuk proses kembali ke start. Program ini diimplementasikan dengan bahasa C# dengan GUI sederhana yang dibuat dengan Unity. 


## Penjelasan Algoritma
Dalam menyelesaikan program ini algoritma yang digunakan yaitu BFS dan DFS. 
* Algoritma BFS 
  Algoritma Breadth First Search merupakan algoritma traversal pencarian simpul-simpul pada graf secara melebar. Adapun metode BFS secara umum adalah sebagai berikut: Kunjungi simpul v kemudian Kunjungi semua simpul yang bertetangga dengan simpul v terlebih dahulu.Setelah itu Kunjungi simpul yang belum dikunjungi dan bertetangga dengan simpul-simpul yang tadi dikunjungi, demikian seterusnya. Dalam penyelesaiannya algoritma ini menggunakan struktur data Queue. Adapun penerapannya dalam kasus Maze Treasure Hunt adalah algoritma BFS ini akan mencari satu treasure hunt kemudian akan dipanggil kembali dengan start adalah posisi treasure hunt yang terakhir kali ditemukan. Jika treasure hunt sudah habis atau sudah tidak ada jalan lain program selesai. 
* Algoritma DFS
  Algoritma Depth First Search merupakan algoritma traversal yang mencari simpul-simpul pada graf secara mendalam. Adapun metode DFS secara umum secara rekursif adalah sebagai berikut Kunjungi simpul v, lalu Kunjungi simpul w yang bertetangga dengan simpul v. Setelah itu, ulangi DFS mulai dari simpul w. Ketika mencapai simpul u sedemikian sehingga semua simpul yang bertetangga dengannya telah dikunjungi, pencarian dirunut-balik (backtrack) ke simpul terakhir yang dikunjungi sebelumnya dan mempunyai simpul w yang belum dikunjungi.Pencarian berakhir bila tidak ada lagi simpul yang belum dikunjungi yang dapat dicapai dari simpul yang telah dikunjungi. Dalam penyelesaiannya algoritma ini menggunakan struktur data Stack. Adapun penerapannya dalam kasus Maze Treasure Hunt adalah algoritma DFS ini akan mencari satu treasure hunt kemudian akan dipanggil kembali dengan start adalah posisi treasure hunt yang terakhir kali ditemukan. Jika treasure hunt sudah habis atau sudah tidak ada jalan lain, program selesai. 


## Requirement
### Running
* C# .NET framework 4.8

### Building
* Visual Studio 2022
* Unity 2022.1.2f1 (or later)


## Cara Menjalankan Program
* Download seluruh program pada [repo ini](https://github.com/JerichoFletcher/Tubes2_bebas-siiiii), kemudian jangan lupa untuk di-extract.
* Buatlah sebuah file txt dimana di dalamnya terkandung peta yang ingin di test. 
* Jangan lupa harus ada karakter K  jika tidak maka tidak akan ada penelusuran yang dilakukan serta pastikan isi file hanya mengandung X, T, R, K.
* Setelah membuat file txt untuk di test, file ini boleh disimpan di folder test kemudian jangan lupa copy properties dari file itu/alamat direktori file itu.
* Setelah di copy pergi ke folder bin kemudian klik GUIApp dan klik BebasFirstVisualize.
* Kemudian inputkan alamat direktori di bagian button yang tertulis ‘Enter text’.
* Kemudian tekan load file akan muncul hasil konversi dari teks ke map.
* Kemudian pilih BFS atau DFS jika ingin merubah DFS ke BFS atau sebaliknya klik tombol panah ke arah bawah. 
* Jika ingin proses kembali ke start awal centang  ‘Return to Krusty Krab’.
* Kemudian tekan Start Search untuk memulai pencarian.


## Struktur Program
```
└───Tucil2_13521077_13521115
    ├───bin
    │   ├───Debug
    |   |    ├───BebasFirstLib.dll
    │   │    └───BebasFIrstTerminal.exe
    │   │    
    │   └───GUIApp
    │        ├───BebasFirstVisualize_Data
    │        ├───MonoBleedingEdge
    │        ├───BebasFirstVisualize.exe
    │        ├───UnityCrashHandler64.exe
    │        └───UnityPlayer.dll
    ├───doc
    │   └───bebas-siiiii.pdf
    ├───src
    │   ├───BebasFirstLib
    │   │    ├───Algorithms
    │   │    ├───IO
    │   │    ├───Properties 
    |   |    └───Struct
    │   │
    │   ├───BebasFirstTerminal
    │   ├───BebasFirstVisualize
    │   └───BebasFirstSearch.sln
    │
    ├───test
    │
    └───README.md  
```

## Kontributor
| NIM      | Nama Anggota              |
| -------- | ------------------------- |
| 13521107 | Jericho Russel Sebastian  |
| 13521115 | Shelma Salsabila          |
| 13521131 | Jeremya Dharmawan Raharjo |
