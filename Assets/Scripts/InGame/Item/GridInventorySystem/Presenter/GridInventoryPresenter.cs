using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

namespace yumehiko.Item.GridInventorySystem
{
    /// <summary>
    /// グリッドインベントリのViewとModelの橋渡し。
    /// </summary>
    public class GridInventoryPresenter : MonoBehaviour
    {
        public GridInventoryCursor Cursor => cursor;

        [SerializeField] private GridInventoryCursor cursor;
        [SerializeField] private Vector2Int size;
        [SerializeField] private GridInventoryView view;
        [SerializeField] private GridItemPresenter itemPresenterPrefab;
        private GridInventory model;

        private void Awake()
        {
            model = new GridInventory(size);
            view.SetSize(size);

            _ = view.OnDropEvent
                .Subscribe(data => TryMoveDragAndDropItem(data))
                .AddTo(this);
        }


        /// <summary>
        /// このインベントリに、無からアイテムを追加する。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slotPosition"></param>
        public void GenerateItem(GridItem item, Vector2Int slotPosition)
        {
            //配置できないならエラー。
            if (!model.CanPlace(slotPosition, item))
            {
                model.DebugSlotEmptyInfo();
                throw new System.Exception("ここには配置できない。");
            }

            itemPresenterPrefab.Instantiate(item, this, slotPosition);
            model.AddItem(item, slotPosition);
        }

        /// <summary>
        /// このインベントリから、指定したアイテムを削除する。
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(GridItem item, Vector2Int slotPosition)
        {
            model.RemoveItem(item, slotPosition);
        }


        /// <summary>
        /// このインベントリに、ドラッグアンドドロップでアイテムを移動させる。
        /// </summary>
        /// <param name="eventData"></param>
        private void TryMoveDragAndDropItem(PointerEventData eventData)
        {
            GridItemPresenter itemPresenter = eventData.pointerDrag.GetComponent<GridItemPresenter>();
            if (itemPresenter == null)
            {
                return;
            }

            Vector2Int slotPosition = view.GetSlotByPoint(eventData.position);
            GridItem item = itemPresenter.ItemData;

            //配置できないなら無視。
            if (!model.CanPlace(slotPosition, item))
            {
                return;
            }

            itemPresenter.Move(this, slotPosition);
            model.AddItem(item, slotPosition);
        }
    }
}
