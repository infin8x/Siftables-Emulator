using System.Windows.Controls;
using System.Windows.Interactivity;
using Siftables.Behaviors;

namespace Siftables
{
    public partial class CubeView : UserControl
    {
        public CubeView()
        {
            InitializeComponent();

            Interaction.GetBehaviors(this).Add(new DragAndDropBehavior(this.Resources["CubeViewModel"]));
            Interaction.GetBehaviors(this).Add(new NeighborRefreshBehavior());
        }
    }
}
