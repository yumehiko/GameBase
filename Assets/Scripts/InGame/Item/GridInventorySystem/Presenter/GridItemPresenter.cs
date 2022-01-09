using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace yumehiko.Item.GridInventorySystem
{
    public class GridItemPresenter : MonoBehaviour
    {
        public GridItem Model => model;
        public Item Item => model.Item;
        public GridInventoryPresenter Inventory { get; private set; }

        [SerializeField] private GridItemView view;
        private GridItem model;
        Vector2Int currentSlotPosition;

        public void Instantiate(GridItem itemData, GridInventoryPresenter inventory, Vector2Int slotPosition)
        {
            GridItemPresenter instance = Instantiate(this, inventory.transform);
            Vector2 localPosition = new Vector2(slotPosition.x, -slotPosition.y) * GridInventory.SlotSize;
            instance.transform.localPosition = localPosition;
            instance.SetItemData(itemData, inventory, slotPosition);
        }

        /// <summary>
        /// このアイテムを、指定したインベントリのスロットに移動させる。
        /// </summary>
        public void Move(GridInventoryPresenter inventory, Vector2Int slotPosition)
        {
            view.Move(inventory.transform, slotPosition);
            Inventory.RemoveItem(model, currentSlotPosition);
            Inventory = inventory;
            currentSlotPosition = slotPosition;
        }

        public void ChangeColorByMergeResult(MergeCheckResult result) => view.ChangeColorByMergeResult(result);
        public void ResetColorToWhite() => view.ResetColorToWhite();


        private void SetItemData(GridItem itemData, GridInventoryPresenter inventory, Vector2Int slotPosition)
        {
            Inventory = inventory;
            model = itemData;
            currentSlotPosition = slotPosition;
            view.SetView(itemData.Sprite, itemData.Stack.Value, itemData.Size);

            GridInventoryCursor Cursor = Inventory.Cursor;

            //viewとmodelのイベント購読。
            _ = view.OnBeginDragAsObservable()
                .Subscribe(eventData => Inventory.Cursor.StartDrag(this, eventData))
                .AddTo(this);

            _ = view.OnEndDragAsObservable()
                .Subscribe(_ => Cursor.EndDrag())
                .AddTo(this);

            _ = view.OnDropAsObservable()
                .Subscribe(_ => Cursor.DropToItem(this))
                .AddTo(this);

            _ = view.OnPointerEnterAsObservable()
                .Subscribe(_ => Cursor.EnterItem(this))
                .AddTo(this);

            _ = view.OnPointerExitAsObservable()
                .Subscribe(_ => Cursor.ExitItem(this))
                .AddTo(this);

            _ = model.Stack
                .Subscribe(amount => view.RefleshAmountView(amount))
                .AddTo(this);

            _ = model.Stack
                .Where(amount => amount <= 0)
                .Subscribe(_ => Remove())
                .AddTo(this);
        }

        private void Remove()
        {
            Inventory.RemoveItem(model, currentSlotPosition);
            Destroy(gameObject);
        }
    }
}
