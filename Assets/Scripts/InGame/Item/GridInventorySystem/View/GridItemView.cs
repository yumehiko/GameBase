using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

namespace yumehiko.Item.GridInventorySystem
{
    public class GridItemView : Selectable, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        public static readonly Color32 MergeForbidColor = new Color32(255, 128, 128, 255);
        public static readonly Color32 MergePermitColor = new Color32(128, 128, 255, 255);

        public Sprite ItemSprite { get; private set; } 

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image boxImage;
        [SerializeField] private Text stackAmountText;

        public void OnBeginDrag(PointerEventData eventData)
        {
            boxImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            boxImage.raycastTarget = true;
        }

        public void OnDrop(PointerEventData eventData) { }

        public void Move(Transform parent, Vector2Int slotPosition)
        {
            rectTransform.SetParent(parent);
            Vector2 localPosition = new Vector2(slotPosition.x, -slotPosition.y) * GridInventory.SlotSize;
            transform.localPosition = localPosition;
        }

        public void SetView(Sprite sprite, int amount, Vector2Int slotSize)
        {
            itemImage.sprite = sprite;
            ItemSprite = sprite;
            stackAmountText.text = $"{amount}";
            rectTransform.sizeDelta = (Vector2)slotSize * GridInventory.SlotSize;
        }

        public void RefleshAmountView(int amount)
        {
            stackAmountText.text = $"{amount}";
        }

        /// <summary>
        /// マージ許可結果に応じて、スプライトの色を変える。
        /// </summary>
        /// <param name="color"></param>
        public void ChangeColorByMergeResult(MergeCheckResult mergeCheckResult)
        {
            if(mergeCheckResult == MergeCheckResult.Permit)
            {
                itemImage.color = MergePermitColor;
                return;
            }

            if(mergeCheckResult == MergeCheckResult.Forbid)
            {
                itemImage.color = MergeForbidColor;
                return;
            }

            itemImage.color = Color.white;
        }

        /// <summary>
        /// スプライトの色をホワイトに戻す。
        /// </summary>
        public void ResetColorToWhite()
        {
            itemImage.color = Color.white;
        }
    }
}
