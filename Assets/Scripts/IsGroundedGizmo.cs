using UnityEngine;

namespace Assets.Scripts
{
    public class IsGroundedGizmo : MonoBehaviour {

        private CharacterController _characterController;

        // Use this for initialization
        void Start()
        {
            _characterController = this.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void OnDrawGizmos() {
            if (_characterController != null)
            {
                if (_characterController.isGrounded)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(_characterController.transform.position, Vector3.one * 5);
                }
            }
        }
    }
}
