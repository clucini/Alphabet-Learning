using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        Random Rand = new Random();
        List<Generation> Generations = new List<Generation>();
        List<char> Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();
        public int NumOfGenerations;
        //System.IO.StreamWriter file = new System.IO.StreamWriter("F:\\test.txt");        
        static void Main(string[] args)
        {
            Program p = new Program();
            p.FirstGen(20);
            for(int i = 0; i < 100000; i++)
            {
                p.Breeding();
            }
            Console.Out.WriteLine(p.Generations[p.Generations.Count - 1].LowestValue.OrderResult);
            Console.Out.WriteLine(p.Generations[p.Generations.Count - 1].HighestValue.OrderResult);
            Console.Read();
        }

        void FirstGen(int NumOfChromosomes)
        {
            Generation NewGen = new Generation();
            for (int i = 0; i < NumOfChromosomes; i++)
            {
                Chromosome NewChrome = new Chromosome();
                List<Char> tempA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();
                while (tempA.Count != 0)
                {
                    int r = Rand.Next(0, tempA.Count);
                    NewChrome.OrderResult[26 - tempA.Count] = tempA[r];
                    tempA.Remove(tempA[r]);
                }
                NewGen.Chromosomes.Add(NewChrome);
            } 
            //Chromosome Perfect = new Chromosome();
            //Perfect.OrderResult = Alphabet.ToArray();
            //NewGen.Chromosomes.Add(Perfect);
            Generations.Add(NewGen);
            Score(Generations.Count - 1);
        }

        void Score(int GenNum)
        {
            for (int i = 0; i < Generations[GenNum].Chromosomes.Count; i++)
            {
                Chromosome CurChrome = Generations[GenNum].Chromosomes[i];
                Generations[GenNum].LowestValue = CurChrome;
                for (int f = 0; f < CurChrome.OrderResult.Length; f++)
                {
                    CurChrome.PointResult += 26 - Math.Abs(f - Alphabet.IndexOf(CurChrome.OrderResult[f]));
                }
                if (CurChrome.PointResult < Generations[GenNum].LowestValue.PointResult)
                    Generations[GenNum].LowestValue = CurChrome;
                if (CurChrome.PointResult > Generations[GenNum].HighestValue.PointResult)
                    Generations[GenNum].HighestValue = CurChrome;
                //Because I know you're going to look at this and think I'm retarded, here's a quick explanation
                //313 is probably (I haven't properly checked it, I'm 90% certain this is right though)
                //the potential lowest value this could ever return (ZYXW.....)
                //Hence, to diversify the results, a perfect score goes from being just double the lowest result
                //676 vs 313
                //to 363 vs 0
                //This means the breeding gives better chromosomes a better chance of survival. 
                //CurChrome.PointResult = CurChrome.PointResult - 313;
                //Console.Write(CurChrome.OrderResult);
                //Console.WriteLine(" - " + CurChrome.PointResult);
            }
            for (int i = 0; i < Generations[GenNum].Chromosomes.Count; i++)
            {
                Chromosome CurChrome = Generations[GenNum].Chromosomes[i];
                //CurChrome.PointResult = CurChrome.PointResult - 313;
                CurChrome.PointResult = CurChrome.PointResult - Generations[GenNum].LowestValue.PointResult + 1;
            }
        }

        void Breeding()
        {
            Generation CurGen = Generations[Generations.Count - 1];
            Generation NewGen = new Generation();
            float totalSum = 0;

            for (int i = 0; i < CurGen.Chromosomes.Count; i++)
            {
                totalSum += CurGen.Chromosomes[i].PointResult;
            }

            for(int i = 0; i < CurGen.Chromosomes.Count; i++)
            {
                CurGen.Chromosomes[i].PercentValue = CurGen.Chromosomes[i].PointResult / totalSum * 100;
            }

            for(int i = 0; i < CurGen.Chromosomes.Count; i++)
            {
                Chromosome Chrome1 = FindChrome(CurGen, Rand.Next(0, 100));
                Chromosome Chrome2 = FindChrome(CurGen, Rand.Next(0, 100));
                Chromosome BabyChrome = new Chromosome();
                //Chromosome BabyChrome2 = new Chromosome();
                for (int f = 0; f < Chrome1.OrderResult.Length; f++)
                {
                    if (Rand.Next(0, 1) == 0)
                    {
                        BabyChrome.OrderResult[f] = Chrome1.OrderResult[f];
                        //BabyChrome.OrderResult[f] = Chrome2.OrderResult[f];
                    }
                    else
                    {
                        BabyChrome.OrderResult[f] = Chrome1.OrderResult[f];
                        //BabyChrome.OrderResult[f] = Chrome2.OrderResult[f];
                    }
                }
                NewGen.Chromosomes.Add(BabyChrome);
                //NewGen.Chromosomes.Add(BabyChrome2);
            }


            //Mutation
            for (int i = 0; i < CurGen.Chromosomes.Count; i++)
            {
                Chromosome CurChrome = NewGen.Chromosomes[i];
                for (int f = 0; f < CurChrome.OrderResult.Length; f++)
                    if (Rand.Next(1, 10001) <= 100)
                    {
                        char a = CurChrome.OrderResult[f];
                        int b = Rand.Next(-1, 2);
                        if (f + b > 25)
                            b = 0;
                        else if (f + b < 0)
                            b = 0;
                        char c = CurChrome.OrderResult[f + b];
                        CurChrome.OrderResult[f] = c;
                        CurChrome.OrderResult[f + b] = a;
                   }
                Console.Out.WriteLine(CurChrome.OrderResult);
            }
            Generations.Add(NewGen);
            //file.WriteLine(CurGen.HighestValue.PointResult + " Highest - Lowest" + CurGen.LowestValue.PointResult);
            Console.Out.WriteLine(NumOfGenerations);
            NumOfGenerations++;
            //Console.Out.WriteLine(totalSum / CurGen.Chromosomes.Count + " - Avg. " + CurGen.HighestValue.PercentValue + " - Highest Percent" + CurGen.LowestValue.PercentValue + " - Lowest Percent");
            Generations.Remove(CurGen);
            Score((Generations.Count - 1));
        }

        Chromosome FindChrome(Generation CurGen, int RandNum)
        {
            float totalSum = 0;
            int curChrome = 0;
            totalSum += CurGen.Chromosomes[curChrome].PercentValue;
            while(totalSum < RandNum)
            {
                curChrome++;
                totalSum += CurGen.Chromosomes[curChrome].PercentValue;
            }
            return CurGen.Chromosomes[curChrome];
        }
    }

    class Generation
    {
        public List<Chromosome> Chromosomes = new List<Chromosome>();
        public Chromosome LowestValue = new Chromosome();
        public Chromosome HighestValue = new Chromosome();
    }

    class Chromosome
    {
        public char[] OrderResult = new char[26];
        public int PointResult;
        public float PercentValue;
    }
}
