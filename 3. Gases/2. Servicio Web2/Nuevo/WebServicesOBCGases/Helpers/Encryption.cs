using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebServicesOBCGases.Helpers
{
    namespace Encryption
    {


        #region "  Symmetric"

        //'' <summary>
        //'' Symmetric encryption uses a single key to encrypt and decrypt.
        //'' Both parties (encryptor and decryptor) must share the same secret key.
        //'' </summary>
        public class Symmetric
        {

            private const string _DefaultIntializationVector = "%1Az=-@qT";
            private const int _BufferSize = 2048;

            public enum Provider
            {
                //'' <summary>
                //'' The Data Encryption Standard provider supports a 64 bit key only
                //'' </summary>
                DES,
                //'' <summary>
                //'' The Rivest Cipher 2 provider supports keys ranging from 40 to 128 bits, default is 128 bits
                //'' </summary>
                RC2,
                //'' <summary>
                //'' The Rijndael (also known as AES) provider supports keys of 128, 192, or 256 bits with a default of 256 bits
                //'' </summary>
                Rijndael,
                //'' <summary>
                //'' The TripleDES provider (also known as 3DES) supports keys of 128 or 192 bits with a default of 192 bits
                //'' </summary>
                TripleDES
            }

            //private Data _data;
            private Data _key;
            private Data _iv;
            private SymmetricAlgorithm _crypto;
            //private byte[] _EncryptedBytes;
            //private bool _UseDefaultInitializationVector;

            private Symmetric()
            {
            }

            //'' <summary>
            //'' Instantiates a new symmetric encryption object using the specified provider.
            //'' </summary>
            public Symmetric(Provider provider, bool useDefaultInitializationVector)
            {
                switch (provider)
                {
                    case Provider.TripleDES:
                        _crypto = new TripleDESCryptoServiceProvider();
                        break;
                    case Provider.Rijndael:
                        _crypto = new RijndaelManaged();
                        break;
                    case Provider.RC2:
                        _crypto = new RC2CryptoServiceProvider();
                        break;
                    case Provider.DES:
                        _crypto = new DESCryptoServiceProvider();
                        break;
                }

                //-- make sure key and IV are always set, no matter what
                this.Key = RandomKey();
                if (useDefaultInitializationVector)
                {
                    this.IntializationVector = new Data(_DefaultIntializationVector);
                }
                else
                {
                    this.IntializationVector = RandomInitializationVector();
                }
            }

            //'' <summary>
            //'' Key size in bytes. We use the default key size for any given provider; if you
            //'' want to force a specific key size, set this property
            //'' </summary>
            public int KeySizeBytes
            {
                get
                {
                    return _crypto.KeySize / 8;
                }
                set
                {
                    _crypto.KeySize = value * 8;
                    _key.MaxBytes = value;
                }
            }

            //'' <summary>
            //'' Key size in bits. We use the default key size for any given provider; if you
            //'' want to force a specific key size, set this property
            //'' </summary>
            public int KeySizeBits
            {
                get
                {
                    return _crypto.KeySize;
                }
                set
                {
                    _crypto.KeySize = value;
                    _key.MaxBits = value;
                }
            }

            //'' <summary>
            //'' The key used to encrypt/decrypt data
            //'' </summary>
            public Data Key
            {
                get
                {
                    return _key;
                }
                set
                {
                    _key = value;
                    _key.MaxBytes = _crypto.LegalKeySizes[0].MaxSize / 8;
                    _key.MinBytes = _crypto.LegalKeySizes[0].MinSize / 8;
                    _key.StepBytes = _crypto.LegalKeySizes[0].SkipSize / 8;
                }
            }

            //'' <summary>
            //'' Using the default Cipher Block Chaining (CBC) mode, all data blocks are processed using
            //'' the value derived from the previous block; the first data block has no previous data block
            //'' to use, so it needs an InitializationVector to feed the first block
            //'' </summary>
            public Data IntializationVector
            {
                get
                {
                    return _iv;
                }
                set
                {
                    _iv = value;
                    _iv.MaxBytes = _crypto.BlockSize / 8;
                    _iv.MinBytes = _crypto.BlockSize / 8;
                }
            }

            //'' <summary>
            //'' generates a random Initialization Vector, if one was not provided
            //'' </summary>
            public Data RandomInitializationVector()
            {
                _crypto.GenerateIV();
                Data d = new Data(_crypto.IV);
                return d;
            }

            //'' <summary>
            //'' generates a random Key, if one was not provided
            //'' </summary>
            public Data RandomKey()
            {
                _crypto.GenerateKey();
                Data d = new Data(_crypto.Key);
                return d;
            }

            //'' <summary>
            //'' Ensures that _crypto object has valid Key and IV
            //'' prior to any attempt to encrypt/decrypt anything
            //'' </summary>
            private void ValidateKeyAndIv(bool isEncrypting)
            {
                if (_key.IsEmpty)
                {
                    if (isEncrypting)
                    {
                        _key = RandomKey();
                    }
                    else
                    {
                        throw (new CryptographicException("No key was provided for the decryption operation!"));
                    }
                }
                if (_iv.IsEmpty)
                {
                    if (isEncrypting)
                    {
                        _iv = RandomInitializationVector();
                    }
                    else
                    {
                        throw (new CryptographicException("No initialization vector was provided for the decryption operation!"));
                    }
                }
                _crypto.Key = _key.Bytes;
                _crypto.IV = _iv.Bytes;
            }

            //'' <summary>
            //'' Encrypts the specified Data using provided key
            //'' </summary>
            public Data Encrypt(Data d, Data key)
            {
                this.Key = key;
                return Encrypt(d);
            }

            //'' <summary>
            //'' Encrypts the specified Data using preset key and preset initialization vector
            //'' </summary>
            public Data Encrypt(Data d)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                ValidateKeyAndIv(true);

                CryptoStream cs = new CryptoStream(ms, _crypto.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(d.Bytes, 0, d.Bytes.Length);
                cs.Close();
                ms.Close();

                return new Data(ms.ToArray());
            }

            //'' <summary>
            //'' Encrypts the stream to memory using provided key and provided initialization vector
            //'' </summary>
            public Data Encrypt(Stream s, Data key, Data iv)
            {
                this.IntializationVector = iv;
                this.Key = key;
                return Encrypt(s);
            }

            //'' <summary>
            //'' Encrypts the stream to memory using specified key
            //'' </summary>
            public Data Encrypt(Stream s, Data key)
            {
                this.Key = key;
                return Encrypt(s);
            }

            //'' <summary>
            //'' Encrypts the specified stream to memory using preset key and preset initialization vector
            //'' </summary>
            public Data Encrypt(Stream s)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                byte[] b = new byte[_BufferSize + 1];
                int i;

                ValidateKeyAndIv(true);

                CryptoStream cs = new CryptoStream(ms, _crypto.CreateEncryptor(), CryptoStreamMode.Write);
                i = s.Read(b, 0, _BufferSize);
                do
                {
                    cs.Write(b, 0, i);
                    i = s.Read(b, 0, _BufferSize);
                } while (i > 0);

                cs.Close();
                ms.Close();

                return new Data(ms.ToArray());
            }

            //'' <summary>
            //'' Decrypts the specified data using provided key and preset initialization vector
            //'' </summary>
            public Data Decrypt(Data encryptedData, Data key)
            {
                this.Key = key;
                return Decrypt(encryptedData);
            }

            //'' <summary>
            //'' Decrypts the specified stream using provided key and preset initialization vector
            //'' </summary>
            public Data Decrypt(Stream encryptedStream, Data key)
            {
                this.Key = key;
                return Decrypt(encryptedStream);
            }

            //'' <summary>
            //'' Decrypts the specified stream using preset key and preset initialization vector
            //'' </summary>
            public Data Decrypt(Stream encryptedStream)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                byte[] b = new byte[_BufferSize + 1];

                ValidateKeyAndIv(false);
                CryptoStream cs = new CryptoStream(encryptedStream, _crypto.CreateDecryptor(), CryptoStreamMode.Read);

                int i;
                i = cs.Read(b, 0, _BufferSize);

                do
                {
                    ms.Write(b, 0, i);
                    i = cs.Read(b, 0, _BufferSize);
                } while (i > 0);
                cs.Close();
                ms.Close();

                return new Data(ms.ToArray());
            }

            //'' <summary>
            //'' Decrypts the specified data using preset key and preset initialization vector
            //'' </summary>
            public Data Decrypt(Data encryptedData)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(encryptedData.Bytes, 0, encryptedData.Bytes.Length);
                byte[] b = new byte[encryptedData.Bytes.Length - 1 + 1];

                ValidateKeyAndIv(false);
                CryptoStream cs = new CryptoStream(ms, _crypto.CreateDecryptor(), CryptoStreamMode.Read);

                try
                {
                    cs.Read(b, 0, encryptedData.Bytes.Length - 1);
                }
                catch (CryptographicException ex)
                {
                    throw (new CryptographicException("Unable to decrypt data. The provided key may be invalid.", ex));
                }
                finally
                {
                    cs.Close();
                }
                return new Data(b);
            }

        }

        #endregion

        #region "  Data"

        //'' <summary>
        //'' represents Hex, Byte, Base64, or String data to encrypt/decrypt;
        //'' use the .Text property to set/get a string representation
        //'' use the .Hex property to set/get a string-based Hexadecimal representation
        //'' use the .Base64 to set/get a string-based Base64 representation
        //'' </summary>
        public class Data
        {
            private byte[] _b;
            private int _MaxBytes;
            private int _MinBytes;
            private int _StepBytes;

            //'' <summary>
            //'' Determines the default text encoding across ALL Data instances
            //'' </summary>
            public static Encoding DefaultEncoding = System.Text.Encoding.GetEncoding("Windows-1252");

            //'' <summary>
            //'' Determines the default text encoding for this Data instance
            //'' </summary>
            public Encoding Encoding;

            //'' <summary>
            //'' Creates new, empty encryption data
            //'' </summary>
            public Data()
            {
                _MaxBytes = 0;
                _MinBytes = 0;
                _StepBytes = 0;
                Encoding = DefaultEncoding;

            }

            //'' <summary>
            //'' Creates new encryption data with the specified byte array
            //'' </summary>
            public Data(byte[] b)
            {
                _MaxBytes = 0;
                _MinBytes = 0;
                _StepBytes = 0;
                Encoding = DefaultEncoding;

                _b = b;
            }

            //'' <summary>
            //'' Creates new encryption data with the specified string;
            //'' will be converted to byte array using default encoding
            //'' </summary>
            public Data(string s)
            {
                _MaxBytes = 0;
                _MinBytes = 0;
                _StepBytes = 0;
                Encoding = DefaultEncoding;

                this.Text = s;
            }

            //'' <summary>
            //'' Creates new encryption data using the specified string and the
            //'' specified encoding to convert the string to a byte array.
            //'' </summary>
            public Data(string s, System.Text.Encoding encoding)
            {
                _MaxBytes = 0;
                _MinBytes = 0;
                _StepBytes = 0;
                Encoding = DefaultEncoding;

                this.Encoding = encoding;
                this.Text = s;
            }

            //'' <summary>
            //'' returns true if no data is present
            //'' </summary>
            public bool IsEmpty
            {
                get
                {
                    if (_b == null)
                    {
                        return true;
                    }
                    if (_b.Length == 0)
                    {
                        return true;
                    }
                    return false;
                }
            }

            //'' <summary>
            //'' allowed step interval, in bytes, for this data; if 0, no limit
            //'' </summary>
            public int StepBytes
            {
                get
                {
                    return _StepBytes;
                }
                set
                {
                    _StepBytes = value;
                }
            }

            //'' <summary>
            //'' allowed step interval, in bits, for this data; if 0, no limit
            //'' </summary>
            public int StepBits
            {
                get
                {
                    return _StepBytes * 8;
                }
                set
                {
                    _StepBytes = value / 8;
                }
            }

            //'' <summary>
            //'' minimum number of bytes allowed for this data; if 0, no limit
            //'' </summary>
            public int MinBytes
            {
                get
                {
                    return _MinBytes;
                }
                set
                {
                    _MinBytes = value;
                }
            }

            //'' <summary>
            //'' minimum number of bits allowed for this data; if 0, no limit
            //'' </summary>
            public int MinBits
            {
                get
                {
                    return _MinBytes * 8;
                }
                set
                {
                    _MinBytes = value / 8;
                }
            }

            //'' <summary>
            //'' maximum number of bytes allowed for this data; if 0, no limit
            //'' </summary>
            public int MaxBytes
            {
                get
                {
                    return _MaxBytes;
                }
                set
                {
                    _MaxBytes = value;
                }
            }

            //'' <summary>
            //'' maximum number of bits allowed for this data; if 0, no limit
            //'' </summary>
            public int MaxBits
            {
                get
                {
                    return _MaxBytes * 8;
                }
                set
                {
                    _MaxBytes = value / 8;
                }
            }

            //'' <summary>
            //'' Returns the byte representation of the data;
            //'' This will be padded to MinBytes and trimmed to MaxBytes as necessary!
            //'' </summary>
            public byte[] Bytes
            {
                get
                {
                    if (_MaxBytes > 0)
                    {
                        if (_b.Length > _MaxBytes)
                        {
                            byte[] b = new byte[_MaxBytes - 1 + 1];
                            int j;
                            for (j = 0; j <= b.Length - 1; j++)
                            {
                                b[j] = _b[j];
                            }
                            _b = b;
                        }
                    }
                    if (_MinBytes > 0)
                    {
                        if (_b.Length < _MinBytes)
                        {
                            byte[] b = new byte[_MinBytes - 1 + 1];
                            int j;
                            for (j = 0; j <= _b.Length - 1; j++)
                            {
                                b[j] = _b[j];
                            }
                            _b = b;
                        }
                    }
                    return _b;
                }
                set
                {
                    _b = value;
                }
            }

            //'' <summary>
            //'' Sets or returns text representation of bytes using the default text encoding
            //'' </summary>
            public string Text
            {
                get
                {
                    if (_b == null)
                    {
                        return "";
                    }
                    else
                    {
                        //-- need to handle nulls here; oddly, C# will happily convert
                        //-- nulls into the string whereas VB stops converting at the
                        //-- first null!
                        int i = Array.IndexOf(_b, ((byte)(0)));
                        if (i >= 0)
                        {
                            return this.Encoding.GetString(_b, 0, i);
                        }
                        else
                        {
                            return this.Encoding.GetString(_b);
                        }
                    }
                }
                set
                {
                    _b = this.Encoding.GetBytes(value);
                }
            }

            //'' <summary>
            //'' Sets or returns Hex string representation of this data
            //'' </summary>
            public string Hex
            {
                get
                {
                    return Utils.ToHex(_b);
                }
                set
                {
                    _b = Utils.FromHex(value);
                }
            }

            //'' <summary>
            //'' Sets or returns Base64 string representation of this data
            //'' </summary>
            public string Base64
            {
                get
                {
                    return Utils.ToBase64(_b);
                }
                set
                {
                    _b = Utils.FromBase64(value);
                }
            }

            //'' <summary>
            //'' Returns text representation of bytes using the default text encoding
            //'' </summary>
            public new string ToString()
            {
                return this.Text;
            }

            //'' <summary>
            //'' returns Base64 string representation of this data
            //'' </summary>
            public string ToBase64()
            {
                return this.Base64;
            }

            //'' <summary>
            //'' returns Hex string representation of this data
            //'' </summary>
            public string ToHex()
            {
                return this.Hex;
            }

        }

        #endregion

        #region "  Utils"

        //'' <summary>
        //'' Friend class for shared utility methods used by multiple Encryption classes
        //'' </summary>
        internal class Utils
        {

            //'' <summary>
            //'' converts an array of bytes to a string Hex representation
            //'' </summary>
            internal static string ToHex(byte[] ba)
            {
                if (ba == null || ba.Length == 0)
                {
                    return "";
                }
                const string HexFormat = "{0:X2}";
                StringBuilder sb = new StringBuilder();
                foreach (byte b in ba)
                {
                    sb.Append(string.Format(HexFormat, b));
                }
                return sb.ToString();
            }

            //'' <summary>
            //'' converts from a string Hex representation to an array of bytes
            //'' </summary>
            internal static byte[] FromHex(string hexEncoded)
            {
                if (hexEncoded == null || hexEncoded.Length == 0)
                {
                    return null;
                }
                try
                {
                    int l = Convert.ToInt32(hexEncoded.Length / 2);
                    byte[] b = new byte[l - 1 + 1];
                    for (int i = 0; i <= l - 1; i++)
                    {
                        b[i] = Convert.ToByte(hexEncoded.Substring(i * 2, 2), 16);
                    }
                    return b;
                }
                catch (Exception ex)
                {
                    throw (new System.FormatException("The provided string does not appear to be Hex encoded:" + Environment.NewLine + hexEncoded + Environment.NewLine, ex));
                }
            }

            //'' <summary>
            //'' converts from a string Base64 representation to an array of bytes
            //'' </summary>
            internal static byte[] FromBase64(string base64Encoded)
            {
                if (base64Encoded == null || base64Encoded.Length == 0)
                {
                    return null;
                }
                try
                {
                    return Convert.FromBase64String(base64Encoded);
                }
                catch (System.FormatException ex)
                {
                    throw (new System.FormatException("The provided string does not appear to be Base64 encoded:" + Environment.NewLine + base64Encoded + Environment.NewLine, ex));
                }
            }

            //'' <summary>
            //'' converts from an array of bytes to a string Base64 representation
            //'' </summary>
            internal static string ToBase64(byte[] b)
            {
                if (b == null || b.Length == 0)
                {
                    return "";
                }
                return Convert.ToBase64String(b);
            }

            //'' <summary>
            //'' retrieve an element from an XML string
            //'' </summary>
            internal static string GetXmlElement(string xml, string element)
            {
                Match m;
                m = Regex.Match(xml, "<" + element + ">(?<Element>[^>]*)</" + element + ">", RegexOptions.IgnoreCase);
                if (m == null)
                {
                    throw (new Exception("Could not find <" + element + "></" + element + "> in provided Public Key XML."));
                }
                return m.Groups["Element"].ToString();
            }

            //'' <summary>
            //'' Returns the specified string value from the application .config file
            //'' </summary>
            internal static string GetConfigString(string key, bool isRequired)
            {

                string s = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get(key));
                if (s == null)
                {
                    if (isRequired)
                    {
                        throw (new System.Configuration.ConfigurationErrorsException("key <" + key + "> is missing from .config file"));
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return s;
                }
            }

            internal static string WriteConfigKey(string key, string value)
            {
                string s = "<add key=\"{0}\" value=\"{1}\" />" + Environment.NewLine;
                return string.Format(s, key, value);
            }

            internal static string WriteXmlElement(string element, string value)
            {
                string s = "<{0}>{1}</{0}>" + Environment.NewLine;
                return string.Format(s, element, value);
            }

            internal static string WriteXmlNode(string element, bool isClosing)
            {
                string s;
                if (isClosing)
                {
                    s = "</{0}>" + Environment.NewLine;
                }
                else
                {
                    s = "<{0}>" + Environment.NewLine;
                }
                return string.Format(s, element);
            }

        }

        #endregion

    }
}