/*********************************************************************/
/*                                                                   */
/* Morovia C# Functions, part of Morovia Font Tools V3.0            */
/* (c)2000-2004 Morovia Corporation. All rights reserved.            */
/* Visit http://www.morovia.com/font/ for more information.          */
/*                                                                   */
/* You may incorporate this source code in your application as long  */
/* as you own a perpetual license to the font product.               */
/* Distributing the source code, as well as its derivatives, require */
/* a developer license.                                              */
/* Please refer to the license agreement for details.                */
/* Last update: April 2005                                           */
/*********************************************************************/
using System;

namespace MoroviaFontTools
{
    /// <summary>
    /// Summary description for MoroviaFontTools.
    /// </summary>
    public class FontTools
    {
        const int MAX_LEN = 256;
        string szOutput;

        public FontTools()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool IsNumeric(string Instr)
        {
            int idx1 = 0;
            int strLen = Instr.Length;
            char[] InChar = Instr.ToCharArray();
            bool Result = true;
            if (strLen > 0)
            {
                for (idx1 = 0; idx1 < strLen; idx1++)
                {
                    if (Char.IsNumber(InChar[idx1]) == false)
                        Result = false;
                }
            }
            else
            {
                Result = false;
            }
            return Result;
        }

        private string SpecialChar(string szInpara)
        {
            string test;
            int i, nLen;
            int nTemp;
            string szTemp = "";
            nLen = szInpara.Length;
            for (i = 0; i < nLen; i++)
            {
                if (szInpara[i] == '\\')
                {
                    if (i + 1 < nLen && szInpara[i + 1] == '\\')
                    {
                        szTemp += '\\';
                        i++;
                    }
                    else if (i + 3 < nLen && char.IsNumber(szInpara[i + 1]) && char.IsNumber(szInpara[i + 2]) && char.IsNumber(szInpara[i + 3]))
                    {
                        test = szInpara.Substring(i + 1, 3);
                        nTemp = Convert.ToInt32(test);
                        if (nTemp == 0)
                            szTemp += (char)240;	// use F0 to replace 0
                        else
                            szTemp += (char)nTemp;
                        i = i + 3;
                    }
                    else
                        szTemp += szInpara[i];
                }
                else
                    szTemp += szInpara[i];
            }
            return szTemp;
        }

        /*---------------------------------------------------------------------*/
        /* Function: Code39 - applies to Morovia Code39 Fontware               */
        /* Code39(text) Converts the input text into a Code 39 barcode string. The    */
        /* function throws off characters not in the Code 39 character set,    */
        /* and adds start/stop characters.                                     */
        /*----------------------------------------------------------------------*/
        public string Code39(string szInpara)
        {
            int i, nPos;
            string szBuffer = "";
            string szCharSet = "0123456789.+-/ $%ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            i = 0;
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (szInpara[i] == ' ')
                    szBuffer += "=";
                else if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            return "*" + szBuffer + "*";

        }
        /*--------------------------Code39Mod43---------------------------------------*/
        /*Converts the input text into a Code39 extended symbol. This function */
        /*should be used to format Morovia code39 font, not Code39 full ASCII font.*/
        /*The text can be any combinations of ASCII characters. Note that the symbol */
        /*generated is an extended Code39, and the scanner must be put in Code39 */
        /*extended mode to read the symbol properly.*/
        /*----------------------------------------------------------------------------*/
        public string Code39Mod43(string szInpara)
        {
            int i = 0, nPos, nCheckSum = 0;
            string inPara = "";
            string szBuffer = "";
            string szSwap = "";
            string szCharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
            string szMappingSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-.=$/+%";
            i = 0;
            inPara = szInpara;
            while (i < inPara.Length)
            {
                nPos = szCharSet.IndexOf(inPara[i]);
                if (nPos >= 0)
                {
                    szBuffer += szMappingSet[nPos];
                    nCheckSum = nCheckSum + nPos;
                }
                i = i + 1;
            }
            nCheckSum = nCheckSum % 43;
            szSwap = '*' + szBuffer + szMappingSet[nCheckSum] + '*';
            return szSwap;
        }
        /*--------------------------------Code39Ascii----------------------------------------------*/
        /*Converts the input text into a Code39 extended symbol. This function should be */
        /*used to format Morovia code39 font, not Code39 full ASCII font. The text can be*/
        /*any combinations of ASCII characters. Note that the symbol generated is an extended*/
        /*Code39, and the scanner must be put in Code39 extended mode to read the symbol properly.*/
        /*-----------------------------------------------------------------------------------------*/
        public string Code39Ascii(string szInpara)
        {
            int i = 0, nPos;
            string szBuffer = "";
            string szSpecial = "";
            string szWap = "";
            szSpecial = SpecialChar(szInpara);
            szInpara = szSpecial;
            string szCharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. " + (char)240;
            string szMappingSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-.=" + (char)240;
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szMappingSet[nPos];
                else if (szInpara[i] == 0)
                    szBuffer += "%U";
                else if (szInpara[i] == ' ')   //control characters
                    szBuffer += '=';
                else if (szInpara[i] == '/')
                    szBuffer += "/O";
                else if (szInpara[i] == ':')
                    szBuffer += "/Z";
                else if (szInpara[i] == 64)
                    szBuffer += "%V";
                else if (szInpara[i] == 96)
                    szBuffer += "%W";
                else if (szInpara[i] > 0 && szInpara[i] <= 26)
                {
                    szBuffer += '$';
                    szBuffer += (char)(szInpara[i] + 'A' - 1);
                }
                else if (szInpara[i] > 32 && szInpara[i] <= 46)
                {
                    szBuffer += '/';
                    szBuffer += (char)((szInpara[i] % 32) + 'A' - 1);
                }
                else if (szInpara[i] >= 97 && szInpara[i] <= 122)
                {
                    szBuffer += '+';
                    szBuffer += (char)((szInpara[i] % 32) + 'A' - 1);
                }
                else if (szInpara[i] >= 27 && szInpara[i] <= 31)
                {
                    szBuffer += '%';
                    szBuffer += (char)(szInpara[i] - 27 + 'A');
                }
                else if (szInpara[i] >= 59 && szInpara[i] <= 63)
                {
                    szBuffer += '%';
                    szBuffer += (char)(szInpara[i] - 59 + 'F');
                }
                else if (szInpara[i] >= 91 && szInpara[i] <= 95)
                {
                    szBuffer += '%';
                    szBuffer += (char)(szInpara[i] - 91 + 'K');
                }
                else if (szInpara[i] >= 123 && szInpara[i] <= 127)
                {
                    szBuffer += '%';
                    szBuffer += (char)(szInpara[i] - 123 + 'P');
                }
                i = i + 1;
            }

            //replace NULL char
            i = 0;
            while (i < szBuffer.Length)
            {
                if (szBuffer[i] == 240)
                    szWap += "%U";
                else
                    szWap += szBuffer[i];
                i++;
            }

