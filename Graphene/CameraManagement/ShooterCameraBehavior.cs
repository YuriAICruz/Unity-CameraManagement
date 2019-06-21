using UnityEngine;

namespace Graphene.CameraManagement
{
    public class ShooterCameraBehavior : CameraBehavior
    {
        public float AimSpeed = 1;

        [HideInInspector] public Vector3 AimDirection;

        public float YMax = 1, YMin = -1;

        public LayerMask LevelMask;
        private Transform _rig;

        protected override void Awake()
        {
            base.Awake();

            _rig = new GameObject("Rig").transform;
            _rig.SetParent(transform);
            _rig.localPosition = Vector3.zero;
        }

        public override void SetTarget(Transform target)
        {
            base.SetTarget(target);
            _rig.position = target.position;

            AimDirection = target.position;
            NomalizeAim();
            //CastAim();
        }

        private void CastAim()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, AimDirection - transform.position, out hit, 100, LevelMask))
            {
                AimDirection = hit.point;
                Debug.DrawRay(AimDirection, Vector3.up * 0.5f, Color.green);
            }
        }

        protected override void Update()
        {
            if (_target == null) return;

            //FollowTarget();
        }

        public void Aim(Vector2 delta)
        {
            if (_target == null) return;

            var d = transform.TransformDirection(delta);

            AimDirection += d * AimSpeed;
            NomalizeAim();

            transform.LookAt(transform.position + AimDirection, Vector3.up);

            _rig.position = _target.position;
            _rig.LookAt(_rig.position + AimDirection, Vector3.up);

            transform.position = _rig.TransformPoint(Offset);
            CheckCollision();
        }

        private void NomalizeAim()
        {
            //var dir = -transform.position + AimPosition;
            AimDirection = AimDirection.normalized;
            AimDirection.y = Mathf.Min(YMax, Mathf.Max(YMin, AimDirection.y));
        }
    }
}