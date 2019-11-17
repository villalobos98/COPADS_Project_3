namespace Project3
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    public class PrimeFunction
    {
        private object locker = new object();

        //This functions will make sure to generate a prime number according to the 
        //documentation provided
        //Returns a BigInteger
        public BigInteger GeneratePrimeNumber(int bitSize)
        {
            RNGCryptoServiceProvider randomNumbers = new RNGCryptoServiceProvider();
            byte[] byteArray = new Byte[bitSize / 8];

            randomNumbers.GetNonZeroBytes(byteArray);

            BigInteger bigInt = new BigInteger(byteArray);

            return bigInt;
        }

        //This should be using the extension class properly now.
        //This functions is going to check if a bigInt is prime.
        //Returns a boolean
        public bool checkPrimeNumber(BigInteger bigInt)
        {
            var isPrime = bigInt.IsProbablyPrime();
            return isPrime;
        }

        public void parallelPrimeFunction(int bitSize, int countsArgument)
        {
            List<BigInteger> intList = new List<BigInteger>();

            Parallel.For(0, Int32.MaxValue, (i, state) =>
            {

                var primeNumber = GeneratePrimeNumber(bitSize);
                var isPrime = checkPrimeNumber(primeNumber);

                if (isPrime)
                {
                    if (countsArgument <= intList.Count)
                    {
                        state.Break();
                    }
                    lock (locker)
                    {
                        intList.Add(primeNumber);
                    }

                }
            });
            //This does all the printing for the integers inside the intList.
            for (int i = 0; i < countsArgument; i++)
            {
                int index = i + 1;
                Console.WriteLine(index + ": " + intList[i]);
                if (i < countsArgument - 1)
                {
                    Console.WriteLine("");
                }
            }

        }
    }
}