            return '[' + szWap + ']';
        }
        /*---------------------------------Code39Extended------------------------------------*/
        /*Converts the input text into a Code39 extended symbol. It accepts any ASCII */
        /*characters as input. The only difference from function Code39Ascii is the former */
        /*is designed to work with Morovia Code39(Full ASCII) font and the latter is designed*/
        /*to work with Morovia Code39 font.*/
        /*-----------------------------------------------------------------------------------*/
        public string Code39Extended(string szInpara)
        {
            int i = 0; char ch;
            string szSwap = "";
            string szSpecial = "";
            string szOutPut = "";
            szSpecial = SpecialChar(szInpara);
            szSwap = szSpecial;

            for (i = 0; i < szSwap.Length; i++)
            {
                ch = szSwap[i];
                if (ch == 240)
                    szOutPut += (char)0xC0;
                else if (ch > 0 && ch <= 0x1F)
                    szOutPut += (char)(0xC0 + ch);
                else if (ch == ' ')  //space is mapped to equal sign
                    szOutPut += '=';
                else if (ch == '*')
                    szOutPut += (char)0xF4;
                else if (ch == '=')
                    szOutPut += (char)0xF0;
                else if (ch == '[')
                    szOutPut += (char)0xF1;
                else if (ch == ']')
                    szOutPut += (char)0xF2;
                else if (ch == 0x7F)
                    szOutPut += (char)0xE0;
                else
                    szOutPut += ch;
            }

            szOutPut = '[' + szOutPut + ']';
            return szOutPut;
        }
        /*--------------------------------Code93-----------------------------------------------*/
        /*Converts the input text into a Code93 symbol. It accepts any ASCII character input,*/
        /*taking care of the check character calculation and adding start/stop characters into*/
        /*the string.*/
        /*------------------------------------------------------------------------------------*/
        public string Code93(string szInpara)
        {
            int i = 0, nPos, nStrLen;
            string szBuffer = "";
            int nWeightC, nWeightK;
            int nCheckSumC, nCheckSumK;
            char cCheckDigitC, cCheckDigitK;
            string szCharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%@#^&";
            string szMappingSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-.=$/+%@#^&";
            string szSpecial = "";

            szSpecial = SpecialChar(szInpara);
            szInpara = szSpecial;

            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szMappingSet[nPos];
                else if (szInpara[i] == 240)
                    szBuffer += "#U";
                else if (szInpara[i] == ' ')   //control characters
                    szBuffer += '=';
                else if (szInpara[i] == '/')
                    szBuffer += "^O";
                else if (szInpara[i] == ':')
                    szBuffer += "^Z";
                else if (szInpara[i] == 64)
                    szBuffer += "#V";
                else if (szInpara[i] == 96)
                    szBuffer += "#W";
                else if (szInpara[i] > 0 && szInpara[i] <= 26)
                {
                    szBuffer += '@';
                    szBuffer += (char)(szInpara[i] + 'A' - 1);
                }
                else if (szInpara[i] > 32 && szInpara[i] <= 46)
                {
                    szBuffer += '^';
                    szBuffer += (char)((szInpara[i] % 32) + 'A' - 1);
                }
                else if (szInpara[i] >= 97 && szInpara[i] <= 122)
                {
                    szBuffer += '&';
                    szBuffer += (char)(szInpara[i] % 32 + 'A' - 1);
                }
                else if (szInpara[i] >= 27 && szInpara[i] <= 31)
                {
                    szBuffer += '#';
                    szBuffer += (char)(szInpara[i] - 27 + 'A');
                }
                else if (szInpara[i] >= 59 && szInpara[i] <= 63)
                {
                    szBuffer += '#';
                    szBuffer += (char)(szInpara[i] - 59 + 'F');
                }
                else if (szInpara[i] >= 91 && szInpara[i] <= 95)
                {
                    szBuffer += '#';
                    szBuffer += (char)(szInpara[i] - 91 + 'K');
                }
                else if (szInpara[i] >= 123 && szInpara[i] <= 127)
                {
                    szBuffer += '#';
                    szBuffer += (char)(szInpara[i] - 123 + 'P');
                }
                i = i + 1;
            }

            i = 0;
            nStrLen = szBuffer.Length;
            nCheckSumC = 0;
            while (i < szBuffer.Length)
            {
                nWeightC = (i + 1) % 20;
                nPos = szMappingSet.IndexOf(szBuffer[nStrLen - i - 1]);
                nCheckSumC = nCheckSumC + nWeightC * nPos;
                i = i + 1;
            }
            nCheckSumC = nCheckSumC % 47;
            cCheckDigitC = szMappingSet[nCheckSumC];
            szBuffer += cCheckDigitC;

