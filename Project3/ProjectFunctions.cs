//@author: Isaias Villalobos
//@date: 11/19/2019
//@description: This class will handle generation of public key and private key, sending a key to a server, 
//              sending a message, decoding a message, getting a message, and getting a key.
//              There are two inner classes which will help convert the JSON object recieved from the server to an object.
//              The other class will help to convert an object into a JSON formatted string to send to the server.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;

namespace Project3
{
    /*
     *This class will handle converting a JSON object into a object. 
     */
    public class KeyForm
    {
        public String email { get; set; }
        public String key { get; set; }
    }

    /*
     *This class will help setting the field of this 
     *object to be converted later and sent to the server as JSON
     */
    public class MessageForm
    {
        public String email { get; set; }
        public String content { get; set; }
    }
    /*
     *This class is to hold all the emails/usernames that will be associated with 1 key.
     * This will be written to file later on.
     */
    public class Users
    {
        public String keys;
        public List<String> userNames = new List<String>();
    }

    /*
    *This class will contain many useful functions that are used throughout the program.
    */
    class ProjectFunctions
    {
        //These are variables to be used later when you need the E/D values or N values.
        public static BigInteger bigE;
        public static BigInteger bigN;

        /*
        * @input: an integer size for the key.
        * @output: void
        * @descr:  this will generate a keypair (public and private keys) and store them locally on the disk 
        *          (in ﬁles called public.key and private.key respectively). 
        */
        public void keyGen(int keySize)
        {
            try
            {
                var randomNum = new Random();
                var psize = keySize / 2;
                var percentage = randomNum.Next(-keySize / 10, keySize / 10);
                var a = psize - percentage / 8 * 8;

                PrimeFunction primeFunctions = new PrimeFunction();
                BigInteger p = primeFunctions.parallelPrimeFunction(a);
                var b = keySize - p.ToByteArray().Length * 8;
                BigInteger q = primeFunctions.parallelPrimeFunction(b);
                BigInteger N = p * q;
                BigInteger r = (p - 1) * (q - 1);
                BigInteger E = 65537;
                BigInteger D = ModInverseExtension.modInverse(E, r);

                var publicKeyFile = File.Create("public.key");
                //var privateKeyFile = File.Create("private.key");

                var arrayE = E.ToByteArray();
                var arrayN = N.ToByteArray();

                var e = BitConverter.GetBytes(arrayE.Length);
                var n = BitConverter.GetBytes(arrayN.Length);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(e, 0, e.Length);
                    Array.Reverse(n, 0, n.Length);
                }

                var publicKey = new Byte[4 + arrayE.Length + 4 + arrayN.Length];
                e.CopyTo(publicKey, 0);
                arrayE.CopyTo(publicKey, 4);
                n.CopyTo(publicKey, 4 + arrayE.Length);
                arrayN.CopyTo(publicKey, 4 + arrayE.Length + 4);
                var encodedPublicKey = Convert.ToBase64String(publicKey);

                try
                {
                    publicKeyFile.Write(Encoding.Default.GetBytes(encodedPublicKey), 0, encodedPublicKey.Length);
                }
                finally
                {
                    publicKeyFile.Close();
                }

                var arrayD = D.ToByteArray();
                var d = BitConverter.GetBytes(arrayD.Length);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(d, 0, d.Length);
                }

                var privateKey = new Byte[4 + arrayD.Length + 4 + arrayN.Length];

                d.CopyTo(privateKey, 0);
                arrayD.CopyTo(privateKey, 4);
                n.CopyTo(privateKey, 4 + arrayD.Length);
                arrayN.CopyTo(privateKey, 4 + arrayN.Length + 4);

