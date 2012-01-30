using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Siftables.Sifteo
{
    public class CubeSet : ObservableCollection<Cube>
    {
        Canvas workspace;

        //public CubeSet(Canvas workspace)
        public CubeSet()
        {
            //this.workspace = workspace;
        }

        public void RemoveCubes(int count)
        {
            int index = this.Count - count;

            //move this code to viewmodel
            //for (int i = index; i < index + count; i++)
            //{
            //    workspace.Children.Remove(this[i]);
            //}
            //this.RemoveRange(index, count);
        }

        public void AddCubes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Cube cube = new Cube();
                //move this code to viewmodel
                //Canvas.SetLeft(cube, 10);
                //Canvas.SetTop(cube, 10);
                //workspace.Children.Add(cube);
                this.Add(cube);
            }
        }
    }

}
