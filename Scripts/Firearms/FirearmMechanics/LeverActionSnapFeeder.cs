using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class LeverActionSnapFeeder : MonoBehaviour
    {
        public BoltSemiautomatic bolt;
        public Transform axis;
        public Transform idlePosition;
        public Transform loadingPosition;
        private BoltBase.BoltState lastState;

        private void FixedUpdate()
        {
            if (lastState != BoltBase.BoltState.Back && bolt.state == BoltBase.BoltState.Back)
                axis.SetLocalPositionAndRotation(loadingPosition.localPosition, loadingPosition.localRotation);
            
            if (lastState != BoltBase.BoltState.Locked && bolt.state == BoltBase.BoltState.Locked)
                axis.SetLocalPositionAndRotation(idlePosition.localPosition, idlePosition.localRotation);
            lastState = bolt.state;
        }
    }
}
