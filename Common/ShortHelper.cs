using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 长编码转换为短编码
    /// </summary>
    public static class ShortHelper
    {
        // 修改这个字符串，就可以生成与别人不同的结果！！！
        private static string cs = "m65pKkCes8VzxhGby9XNcfqPaWuE2jFYnUDT104LOdA3HIZoSvBigMwlrQ7JRt";
        private static Hashtable hash = new Hashtable();
        private static uint offset = (uint)cs.Length / 2;

        public static uint begin = 1020304050;

        public static string Encode(double v)
        {
            StringBuilder sb = new StringBuilder();
            uint va = (uint)v;
            va += begin;
            uint lastv = 0;
            while (va > 0)
            {
                uint vb = va % offset;
                va = (va - vb) / offset;
                if (sb.Length == 0) lastv = vb;
                sb.Append((sb.Length == 0) ? cs.Substring((int)vb, 1) : cs.Substring((int)(vb + lastv), 1));
            }
            return sb.ToString();
        }

        private static void Init()
        {
            for (int i = 0; i < cs.Length; i++)
            {
                hash.Add(cs.Substring(i, 1), i);
            }
        }

        public static int Decode(string s)
        {
            if (hash.Count == 0) Init();
            if (s.Length < 2) return 0;
            uint v = 0;
            uint lastv = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0) { v = (uint)((int)hash[s.Substring(i, 1)]); lastv = v; continue; }
                else { v += ((uint)((int)hash[s.Substring(i, 1)] - lastv)) * (uint)Math.Pow(offset, i); }
            }
            v -= begin;
            return (int)v;
        }
    }
}
