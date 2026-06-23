using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Keeps this transform's rotation at identity in world space.
    /// Used for particle anchors so particles don't rotate with the ball.
    /// Position follows parent, but rotation stays fixed.
    /// </summary>
    public class WorldSpaceAnchor : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
