using GamepadInput;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject Camera;
        public float Friction = 0.15f;
        public float Speed = 2.0f;
        public float Gravity = 30.0f;
        public float FallingGravityFactor = 2.5f;
        public float JumpingGravityFactor = 1.5f;
        public float JumpSpeed = 15.0f;
        public float AirMovement = 0.5f;

        private CharacterController _characterController;

        // Use this for initialization
        void Start()
        {
            _characterController = this.GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            var velocity = _characterController.velocity;

            velocity += GetMovementVelocity();
            velocity += GetJumpVelocity();
            velocity += GetFriction();
            velocity += GetGravity();

            _characterController.Move(velocity * Time.fixedDeltaTime);
        }

        private Vector3 GetFriction()
        {
            var velocity = _characterController.velocity;
            velocity.y = 0;

            var acceleration = -Friction * velocity;

            return _characterController.isGrounded
                ? acceleration
                : acceleration * AirMovement;
        }

        private Vector3 GetGravity()
        {
            var velocity = Vector3.down * Gravity * Time.fixedDeltaTime;

            if (_characterController.velocity.y < 0)
            {
                return velocity * FallingGravityFactor;
            }
            else if (!GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) && !_characterController.isGrounded)
            {
                return velocity * JumpingGravityFactor;
            }

            return velocity;
        }

        private Vector3 GetJumpVelocity()
        {
            return _characterController.isGrounded && GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any)
                ? Vector3.up * JumpSpeed
                : Vector3.zero;
        }

        private Vector3 GetMovementVelocity()
        {
            var joyStick = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);

            var cameraDir = Camera.transform.eulerAngles;
            var cameraRotation = Quaternion.Euler(0.0f, cameraDir.y, 0.0f);

            var velocity = cameraRotation * new Vector3(joyStick.x, 0.0f, joyStick.y) * Speed;

            return _characterController.isGrounded
                ? velocity
                : velocity * AirMovement;
        }
    }
}
