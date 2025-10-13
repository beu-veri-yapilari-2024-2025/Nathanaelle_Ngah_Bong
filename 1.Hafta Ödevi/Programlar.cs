class Programlama
{


   public static int Dizi(int[] sayılar)
    {
        int toplam = 0;
        for (int i = 0; i < sayılar.Length; i++)
        {
            toplam += sayılar[i];
        }
        return toplam;
    }


   public static int Arama(int[] A, int N, int sayı)
    {
        int sol = 0;
        int sağ = N - 1;
        while (sol <= sağ)
        {
            int orta = (sol + sağ) / 2;
            if (A[orta] == sayı)
            {
                return orta;
            }
            else
            {
                sol = orta + 1;
            }
        }
        return -1;
    }



    public static int[,] çarpım(int[,] A, int[,] B)
    {
        int Arow = A.GetLength(0);
        int Acolumn = A.GetLength(1);
        int Bcolumn = B.GetLength(1);
        int[,] result = new int[Arow, Bcolumn];
        for (int i = 0; i < Arow; i++)
        {
            for (int j = 0; j < Bcolumn; j++)
            {
                int sum = 0;
                for (int k = 0; k < Acolumn; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                result[i, j] = sum;
            }
        }
        return result;
    }

   
    public static void main()
    {
        
        int [] dizi = { 16, 25, 36, 41, 62, 77, 88, 92 };
        int N = dizi.Length;
        int aranan_sayısı = 77;
        int index = Arama(dizi,N, aranan_sayısı);
        Console.WriteLine("{0} sayı {1} indeksindedir", aranan_sayısı, index);

        int[,] matris1 = { { 1, 5 }, { 3, 7 } };
        int[,] matris2 = { { 9, 2 }, { 5, 1 } };
        int[,] Cevap = çarpım(matris1, matris2);

        for(int i= 0; i< Cevap.GetLength(0);i++)
        {
            for (int j=0; j< Cevap.GetLength(1); j++)
            {
                Console.Write(Cevap[i, j].ToString().PadLeft(4));
            }
            Console.WriteLine();
        }
    }

}
