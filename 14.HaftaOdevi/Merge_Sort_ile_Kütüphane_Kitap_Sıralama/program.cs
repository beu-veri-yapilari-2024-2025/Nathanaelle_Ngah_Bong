using System;
using System.Collections.Generic;

namespace KutuphaneSiralama
{
    // Kitap sınıfı - get/set kullanmadan
    public class Kitap
    {
        public string Ad;
        public int BasimYili;

        public Kitap(string ad, int basimYili)
        {
            Ad = ad;
            BasimYili = basimYili;
        }

        public void Yazdir()
        {
            Console.WriteLine($"- {Ad} - {BasimYili}");
        }
    }

    class Program
    {
        // Kitapları yazdırma metodu
        static void KitaplariYazdir(List<Kitap> kitaplar, string baslik)
        {
            Console.WriteLine(baslik);
            Console.WriteLine(new string('-', 40));
            foreach (var kitap in kitaplar)
            {
                kitap.Yazdir();
            }
            Console.WriteLine();
        }

        // Merge Sort algoritması - Kitapları BasimYili'na göre sıralar
        static void MergeSort(List<Kitap> kitaplar, int baslangic, int bitis)
        {
            if (baslangic < bitis)
            {
                int orta = (baslangic + bitis) / 2;

                // Sol yarıyı sırala
                MergeSort(kitaplar, baslangic, orta);

                // Sağ yarıyı sırala
                MergeSort(kitaplar, orta + 1, bitis);

                // İki yarıyı birleştir
                Merge(kitaplar, baslangic, orta, bitis);
            }
        }

        // Birleştirme işlemi
        static void Merge(List<Kitap> kitaplar, int baslangic, int orta, int bitis)
        {
            int solBoyut = orta - baslangic + 1;
            int sagBoyut = bitis - orta;

            // Geçici diziler oluştur
            Kitap[] solDizi = new Kitap[solBoyut];
            Kitap[] sagDizi = new Kitap[sagBoyut];

            for (int i = 0; i < solBoyut; i++)
            {
                solDizi[i] = kitaplar[baslangic + i];
            }

            for (int j = 0; j < sagBoyut; j++)
            {
                sagDizi[j] = kitaplar[orta + 1 + j];
            }

            int solIndex = 0, sagIndex = 0;
            int birlesimIndex = baslangic;

            // İki diziyi birleştir (BasimYili'na göre karşılaştır)
            while (solIndex < solBoyut && sagIndex < sagBoyut)
            {
                if (solDizi[solIndex].BasimYili <= sagDizi[sagIndex].BasimYili)
                {
                    kitaplar[birlesimIndex] = solDizi[solIndex];
                    solIndex++;
                }
                else
                {
                    kitaplar[birlesimIndex] = sagDizi[sagIndex];
                    sagIndex++;
                }
                birlesimIndex++;
            }

            // Sol dizide kalan elemanları kopyala
            while (solIndex < solBoyut)
            {
                kitaplar[birlesimIndex] = solDizi[solIndex];
                solIndex++;
                birlesimIndex++;
            }

            // Sağ dizide kalan elemanları kopyala
            while (sagIndex < sagBoyut)
            {
                kitaplar[birlesimIndex] = sagDizi[sagIndex];
                sagIndex++;
                birlesimIndex++;
            }
        }

        static void Main(string[] args)
        {
            // Kitap listesi oluşturma (en az 6 kitap)
            List<Kitap> kitaplar = new List<Kitap>();

            // Kitapları oluştur ve listeye ekle
            kitaplar.Add(new Kitap("Suç ve Ceza", 1866));
            kitaplar.Add(new Kitap("1984", 1949));
            kitaplar.Add(new Kitap("Hayvan Çiftliği", 1945));
            kitaplar.Add(new Kitap("Dönüşüm", 1915));
            kitaplar.Add(new Kitap("Simyacı", 1988));
            kitaplar.Add(new Kitap("Küçük Prens", 1943));
            kitaplar.Add(new Kitap("Beyaz Zambaklar Ülkesinde", 1923));
            kitaplar.Add(new Kitap("Satranç", 1942));

            // Sıralama öncesi kitapları yazdır
            KitaplariYazdir(kitaplar, "SIRALAMA ÖNCESİ KİTAP LİSTESİ");

            // Kitapları Merge Sort ile sırala
            MergeSort(kitaplar, 0, kitaplar.Count - 1);

            // Sıralama sonrası kitapları yazdır
            KitaplariYazdir(kitaplar, "SIRALAMA SONRASI KİTAP LİSTESİ (Basım Yılına Göre)");

            Console.WriteLine("Program sonu. Çıkmak için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}
