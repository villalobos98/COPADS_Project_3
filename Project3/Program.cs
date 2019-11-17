using System;

namespace Project3
{
    //public class KeyForm
    //{
    //    public String email { get; set; }
    //    public String key { get; set; }

    //}

    //public class MessageForm
    //{
    //    public String email { get; set; }
    //    public String content { get; set; }

    //}

    class Program
    {
        //public static BigInteger bigE;
        //public static BigInteger bigN;

        //static BigInteger modInverse(BigInteger a, BigInteger n)
        //{
        //    BigInteger i = n, v = 0, d = 1;
        //    while (a > 0)
        //    {
        //        BigInteger t = i / a, x = a;
        //        a = i % x;
        //        i = x;
        //        x = d;
        //        d = v - t * x;
        //        v = x;
        //    }
        //    v %= n;
        //    if (v < 0) v = (v + n) % n;
        //    return v;
        //}


        //public static void keyGen(int keySize)
        //{
        //    var randomNum = new Random();
        //    //var percentage = randomNum.Next(1, 8) / 8;
        //    keySize /= 2;
        //    var a = (int)(((float)( new Random().Next(9800,10200)) /(float) 10000) * (float) keySize);
        //    var b = keySize * 2 - a;
            
        //    //when you mutliple a and b it should equal the number of bits in the keysize
        //    //var a = keySize + (keySize * percentage); //Find a percentage of the original keysize
        //    //var b = keySize - (keySize * percentage); //Find a percentage of the original keysize

        //    PrimeFunction primeFunctions = new PrimeFunction();
        //    BigInteger p = primeFunctions.GeneratePrimeNumber(a);
        //    BigInteger q = primeFunctions.GeneratePrimeNumber(b);


        //    BigInteger N = p * q;
        //    BigInteger r = (p - 1) * (q - 1);

        //    BigInteger E = 65537;
            
        //    BigInteger D = modInverse(E, r);

        //    var publicKeyFile = File.Create("public.key");
        //    var privateKeyFile = File.Create("private.key");

        //    var arrayE = E.ToByteArray();
        //    var arrayN = N.ToByteArray();

        //    var e = BitConverter.GetBytes(arrayE.Length);
        //    var n = BitConverter.GetBytes(arrayN.Length);

        //    if (BitConverter.IsLittleEndian)
        //    {
        //        Array.Reverse(e, 0, e.Length); //supposed to reverse e
        //        Array.Reverse(n, 0, n.Length); //supposed to reverse n
        //    }

        //    var publicKey = new Byte[4 + arrayE.Length + 4 + arrayN.Length];
        //    e.CopyTo(publicKey, 0);
        //    arrayE.CopyTo(publicKey, 4);
        //    n.CopyTo(publicKey, 4 + arrayE.Length);
        //    arrayN.CopyTo(publicKey, 4 + arrayE.Length + 4);
        //    var encodedPublicKey = Convert.ToBase64String(publicKey);

        //    try
        //    {
        //        publicKeyFile.Write(Encoding.Default.GetBytes(encodedPublicKey), 0, encodedPublicKey.Length);
        //    }
        //    finally
        //    {
        //        publicKeyFile.Close();
        //    }

        //    var arrayD = D.ToByteArray();
        //    var d = BitConverter.GetBytes(arrayD.Length);

        //    if (BitConverter.IsLittleEndian)
        //    {
        //        Array.Reverse(d, 0, d.Length);
        //    }

        //    var privateKey = new Byte[4 + arrayD.Length + 4 + arrayN.Length];

        //    d.CopyTo(privateKey, 0);
        //    arrayD.CopyTo(privateKey, 4);
        //    n.CopyTo(privateKey, 4 + arrayD.Length);
        //    arrayN.CopyTo(privateKey, 4 + arrayN.Length + 4);

        //    var encodedPrivateKey = Convert.ToBase64String(privateKey);

        //    try
        //    {
        //        privateKeyFile.Write(Encoding.Default.GetBytes(encodedPrivateKey), 0, encodedPrivateKey.Length);
        //    }
        //    finally
        //    {
        //        privateKeyFile.Close();
        //    }

        //}


        //public static async void sendKey(String email)
        //{
        //    HttpClient client = new HttpClient();
        //    var URL = "http://kayrun.cs.rit.edu:5000/Key/" + email;

        //    KeyForm keyForm = new KeyForm();
        //    keyForm.email = email;
        //    keyForm.key = File.ReadAllText("public.key");

        //    var content = new StringContent(JsonConvert.SerializeObject(keyForm), Encoding.Default, "application/json");

        //    try
        //    {
        //        var result = await client.PutAsync(URL, content);
        //        result.EnsureSuccessStatusCode();
        //        //Console.WriteLine(result.EnsureSuccessStatusCode.ToString());

        //    }
        //    catch (HttpRequestException e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
                    

        //}
        ///*
        // * email plaintext - this will base64 encode a message for a user in the to
        //   field. If you do not have a public key for that particular user, you should show an
        //   error message indicating that the key must be downloaded first.
        // */
        //public static async void sendMsg(String email, String message)
        //{
        //    byte[] data = Encoding.Default.GetBytes(message);
        //    BigInteger encryptedMessage = new BigInteger(data);

