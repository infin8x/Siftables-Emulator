﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Siftables.Sifteo
{
    public class CubeSet : Collection<CubeSift>
    {

        public CubeSet()
        {
        }

        public void ClearEvents()
        {
            throw new NotImplementedException();
        }

        public void ClearUserData()
        {
            throw new NotImplementedException();
        }

        public CubeSift[] toArray()
        {
            CubeSift[] cubeArr = new CubeSift[Count];
            for (int i = 0; i < Count; i++)
            {
                cubeArr[i++] = this[i];
            }

            return cubeArr;
        }

        // Currently always returns the first Cube
        public CubeSift CubeByID(String id)
        {
            return this[0];
        }

    }

}
