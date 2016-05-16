using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;
using System.IO;

namespace Steganography
{
    public static class Prime
    {
        private static List<BigInteger> primeList = new List<BigInteger>();
        private static BigInteger index = 0;
        private static Thread thread;
        private static Thread loadThread;
        private static readonly string file = "primes.bin";
        private static StringBuilder sbuilder = new StringBuilder();
        private static int intent = 0;
        private static readonly int maxIntent = 300;
        private static BigInteger max = 1000000;


        public static void Initialize()
        {
            primeList = new List<BigInteger>();

            loadThread = new Thread(InitializeUsingFiles);
            loadThread.Start();
        }

        private static void startGenerator()
        {
            thread = new Thread(generate);
            thread.Start();
        }

        private static void InitializeUsingFiles()
        {
            if (!File.Exists(file))
            {
                primeList.Add(2);
                primeList.Add(3);
                index = 5;
                File.WriteAllText(file, "2\r\n3\r\n");
                startGenerator();
            }
            else
            {
                foreach (string s in File.ReadAllLines(file))
                {
                    BigInteger b = 0;
                    if (BigInteger.TryParse(s.TrimEnd('\r','\n'), out b))
                    {
                        primeList.Add(b);
                    }else
                    {
                        primeList.Clear();
                        primeList.Add(2);
                        primeList.Add(3);
                        index = 5;
                        startGenerator();
                        return;
                    }
                }
                index = primeList[primeList.Count - 1];
                startGenerator();
            }
        }

        private static void generate()
        {
            for(BigInteger i = index; i < max; i+=2)
            {
                bool e = true;
                if (i % 2 == 0 || i % 3 == 0)
                {
                    e = false;
                }
                for (BigInteger n = 5; n*n <= i;n+=6)
                {
                    if(i % n == 0 || i % (n+2) == 0)
                    {
                        e = false;
                    }
                }
                if (e)
                {
                    lock (primeList)
                    {
                        primeList.Add(i);
                        sbuilder.Append(i.ToString() + "\r\n");
                        intent++;
                        if (intent > maxIntent)
                        {
                            try
                            {
                                File.AppendAllText(file, sbuilder.ToString());
                                intent = 0;
                                sbuilder.Clear();
                                sbuilder = new StringBuilder();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
                index+=2;
            }
        }

        public static List<BigInteger> getPrimes(BigInteger m)
        {
            if (max < m)
            {
                IncreaseMax(m);
            }
            while (index < m)
            {
                OutputConsole.Write($"Current index:{index}, needed:{m}");
                Thread.Sleep(1000);
            }
            List<BigInteger> list = new List<BigInteger>();
            lock (primeList)
            {
                list = primeList.Where(n => n <= m).ToList();
            }

            return list;
        }

        public static void IncreaseMax(BigInteger nmax)
        {
            max = nmax;
            if (thread != null)
            {
                if (!thread.IsAlive)
                {
                    thread = new Thread(generate);
                    thread.Start();
                }
            }
        }

        public static void Finish()
        {
            try
            {
                if (thread != null)
                {
                    thread.Abort();
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                if (loadThread != null)
                {
                    loadThread.Abort();
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                if (!string.IsNullOrWhiteSpace(sbuilder.ToString()))
                {
                    File.AppendAllText(file, sbuilder.ToString());
                }
            }catch(Exception e)
            {

            }
        }
    }
}
