using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    public class RideTest : MonoBehaviour, IRideable
    {
        [SerializeField] private Rigidbody2D body;
        public void SetRiderVelocity(Vector2 velocity)
        {
            body.velocity = velocity;
        }
    }
}
