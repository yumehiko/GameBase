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
    public class MoveFloor : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float speed;
        [SerializeField] private List<Vector2> paths;

        private Vector2ReactiveProperty riderVelocity = new Vector2ReactiveProperty();
        private List<IRideable> riders = new List<IRideable>();

        private void Awake()
        {
            riderVelocity.Subscribe(_ => VelocityChanged());
            _ = body.DOLocalPath(paths.ToArray(), speed)
                .SetOptions(true)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .SetLoops(-1, LoopType.Restart)
                .OnWaypointChange(id => OnReachPath(id))
                .SetLink(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IRideable rider = collision.gameObject.GetComponent<IRideable>();
            if (rider == null)
            {
                return;
            }

            rider.SetRiderVelocity(riderVelocity.Value);
            riders.Add(rider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            IRideable rider = collision.gameObject.GetComponent<IRideable>();
            if (rider == null)
            {
                return;
            }

            rider.SetRiderVelocity(Vector2.zero);
            riders.Remove(rider);
        }



        private void OnReachPath(int pathID)
        {
            Vector2 currentPoint = transform.localPosition;
            int nextPathID = pathID + 1 >= paths.Count ? 0 : pathID + 1;
            Vector2 nextPoint = paths[nextPathID];

            riderVelocity.Value = -GetVectorByPointAndSpeed(currentPoint, nextPoint, speed);
        }

        private Vector2 GetVectorByPointAndSpeed(Vector2 currentPoint, Vector2 targetPoint, float speed)
        {
            var heading = currentPoint - targetPoint;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction * speed;
        }

        private void VelocityChanged()
        {
            foreach (IRideable rider in riders)
            {
                rider.SetRiderVelocity(riderVelocity.Value);
            }
        }
    }
}
