using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickActionGameApp
{
    class CubeData
    {
        public string ImgSource { get; set; }
        public int TickCount { get; set; }
        private const int TickLimit = 5;

        public CubeData(String imgSource)
        {
            ImgSource = imgSource;
            TickCount = 0;

        }

        public void Tick()
        {
            TickCount++;
        }

        public bool TickLimitReached()
        {
            return TickCount >= TickLimit;
        }



        


    }
}
