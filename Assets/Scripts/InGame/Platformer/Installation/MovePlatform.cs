using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 動く足場。
    /// </summary>
    public class MovePlatform : MonoBehaviour, IMovePlatform
    {
        /// <summary>
        /// 乗ったものに与える速度。
        /// </summary>
        public ReadOnlyReactiveProperty<Vector2> VelocityToRider => velocityToRider.ToReadOnlyReactiveProperty();


        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float speed;
        [SerializeField] private List<Vector2> paths;
        private Vector2ReactiveProperty velocityToRider = new Vector2ReactiveProperty();


        private void Awake()
        {
            //指定したパスに沿って移動する。
            _ = body.DOLocalPath(paths.ToArray(), speed)
                .SetOptions(true)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .SetLoops(-1, LoopType.Restart)
                .OnWaypointChange(id => SetVelocityToRider(id))
                .SetLink(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IMovePlatformRider rider = collision.gameObject.GetComponent<IMovePlatformRider>();
            if (rider == null)
            {
                return;
            }
            rider.Ride(this);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            IMovePlatformRider rider = collision.gameObject.GetComponent<IMovePlatformRider>();
            if (rider == null)
            {
                return;
            }

            rider.GetOff(this);
        }

        /// <summary>
        /// 次のポイントへの角度から速度を計算し、速度を保持する。
        /// </summary>
        /// <param name="pathID"></param>
        private void SetVelocityToRider(int pathID)
        {
            Vector2 currentPoint = transform.localPosition;
            int nextPathID = pathID + 1 >= paths.Count ? 0 : pathID + 1;
            Vector2 nextPoint = paths[nextPathID];

            var velocity = -GetVectorByPointAndSpeed(currentPoint, nextPoint, speed);
            velocityToRider.Value = velocity;
        }

        /// <summary>
        /// 2点の座標から方向を計算し、速度と掛けたベクトルを返す。
        /// </summary>
        /// <param name="currentPoint"></param>
        /// <param name="targetPoint"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        private Vector2 GetVectorByPointAndSpeed(Vector2 currentPoint, Vector2 targetPoint, float speed)
        {
            var heading = currentPoint - targetPoint;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction * speed;
        }
    }
}
