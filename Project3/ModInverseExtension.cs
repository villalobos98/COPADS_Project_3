//@author: Isaias Villalobos
//@date: 11/19/2019
//@description: This class is an Extension class, which will provide a helpful extension method.
//              The extension method, modInverse, is to be used elsewhere in other classes.

using System.Numerics;

public static class ModInverseExtension
{
    /* @input: a BigInteger a, an BigInteger n
    *  @output: a BigInteger
    */
    public static BigInteger modInverse(BigInteger a, BigInteger n)
    {
        BigInteger i = n, v = 0, d = 1;
        while (a > 0)
        {
            BigInteger t = i / a, x = a;
            a = i % x;
            i = x;
            x = d;
            d = v - t * x;
            v = x;
        }
        v %= n;
        if (v < 0) v = (v + n) % n;
        return v;
    }
}
