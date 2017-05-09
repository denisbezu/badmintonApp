using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonWPF.Helpers
{
    public class Nums
    {
        public List<int> Nums64 { get; set; }
        public List<int> Nums32 { get; set; }
        public List<int> Nums16 { get; set; }
        public List<int> Nums8 { get; set; }
        public List<int> Nums4 { get; set; }

        public Nums()
        {
            Nums64 = new List<int>(64);
            Nums32 = new List<int>(32);
            Nums16 = new List<int>(16);
            Nums8 = new List<int>(8);
            Nums4 = new List<int>(4);
            ZeroInit();
            list_add_nums16();
            list_add_nums32();
            list_add_nums4();
            list_add_nums8();
            list_add_nums64();
        }
        public void list_add_nums32()
        {
            Nums32[0] = 1;
            Nums32[1] = 32;
            Nums32[2] = 17;
            Nums32[3] = 16;
            Nums32[4] = 9;
            Nums32[5] = 24;
            Nums32[6] = 25;
            Nums32[7] = 8;
            Nums32[8] = 5;
            Nums32[9] = 28;
            Nums32[10] = 21;
            Nums32[11] = 12;
            Nums32[12] = 13;
            Nums32[13] = 20;
            Nums32[14] = 29;
            Nums32[15] = 4;
            Nums32[16] = 3;
            Nums32[17] = 30;
            Nums32[18] = 19;
            Nums32[19] = 14;
            Nums32[20] = 11;
            Nums32[21] = 22;
            Nums32[22] = 27;
            Nums32[23] = 6;
            Nums32[24] = 7;
            Nums32[25] = 26;
            Nums32[26] = 23;
            Nums32[27] = 10;
            Nums32[28] = 15;
            Nums32[29] = 18;
            Nums32[30] = 31;
            Nums32[31] = 2;
        }
        public void list_add_nums16()
        {
            Nums16[0] = 1;
            Nums16[1] = 16;
            Nums16[2] = 9;
            Nums16[3] = 8;
            Nums16[4] = 5;
            Nums16[5] = 12;
            Nums16[6] = 13;
            Nums16[7] = 4;
            Nums16[8] = 3;
            Nums16[9] = 14;
            Nums16[10] = 11;
            Nums16[11] = 6;
            Nums16[12] = 7;
            Nums16[13] = 10;
            Nums16[14] = 15;
            Nums16[15] = 2;
        }
        public void list_add_nums8()
        {
            Nums8[0] = 1;
            Nums8[1] = 8;
            Nums8[2] = 5;
            Nums8[3] = 4;
            Nums8[4] = 3;
            Nums8[5] = 6;
            Nums8[6] = 7;
            Nums8[7] = 2;
        }
        public void list_add_nums4()
        {
            Nums4[0] = 1;
            Nums4[1] = 4;
            Nums4[2] = 3;
            Nums4[3] = 2;
        }
        public void list_add_nums64()
        {
            Nums64[0] = 1;
            Nums64[1] = 64;
            Nums64[2] = 33;
            Nums64[3] = 32;
            Nums64[4] = 48;
            Nums64[5] = 17;
            Nums64[6] = 16;
            Nums64[7] = 49;
            Nums64[8] = 56;
            Nums64[9] = 9;
            Nums64[10] = 24;
            Nums64[11] = 41;
            Nums64[12] = 40;
            Nums64[13] = 25;
            Nums64[14] = 8;
            Nums64[15] = 57;
            Nums64[16] = 60;
            Nums64[17] = 5;
            Nums64[18] = 28;
            Nums64[19] = 37;
            Nums64[20] = 44;
            Nums64[21] = 21;
            Nums64[22] = 12;
            Nums64[23] = 53;
            Nums64[24] = 52;
            Nums64[25] = 13;
            Nums64[26] = 20;
            Nums64[27] = 45;
            Nums64[28] = 36;
            Nums64[29] = 29;
            Nums64[30] = 61;
            Nums64[31] = 4;
            Nums64[32] = 3;
            Nums64[33] = 62;
            Nums64[34] = 35;
            Nums64[35] = 30;
            Nums64[36] = 19;
            Nums64[37] = 46;
            Nums64[38] = 51;
            Nums64[39] = 14;
            Nums64[40] = 11;
            Nums64[41] = 54;
            Nums64[42] = 43;
            Nums64[43] = 22;
            Nums64[44] = 27;
            Nums64[45] = 38;
            Nums64[46] = 59;
            Nums64[47] = 6;
            Nums64[48] = 7;
            Nums64[49] = 58;
            Nums64[50] = 39;
            Nums64[51] = 26;
            Nums64[52] = 23;
            Nums64[53] = 42;
            Nums64[54] = 55;
            Nums64[55] = 10;
            Nums64[56] = 15;
            Nums64[57] = 50;
            Nums64[58] = 47;
            Nums64[59] = 18;
            Nums64[60] = 31;
            Nums64[61] = 34;
            Nums64[62] = 63;
            Nums64[63] = 2;
        }
        private void ZeroInit()
        {
            for (int i = 0; i < 32; i++)
            {
                Nums32.Add(0);
            }
            for (int i = 0; i < 64; i++)
            {
                Nums64.Add(0);
            }
            for (int i = 0; i < 16; i++)
            {
                Nums16.Add(0);
            }
            for (int i = 0; i < 8; i++)
            {
                Nums8.Add(0);
            }
            for (int i = 0; i < 4; i++)
            {
                Nums4.Add(0);
            }
        }
    }
}