                var encodedPrivateKey = Convert.ToBase64String(privateKey);
                try
                {
                    //privateKeyFile.Write(Encoding.Default.GetBytes(encodedPrivateKey), 0, encodedPrivateKey.Length);
                    Users names = new Users();
                    names.keys = encodedPrivateKey;
                    File.WriteAllText("private.key", JsonConvert.SerializeObject(names));
                }
                catch (UnauthorizedAccessException g)
                {
                    Console.WriteLine(g.Message);
                }
                catch (DirectoryNotFoundException g)
                {
                    Console.WriteLine(g.Message);
                }
            }
            catch (ArgumentNullException a)
            {
                Console.WriteLine(a.Message);
            }
        }

        /*
        * @input: an string representing the email of the user you want to send a key to.
        * @output: void
        * @descr:  this option base64 encodes the public key and uploads it to the server. 
        *          The server will then register this email address as a valid receiver of messages. 
        *          The private key will remain locally.
        */
        public async void sendKey(String email)
        {
            try
            {
                HttpClient client = new HttpClient();
                var URL = "http://kayrun.cs.rit.edu:5000/Key/" + email;
                KeyForm keyForm = new KeyForm();
                var names = JsonConvert.DeserializeObject<Users>(File.ReadAllText("private.key"));
                names.userNames.Add(email); // add the email that will recieve a message to the list
                File.WriteAllText("private.key", JsonConvert.SerializeObject(names));
                keyForm.email = email;
                keyForm.key = File.ReadAllText("public.key");
                var content = new StringContent(JsonConvert.SerializeObject(keyForm), Encoding.Default, "application/json");
                var result = await client.PutAsync(URL, content);
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (UnauthorizedAccessException g)
            {
                Console.WriteLine(g.Message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /*
         *  @input: A string representing the email, a string representing the message 
         *  @output: void
         *  @decr: this will base64 encode a message for a user in the to
                   field. If you do not have a public key for that particular user, you should show an
                    error message indicating that the key must be downloaded first.
         */
        public async void sendMsg(String email, String message)
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(message);
                BigInteger encryptedMessage = new BigInteger(data);

                //make sure to have key from a user first
                if (!File.Exists(email + ".key"))
                {
                    Console.WriteLine("Key does not exist for " + email);
                    return;
                }

                //call decode key on the public key to get e and n
                decodeKey(File.ReadAllText(email + ".key"));

                var encryptedText = BigInteger.ModPow(encryptedMessage, bigE, bigN);
                var base64EncodedEncryptedText = Convert.ToBase64String(encryptedText.ToByteArray());
                var URL = "http://kayrun.cs.rit.edu:5000/Message/" + email;

                HttpClient client = new HttpClient();
                MessageForm messageForm = new MessageForm();
                messageForm.email = email;
                messageForm.content = base64EncodedEncryptedText;
                var content = new StringContent(JsonConvert.SerializeObject(messageForm), Encoding.Default, "application/json");

                var result = await client.PutAsync(URL, content);
                result.EnsureSuccessStatusCode();
                Console.WriteLine("Message Written");
            }
            catch(HttpRequestException h)
            {
                Console.WriteLine(h.Message);
            }
            catch(FileNotFoundException f)
            {
                Console.WriteLine(f.Message);
            }
            catch(DirectoryNotFoundException d)
            {
                Console.WriteLine(d.Message);
            }

        }

        /*
         *  @input: A string representing the key, which may be a public key or private key
         *  @output: void
         *  @decr: This function will convert a key to a base64 string and then extract 'e' and 'n' for a public key
         *         This function will also extract 'd' and 'n' from a private key.
         */
        public void decodeKey(String key)
        {
            try
            {
                //This is for decoding little e
                byte[] data = Convert.FromBase64String(key);
                var e = data.Take(4).ToArray();
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(e, 0, e.Length);
                }
                var eVal = BitConverter.ToInt32(e, 0);
                var extractByteE = data.Skip(4).Take(eVal);
                bigE = new BigInteger(extractByteE.ToArray());

                //This part is for decoding little n
                var n = data.Skip(eVal + 4).Take(4).ToArray();

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(n, 0, n.Length);
                }
                var nVal = BitConverter.ToInt32(n, 0);
                var extractByteN = data.Skip(8 + eVal).Take(nVal);
                bigN = new BigInteger(extractByteN.ToArray());
            }
            catch (ArgumentNullException a)
            {
                Console.WriteLine(a.Message);
            }
        }

        /*
         *  @input: A string representing a user's email
         *  @output: void
         *  @decr: This function will get the public key for another user.
         */
        public async void getKey(String emailAddress)
        {
            try
            {
                //Create client to get key from server
                HttpClient client = new HttpClient();
                var URL = "http://kayrun.cs.rit.edu:5000/Key/" + emailAddress;
                var result = await client.GetAsync(URL);
                result.EnsureSuccessStatusCode();
                var str = await result.Content.ReadAsStringAsync();

                //Extract information from the response from the server, should contain an email and key
                var keyform = JsonConvert.DeserializeObject<KeyForm>(str);

                //Check to see if key can be retrieved
                if (keyform.key == null)
                {
                    Console.WriteLine("There is an error with retrieving the key for: " + emailAddress);
                }

                //Decode the key
                decodeKey(keyform.key);

                var keyFile = File.Create(keyform.email.ToString() + ".key");
                try
                {
                    keyFile.Write(Encoding.Default.GetBytes(keyform.key), 0, keyform.key.Length);
                }
                finally
                {
                    keyFile.Close();
                }
            }
            catch(HttpRequestException h)
            {
                Console.WriteLine(h.Message);
            }
            catch (ArgumentNullException a)
            {
                Console.WriteLine(a.Message);
            }
        }

        /*
         *  @input: A string representing a user's email
         *  @output: void
         *  @decr: This function will get a message from another user.
         */
        public async void getMsg(String emailAddress)
        {
            try
            {
                HttpClient client = new HttpClient();
                var URL = "http://kayrun.cs.rit.edu:5000/Message/" + emailAddress;
                var result = await client.GetAsync(URL);
                result.EnsureSuccessStatusCode();
                MessageForm messageForm = JsonConvert.DeserializeObject<MessageForm>(await result.Content.ReadAsStringAsync());

                var emailsUsers = JsonConvert.DeserializeObject<Users>(File.ReadAllText("private.key"));

                //Going to check if an email has been associated with a key, and see if the message from the email can be decoded.
                //If there is a key available.
                if (emailsUsers.userNames.Contains(emailAddress))
                {
                    decodeKey(emailsUsers.keys);
                }
                else
                {
                    Console.WriteLine("Cannot decode the message, private key for: " + emailAddress + " is missing.");
                    return;
                }

                var ciperText = Convert.FromBase64String(messageForm.content);
                BigInteger cipher = new BigInteger(ciperText);
                var messageBytes = BigInteger.ModPow(cipher, bigE, bigN);
                var message = Encoding.Default.GetString(messageBytes.ToByteArray());
                Console.WriteLine(message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(ArgumentNullException a)
            {
                Console.WriteLine(a.Message);
            }
            catch(DivideByZeroException d)
            {
                Console.WriteLine(d.Message);
            }
            catch(HttpRequestException h)
            {
                Console.WriteLine(h.Message);
            }
        }
    }
}
