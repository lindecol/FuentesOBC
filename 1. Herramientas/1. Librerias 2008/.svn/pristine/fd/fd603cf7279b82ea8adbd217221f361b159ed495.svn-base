using System;
using System.Data;
using System.Configuration;
using System.Text;
/*using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;*/
//using System.IO.Compression;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Summary description for DataCompression
/// </summary>

#if NETCF20
namespace MovilidadCF.Compression
{
#else
namespace Desktop.Compression
{
#endif
    public class DataCompression
    {
        private static Encoding _encoding;

        static DataCompression()
        {
            _encoding = System.Text.Encoding.GetEncoding(1252);
        }



        public static MemoryStream Compress(string msg)
        {           
            MemoryStream memoryBuffer = new MemoryStream();
            GZipOutputStream outStream = new GZipOutputStream(memoryBuffer);

            byte[] buf = _encoding.GetBytes(msg);
            
            outStream.Write(buf, 0, buf.Length);
            outStream.Flush();
            outStream.Finish();

            memoryBuffer.Seek(0, SeekOrigin.Begin);

            //Informacion despues de la compresion            

            return memoryBuffer;

            //System.IO.MemoryStream stream = new MemoryStream();
            //CompactFormatter.CompactFormatter formater = new CompactFormatter.CompactFormatter(CompactFormatter.CFormatterMode.SURROGATE);
            //formater.Serialize(stream, msg);
            //size = msg.Length;
            //buffer = stream.GetBuffer();
        }

        public static string UnCompress(byte[] Buffer, int Size)
        {           
            MemoryStream ms = new MemoryStream(Buffer, 0, Size);
            ms.Seek(0, SeekOrigin.Begin);

            GZipInputStream inStream = new GZipInputStream(ms);

            byte[] buf2 = new byte[256];
            int count = Size;

            //TODO:Ojo con el mensaje cuando hay un error. Validar
            System.Text.StringBuilder message = new System.Text.StringBuilder(String.Empty);


            while (true)
            {
                int numRead = inStream.Read(buf2, 0, 256);
                if (numRead <= 0)
                {
                    break;
                }
                message.Append(_encoding.GetString(buf2, 0, numRead));
            }


            //System.Text.StringBuilder builder = new System.Text.StringBuilder();

            //builder.Append("");

            //builder.ToString();

            return message.ToString();
        }

        public static string ReadLine()
        {
            if (inStream == null)
            {
                return null;
            }            
            
            //TODO:Ojo con el mensaje cuando hay un error. Validar
            System.Text.StringBuilder message = new System.Text.StringBuilder(String.Empty);
            
            int iCodigo = -1;
            List<Byte> buf2 = new List<Byte>();
            while (true)            
            {
                if (inStream.CanRead)
                {
                    iCodigo = inStream.ReadByte();
                }
                else
                {
                    iCodigo = -1;
                }
                
                if (iCodigo == 13 || iCodigo < 0)
                {                    
                    break;
                }
                else
                {
                    buf2.Add(Convert.ToByte(iCodigo));                    
                }
            }
            if (buf2.Count == 0)
            {
                return null;
            }
            message.Append(_encoding.GetString(buf2.ToArray(), 0, buf2.Count));
            return message.ToString().Trim();
        }

        private static GZipInputStream inStream = null;
        private static MemoryStream ms = null;

        public static void UnCompressText(byte[] Buffer, int Size)
        {
            inStream = null;
            ms = null;
            ms = new MemoryStream(Buffer, 0, Size);
            ms.Seek(0, SeekOrigin.Begin);

            inStream = new GZipInputStream(ms);            

        }

        public static void Close()
        {
            if (inStream != null)
            {
                inStream.Close();
            }
        }
    }
}
