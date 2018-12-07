using System;
using UnityEngine;

namespace Graphene.CameraManagement
{
    public interface ICameraBehavior
    {
        event Action BlockScene, UnblockScene;
        
        void SetTarget(Transform target);
    }
}