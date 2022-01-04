using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

namespace yumehiko.Item.GridInventorySystem
{
    public class GridItemView : Selectable, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public System.IObservable<PointerEventData> OnDragStart => onDragStart;
        public System.IObservable<Unit> OnDragEnd => onDragEnd;
        public Sprite ItemSprite { get; private set; } 

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image itemImage;
        [SerializeField] private Text stackAmountText;

        private Subject<PointerEventData> onDragStart = new Subject<PointerEventData>();
        private Subject<Unit> onDragEnd = new Subject<Unit>();


        public void OnBeginDrag(PointerEventData eventData) => onDragStart.OnNext(eventData);

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData) => onDragEnd.OnNext(Unit.Default);


        public void Move(Transform parent, Vector2Int slotPosition)
        {
            rectTransform.SetParent(parent);
            Vector2 localPosition = new Vector2(slotPosition.x, -slotPosition.y) * GridInventory.SlotSize;
            transform.localPosition = localPosition;
        }

        public void SetItemData(GridItem itemData)
        {
            itemImage.sprite = itemData.Sprite;
            ItemSprite = itemData.Sprite;
            stackAmountText.text = $"{itemData.Amount}";
            rectTransform.sizeDelta = (Vector2)itemData.Size * GridInventory.SlotSize;
        }
    }
}