            i = 0;
            nStrLen = szBuffer.Length;
            nCheckSumK = 0;
            while (i < nStrLen)
            {
                nWeightK = (i + 1) % 15;
                nPos = szCharSet.IndexOf(szBuffer[nStrLen - i - 1]);
                nCheckSumK = nCheckSumK + nWeightK * nPos;
                i = i + 1;
            }
            nCheckSumK = nCheckSumK % 47;
            cCheckDigitK = szMappingSet[nCheckSumK];
            szBuffer += cCheckDigitK;
            return '[' + szBuffer + "]|";
        }

        private char getUPCCheck(string pszInpara)
        {
            int nCheckSum = 0, nStrlen;
            nStrlen = pszInpara.Length;
            for (int i = 0; i < nStrlen; i++)
            {
                if (i % 2 == 0)
                    nCheckSum = nCheckSum + 3 * (pszInpara[nStrlen - i - 1] - '0');
                else
                    nCheckSum = nCheckSum + (pszInpara[nStrlen - i - 1] - '0');
            }
            nCheckSum = nCheckSum % 10;
            if (nCheckSum != 0)
                nCheckSum = 10 - nCheckSum;
            return (char)('0' + nCheckSum);
        }

        private char textonly(char cDigit)
        {
            int nDigit = cDigit - '0';
            if (nDigit >= 0 && nDigit <= 9)
                return (char)(192 + nDigit);
            return (char)0;
        }

        private char convertSetAText(char cChar)
        {
            int i;
            string szCode = "0123456789";
            string szDecode = "0123456789";
            i = szCode.IndexOf(cChar);
            if (i >= 0)
                return szDecode[i];
            return (char)0;
        }

        private char convertSetANoText(char cChar)
        {
            int i;
            string szCode = "1234567890";
            string szDecode = "!@#$%^&*()";
            i = szCode.IndexOf(cChar);
            if (i >= 0)
                return szDecode[i];
            return (char)0;
        }

        private char convertSetBText(char cChar)
        {
            int i;
            string szCode = "1234567890";
            string szDecode = "qwertyuiop";
            i = szCode.IndexOf(cChar);
            if (i >= 0)
                return szDecode[i];
            return (char)0;
        }

        private char convertSetBNoText(char cChar)
        {
            int i;
            string szCode = "1234567890";
            string szDecode = "QWERTYUIOP";
            i = szCode.IndexOf(cChar);
            if (i >= 0)
                return szDecode[i];
            return (char)0;
        }

        private char convertSetCText(char cChar)
        {
            int i;
            string szCode = "1234567890";
            string szDecode = "asdfghjkl;";
            i = szCode.IndexOf(cChar);
            if (i >= 0)
                return szDecode[i];
            return (char)0;
        }

        private char convertSetCNoText(char cChar)
        {
            int i;
            string szCode = "1234567890";
            string szDecode = "ASDFGHJKL:";
            i = szCode.IndexOf(cChar);
            if (i >= 0)
                return szDecode[i];
            return (char)0;
        }
        private string Parity5(int nDigit)
        {
            string pParity = "";
            switch (nDigit)
            {
                case 0:
                    pParity = "00111";
                    break;
                case 1:
                    pParity = "01011";
                    break;
                case 2:
                    pParity = "01101";
                    break;
                case 3:
                    pParity = "01110";
                    break;
                case 4:
                    pParity = "10011";
                    break;
                case 5:
                    pParity = "11001";
                    break;
                case 6:
                    pParity = "11100";
                    break;
                case 7:
                    pParity = "10101";
                    break;
                case 8:
                    pParity = "10110";
                    break;
                case 9:
                    pParity = "11010";
                    break;
            }
            return pParity;
        }
        private char LefthandEncoding(int nDigit, int nParity)
        {
            switch (nDigit)
            {
                case 0:
                    if (nParity == 1)
                        return '/';
                    else if (nParity == 0)
                        return '?';
                    break;
                case 1:
                    if (nParity == 1)
                        return 'z';
                    else if (nParity == 0)
                        return 'Z';
                    break;
                case 2:
                    if (nParity == 1)
                        return 'x';
                    else if (nParity == 0)
                        return 'X';
                    break;
                case 3:
                    if (nParity == 1)
                        return 'c';
                    else if (nParity == 0)
                        return 'C';
                    break;
                case 4:
                    if (nParity == 1)
                        return 'v';
                    else if (nParity == 0)
                        return 'V';
                    break;
                case 5:
                    if (nParity == 1)
                        return 'b';
                    else if (nParity == 0)
                        return 'B';
                    break;
                case 6:
                    if (nParity == 1)
                        return 'n';
                    else if (nParity == 0)
                        return 'N';
                    break;
                case 7:
                    if (nParity == 1)
                        return 'm';
                    else if (nParity == 0)
                        return 'M';
                    break;
                case 8:
                    if (nParity == 1)
                        return ',';
                    else if (nParity == 0)
                        return '<';
                    break;
                case 9:
                    if (nParity == 1)
                        return '.';
                    else if (nParity == 0)
                        return '>';
                    break;
            }
            return (char)0;
        }

        private string UPC5SUPP(string szInpara)
        {
            int i = 0, nCheckSum = 0;
            string szBuffer = "";
            string szParity = "";
            nCheckSum = 3 * (szInpara[0] - '0') + 9 * (szInpara[1] - '0') + 3 * (szInpara[2] - '0') + 9 * (szInpara[3] - '0') + 3 * (szInpara[4] - '0');
            szParity = Parity5(nCheckSum % 10);
            szBuffer += '{';
            for (i = 0; i < 5; i++)
            {
                szBuffer += LefthandEncoding(szInpara[i] - '0', szParity[i] - '0');
                if (i < 4)
                    szBuffer += '\\';
            }
            return szBuffer;

        }

        private string UPC2SUPP(string szInpara)
        {
            int i = 0;
            string szBuffer = "";
            int nNum = 0;
            int nParity1 = 0, nParity2 = 0;

            while (i < szInpara.Length)
            {
                nNum = 10 * nNum + szInpara[i] - '0';
                i = i + 1;
            }
            nNum = nNum % 4;
            switch (nNum)
            {
                case 0:
                    nParity1 = 1;
                    nParity2 = 1;
                    break;
                case 1:
                    nParity1 = 1;
                    nParity2 = 0;
                    break;
                case 2:
                    nParity1 = 0;
                    nParity2 = 1;
                    break;
                case 3:
                    nParity1 = 0;
                    nParity2 = 0;
                    break;
            }
            szBuffer += '{';
            szBuffer += LefthandEncoding(szInpara[0] - '0', nParity1);
            szBuffer += '\\';
            szBuffer += LefthandEncoding(szInpara[1] - '0', nParity2);
            return szBuffer;
        }

        private string UPC25SUPP(string szInpara)
        {
            int i = 0, nStrLen;
            string szBuffer = "";
            string szSwap = "";

            while (i < szInpara.Length)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9')
                    szBuffer += szInpara[i];
                i = i + 1;
            }

            nStrLen = szBuffer.Length;
            if (nStrLen == 0)
                szSwap = "";
            else if (nStrLen == 1)
            {
                szBuffer += '0';
                szSwap = UPC2SUPP(szBuffer);
            }
            else if (nStrLen == 2)
                szSwap = UPC2SUPP(szBuffer);
            else if (nStrLen == 3)
            {
                szBuffer += "00";
                szSwap = UPC5SUPP(szBuffer);
            }
            else if (nStrLen == 4)
            {
                szBuffer += "0";
                szSwap = UPC5SUPP(szBuffer);
            }
            else if (nStrLen == 5)
                szSwap = UPC5SUPP(szBuffer);
            else if (nStrLen > 5)
            {
                szBuffer = szBuffer.Substring(0, 5);
                szSwap = UPC5SUPP(szBuffer);
            }

            return szSwap;
        }
        /*------------------------------------------EAN13-----------------------------------------*/
        /*Converts the input text into an EAN barcode. Accepts input of 12 digits of numeric data.*/
        /*----------------------------------------------------------------------------------------*/
        public string EAN13(string szInpara)
        {
            int i, nStrlen, nCharPos;
            string szBuffer = "";
            string szSwap = "", szTemp = "", szSUPP = "";
            string szParity = "";
            char cCheckDigit;
            string pszSupplement = "";

            nStrlen = szInpara.Length;
            for (i = 0; i < nStrlen; i++)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9' || szInpara[i] == '|')
                    szTemp += szInpara[i];
            }
            nCharPos = szTemp.IndexOf('|');
            if (nCharPos >= 0)
            {
                pszSupplement = szTemp.Substring(nCharPos + 1, szTemp.Length - nCharPos - 1);
                szSUPP = UPC25SUPP(pszSupplement);
                szTemp = szTemp.Substring(0, nCharPos);
            }

            nStrlen = szTemp.Length;
            if (nStrlen > 12)
                szTemp = szTemp.Substring(0, 12);
            else if (nStrlen < 12)
            {
                while (nStrlen < 12)
                {
                    szTemp += '0';
                    nStrlen++;
                }
            }

            switch (szTemp[0])
            {
                case '0':
                    szParity = "AAAAAA";
                    break;
                case '1':
                    szParity = "AABABB";
                    break;
                case '2':
                    szParity = "AABBAB";
                    break;
                case '3':
                    szParity = "AABBBA";
                    break;
                case '4':
                    szParity = "ABAABB";
                    break;
                case '5':
                    szParity = "ABBAAB";
                    break;
                case '6':
                    szParity = "ABBBAA";
                    break;
                case '7':
                    szParity = "ABABAB";
                    break;
                case '8':
                    szParity = "ABABBA";
                    break;
                case '9':
                    szParity = "ABBABA";
                    break;
            }

            for (i = 1; i < 7; i++)
            {
                if (szParity[i - 1] == 'A')
                    szBuffer += convertSetAText(szTemp[i]);
                else if (szParity[i - 1] == 'B')
                    szBuffer += convertSetBText(szTemp[i]);
            }
            szBuffer += '|';
            for (i = 7; i < 12; i++)
                szBuffer += convertSetCText(szTemp[i]);
            cCheckDigit = getUPCCheck(szTemp);

            szSwap += textonly(szTemp[0]);
            szSwap += '[';
            szSwap += szBuffer;
            szSwap += convertSetCText(cCheckDigit);
            szSwap += ']';

            if (szSUPP != "")
            {
                szSwap += ' ';
                szSwap += szSUPP;
            }
            return szSwap;
        }

        public string EAN128(string inpara)
        {
            return "";
            //int i = 0;
            //string charToEncode = null;
            //string strCodeWord = null;
            //string strTemp = null;
            //int strLen = 0;
            //int checkSum = 0;
            //string checkDigit = null;
            //int weight = 0;
            //int charValue = 0;
            //string mappingSet = null;

            //mappingSet = code128MappingSet();

            //inpara = SpecialChar(inpara);
            //strLen = inpara.Length;
            //for (i = 1; i <= strLen; i++)
            //{
            //    if (inpara.Substring(i - 1, 1) == (char)(199).ToString())    
            //    {
            //        strTemp = strTemp + ((char)(199)).ToString();
            //    }
            //    else if (Microsoft.VisualBasic.Information.IsNumeric(inpara.Substring(i - 1, 1)))
            //    {
            //        if (i + 1 <= strLen && Microsoft.VisualBasic.Information.IsNumeric(inpara.Substring(i, 1)))
            //        {
            //            strTemp = strTemp + inpara.Substring(i - 1, 2);
            //            i = i + 1;
            //        }
            //        else
            //        {
            //            strTemp = strTemp + inpara.Substring(i - 1, 1) + "0";
            //        }
            //    }
            //}

            //strLen = strTemp.Length;
            //checkSum = 105 + 102;
            //weight = 2;

            //for (i = 1; i <= strLen; i++)
            //{
            //    charToEncode = strTemp.Substring(i - 1, 1);
            //    if (charToEncode != (char)(199)) // not FNC1
            //    {
            //        charValue = Microsoft.VisualBasic.Conversion.Val(strTemp.Substring(i - 1, 2));
            //        strCodeWord = strCodeWord + mappingSet.Substring(charValue, 1);
            //        charValue = charValue * weight;
            //        i = i + 1;
            //    }
            //    else // Fnc1
            //    {
            //        strCodeWord = strCodeWord + ((char)(199)).ToString();
            //        charValue = 102 * weight;
            //    }
            //    checkSum = checkSum + charValue;
            //    weight = weight + 1;
            //}

            //checkSum = checkSum % 103;
            //checkDigit = mappingSet.Substring(checkSum, 1);
            //return ((char)(202)).ToString() + ((char)(199)).ToString() + strCodeWord + checkDigit + ((char)(203)).ToString() + ((char)(205)).ToString();
        }

        /*--------------------------------------EAN8----------------------------------------------*/
        /*Converts the input text into an EAN-8 barcode. Accepts input of 7 digits of numeric data.*/
        /*----------------------------------------------------------------------------------------*/
        public string EAN8(string szInpara)
        {
            int i, nStrlen, nCharPos;
            string szBuffer = "";
            string szSwap = "", szTemp = "", szSUPP = "";
            char cCheckDigit;
            string pszSupplement;

            nStrlen = szInpara.Length;
            for (i = 0; i < nStrlen; i++)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9' || szInpara[i] == '|')
                    szTemp += szInpara[i];
            }
            nCharPos = szTemp.IndexOf('|');
            if (nCharPos >= 0)
            {
                pszSupplement = szTemp.Substring(nCharPos + 1, szTemp.Length - nCharPos - 1);
                szSUPP = UPC25SUPP(pszSupplement);
                szTemp = szTemp.Substring(0, nCharPos);
            }

            nStrlen = szTemp.Length;
            if (nStrlen > 7)
                szTemp = szTemp.Substring(0, 7);
            else if (nStrlen < 7)
            {
                while (nStrlen < 7)
                {
                    szTemp += '0';
                    nStrlen++;
                }
            }

            for (i = 0; i < 4; i++)
                szBuffer += convertSetAText(szTemp[i]);
            szBuffer += '|';
            for (i = 4; i < 7; i++)
                szBuffer += convertSetCText(szTemp[i]);
            cCheckDigit = getUPCCheck(szTemp);

            szSwap += '[';
            szSwap += szBuffer;
            szSwap += convertSetCText(cCheckDigit);
            szSwap += ']';

            if (szSUPP != "")
            {
                szSwap += ' ';
                szSwap += szSUPP;
            }
            return szSwap;
        }
        /*-------------------------------------UPC_A----------------------------------------------*/
        /*Converts the input text into a UPC-A barcode. Accepts input of 11 digits of numeric data.*/
        /*----------------------------------------------------------------------------------------*/
        public string UPC_A(string szInpara)
        {
            int i, nStrlen, nCharPos;
            string szBuffer = "";
            string szSwap = "", szTemp = "", szSUPP = "";
            char cCheckDigit;
            string pszSupplement;

            nStrlen = szInpara.Length;
            for (i = 0; i < nStrlen; i++)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9' || szInpara[i] == '|')
                    szTemp += szInpara[i];
            }
            nCharPos = szTemp.IndexOf('|');
            if (nCharPos >= 0)
            {
                pszSupplement = szTemp.Substring(nCharPos + 1, szTemp.Length - nCharPos - 1);
                szSUPP = UPC25SUPP(pszSupplement);
                szTemp = szTemp.Substring(0, nCharPos);
            }

            nStrlen = szTemp.Length;
            if (nStrlen > 11)
                szTemp = szTemp.Substring(0, 11);
            else if (nStrlen < 11)
            {
                while (nStrlen < 11)
                {
                    szTemp += '0';
                    nStrlen++;
                }
            }

            for (i = 1; i < 6; i++)
                szBuffer += convertSetAText(szTemp[i]);
            szBuffer += '|';
            for (i = 6; i < 11; i++)
                szBuffer += convertSetCText(szTemp[i]);
            cCheckDigit = getUPCCheck(szTemp);

            szSwap = "" + textonly(szTemp[0]);
            szSwap += '[';
            szSwap += convertSetANoText(szTemp[0]);
            szSwap += szBuffer;
            szSwap += convertSetCNoText(cCheckDigit);
            szSwap += ']';
            szSwap += textonly(cCheckDigit);

            if (szSUPP != "")
            {
                szSwap += ' ';
                szSwap += szSUPP;
            }
            return szSwap;
        }

        private string upce2upca(string szInpara)
        {
            string szBuffer = "";
            if (szInpara[0] != '0' || szInpara.Length != 7)
                return "";
            switch (szInpara[6])
            {
                case '0':
                case '1':
                case '2':
                    szBuffer += szInpara[0] + szInpara[1] + szInpara[2] + szInpara[6] + "0000" + szInpara[3] + szInpara[4] + szInpara[5];
                    break;
                case '3':
                    if (szInpara[3] == '0' || szInpara[3] == '1' || szInpara[3] == '2')
                        return "";
                    szBuffer += szInpara[0] + szInpara[1] + szInpara[2] + szInpara[3] + "00000" + szInpara[4] + szInpara[5];
                    break;
                case '4':
                    szBuffer += szInpara[0] + szInpara[1] + szInpara[2] + szInpara[3] + szInpara[4] + "00000" + szInpara[5];
                    break;
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    szBuffer += szInpara[0] + szInpara[1] + szInpara[2] + szInpara[3] + szInpara[4] + szInpara[5] + "0000" + szInpara[6];
                    break;
            }
            return szBuffer;

        }
        /*------------------------------------UPC_E-----------------------------------------------*/
        /*Converts the input text into a UPC-E barcode. Accepts input of 6 digits of numeric data.*/
        /*----------------------------------------------------------------------------------------*/
        public string UPC_E(string szInpara)
        {
            int i, nStrlen, nCharPos;
            string szBuffer = "";
            string szSwap = "", szTemp = "", szSUPP = "";
            string szParity = "";
            char cCheckDigit;
            string pszSupplement;

            nStrlen = szInpara.Length;
            for (i = 0; i < nStrlen; i++)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9' || szInpara[i] == '|')
                    szTemp += szInpara[i];
            }
            nCharPos = szTemp.IndexOf('|');
            if (nCharPos >= 0)
            {
                pszSupplement = szTemp.Substring(nCharPos + 1, szTemp.Length - nCharPos - 1);
                szSUPP = UPC25SUPP(pszSupplement);
                szTemp = szTemp.Substring(0, nCharPos);
            }

            nStrlen = szTemp.Length;
            if (nStrlen > 6)
                szTemp = szTemp.Substring(0, 6);
            else if (nStrlen < 6)
            {
                while (nStrlen < 6)
                {
                    szTemp += '0';
                    nStrlen++;
                }
            }

            szTemp = '0' + szTemp;
            szSwap = upce2upca(szTemp);
            cCheckDigit = getUPCCheck(szSwap);

            switch (cCheckDigit)
            {
                case '0':
                    szParity = "BBBAAA";
                    break;
                case '1':
                    szParity = "BBABAA";
                    break;
                case '2':
                    szParity = "BBAABA";
                    break;
                case '3':
                    szParity = "BBAAAB";
                    break;
                case '4':
                    szParity = "BABBAA";
                    break;
                case '5':
                    szParity = "BAABBA";
                    break;
                case '6':
                    szParity = "BAAABB";
                    break;
                case '7':
                    szParity = "BABABA";
                    break;
                case '8':
                    szParity = "BABAAB";
                    break;
                case '9':
                    szParity = "BAABAB";
                    break;
            }

            for (i = 1; i < 7; i++)
            {
                if (szParity[i - 1] == 'A')
                    szBuffer += convertSetAText(szTemp[i]);
                else if (szParity[i - 1] == 'B')
                    szBuffer += convertSetBText(szTemp[i]);
            }

            szSwap = "";
            szSwap += textonly('0');
            szSwap += '[';
            szSwap += szBuffer;
            szSwap += '\'';
            szSwap += textonly(cCheckDigit);

            if (szSUPP != "")
            {
                szSwap += ' ';
                szSwap += szSUPP;
            }
            return szSwap;
        }
        /*------------------------------------------Codabar---------------------------------------*/
        /*Converts the input into a valid Codabar symbol. The default start/stop characters are “A”*/
        /*and “B”.*/
        /*----------------------------------------------------------------------------------------*/
        public string Codabar(string szInpara)
        {
            int i = 0, nPos;
            string szBuffer = "A";
            string szCharSet = "0123456789-$:/.+";
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            szBuffer += 'B';
            return szBuffer;
        }
        /*--------------------------------------Code25--------------------------------------------*/
        /*Converts the input into a valid Code25 symbol. No check character appended.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code25(string szInpara)
        {
            int i = 0, nPos;
            string szBuffer = "[";
            string szCharSet = "0123456789";
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            szBuffer += ']';
            return szBuffer;
        }
        /*----------------------------------Code25Check-------------------------------------------*/
        /*Converts the input into a valid Code25 symbol. Append a check digit.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code25Check(string szInpara)
        {
            int i = 0, nCheckSum = 0, nStrLen;
            string szBuffer = "";
            string szSwap = "";
            char cCheckDigit;

            while (i < szInpara.Length)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9')
                    szBuffer += szInpara[i];
                i = i + 1;
            }

            i = 0;
            nStrLen = szBuffer.Length;
            while (i < szBuffer.Length)
            {
                if (i % 2 == 0)
                    nCheckSum = nCheckSum + 3 * (szBuffer[nStrLen - i - 1] - '0');
                else
                    nCheckSum = nCheckSum + szBuffer[nStrLen - i - 1] - '0';
                i = i + 1;
            }
            nCheckSum = nCheckSum % 10;
            if (nCheckSum != 0)
                nCheckSum = 10 - nCheckSum;
            cCheckDigit = (char)('0' + nCheckSum);

            szSwap += '[';
            szSwap += szBuffer;
            szSwap += cCheckDigit;
            szSwap += ']';
            return szSwap;
        }
        /*----------------------Bookland--------------------------*/
        /*Converts an ISBN string into a valid Bookland  barcode. */
        /*--------------------------------------------------------*/
        public string Bookland(string szInpara)
        {
            int i = 0, nPos, nStrlen, nCharPos;
            string szBuffer = "";
            string szSwap = "";
            string szLeft = "";
            string szRight = "";
            string szCharSet = "0123456789";

            nStrlen = szInpara.Length;
            nCharPos = szInpara.IndexOf('|');
            if (nCharPos > -1)
            {
                for (i = 0; i < nCharPos; i++)
                    szLeft += szInpara[i];
                for (i = nCharPos + 1; i < nStrlen; i++)
                    szRight += szInpara[i];
            }
            else
                szLeft = szInpara;

            i = 0;
            while (i < szLeft.Length)
            {
                nPos = szCharSet.IndexOf(szLeft[i]);
                if (nPos >= 0)
                    szBuffer += szLeft[i];
                i = i + 1;
            }
            nStrlen = szBuffer.Length;
            if (nStrlen > 10)
                szBuffer = szBuffer.Substring(0, 10);
            else if (nStrlen < 10)
            {
                while (nStrlen < 10)
                {
                    szBuffer += '0';
                    nStrlen++;
                }
            }

            szSwap = "978";
            for (i = 0; i < 9; i++)
                szSwap += szBuffer[i];
            szBuffer = "";
            szBuffer = EAN13(szSwap);

            szSwap = "";
            if (nCharPos > -1)
            {
                szSwap += ' ';
                szSwap += UPC25SUPP(szRight);
            }
            return szBuffer + szSwap;
        }
        /*-------------------------------------------Code11---------------------------------------*/
        /*Converts the input into a valid Code11 symbol. Check digit as well start/stop characters*/
        /*are added into the input.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code11(string szInpara)
        {
            int i = 0, nPos, nCheckSumC = 0, nCheckSumK = 0, nStrLen;
            string szBuffer = "";
            string szSwap = "";
            char cCheckDigitC, cCheckDigitK;
            string szCharSet = "0123456789-";

            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }

            nStrLen = szBuffer.Length;
            for (i = 0; i < nStrLen; i++)
            {
                nPos = szCharSet.IndexOf(szBuffer[nStrLen - i - 1]);
                if (nPos >= 0)
                    nCheckSumC = nCheckSumC + ((i + 1) % 10) * nPos;
            }
            nCheckSumC = nCheckSumC % 11;
            cCheckDigitC = szCharSet[nCheckSumC];
            szBuffer += cCheckDigitC;

            nStrLen = szBuffer.Length;
            if (nStrLen > 11)
            {
                i = 0;
                while (i < szBuffer.Length)
                {
                    nPos = szCharSet.IndexOf(szBuffer[nStrLen - i - 1]);
                    if (nPos >= 0)
                        nCheckSumK = nCheckSumK + ((i + 1) % 9) * nPos;
                    i = i + 1;
                }
                nCheckSumK = nCheckSumK % 11;
                cCheckDigitK = szCharSet[nCheckSumK];
                szBuffer += cCheckDigitK;
            }

            szSwap += '[';
            szSwap += szBuffer;
            szSwap += ']';
            return szSwap;
        }
        /*--------------------------------------MSIMod10------------------------------------------*/
        /*Converts the input into a valid MSI/Plessey symbol. Check digit is calculated based on  */
        /*Modulo 10 algorithm.*/
        /*----------------------------------------------------------------------------------------*/
        public string MSIMod10(string szInpara)
        {
            int i = 0, nCheckSum = 0, nStrLen;
            string szBuffer = "";
            string szSwap = "";
            string szNewNo = "";
            char cCheckDigit;
            int nChoice, nNewNo = 0;

            while (i < szInpara.Length)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9')
                    szBuffer += szInpara[i];
                i = i + 1;
            }

            i = 0;
            nStrLen = szBuffer.Length;
            nChoice = nStrLen % 2;
            while (i < szBuffer.Length)
            {
                if (i % 2 == nChoice)
                    nCheckSum = nCheckSum + szBuffer[i] - '0';
                else
                    szNewNo += szBuffer[i];
                i = i + 1;
            }

            i = 0;
            nStrLen = szNewNo.Length;
            while (i < szNewNo.Length)
            {
                nNewNo = 10 * nNewNo + szNewNo[i] - '0';
                i = i + 1;
            }
            nNewNo = 2 * nNewNo;
            szNewNo = "";
            while (nNewNo > 0)
            {
                szNewNo = (char)((nNewNo % 10) + '0') + szNewNo;
                nNewNo = nNewNo / 10;
                i = i + 1;
            }
            i = 0;
            while (i < szNewNo.Length)
            {
                nCheckSum = nCheckSum + szNewNo[i] - '0';
                i = i + 1;
            }
            nCheckSum = nCheckSum % 10;
            if (nCheckSum != 0)
                nCheckSum = 10 - nCheckSum;
            cCheckDigit = (char)(nCheckSum + '0');
            szBuffer += cCheckDigit;

            szSwap += '[';
            szSwap += szBuffer;
            szSwap += ']';
            return szSwap;
        }
        private string code128aCharSet()
        {
            string pSet = "";
            int i;
            for (i = 32; i <= 95; i++)
                pSet += (char)i;
            pSet += (char)240;
            for (i = 1; i < 32; i++)
                pSet += (char)i;
            for (i = 193; i < 200; i++)
                pSet += (char)i;
            return pSet;
        }

        private string code128bCharSet()
        {
            string pSet = "";
            int i;
            for (i = 32; i < 128; i++)
                pSet += (char)i;
            for (i = 193; i < 200; i++)
                pSet += (char)i;
            return pSet;
        }

        private string code128cCharSet()
        {
            string pSet = "";
            int i;
            for (i = 0; i < 10; i++)
                pSet += (char)(i + '0');
            for (i = 192; i < 200; i++)
                pSet += (char)i;
            return pSet;
        }

        private string code128MappingSet()
        {
            string pSet = "";
            int i;
            pSet += (char)204;
            for (i = 33; i < 127; i++)
                pSet += (char)i;
            for (i = 192; i < 203; i++)
                pSet += (char)i;
            return pSet;
        }
        /*------------------------------------------ Code128A-------------------------------------*/
        /*Accepts the input of character set A. Code128 character set consists of capital letters */
        /*and control characters.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code128A(string szInpara)
        {
            int i = 0, nPos, nCheckSum = 0;
            string szBuffer = "";
            string szSwap = "";
            char cCheckDigit;
            string szCharSet = "", szMappingSet = "";
            string szSpecial = "";

            szSpecial = SpecialChar(szInpara);
            szInpara = szSpecial;

            szCharSet = code128aCharSet();
            szMappingSet = code128MappingSet();
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            nCheckSum = 103;
            i = 0;
            while (i < szBuffer.Length)
            {
                nPos = szCharSet.IndexOf(szBuffer[i]);
                if (nPos >= 0)
                {
                    szSwap += szMappingSet[nPos];
                    nCheckSum = nCheckSum + (i + 1) * nPos;
                }
                i = i + 1;
            }
            nCheckSum = nCheckSum % 103;
            cCheckDigit = szMappingSet[nCheckSum];

            szBuffer = "";
            szBuffer += (char)200;
            szBuffer += szSwap;
            szBuffer += cCheckDigit;
            szBuffer += (char)203;
            szBuffer += (char)205;
            return szBuffer;
        }
        /*----------------------------------------Code128B----------------------------------------*/
        /*Accepts the input of character set B. Code128 character set consist of all printable */
        /*character s in the ASCII table.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code128B(string szInpara)
        {
            int i = 0, nPos, nCheckSum = 0;
            string szBuffer = "";
            string szSwap = "";
            char cCheckDigit;
            string szCharSet = "", szMappingSet = "\0";

            szCharSet = code128bCharSet();
            szMappingSet = code128MappingSet();
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            nCheckSum = 104;
            i = 0;
            while (i < szBuffer.Length)
            {
                nPos = szCharSet.IndexOf(szBuffer[i]);
                if (nPos >= 0)
                {
                    szSwap += szMappingSet[nPos];
                    nCheckSum = nCheckSum + (i + 1) * nPos;
                }
                i = i + 1;
            }
            nCheckSum = nCheckSum % 103;
            cCheckDigit = szMappingSet[nCheckSum];

            szBuffer = "";
            szBuffer += (char)201;
            szBuffer += szSwap;
            szBuffer += cCheckDigit;
            szBuffer += (char)203;
            szBuffer += (char)205;
            return szBuffer;
        }
        /*-------------------------------------------Code128C-------------------------------------*/
        /*Code128 character set C only contains numeric characters. Used when the encoded data  */
        /*containing only numbers.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code128C(string szInpara)
        {
            int i = 0, nPos, nStrLen, nCheckSum = 0;
            string szBuffer = "";
            string szSwap = "";
            string szTemp = "";
            char cCheckDigit;
            string szCharSet = "", szMappingSet = "";

            szCharSet = code128cCharSet();
            szMappingSet = code128MappingSet();
            while (i < szInpara.Length)
            {
                nPos = szCharSet.IndexOf(szInpara[i]);
                if (nPos >= 0)
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            nCheckSum = 105;
            nStrLen = szBuffer.Length;
            if (nStrLen % 2 == 1)
                szTemp += '0';
            szTemp += szBuffer;

            i = 0;
            while (i < nStrLen)
            {
                nPos = 10 * (szTemp[i] - '0') + szTemp[i + 1] - '0';
                if (nPos >= 0)
                    szSwap += szMappingSet[nPos];
                i = i + 2;
            }
            i = 0;
            while (i < szSwap.Length)
            {
                if (szSwap[i] == (char)204)
                    ;
                else if (szSwap[i] >= 33 && szSwap[i] < 127)
                    nCheckSum = nCheckSum + (i + 1) * (szSwap[i] - 32);
                else if (szSwap[i] >= 192)
                    nCheckSum = nCheckSum + (i + 1) * (szSwap[i] - 97);
                i = i + 1;
            }
            nCheckSum = nCheckSum % 103;
            cCheckDigit = szMappingSet[nCheckSum];

            szBuffer = "";
            szBuffer += (char)202;
            szBuffer += szSwap;
            szBuffer += cCheckDigit;
            szBuffer += (char)203;
            szBuffer += (char)205;
            return szBuffer;
        }
        /*------------------------------------------Code128Auto-----------------------------------*/
        /*Encode any ASCII characters. It automatically shift to another character set when the   */
        /*encoded character is not found in the current character set.*/
        /*----------------------------------------------------------------------------------------*/
        public string Code128Auto(string szInpara)
        {
            int i = 0, nPos, nStrLen, nCheckSum = 0;
            string szBuffer = "";
            string szSwap = "";
            char cCheckDigit;
            string szCharASet = "", szCharBSet = "", szCharCSet = "", szMappingSet = "";
            int nValue, nWeight;
            string pCurCharSet = "";
            string szSpecial = "";

            szSpecial = SpecialChar(szInpara);
            szInpara = szSpecial;

            szCharASet = code128aCharSet();
            szCharBSet = code128bCharSet();
            szCharCSet = code128cCharSet();
            szMappingSet = code128MappingSet();
            nStrLen = szInpara.Length;
            if (szInpara[0] <= 31) pCurCharSet = szCharASet;
            if (szInpara[0] >= 32 && szInpara[0] <= 126) pCurCharSet = szCharBSet;
            if (nStrLen > 4)
            {
                i = 0;
                while (i < 4)
                {
                    if (szInpara[0] < '0' || szInpara[0] > '9')
                        break;
                    i = i + 1;
                }
                if (i == 4)
                    pCurCharSet = szCharCSet;
            }
            if (pCurCharSet == szCharASet)
                szBuffer += (char)200;
            else if (pCurCharSet == szCharBSet)
                szBuffer += (char)201;
            else if (pCurCharSet == szCharCSet)
                szBuffer += (char)202;

            i = 0;
            while (i < szInpara.Length)
            {
                if (szInpara[i] == (char)199)
                    szBuffer += (char)199;
                else if ((i < nStrLen - 4) && IsNumeric(szInpara.Substring(i, 4)) || ((pCurCharSet == szCharCSet) && (i < nStrLen - 1) &&
                    (IsNumeric(szInpara.Substring(i, 2)))))
                {
                    if (pCurCharSet != szCharCSet)
                    {
                        szBuffer += (char)196;
                        pCurCharSet = szCharCSet;
                    }
                    nPos = 10 * (szInpara[i] - '0') + szInpara[i + 1] - '0';
                    szBuffer += szMappingSet[nPos];
                    i = i + 1;
                }
                else if (((i < nStrLen) && (szInpara[i] < 31 || szInpara[i] == 240)) ||
                    ((pCurCharSet == szCharASet) && (szInpara[i] > 32 && szInpara[i] < 96)))
                {
                    if (pCurCharSet != szCharASet)
                    {
                        szBuffer += (char)198;
                        pCurCharSet = szCharASet;
                    }
                    nPos = pCurCharSet.IndexOf(szInpara[i]);
                    if (nPos >= 0)
                        szBuffer += szMappingSet[nPos];
                }
                else if (i < nStrLen && szInpara[i] > 31 && szInpara[i] < 127)
                {
                    if (pCurCharSet != szCharBSet)
                    {
                        szBuffer += (char)197;
                        pCurCharSet = szCharBSet;
                    }
                    nPos = pCurCharSet.IndexOf(szInpara[i]);
                    if (nPos >= 0)
                        szBuffer += szMappingSet[nPos];
                }
                i = i + 1;
            }

            i = 0;
            nValue = 0;
            while (i < szBuffer.Length)
            {
                if (szBuffer[i] == 204)
                    nValue = 0;
                else if (szBuffer[i] >= 33 && szBuffer[i] <= 126)
                    nValue = szBuffer[i] - 32;
                else if (szBuffer[i] >= 192)
                    nValue = szBuffer[i] - 97;
                if (i > 1)
                    nWeight = i;
                else
                    nWeight = 1;
                nCheckSum = nCheckSum + nValue * nWeight;
                i = i + 1;
            }
            nCheckSum = nCheckSum % 103;
            cCheckDigit = szMappingSet[nCheckSum];

            szSwap += szBuffer;
            szSwap += cCheckDigit;
            szSwap += (char)203;
            szSwap += (char)205;
            return szSwap;
        }
        /*---------------------------------------USPS_EAN128--------------------------------------*/
        /*Used for 22 digit USPS special services labels such as delivery confirmation in EAN128. */
        /*This function takes 19 digit input which is made up of the three parts: 2 digit service */
        /*code, 9 digit customer ID and 8 digit sequential package ID. This function calculates the*/
        /*check digit (Mod10), add the application identifier 91 as required by the USPS standard,*/
        /*and format the data with EAN128 standard. */
        /*----------------------------------------------------------------------------------------*/
        public string USPS_EAN128(string szInpara)
        {
            int i = 0, nLen, charVal = 0, checkSum = 0, weight = 0;
            string szTemp = "", szEAN128 = "";
            char checkDigit;
            while (i < szInpara.Length)
            {
                if (char.IsNumber(szInpara[i]))
                    szTemp += szInpara[i];
                i++;
            }
            nLen = szTemp.Length;
            if (nLen >= 19)
            {
                for (i = 0; i < 19; i++)
                    szEAN128 += szTemp[i];
            }
            else if (nLen < 19)
                szEAN128 = "0000000000000000000";
            szEAN128 = "91" + szEAN128;
            nLen = szEAN128.Length;
            if (nLen != 21)
            {
                return "";
            }

            for (i = 0; i < nLen; i++)
            {
                charVal = szEAN128[nLen - i - 1] - '0';
                if ((i + 1) % 2 == 1)
                    weight = 3;
                else
                    weight = 1;
                checkSum = checkSum + charVal * weight;
            }
            checkSum = checkSum % 10;
            if (checkSum == 0)
                checkDigit = '0';
            else
                checkDigit = (char)(10 - checkSum + '0');
            return EAN128(szEAN128 + checkDigit);
        }

        public string USPS_USS128(string szInpara)
        {
            int i = 0, nLen, charVal = 0, checkSum = 0, weight = 0;
            string szTemp = "", szUSS128 = "";
            char checkDigit;
            while (i < szInpara.Length)
            {
                if (char.IsNumber(szInpara[i]))
                    szTemp += szInpara[i];
                i++;
            }
            nLen = szTemp.Length;
            if (nLen == 20)
            {
                for (i = 0; i < 19; i++)
                    szUSS128 += szTemp[i];
            }
            else
            {
                return "";
            }

            nLen = szUSS128.Length;
            for (i = 0; i < nLen; i++)
            {
                charVal = szUSS128[nLen - i - 1] - '0';
                if ((i + 1) % 2 == 1)
                    weight = 3;
                else
                    weight = 1;
                checkSum = checkSum + charVal * weight;
            }
            checkSum = checkSum % 10;
            if (checkSum == 0)
                checkDigit = '0';
            else
                checkDigit = (char)(10 - checkSum + '0');
            szUSS128 += checkDigit;
            return EAN128(szUSS128);
        }
        /*-------------------------------------------ITF25----------------------------------------*/
        /*Converts the input into a valid interleaved 2 of 5 barcode. Append a check digit.*/
        /*----------------------------------------------------------------------------------------*/
        public string ITF25(string input)
        {
            string temp_str = "", szITF25 = "";
            int i;
            char ch;
            int value1, value2, value3;

            //filter no-numeric string
            for (i = 0; i < input.Length; i++)
            {
                ch = input[i];
                if (ch >= '0' && ch <= '9')
                    temp_str += ch;
            }

            if (temp_str.Length % 2 == 1)
            {
                temp_str += '0';
            }

            ///////////////////////////////
            szITF25 += (char)202;

            for (i = 0; i < temp_str.Length; i = i + 2)
            {
                value1 = temp_str[i] - '0';
                value2 = temp_str[i + 1] - '0';
                value3 = value1 * 10 + value2;

                if (value3 >= 0 && value3 <= 93)
                    szITF25 += (char)('!' + value3);
                else
                    szITF25 += (char)(value3 - 94 + 196);
            }

            szITF25 += (char)203;
            return szITF25;
        }
        /*---------------------------------------------ITF25Check---------------------------------*/
        /*Converts the input into a valid interleaved 2 of 5 barcode. No check digit appended in  */
        /*this function.*/
        /*----------------------------------------------------------------------------------------*/
        public string ITF25Check(string input)
        {
            string temp_str = "", szITF25 = "";
            int i;
            char ch; int value1, value2, value3;
            int check_sum;
            char check_digit;

            //filter no-numeric string
            for (i = 0; i < input.Length; i++)
            {
                ch = input[i];
                if (ch >= '0' && ch <= '9')
                    temp_str += ch;
            }

            if (temp_str.Length % 2 == 0)
            {
                temp_str += '0';
            }

            //calc the check sum
            check_sum = 0;
            for (i = 0; i < temp_str.Length; i++)
            {
                if (i % 2 == 0)
                    check_sum += 3 * (temp_str[temp_str.Length - i - 1] - '0');
                else
                    check_sum += temp_str[temp_str.Length - i - 1] - '0';
            }
            check_sum = check_sum % 10;
            if (check_sum == 0)
                check_digit = '0';
            else
                check_digit = (char)('0' + 10 - check_sum);
            //append check digit

            temp_str += check_digit;
            szITF25 += (char)202;

            for (i = 0; i < temp_str.Length; i = i + 2)
            {
                value1 = temp_str[i] - '0';
                value2 = temp_str[i + 1] - '0';
                value3 = value1 * 10 + value2;

                if (value3 >= 0 && value3 <= 93)
                    szITF25 += (char)('!' + value3);
                else
                    szITF25 += (char)(value3 - 94 + 196);
            }

            szITF25 += (char)203;
            return szITF25;
        }
        /*---------------------------------Postnet--------------------------------*/
        /*Converts the input into a valid POSTNET barcode string with checksum.*/
        /*The function adds the start/stop frame bar, calculating the check digit*/
        /*and forms the correct symbol. This function can also be used to generate*/
        /*PLANET barcode string.*/
        /*-----------------------------------------------------------------------*/
        public string Postnet(string szInpara)
        {
            int i, nLen, nCheckSum = 0;
            string szTemp = "", szBuffer = "";
            nLen = szInpara.Length;
            //filter
            for (i = 0; i < nLen; i++)
            {
                if (char.IsNumber(szInpara[i]))
                    szBuffer += szInpara[i];
            }
            nLen = szBuffer.Length;
            if (nLen >= 0 && nLen < 5)
            {
                while (szBuffer.Length < 5)
                    szBuffer += '0';
            }
            else if (nLen > 5 && nLen < 9)
            {
                while (szBuffer.Length < 9)
                    szBuffer += '0';
            }
            else if (nLen > 9 && nLen < 13)
            {
                while (szBuffer.Length < 13)
                    szBuffer += '0';
            }
            else if (nLen > 13)
                szBuffer = szBuffer.Substring(0, 13);

            nLen = szBuffer.Length;
            for (i = 0; i < nLen; i++)
            {
                if (char.IsNumber(szBuffer[i]))
                {
                    szTemp += szBuffer[i];
                    nCheckSum = nCheckSum + szBuffer[i] - '0';
                }
            }
            nCheckSum = nCheckSum % 10;
            if (nCheckSum != 0) nCheckSum = 10 - nCheckSum;
            szTemp = '[' + szTemp;
            szTemp += (char)('0' + nCheckSum);
            szTemp += ']';
            return szTemp;
        }
        /*-----------------------------------RoyalMail--------------------------------*/
        /*Converts the input into a valid UK royal mail barcode symbol with checksum. */
        /*The function adds the start/stop frame bar, calculating the check digit and */
        /*forms the correct symbol.*/
        /*----------------------------------------------------------------------------*/
        public string RoyalMail(string szInpara)
        {
            int i, nLen, nCharVal = 0, nCheckSum = 0, ntu = 0, ntl = 0, nTemp = 0;
            string szCharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string szTemp = "";

            nLen = szInpara.Length;
            for (i = 0; i < nLen; i++)
            {
                if (szCharSet.IndexOf(szInpara[i]) >= 0)
                {
                    szTemp += szInpara[i];
                    if (szInpara[i] < 65)
                        nCharVal = szInpara[i] - 48;
                    else
                        nCharVal = szInpara[i] - 55;
                    nTemp = nCharVal / 6;
                    if (nTemp >= 5)
                        nCheckSum = 0;
                    else
                        nCheckSum = nTemp + 1;
                    ntu = ntu + nCheckSum;
                    nTemp = nCharVal - nTemp * 6;
                    if (nTemp >= 5)
                        nCheckSum = 0;
                    else
                        nCheckSum = nTemp + 1;
                    ntl = ntl + nCheckSum;
                }
            }
            ntu = ntu % 6;
            if (ntu == 0) ntu = 6;
            ntl = ntl % 6;
            if (ntl == 0) ntl = 6;
            nCheckSum = (ntu - 1) * 6 + ntl - 1;
            szTemp = '[' + szTemp;
            if (nCheckSum < 10)
                szTemp += (char)(nCheckSum + 48);
            else
                szTemp += (char)(nCheckSum + 55);
            szTemp += ']';
            return szTemp;
        }
        /*--------------------------------Telepen-------------------------------------*/
        /*Converts the input into a Telepen barcode string. The function accepts any  */
        /*ASCII character input, taking care of the check digit calculation and adding*/
        /*start/stop characters.*/
        /*----------------------------------------------------------------------------*/
        public string Telepen(string szInpara)
        {
            int i = 0, nCheckSum = 0;
            string szBuffer = "";
            string szSwap = "";
            string szSpecial = "";
            char cCheckDigit;

            szSpecial = SpecialChar(szInpara);
            szInpara = szSpecial;
            while (i < szInpara.Length)
            {
                if (szInpara[i] >= 0 && szInpara[i] <= 127)
                {
                    szBuffer += szInpara[i];
                    nCheckSum = nCheckSum + szInpara[i];
                }
                i = i + 1;
            }

            cCheckDigit = (char)(127 - (nCheckSum % 127));
            szBuffer += cCheckDigit;

            i = 0;
            while (i < szBuffer.Length)
            {
                if (szBuffer[i] == ' ')
                    szSwap += '=';
                else if (szBuffer[i] == '=')
                    szSwap += (char)240;
                else if (szBuffer[i] == '[')
                    szSwap += (char)241;
                else if (szBuffer[i] == ']')
                    szSwap += (char)242;
                else if (szBuffer[i] >= 0 && szBuffer[i] <= 31)
                    szSwap += (char)(szBuffer[i] + 192);
                else if (szBuffer[i] == 127)
                    szSwap += (char)224;
                else
                    szSwap += szBuffer[i];
                i = i + 1;
            }

            szSwap = '[' + szSwap + ']';
            return szSwap;
        }
        /*-------------------------------------TelepenNumeric-------------------------*/
        /*Converts the input into a Telepen Numeric barcode string. This functions */
        /*accepts numeric input only.*/
        /*----------------------------------------------------------------------------*/
        public string TelepenNumeric(string szInpara)
        {
            int i = 0, nTemp, nStrLen, nCheckSum = 0;
            string szBuffer = "";
            string szSwap = "";
            char cCheckDigit;


            // filter numeric character
            while (i < szInpara.Length)
            {
                if (szInpara[i] >= '0' && szInpara[i] <= '9')
                    szBuffer += szInpara[i];
                i = i + 1;
            }
            nStrLen = szBuffer.Length;
            if (nStrLen % 2 == 1) szBuffer += '0';

            i = 0;
            while (i < szBuffer.Length)
            {
                nTemp = 10 * (szBuffer[i] - '0') + szBuffer[i + 1] - '0' + 27;
                if (nTemp >= 0)
                    szSwap += (char)nTemp;
                i = i + 2;
            }

            i = 0;
            while (i < szSwap.Length)
            {
                nCheckSum = nCheckSum + szSwap[i];
                i = i + 1;
            }
            cCheckDigit = (char)(127 - (nCheckSum % 127));
            szSwap += (char)cCheckDigit;

            // set null string
            szBuffer = "";
            i = 0;
            while (i < szSwap.Length)
            {
                if (szSwap[i] == ' ')
                    szBuffer += '=';
                else if (szSwap[i] == '=')
                    szBuffer += (char)240;
                else if (szSwap[i] == '[')
                    szBuffer += (char)241;
                else if (szSwap[i] == ']')
                    szBuffer += (char)242;
                else if (szSwap[i] >= 0 && szSwap[i] <= 31)
                    szBuffer += (char)(szSwap[i] + 192);
                else if (szSwap[i] == 127)
                    szBuffer += (char)224;
                else
                    szBuffer += szSwap[i];
                i = i + 1;
            }

            szBuffer = '[' + szBuffer + ']';
            return szBuffer;
        }

    }
}

