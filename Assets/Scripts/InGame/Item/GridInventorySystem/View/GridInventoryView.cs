using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    /// <summary>
    /// インベントリのゲーム上での表現と操作。
    /// </summary>
    public class GridInventoryView : UIBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image outline;
        [SerializeField] private RawImage slotImage;

        private Vector2 leftTopCorner;


        protected override void Awake()
        {
            leftTopCorner = GetLeftTopCorner();
        }

        public void OnPointerEnter(PointerEventData eventData) { }
        public void OnPointerExit(PointerEventData eventData) { }
        public void OnDrop(PointerEventData eventData) { }

        /// <summary>
        /// インベントリのサイズを設定する。
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(Vector2Int size)
        {
            outline.rectTransform.sizeDelta = (Vector2)size * GridInventory.SlotSize + new Vector2(2.0f, 2.0f);
            Rect uvRect = slotImage.uvRect;
            uvRect.size = size;
            slotImage.uvRect = uvRect;
            slotImage.SetNativeSize();
        }

        /// <summary>
        /// 座標からこのポケット内で選ばれたスロットを返す。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2Int GetSlotByPoint(Vector2 point)
        {
            Vector2 pointInRect = (point - leftTopCorner) * new Vector2(1, -1);
            Vector2 pointToSlot = pointInRect / GridInventory.SlotSize;
            return new Vector2Int((int)pointToSlot.x, (int)pointToSlot.y);
        }

        /// <summary>
        /// スロット座標から、そのスロット座標の左上のスクリーン座標を返す。
        /// </summary>
        /// <param name="slotPosition"></param>
        /// <returns></returns>
        public Vector2 GetPointBySlot(Vector2Int slotPosition)
        {
            //スロット座標を左下原点のスクリーン座標に変換する。
            Vector2 slotToScreenPoint = leftTopCorner + (slotPosition * new Vector2(1, -1) * GridInventory.SlotSize);
            return slotToScreenPoint;
        }


        /// <summary>
        /// このRectTransformの左上を原点としたスクリーン座標を返す。
        /// </summary>
        /// <returns></returns>
        private Vector2 GetLeftTopCorner()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            //Pivotの設定に応じてサイズオフセットを取得。Pivotが左下なら0,1 * サイズ。
            Vector2 offsetToLeftTop = ((rectTransform.pivot * -1.0f) + Vector2.up) * rectTransform.rect.size;
            return (Vector2)rectTransform.position + offsetToLeftTop;
        }
    }
}
