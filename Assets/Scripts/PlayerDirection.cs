using System;
using GamepadInput;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerDirection : MonoBehaviour
    {
        public GameObject Camera;
        public float TurnSpeed = 0.5f;

        private Vector3 _lookAt;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void LateUpdate()
        {
            var joyStick = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
            if (Math.Abs(joyStick.sqrMagnitude) < 0.1f)
                return;

            joyStick.Normalize();

            var cameraDir = Camera.transform.eulerAngles;
            var cameraRotation = Quaternion.Euler(0.0f, cameraDir.y, 0.0f);

            var desiredLookAt = transform.position + cameraRotation * new Vector3(joyStick.x, 0.0f, joyStick.y) * 10;
            _lookAt = Vector3.Lerp(_lookAt, desiredLookAt, TurnSpeed);
            _lookAt.y = transform.position.y;

            transform.LookAt(_lookAt);
        }
    }
}
