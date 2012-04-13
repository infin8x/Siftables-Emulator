
namespace Sifteo.Util
{
    public interface IStateController
    {
        void OnSetup(string transitionId);

        void OnTick(float dt);

        void OnPaint(bool canvasDirty);

        void OnDispose();
    }
}