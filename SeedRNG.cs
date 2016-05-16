using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Steganography
{
    public class SeedRNG
    {
        private int seed { get; set; }
        private List<int> obtained;
        private int limit { get; set; }
        private Random r;
        private List<int> exp = new List<int>();
        private LCG lcg;
        public SeedRNG(int seed, int limit, bool b = false)
        {
            this.seed = seed;
            this.limit = limit;
            lcg = new LCG(limit, new Random(seed).Next(0, limit),seed);
            obtained = new List<int>();
            r = new Random(seed);
            if (b)
            {
                exp = Enumerable.Range(0, limit).ToList();
            }
        }

        public int Next
        {
            get
            {
                return (int)lcg.next();
            }
        }

        public int NextN
        {
            get
            {
                int x = 0;
                int n = r.Next(0, exp.Count);
                x = exp[n];
                exp.RemoveAt(n);
                return x;
            }
        }
    }
    public class SeedURNG
    {
        private uint seed { get; set; }
        private List<uint> obtained;
        private uint limit { get; set; }
        private Random r;
        private List<uint> exp = new List<uint>();
        public SeedURNG(uint seed, uint limit, bool b = false)
        {
            this.seed = seed;
            this.limit = limit;
            obtained = new List<uint>();
            r = new Random((int)seed);
            if (b)
            {
                for (uint i = 0; i < limit; i++)
                {
                    exp.Add(i);
                }
            }
        }

        public uint Next
        {
            get
            {
                uint x = 0;
                do
                {
                    x = (uint)r.Next(0, (int)limit);
                } while (obtained.Contains(x));
                obtained.Add(x);
                return x;
            }
        }

        public uint NextN
        {
            get
            {
                uint x = 0;
                int n = r.Next(0, exp.Count);
                x = exp[n];
                exp.RemoveAt(n);
                return x;
            }
        }
    }

}
