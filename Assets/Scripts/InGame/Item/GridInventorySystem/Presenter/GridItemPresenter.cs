using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace yumehiko.ItemSystem.GridInventory
{
    public class GridItemPresenter : MonoBehaviour
    {
        public GridItem Model => model;
        public GridItemIdea Idea => model.Idea;
        public GridInventoryPresenter Inventory { get; private set; }

        [SerializeField] private GridItemView view;
        private GridItem model;


        public void Initialize(GridItem itemData, GridInventoryPresenter inventory)
        {
            Vector2 localScreenPosition = new Vector2(itemData.SlotPosition.x, -itemData.SlotPosition.y) * GridInventory.SlotSize;
            transform.localPosition = localScreenPosition;
            SetItemData(itemData, inventory);
        }

        /// <summary>
        /// このアイテムを、指定したインベントリの指定したスロットに移動させる。
        /// </summary>
        public void MoveTo(GridInventoryPresenter inventory, Vector2Int slotPosition)
        {
            Inventory.RemoveItemAtModel(model);
            model.Move(slotPosition);
            view.Move(inventory.transform, slotPosition);
            Inventory = inventory;
        }

        public void ChangeColorByMergeResult(MergeCheckResult result) => view.ChangeColorByMergeResult(result);
        public void ResetColorToWhite() => view.ResetColorToWhite();


        private void SetItemData(GridItem itemData, GridInventoryPresenter inventory)
        {
            Inventory = inventory;
            model = itemData;
            view.SetView(itemData.Idea.Sprite, itemData.Stack.Value, itemData.Size);

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
            Inventory.RemoveItemAtModel(model);
            Destroy(gameObject);
        }
    }
}
