using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    public class GridInventoryCursor : MonoBehaviour
    {
        [SerializeField] private Image ghostImage;

        private System.IDisposable cursorObserveer;

        /// <summary>
        /// ドラッグ中のゴースト（ドラッグ中のアイテムの補助表示）を切り替える。
        /// </summary>
        /// <param name="isOn"></param>
        /// <param name="sprite"></param>
        public void StartDragGhost(PointerEventData eventData, Sprite sprite, Vector2 pivot)
        {
            ghostImage.enabled = true;
            ghostImage.sprite = sprite;
            ghostImage.SetNativeSize();
            transform.position = eventData.position;

            Vector2 offset = pivot - eventData.position;

            cursorObserveer = Observable.EveryUpdate()
                .Subscribe(_ => transform.position = eventData.position + offset)
                .AddTo(this);
        }

        public void EndDragGhost()
        {
            cursorObserveer.Dispose();
            ghostImage.enabled = false;
        }
    }
}