        //    //call decode key on the public key to get e and n
        //    decodeKey(File.ReadAllText(email + ".key"));

        //    var encryptedText = BigInteger.ModPow(encryptedMessage, bigE, bigN);

        //    var base64EncodedEncryptedText = Convert.ToBase64String(encryptedText.ToByteArray());
        //    var URL = "http://kayrun.cs.rit.edu:5000/Message/" + email;

        //    HttpClient client = new HttpClient();
        //    MessageForm messageForm = new MessageForm();
        //    messageForm.email = email;
        //    messageForm.content = base64EncodedEncryptedText;
        //    var content = new StringContent(JsonConvert.SerializeObject(messageForm), Encoding.Default, "application/json");

        //    var result = await client.PutAsync(URL, content);
        //    result.EnsureSuccessStatusCode();

        //    Console.WriteLine(messageForm.content);

        //}


        //public static void decodeKey(String key)
        //{
        //    //This is for decoding little e
        //    byte[] data = System.Convert.FromBase64String(key);
        //    var e = data.Take(4).ToArray();
        //    if (BitConverter.IsLittleEndian)
        //    {
        //        Array.Reverse(e, 0, e.Length);
        //    }
        //    var eVal = BitConverter.ToInt32(e, 0);
        //    var extractByteE = data.Skip(4).Take(eVal);
        //    bigE = new BigInteger(extractByteE.ToArray());

        //    //This part is for decoding little n
        //    var n = data.Skip(eVal + 4).Take(4).ToArray();

        //    if (BitConverter.IsLittleEndian)
        //    {
        //        Array.Reverse(n, 0, n.Length);
        //    }
        //    var nVal = BitConverter.ToInt32(n, 0);
        //    var extractByteN = data.Skip(8 + eVal).Take(nVal);
        //    bigN = new BigInteger(extractByteN.ToArray());

        //}

        //public static async void getkey(String emailAddress)
        //{

        //    //Create client to get key from server
        //    HttpClient client = new HttpClient();
        //    var URL = "http://kayrun.cs.rit.edu:5000/Key/" + emailAddress;
        //    var result = await client.GetAsync(URL);

        //    result.EnsureSuccessStatusCode();
        //    Console.WriteLine(await result.Content.ReadAsStringAsync());

        //    //Extract information from the response from the server, should contain an email and key
        //    KeyForm keyform = JsonConvert.DeserializeObject<KeyForm>(await result.Content.ReadAsStringAsync());

        //    //Decode the key
        //    decodeKey(keyform.key);
        //    var keyFile = File.Create(keyform.email.ToString() + ".key");

        //    //Save the key locally
        //    try
        //    {
        //        keyFile.Write(Encoding.Default.GetBytes(keyform.key), 0, keyform.key.Length);
        //    }
        //    finally
        //    {
        //        keyFile.Close();
        //    }

        //}
        //public static async void getMsg(String emailAddress)
        //{
        //    HttpClient client = new HttpClient();
        //    var URL = "http://kayrun.cs.rit.edu:5000/Message/" + emailAddress;
        //    var result = await client.GetAsync(URL);
        //    result.EnsureSuccessStatusCode();
        //    MessageForm messageForm = JsonConvert.DeserializeObject<MessageForm>(await result.Content.ReadAsStringAsync());
        //    Console.WriteLine(messageForm.content);

        //    decodeKey(File.ReadAllText("private.key"));
        //    var ciperText = Convert.FromBase64String(messageForm.content);
        //    BigInteger cipher = new BigInteger(ciperText);
        //    var messageBytes = BigInteger.ModPow(cipher, bigE, bigN);

        //    Console.WriteLine((messageBytes));
        //    var message = Encoding.Default.GetString(messageBytes.ToByteArray());
        //    Console.WriteLine(message);
        //    //Decode the base64message that was sent and use teh private key on it.

        //}


        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("dotnet run <option> <other arguments>");
                return;
            }
            if (args[0] == "sendMsg")
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("dotnet run <option> <email> <plaintext> ");
                    return;
                }
                var URL = args[1];
                var plainText = args[2];
                ProjectFunctions functions = new ProjectFunctions();
                functions.sendMsg(URL, plainText);

                Console.ReadLine();
            }
            if (args[0] == "sendKey")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <email>");
                    return;
                }
                var email = args[1];

                ProjectFunctions functions = new ProjectFunctions();
                functions.sendKey(email);
                
                Console.ReadLine();
            }
            if (args[0] == "getMsg")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <email>");
                    return;
                }

                var email = args[1];
                //getMsg(email);
                ProjectFunctions functions = new ProjectFunctions();
                functions.getMsg(email);
                Console.ReadLine();

            }
            if (args[0] == "getKey")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <email>");
                    return;
                }
                ProjectFunctions functions = new ProjectFunctions();
                functions.getkey(args[1]);
                //getkey(args[1]);

                Console.ReadLine(); 
            }
            if (args[0] == "keygen")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <bitsize>");
                    return;
                }
                //keyGen(Convert.ToInt32(args[1]));
                ProjectFunctions functions = new ProjectFunctions();
                functions.keyGen(Convert.ToInt32(args[1]));

                Console.ReadLine(); 
            }
        }
    
    }
}
