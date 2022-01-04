using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    public class GridItemPresenter : MonoBehaviour
    {
        public GridItem ItemData => model;
        public GridInventoryPresenter Inventory { get; private set; }

        [SerializeField] private GridItemView view;
        private GridItem model;
        Vector2Int currentSlotPosition;

        public void Instantiate(GridItem itemData, GridInventoryPresenter inventory, Vector2Int slotPosition)
        {
            GridItemPresenter instance = Instantiate(this, inventory.transform);
            Vector2 localPosition = new Vector2(slotPosition.x, -slotPosition.y) * GridInventory.SlotSize;
            instance.transform.localPosition = localPosition;
            instance.SetData(itemData, inventory, slotPosition);
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


        private void SetData(GridItem itemData, GridInventoryPresenter inventory, Vector2Int slotPosition)
        {
            Inventory = inventory;
            model = itemData;
            currentSlotPosition = slotPosition;
            view.SetItemData(itemData);
            _ = view.OnDragStart
                .Subscribe(eventData => Inventory.Cursor.StartDragGhost(eventData, view.ItemSprite, transform.position))
                .AddTo(this);

            _ = view.OnDragEnd
                .Subscribe(_ => Inventory.Cursor.EndDragGhost())
                .AddTo(this);
        }
    }
}
