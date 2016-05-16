using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Steganography
{
    public class LCG
    {
        private BigInteger m;
        private BigInteger a;
        private BigInteger c;
        private BigInteger x0;
        private int index = 0;
        private int seed = 0;

        public List<BigInteger> x { get; private set; }

        public LCG(BigInteger m, BigInteger x0)
        {
            this.m = m;
            this.x0 = x0 % m;
            Initialize();
        }

        public LCG(BigInteger m, BigInteger x0, int seed)
        {
            this.m = m;
            this.x0 = x0 % m;
            this.seed = seed;
            Initialize();
        }

        private void Initialize()
        {
            x = new List<BigInteger>();
            List<BigInteger> p = primes();
            List<BigInteger> f = factors(p);
            c = calcC(p, f);
            a = f.Aggregate((x, y) => x * y);
            if (m % 4 == 0)
            {
                if (a % 4 == 0)
                {
                    a++;
                }
                else
                {
                    a = (a * 4) + 1;
                }
            }
            else
            {
                a++;
            }

            index = 0;
            x.Add(x0);
            for (BigInteger i = 1; i < m; i++)
            {
                x.Add(((a * x[(int)i - 1]) + c) % m);
            }
        }

        public void setIndex(int i)
        {
            index = i;
        }

        public BigInteger get(int i)
        {
            return x[i];
        }

        public BigInteger next()
        {
            BigInteger r = x[index];
            index++;
            if (index > x.Count)
            {
                index = 0;
            }
            return r;
        }

        public List<double> getList()
        {
            List<double> list = new List<double>();
            for (BigInteger i = 0; i < m; i++)
            {
                list.Add((double)x[(int)i]);
            }
            return list;
        }

        private BigInteger calcC(List<BigInteger> prime, List<BigInteger> factor)
        {

            List<BigInteger> l = new List<BigInteger>();

            l.AddRange(prime);
            for(int i = 0; i < factor.Count; i++)
            {
                l.Remove(factor[i]);
            }

            if(l.Count > 0)
            {
                return l[new Random(seed).Next(0, l.Count)];
            }
            
            return 1;
        }

        private List<BigInteger> factors(List<BigInteger> prime)
        {
            List<BigInteger> f = new List<BigInteger>();
            foreach(BigInteger p in prime)
            {
                if(m % p == 0)
                {
                    f.Add(p);
                }
            }
            return f;
        }


        private List<BigInteger> primes()
        {
            List<BigInteger> p = new List<BigInteger>();
            p = Prime.getPrimes(m);
            p.Add(1); //Add 1, needed if m == prime

            return p;

        }




    }
}
