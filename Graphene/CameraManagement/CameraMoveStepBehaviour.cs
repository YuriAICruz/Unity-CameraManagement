using System;
using System.Collections;
using UnityEngine;
using Vuforia;

namespace Graphene.CameraManagement
{
    [RequireComponent(typeof(Camera))]
    public class CameraMoveStepBehaviour : MonoBehaviour, ICameraBehavior
    {
        public event Action BlockScene;
        public event Action UnblockScene;

        private Transform _target;

        public float Speed;
        private Vector3 _position;
        public Vector3 Offset;
        private Camera _cam;
        private float _horzExtent;
        private float _vertExtent;

        public float Border;
        public Vector3 RoomSize;

        [HideInInspector]
        public Vector3 TopLeft;
        [HideInInspector]
        public Vector3 BottonRight;

        private bool _blocked;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            _cam.orthographic = true;

            _position = transform.position;
        }

        public void CalculateCameraEdges()
        {
            _vertExtent = _cam.orthographicSize;
            _horzExtent = (float) _cam.orthographicSize * 16 / 9f;

            TopLeft = new Vector3(
                _position.x + _horzExtent - Border,
                _position.y + _vertExtent - Border
            );

            BottonRight = new Vector3(
                _position.x - _horzExtent + Border,
                _position.y - _vertExtent + Border
            );
        }

        private void Update()
        {
            if (_blocked) return;
            
            _position = transform.position;
            CalculateCameraEdges();

            var tlDir =  _target.position - TopLeft;
            var brDir = _target.position - BottonRight;

            Debug.Log($"{TopLeft}-{tlDir} {BottonRight}-{brDir}");

            if (tlDir.x > 0)
            {
                StartCoroutine(MoveTo(new Vector3Int(1, 0, 0)));
                return;
            }
            if (tlDir.y > 0)
            {
                StartCoroutine(MoveTo(new Vector3Int(0, 1, 0)));
                return;
            }
            if (brDir.x < 0)
            {
                StartCoroutine(MoveTo(new Vector3Int(-1, 0, 0)));
                return;
            }
            if (brDir.y < 0)
            {
                StartCoroutine(MoveTo(new Vector3Int(0, -1, 0)));
                return;
            }
        }

        IEnumerator MoveTo(Vector3Int dir)
        {
            _blocked = true;
            BlockScene?.Invoke();

            var t = 0f;

            var p = _position;
            var destination = _position + Vector3.Scale(dir, RoomSize);
            while (t < 1)
            {
                _position = Vector3.Lerp(p, destination, t);

                t += Time.deltaTime * Speed;

                transform.position = _position;
                yield return null;
            }

            _position = Vector3.Lerp(p, destination, 1);
            transform.position = _position;
            
            _blocked = false;
            UnblockScene?.Invoke();
        }


        public void SetTarget(Transform target)
        {
            _target = target;
            _position = _target.position;
        }
    }
}