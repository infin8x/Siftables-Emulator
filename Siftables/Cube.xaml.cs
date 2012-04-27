using System.Windows.Controls;
using System.Windows.Interactivity;
using Siftables.Behaviors;

namespace Siftables
{
    public partial class CubeView : UserControl
    {
        //private ItemsControl _workspaceCubes;

        //public ItemsControl WorkspaceCubes { get { return _workspaceCubes; } set { value = _workspaceCubes; } }

        public CubeView()
        {
            InitializeComponent();

            Interaction.GetBehaviors(this).Add(new DragAndDropBehavior(this.Resources["CubeViewModel"]));
            Interaction.GetBehaviors(this).Add(new NeighborRefreshBehavior());
        }
    }
}
