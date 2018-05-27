using GamepadInput;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public GameObject Player;
        public float SmoothSpeed = 0.125f;
        public float TurnSpeed = 2.0f;

        private Vector3 _positionOffset;
        private Vector3 _positionRotation;
        private Vector3 _position;
        private Vector3 _lookAtOffset;
        private Vector3 _lookAtRotation;
        private Vector3 _lookAt;

        void Start()
        {
            _positionOffset = GetPositionOffset();
            _positionRotation = Vector3.zero;

            _lookAtOffset = GetLookAtOffset();
            _lookAtRotation = Vector3.zero;

            _lookAt = Player.transform.position + _lookAtOffset;
        }

        protected virtual void FixedUpdate()
        {
            UpdateRotation();

            var desiredPosition = Player.transform.position + Quaternion.Euler(_positionRotation) * _positionOffset;
            _position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
            transform.position = _position;

            var desiredLookAt = Player.transform.position + Quaternion.Euler(_lookAtRotation) * _lookAtOffset;
            _lookAt = Vector3.Lerp(_lookAt, desiredLookAt, SmoothSpeed);
            transform.LookAt(_lookAt);
        }

        private void UpdateRotation()
        {
            var joyStick = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);
            if (joyStick.sqrMagnitude < 0.01f)
                return;

            var angle = Vector3.Angle(Quaternion.Euler(_positionRotation) * _positionOffset, Vector3.up);
            if (angle > 30 && joyStick.y < 0 || angle < 150 && joyStick.y > 0)
            {
                _positionRotation.x -= joyStick.y * TurnSpeed;
                _lookAtRotation.x -= joyStick.y * TurnSpeed;
            }

            _positionRotation.y += joyStick.x * TurnSpeed;
            _lookAtRotation.y += joyStick.x * TurnSpeed;
        }

        //private void UpdateRotation()
        //{
        //    var joyStick = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);
        //    if (joyStick.sqrMagnitude < 0.01f)
        //        return;

        //    var upDownAngle = Vector3.Angle(_lookAtOffset - _positionOffset, Vector3.up);
        //    if (upDownAngle > 30 && joyStick.y > 0 || upDownAngle < 150 && joyStick.y < 0)
        //    {
        //        var upDown = Quaternion.AngleAxis(-joyStick.y * TurnSpeed, Vector3.Cross(Vector3.up, _lookAtOffset));
        //        _lookAtOffset = upDown * _lookAtOffset;
        //        _positionOffset = upDown * _positionOffset;
        //    }

        //    var leftRight = Quaternion.AngleAxis(joyStick.x * TurnSpeed, Vector3.up);
        //    _lookAtOffset = leftRight * _lookAtOffset;
        //    _positionOffset = leftRight * _positionOffset;
        //}

        private Vector3 GetPositionOffset()
        {
            return transform.position - Player.transform.position;
        }

        private Vector3 GetLookAtOffset()
        {
            var playerPlane = new Plane(Vector3.up, Player.transform.position);
            var ray = new Ray(transform.position, transform.forward);
            float dist = 0;
            playerPlane.Raycast(ray, out dist);
            var point = ray.GetPoint(dist);
            return point - Player.transform.position;
        }
    }
}
