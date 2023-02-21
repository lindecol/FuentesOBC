using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace GenerarPDF
{
    public class ZPLConverterHelper
    {
        public static String getZplCode(string pathFirma)
        {
            try
            {
                Bitmap firma = new Bitmap(pathFirma);
                ZPLConverterHelper zp = new ZPLConverterHelper();
                return zp.ConvertFromImage(firma, true);
            }
            catch (Exception ex)
            {
                MovilidadCF.Logging.Logger.Write(ex);
                return null;
            }
        }

        // defaults black monochromatic threshold to 50%
        private int blackLimit = 380;
        private int total;
        private int widthBytes;
        private bool compressHex = false;

        private static readonly Dictionary<int, String> MapCode = new Dictionary<int, String>()
        {
            {1, "G"},
            {2, "H"},
            {3, "I"},
            {4, "J"},
            {5, "K"},
            {6, "L"},
            {7, "M"},
            {8, "N"},
            {9, "O" },
            {10, "P"},
            {11, "Q"},
            {12, "R"},
            {13, "S"},
            {14, "T"},
            {15, "U"},
            {16, "V"},
            {17, "W"},
            {18, "X"},
            {19, "Y"},
            {20, "g"},
            {40, "h"},
            {60, "i"},
            {80, "j" },
            {100, "k"},
            {120, "l"},
            {140, "m"},
            {160, "n"},
            {180, "o"},
            {200, "p"},
            {220, "q"},
            {240, "r"},
            {260, "s"},
            {280, "t"},
            {300, "u"},
            {320, "v"},
            {340, "w"},
            {360, "x"},
            {380, "y"},
            {400, "z" }
        };

        /// <summary>
        /// Converts a Bitmap into a ZPL ^GF[A] command input.  Can also specify ZPL header and footer to allow easy printing of label.
        /// </summary>
        /// <param name="image">Bitmap containing image source for ^GF[A] command input string.</param>
        /// <param name="addHeaderFooter">if true surrounds the command input string with the ZPL headers and footers required to generate valid ZPL.</param>
        /// <returns>^GF[A] command input string</returns>
        public String ConvertFromImage(Bitmap image, Boolean addHeaderFooter)
        {
            String hexAscii = CreateBody(image);
            if (compressHex)
            {
                hexAscii = EncodeHexAscii(hexAscii);
            }

            String zplCode = "^GFA," + total + "," + total + "," + widthBytes + ", " + hexAscii;

            if (addHeaderFooter)
            {
                String header = "^XA " + "^FO0,0^GFA," + total + "," + total + "," + widthBytes + ", ";
                String footer = "^FS" + "^XZ";
                zplCode = header + zplCode + footer;
            }
            return zplCode;
        }

        /// <summary>
        /// Driver for generating command input string.
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        private String CreateBody(Bitmap bitmapImage)
        {
            StringBuilder sb = new StringBuilder();
            int height = bitmapImage.Height;
            int width = bitmapImage.Width;
            int rgb, red, green, blue, index = 0;
            var auxBinaryChar = new char[] { '0', '0', '0', '0', '0', '0', '0', '0' };
            widthBytes = width / 8;
            if (width % 8 > 0)
            {
                widthBytes = (((int)(width / 8)) + 1);
            }
            else
            {
                widthBytes = width / 8;
            }
            this.total = widthBytes * height;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    rgb = bitmapImage.GetPixel(w, h).ToArgb();
                    red = (rgb >> 16) & 0x000000FF;
                    green = (rgb >> 8) & 0x000000FF;
                    blue = (rgb) & 0x000000FF;
                    char auxChar = '1';
                    int totalColor = red + green + blue;
                    if (totalColor > blackLimit)
                    {
                        auxChar = '0';
                    }
                    auxBinaryChar[index] = auxChar;
                    index++;
                    if (index == 8 || w == (width - 1))
                    {
                        sb.Append(FourByteBinary(new String(auxBinaryChar)));
                        auxBinaryChar = new char[] { '0', '0', '0', '0', '0', '0', '0', '0' };
                        index = 0;
                    }
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts binary into integer representation of two hex digits 
        /// </summary>
        /// <param name="binaryStr"></param>
        /// <returns></returns>
        private String FourByteBinary(String binaryStr)
        {
            int value = Convert.ToInt32(binaryStr, 2);
            if (value > 15)
            {
                return Convert.ToString(value, 16).ToUpper();
            }
            else
            {
                return "0" + Convert.ToString(value, 16).ToUpper();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private String EncodeHexAscii(String code)
        {
            int maxlinea = widthBytes * 2;
            StringBuilder sbCode = new StringBuilder();
            StringBuilder sbLinea = new StringBuilder();
            String previousLine = null;
            int counter = 1;
            char aux = code[0];
            bool firstChar = false;
            for (int i = 1; i < code.Length; i++)
            {
                if (firstChar)
                {
                    aux = code[i];
                    firstChar = false;
                    continue;
                }
                if (code[i] == '\n')
                {
                    if (counter >= maxlinea && aux == '0')
                    {
                        sbLinea.Append(",");
                    }
                    else if (counter >= maxlinea && aux == 'F')
                    {
                        sbLinea.Append("!");
                    }
                    else if (counter > 20)
                    {
                        int multi20 = (counter / 20) * 20;
                        int resto20 = (counter % 20);
                        sbLinea.Append(MapCode[multi20]);
                        if (resto20 != 0)
                        {
                            sbLinea.Append(MapCode[resto20]).Append(aux);
                        }
                        else
                        {
                            sbLinea.Append(aux);
                        }
                    }
                    else
                    {
                        sbLinea.Append(MapCode[counter]).Append(aux);
                    }
                    counter = 1;
                    firstChar = true;
                    if (sbLinea.ToString().Equals(previousLine))
                    {
                        sbCode.Append(":");
                    }
                    else
                    {
                        sbCode.Append(sbLinea.ToString());
                    }
                    previousLine = sbLinea.ToString();
                    sbLinea.Length = 0;
                    continue;
                }
                if (aux == code[i])
                {
                    counter++;
                }
                else
                {
                    if (counter > 20)
                    {
                        int multi20 = (counter / 20) * 20;
                        int resto20 = (counter % 20);
                        sbLinea.Append(MapCode[multi20]);
                        if (resto20 != 0)
                        {
                            sbLinea.Append(MapCode[resto20]).Append(aux);
                        }
                        else
                        {
                            sbLinea.Append(aux);
                        }
                    }
                    else
                    {
                        sbLinea.Append(MapCode[counter]).Append(aux);
                    }
                    counter = 1;
                    aux = code[i];
                }
            }
            return sbCode.ToString();
        }

        public void SetCompressHex(bool compressHex)
        {
            this.compressHex = compressHex;
        }

        /// <summary>
        /// Sets black pixel threshold for comparison of zpl pixels which determining whether to render or ignore pixels.  
        /// </summary>
        /// <param name="percentage">threshold percentage for comparison of pixels</param>
        /// <remarks>100+ percentage values will generate entirely black label.</remarks>
        public void SetBlacknessLimitPercentage(int percentage)
        {
            blackLimit = (percentage * 768 / 100);
        }

        public static List<string> CrearFirmaLista(string path, int xPos, int yPos)
        {
            try
            {
                List<string> firma = new List<string>();
                string cpclData;

                if (!File.Exists(path))
                {
                    return null;
                }
                Bitmap bmp = new Bitmap(path);

                if (bmp == null)
                {
                    return null;
                }

                //Make sure the width is divisible by 8
                int loopWidth = 8 - (bmp.Width % 8);
                if (loopWidth == 8)
                {
                    loopWidth = bmp.Width;
                }
                else
                {
                    loopWidth += bmp.Width;
                }

                cpclData = "";
                cpclData = cpclData + (String.Format("EG {0} {1} {2} {3} ", loopWidth / 8, bmp.Height, xPos, yPos));

                for (int y = 0; y <= bmp.Height - 1; y++)
                {
                    int bit = 128;
                    int currentValue = 0;
                    for (int x = 0; x <= loopWidth - 1; x++)
                    {
                        int intensity;

                        if (x < bmp.Width)
                        {
                            Color color = bmp.GetPixel(x, y);

                            int MyR = color.R;
                            int MyG = color.G;
                            int MyB = color.B;

                            intensity = Convert.ToInt32(255 - ((MyR + MyG + MyB) / 3));
                        }
                        else
                        {
                            intensity = 0;
                        }

                        if (intensity >= 128)
                        {
                            currentValue = currentValue = currentValue | bit;
                        }
                        bit = bit >> 1;
                        if (bit == 0)
                        {
                            if (cpclData.Length <= 20000)
                            {
                                cpclData = cpclData + (currentValue.ToString("X2"));
                            }
                            else
                            {
                                firma.Add(cpclData);
                                cpclData = "";
                            }
                            bit = 128;
                            currentValue = 0;
                        }
                        //x
                    }
                }
                if (cpclData != "")
                {
                    firma.Add(cpclData);
                }
                //y            
                return firma;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static string CrearFirma(string path, int xPos, int yPos)
        {
            try
            {                
                StringBuilder cpclData = new StringBuilder();

                if (!File.Exists(path))
                {
                    return null;
                }
                Bitmap bmp = new Bitmap(path);

                if (bmp == null)
                {
                    return null;
                }

                //Make sure the width is divisible by 8
                int loopWidth = 8 - (bmp.Width % 8);
                if (loopWidth == 8)
                {
                    loopWidth = bmp.Width;
                }
                else
                {
                    loopWidth += bmp.Width;
                }


                cpclData.Append((String.Format("EG {0} {1} {2} {3} ", loopWidth / 8, bmp.Height, xPos, yPos)));

                for (int y = 0; y <= bmp.Height - 1; y++)
                {
                    int bit = 128;
                    int currentValue = 0;
                    for (int x = 0; x <= loopWidth - 1; x++)
                    {
                        int intensity;

                        if (x < bmp.Width)
                        {
                            Color color = bmp.GetPixel(x, y);

                            int MyR = color.R;
                            int MyG = color.G;
                            int MyB = color.B;

                            intensity = Convert.ToInt32(255 - ((MyR + MyG + MyB) / 3));
                        }
                        else
                        {
                            intensity = 0;
                        }

                        if (intensity >= 128)
                        {
                            currentValue = currentValue = currentValue | bit;
                        }
                        bit = bit >> 1;
                        if (bit == 0)
                        {
                            cpclData.Append(currentValue.ToString("X2"));                            
                            bit = 128;
                            currentValue = 0;
                        }
                        //x
                    }
                }
               
                //y            
                return cpclData.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
