using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace com.Sconit.Utility
{
    public static class StringHelper
    {
        /// <summary>
        /// "Code [Description]"
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetCodeDescriptionString(string code, string description)
        {
            if (code == null || code.Trim() == string.Empty)
                code = string.Empty;
            if (description == null || description.Trim() == string.Empty)
                description = string.Empty;

            if (description == string.Empty)
                return code;
            else
                return code + " [" + description + "]";
        }

        public static string SubStr(string sString, int nLeng)
        {
            int totalLength = 0;
            int currentIndex = 0;
            while (totalLength < nLeng && currentIndex < sString.Length)
            {
                if (sString[currentIndex] < 0 || sString[currentIndex] > 255)
                    totalLength += 2;
                else
                    totalLength++;

                currentIndex++;
            }

            if (currentIndex < sString.Length)
                return sString.Substring(0, currentIndex) + "...";
            else
                return sString.ToString();
        }

        /// <summary>
        /// Sconit Common String Comparer, ignore case, support Null
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Eq(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        public static List<string> SplitStringByLength(string sString, int segmentLength)
        {
            List<string> segmentList = new List<string>();
            if (sString == null || sString.Trim().Length == 0)
            {
                return segmentList;
            }
            if (sString.IndexOf("\r\n") != -1)
            {
                string[] str = Regex.Split(sString, "\r\n", RegexOptions.IgnoreCase);
                segmentList.AddRange(str);
            }
            else
            {
                int byteLen = System.Text.Encoding.Default.GetByteCount(sString);  //单字节字符长度
                int charLen = sString.Length; //把字符平等对待时的字符串长度
                int byteCount = 0;  //记录读取进度{中文按两单位计算}
                //int regularSegmentCount = sString.Length / segmentLength;
                int charIndex = 0;
                for (int i = 0; i < charLen; i++)
                {

                    if (Convert.ToInt32(sString.ToCharArray()[i]) > 255)  //遇中文字符计数加2
                        byteCount += 2;
                    else         //按英文字符计算加1
                        byteCount += 1;
                    if (byteCount == segmentLength)   //到达指定长度时，记录指针位置并停止
                    {
                        segmentList.Add(sString.Substring(charIndex, i + 1 - charIndex));
                        charIndex = i + 1;
                        byteCount = 0;
                    }else if ((byteCount - 1) == segmentLength)
                    {
                        i -= 1;
                        segmentList.Add(sString.Substring(charIndex, i + 1 - charIndex));
                        charIndex = i + 1;
                        byteCount = 0;
                    }
                }
                if (charIndex < sString.Length)
                {
                    string lastSegment = sString.Substring(charIndex);
                    segmentList.Add(lastSegment);
                }
            }

            return segmentList;

        }


    }
}
