using Graphene.CameraManagement;
using UnityEngine;

namespace DefaultNamespace
{
    public class FirstPersonCameraBehavior: CameraBehavior
    {
        public float AimSpeed = 1;

        [HideInInspector] public Vector3 AimDirection;

        public float YMax = 1, YMin = -1;

        public LayerMask LevelMask;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void SetTarget(Transform target)
        {
            base.SetTarget(target);

            AimDirection = target.position;
            NomalizeAim();
        }

        protected override void Update()
        {
            if (_target == null) return;
        }

        public void Aim(Vector2 delta)
        {
            if (_target == null) return;
            
            var d = transform.TransformDirection(delta);

            AimDirection += AimSpeed * Time.deltaTime * d;
            NomalizeAim();

            transform.LookAt(transform.position + AimDirection, Vector3.up);
        }

        private void NomalizeAim()
        {
            AimDirection = AimDirection.normalized;
            AimDirection.y = Mathf.Min(YMax, Mathf.Max(YMin, AimDirection.y));
        }
    }
}