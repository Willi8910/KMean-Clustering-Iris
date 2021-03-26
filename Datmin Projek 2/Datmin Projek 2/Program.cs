using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datmin_Projek_2
{
    class Program
    {
        static int iteration = 0;
        static int num = 0; //ini menunjukka ini iterasi ke berapa
        static void Main(string[] args)
        {
            Dictionary<int, Iris> DIris = new Dictionary<int, Iris>(); // ini tempat simpan semua data bunganya
            if (!Directory.Exists("Hasil"))
            {
                DirectoryInfo di = Directory.CreateDirectory("Hasil");
            }

            List<double> listSSE = new List<double>();
            

            StreamReader sr = new StreamReader(@"iris.csv");
            String line = "";

            int urut = 0; // sebenarnya bukan apa juga cuma pasang sebagai key pada DIris di atas
            while ((line = sr.ReadLine()) != null) //Baca semua line nya
            {
                //Console.WriteLine(line);
                if (line != "")
                {
                    urut++;
                    string[] data = line.Split(',');
                    Iris bunga = new Iris(double.Parse(data[0]), double.Parse(data[1]), double.Parse(data[2]), double.Parse(data[3]), data[4]);
                    DIris.Add(urut, bunga);
                                       
                    // ini masukkan data berdasarkan data clusternya(data clusternya diambil dari split ke 4 di atas)
                   // DCluster[data[4]].Add(bunga); 
                }
            }
            Console.WriteLine("Hasil SSE dari 50 Run: ");

            string filePath = @"Hasil/Result.csv";
            List<string> lines = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                
               listSSE.Add( Hitung(DIris));
                if(i+1== 5 || i + 1 == 10 || i + 1 == 15 || i + 1 == 20 || i + 1 == 25 || i + 1 == 50)
                {
                    int dx = listSSE.IndexOf(listSSE.Min());
                    lines.Add("Run "+(i+1)+": Urutan "+ (dx+1) +"("+listSSE[dx]+")");
                }
            }
            File.WriteAllLines(filePath, lines);

            Console.WriteLine("");
            Console.WriteLine("Perhitungan selesai, hasil perhitungan dapat dilihat pada folder Hasil ");

            
            Console.ReadKey(); //Cobaaaaa apa gunanya ini?????
        }

        public static double Hitung(Dictionary<int, Iris> DIris)
        {
            iteration++;
            num = 0;

            // hmm.... ini berisi cluster apa dia, lalu di cluster itu dia berisi 
            // clas iris apa saja yang menjadi bagian dari cluster itu
            // btw string(key) itu menunjukkan dia cluster apa
            Dictionary<string, List<Iris>> DCluster = new Dictionary<string, List<Iris>>();

            // DAverage itu untuk simpan clusternya beserta data average clusternya, beserta data average cluster lalu
            // seperti di atas kurang lebih sihh, keynya berupa cluster, kemudian valuenya.... saya jelaskan pelan"
            // double[] ini berupa seperti di class iris sebenarnya, walaupun sebenarnya bisa dibuat class iris, 
            // tapi begini saja lahh, isinya seperti data member iris, ada 4(kecuali nama clas iris)
            // Selanjutnya kenapa di List???? karna nanti saya simpan data averagenya banyak(ada data average lalu
            // Kenapa 1 list, 1 array double?? karna array berisi data member jadi isi arraynya static pasti berisi 4 data
            // 1 nya list karna saya add terus menerus setiap kali iterasi berjalan jadi mending pake list
            Dictionary<string, List<double[]>> DAverage = new Dictionary<string, List<double[]>>();

            double SSE = 0;
            // ini mengisi clusternya duluu
            DCluster.Add("Iris-setosa", new List<Iris>());
            DCluster.Add("Iris-versicolor", new List<Iris>());
            DCluster.Add("Iris-virginica", new List<Iris>());
            // ini tujuannya buat data pertama pada DAverage dulu, jadi secara singkat kita inisialisasi dulu
            // pertama buat dulu dummy untuk isi data pertama, baru kita input cluster yg kita mau baru input
            Dictionary<string, double[]> dummy = new Dictionary<string, double[]>();
            Random rd = new Random();
            //double dt = rd.Next(43, 79) / 10;
           // dummy.Add("Iris-setosa", new double[] { rd.Next(43, 79) / 10f, rd.Next(20, 44) / 10f, rd.Next(10, 69) / 10f, rd.Next(1, 25) / 10f });
           // dummy.Add("Iris-versicolor", new double[] { rd.Next(43, 79) / 10f, rd.Next(20, 44) / 10f, rd.Next(10, 69) / 10f, rd.Next(1, 25) / 10f });
           // dummy.Add("Iris-virginica", new double[] { rd.Next(43, 79) / 10f, rd.Next(20, 44) / 10f, rd.Next(10, 69) / 10f, rd.Next(1, 25) / 10f });

            dummy.Add("Iris-setosa", DIris[rd.Next(1,150)].getNum());
            dummy.Add("Iris-versicolor", DIris[rd.Next(1, 150)].getNum());
            dummy.Add("Iris-virginica", DIris[rd.Next(1, 150)].getNum());

            // hmmm begitulah baca sendiri sudahh sisa diinput sesuai nama clusternya...
            // jadi nanti semuanya cuma ada cluster ini tok sebagai key nya supaya gampang pengaturan pencocokan nanti
            foreach (KeyValuePair<string, double[]> lst in dummy)
            {
                // ini inisialisasi sek dulu list nya baru boleh diinput... 
                // kenapa harus diinisialisasi dulu????
                DAverage.Add(lst.Key, new List<double[]>());
                DAverage[lst.Key].Add(lst.Value);
            }

            DCluster = KMean(DIris, DAverage, ref SSE); // Panggil methodnya baru simpan ke DCLuster

            string filePath = @"Hasil/run" + iteration + ".csv";

            List<string> lines = new List<string>();
            lines.Add("Iterasi : " + iteration);
            lines.Add("Total SSE : " + SSE);
            lines.Add("");
            double se = 0;

            foreach (KeyValuePair<string, List<Iris>> data in DCluster)
            {
                double[] centroid = DAverage[data.Key][num - 1];
                lines.Add(data.Key);
                lines.Add("Centroid," + centroid[0] + "," + centroid[1] + "," + centroid[2] + "," + centroid[3]);
                lines.Add("Total: " + data.Value.Count + " data");


                foreach (Iris flower in data.Value)
                {
                    lines.Add(flower.ToString());
                }
                lines.Add("");
              

            }
           

            File.WriteAllLines(filePath, lines);
            Console.WriteLine("Run " + iteration + ": " + SSE);           

            return SSE;
        }

        public static Dictionary<string, List<Iris>> KMean(Dictionary<int, Iris> DIris, Dictionary<string, List<double[]>> DAverage, ref double SSE)
        {

            // Penampung data sementara, dan buat clusternya kek di atas"nya
            Dictionary<string, List<Iris>> DCluster2 = new Dictionary<string, List<Iris>>();
            DCluster2.Add("Iris-setosa", new List<Iris>());
            DCluster2.Add("Iris-versicolor", new List<Iris>());
            DCluster2.Add("Iris-virginica", new List<Iris>());

            // ini cuma hitung similarity dari setiap data membernya
            foreach (Iris iris in DIris.Values)
            {
                Dictionary<string, double> DAvgSim = new Dictionary<string, double>();

                // pertama DAverage dilooping dulu....
                foreach (KeyValuePair<string, List<double[]>> cluster in DAverage)
                {
                    // nanti kan Average di Index terakhir yang dihitung jadi itu yg dipake lahh
                    // semoga kalian mengerti hikshiks...
                    // kan ada banyak kek sy jelaskan tadi, kalo kalian betulan baca ini
                    // jadi kita di sini cuma ambil average terbaru saja secara gampangnya dan yang terbaru itu 
                    // index terakhir
                    int dex = cluster.Value.Count - 1;
                    double sim_sp_length = Math.Pow(iris.sp_length - cluster.Value[dex][0], 2);
                    double sim_sp_witdh = Math.Pow(iris.sp_width - cluster.Value[dex][1], 2);
                    double sim_pt_length = Math.Pow(iris.pt_length - cluster.Value[dex][2], 2);
                    double sim_pt_witdh = Math.Pow(iris.pt_width - cluster.Value[dex][3], 2);                
                    double sim = Math.Sqrt(sim_pt_length + sim_pt_witdh + sim_sp_length + sim_sp_witdh);
                    DAvgSim.Add(cluster.Key, sim);
                }

                // ini berarti dia pertama di dulu nilai terkecil dari seluruh cluster yang ada (yang x=>x.Value..)
                // kemudian kita akan cari keynya dari value dari itu, karena nda bisa diambil langsung jadi begitu caranya
                double dist = DAvgSim.Min(x => x.Value);
                //if (dist == 0)
                //{
                //    Console.Write("la");

                //}
                string kunci = DAvgSim.First(y => y.Value == dist).Key;
                SSE += dist * dist;

                // kan keynya berupa cluster, jadi dari cluster itu langsung kita input ke DCluster2(sudah di inisialisasi tadi)
                DCluster2[kunci].Add(iris); 
            }
            
            num++; // menambah jumlah iterasi
          //  Console.WriteLine("iterasi: " + num + ":" + SSE);
            Dictionary<string, double[]> DAvgGenerate = FindAverage(DCluster2); // cari rata" dari ini....

            // Nahhh ini bagian yang paling susah kawan", nda usah dimengerti gpp rasanya karna nanti sy yg capek
            // saya jelaskan intinya dulu dahhh, begini nanti kita hitung semua rata" dari setiap cluster
            //(ituuu di atas DAvgGenerate), nanti kita bandingkan dengan semua data average sebelumnya...
            // kenapa kita harus bandingkan semua sebelumnya dan bukan yang terakhir saja?? karna nanti ada dia punya avg sekarang sama
            // dengan avg kedua sebelumnya, dan begitu berulang terus sampe infinite loop, jadi untuk jaga" sy pake
            // semuanya saja untuk disamakan averagenya, kalo berat baru saya batasi sampe 3 atau 4 average terbaru

            // ini menunjukkan kalo belum ada avg saat ini yang cocok dengan avg sebelumnya
            bool cocok = false;

            // pertama kita urutkan sesuai jumlah iterasi sekk, kita buat menurun karna kemungkinan avg itu sama
            // itu dari yang terbaru ke terlama
            // btw kalo kau bingung kenapa pake num, itu seperti jumlah dari List<double[]>.count di DCluster
            // btw lagi, jumlah itu list<double... blabla sama semua setiap cluster, kenapa bisa sama semua jumlahnya????
            for (int i = num - 1; i >= 0; i--) 
            {

                // nahh yg bool ini berbeda dengan bool di atas, ini seperti mengecek kalo seandainya avg nya sudah beda
                // artinya sudah nda mungkin sama sisanya, jadi kalo misalkan semua datanya sama kan jadi betul semua jadi bool nya true terus
                // pasti kau nda ngerti.. mending baca di baca nya dulu lahhh
                bool same = true;

                //ini hanya looping untuk setiap cluster yang tersedia
                foreach (string bunga in DAverage.Keys)
                {
                    // kalo ini nanti di looping untuk setiap anggota dalam double[] itu tadi sebelumnya...
                    // seperti yang sy bilang sebelumnya ini pasti 4 kali dilakukan karna isinya cuma 4
                    // ini dilakukan per cluster, bacaaaa cara kerja K-Meansss!!!!
                    for (int j = 0; j < DAverage[bunga][i].Length; j++) 
                    {
                        // ini untuk samakan apakah average dari cluster saat ini sama nda dengan DAverage(daftar
                        //semua average) pada setiap bagian. kalo ada beda langsung bool same = false dan 
                        // nanti akan dilakukan cek lagi pada average yg lain
                        if (DAverage[bunga][i][j].CompareTo(DAvgGenerate[bunga][j]) != 0)
                        {
                            same = false;
                        }
                    }
                }

                // kalo ini true berarti dia sudah menemukan pasangan sejatinya....
                // maksudnya di sini semua averagenya sama pada setiap cluster dan setiap pembanding data member 
                // pada clusternya dengan data sebelumnya jadi dihentikan saja
                // baru ganti bool cocok jadi true sebagai penanda sudah benar
                if (same)
                {
                    cocok = true;
                    break;
                }
            }
            // yaaah ini cuma tambahkan dia sebagai daftar DAverage baru sihh, nda ada modus lain
            foreach (string nama in DAvgGenerate.Keys)
            {
                DAverage[nama].Add(DAvgGenerate[nama]);
            }

            // ini seandainya dia masih jomblo, artinya dia harus cari lagi sampe dapat lahh
            // intinya dia ulang terus menerus lahh, dan semakin lama DAverage terisi dengan data sebelumnya 
            // Sekarang saya tanya lagi kalo dia panggil methodnya sendiri apa namanya???
            if (!cocok)
            {
                SSE = 0;
                DCluster2 = KMean(DIris, DAverage,ref SSE);
            }

            return DCluster2; // kalo benar sudah dia kembalikan hasilnya yang benar setiap bunga berada di cluster apa
        }

        // Ini tujuan utamanya untuk cari nilai rata" dari data" apa saja yang ada pada suatu cluster
        public static Dictionary<string, double[]> FindAverage(Dictionary<string, List<Iris>> DSim)
        {
            // buat penampung sementara dulu....
            Dictionary<string, double[]> DAvg = new Dictionary<string, double[]>();
            foreach (KeyValuePair<string, List<Iris>> lst in DSim)
            {
                if (lst.Value.Count > 0) // ini untuk menghindari cluster yang datanya tidak ada
                {
                    // yaaa pake method sajaaaa dari bawaan dictionary, yaa kita cuma buat rujukan ke setiap data 
                    // member, sy bukan org nda jelas yang cari rata" itu setiapnya kalo sudah ada cara gampangnya
                    double avg_sp_lenght = lst.Value.Average(x => x.sp_length);
                    double avg_sp_width = lst.Value.Average(x => x.sp_width);
                    double avg_pt_length = lst.Value.Average(x => x.pt_length);
                    double avg_pt_width = lst.Value.Average(x => x.pt_width);

                    // Seperti yang sy bilang sebelumnya ini nanti dijadikan array, karna datanya sudah pasti 4
                    // jadi ini cara gampangnya buat array yaaa....
                    double[] avg = { avg_sp_lenght, avg_sp_width, avg_pt_length, avg_pt_width };
                    DAvg.Add(lst.Key, avg); // simpan rata" suatu cluster ke Dictionarynya
                }
                else
                {
                    double[] avg = { 0, 0, 0, 0 }; // yaa kalo kosong sisa dibuat 0 saja
                    DAvg.Add(lst.Key, avg);
                }
            }
            return DAvg;
        }
    }
}


// looping buangan
//foreach(KeyValuePair<string, List<double[]>> KBunga in DAverage)
//{
//    for (int j = 0; j < KBunga.Value.Count; j++) //ini looping persamaan lalu juga
//    {
//        bool same = true;
//        for (int i = 0; i < KBunga.Value[j].Length; i++) // ini bandingkan persamaan per bagian
//        {
//            if (DAverage[KBunga.Key][j][i].CompareTo(DAvgGenerate[KBunga.Key][i]) !=0)
//            {
//                same = false;
//            }
//        }
//        if(same)
//        {
//            cocok = true;
//            break;
//        }
//    }
//    if(cocok)
//    {
//        break;
//    }
//}
