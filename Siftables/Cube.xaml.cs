using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using Siftables;
using System.Windows.Media.Imaging;
using Siftables.Behaviors;

namespace Siftables
{
    public partial class CubeView : UserControl
    {

        public CubeView()
        {
            InitializeComponent();

            Interaction.GetBehaviors(this).Add(new DragAndDropBehavior());
            Interaction.GetBehaviors(this).Add(new NeighborRefreshBehavior());
        }
        
    }
}
