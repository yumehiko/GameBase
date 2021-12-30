using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    public class RideTest : MonoBehaviour, IMovePlatformRider
    {
        [SerializeField] private Rigidbody2D body;
        private Vector2 additionVelocity;
        private System.IDisposable platformObserver;
        private IMovePlatform ridingPlatform;

        private void FixedUpdate()
        {
            body.velocity = new Vector2(additionVelocity.x, body.velocity.y);
        }

        public void Ride(IMovePlatform platform)
        {
            platformObserver?.Dispose();
            ridingPlatform = platform;
            platformObserver = platform.VelocityToRider.Subscribe(velocity => additionVelocity = velocity).AddTo(this);
        }

        public void GetOff(IMovePlatform platform)
        {
            if(ridingPlatform != platform)
            {
                return;
            }

            ridingPlatform = null;
            platformObserver?.Dispose();
            additionVelocity = Vector2.zero;
        }
    }
}
