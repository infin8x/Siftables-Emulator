using System;
using System.Collections.Generic;
using System.Linq;
using Siftables.ViewModel;
using Sifteo;

namespace Siftables
{
    public static class NeighborCalculator
    {

        public static void CalculateNeighbors(ICollection<CubeViewModel> cubeViewModels)
        {
            // I'd like to eliminate this loop... but we have to reset everything before we can start processing neighbors
            foreach (var cube in cubeViewModels.Select(cubeViewModel => cubeViewModel.CubeModel))
            {
                cube.Neighbors = new Neighbors();
            }
            for (var i = 0; i < cubeViewModels.Count - 1; i++)
            {
                var cubeViewModel = cubeViewModels.ToArray()[i];
                var cubeViewModelX = cubeViewModel.PositionX;
                var cubeViewModelY = cubeViewModel.PositionY;
                var cube = cubeViewModel.CubeModel;
                for (var j = i + 1; j < cubeViewModels.Count; j++)
                {
                    var otherCubeViewModel = cubeViewModels.ToArray()[j];
                    // If anybody knows a better way to do this, please fix it.  The only way I could get
                    // the DP to expose its value is through its ToString method...
                    var otherCubeViewModelX = otherCubeViewModel.PositionX;
                    var otherCubeViewModelY = otherCubeViewModel.PositionY;
                    var otherCube = otherCubeViewModel.CubeModel;
                    if ((Math.Abs(cubeViewModelX - otherCubeViewModelX) <= (Neighbors.GAP_TOLERANCE + Cube.dimension)) && (Math.Abs(cubeViewModelY - otherCubeViewModelY) <= (Cube.dimension - Neighbors.SHARED_EDGE_MINIMUM)))
                    {
                        if (cubeViewModelX < otherCubeViewModelX)
                        {
                            cube.Neighbors.Right = otherCube;
                            otherCube.Neighbors.Left = cube;
                        }
                        else
                        {
                            cube.Neighbors.Left = otherCube;
                            otherCube.Neighbors.Right = cube;
                        }
                    }
                    if ((Math.Abs(cubeViewModelY - otherCubeViewModelY) <= (Neighbors.GAP_TOLERANCE + Cube.dimension)) && (Math.Abs(cubeViewModelX - otherCubeViewModelX) <= (Cube.dimension - Neighbors.SHARED_EDGE_MINIMUM)))
                    {
                        if (cubeViewModelY < otherCubeViewModelY)
                        {
                            cube.Neighbors.Top = otherCube;
                            otherCube.Neighbors.Bottom = cube;
                        }
                        else
                        {
                            cube.Neighbors.Bottom = otherCube;
                            otherCube.Neighbors.Top = cube;
                        }
                    }
                }
            }
        }
    }
}
