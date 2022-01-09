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

            _ = view.OnDropAsObservable()
                .Subscribe(eventData => cursor.DropToInventory(this))
                .AddTo(this);

            _ = view.OnPointerEnterAsObservable()
                .Subscribe(eventData => cursor.EnterInventory(this))
                .AddTo(this);

            _ = view.OnPointerExitAsObservable()
                .Subscribe(_ => cursor.ExitInventory())
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

        public Vector2Int GetSlotByPoint(Vector2 point) => view.GetSlotByPoint(point);
        public Vector2 GetPointBySlot(Vector2Int slotPosition) => view.GetPointBySlot(slotPosition);
        public bool CanPlace(Vector2Int slotPosition, GridItem item) => model.CanPlace(slotPosition, item);
        public bool CanPlaceWithSize(Vector2Int slotPosition, GridItem item, out Vector2Int size) => model.CanPlaceWithSize(slotPosition, item, out size);
        public void AddItem(GridItem item, Vector2Int slotPosition) => model.AddItem(item, slotPosition);
    }
}
