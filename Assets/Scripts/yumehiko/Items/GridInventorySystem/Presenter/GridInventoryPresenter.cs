using UniRx;
using UniRx.Triggers;
using UnityEngine;
using yumehiko.Items.GridInventorySystem.Model;
using yumehiko.Items.GridInventorySystem.View;

namespace yumehiko.Items.GridInventorySystem.Presenter
{
    /// <summary>
    /// グリッドインベントリのViewとModelの橋渡し。
    /// </summary>
    public class GridInventoryPresenter : MonoBehaviour
    {
        public GridInventoryCursor Cursor { get; private set; }

        [SerializeField] private GridInventoryView view;
        [SerializeField] private GridItemPresenter itemPresenterPrefab;
        [SerializeField] private Transform itemParent;
        private GridInventory model;
        
        /// <summary>
        /// モデルを読み込み、Viewを構築する。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cursor"></param>
        public void Initialize(GridInventory model, GridInventoryCursor cursor)
        {
            Cursor = cursor;
            this.model = model;
            view.SetSize(model.Size);
            ReloadModel(model);

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
        /// モデルを読み込んで、Viewを再構築する。
        /// </summary>
        /// <param name="model"></param>
        public void ReloadModel(GridInventory model)
        {
            this.model = model;
            view.SetSize(model.Size);

            //すべてのアイテム実体を破棄。
            foreach(Transform item in itemParent)
            {
                Destroy(item.gameObject);
            }

            //すべてのアイテムモデルを実体化。
            foreach(GridItem item in model.Items)
            {
                InstantiateItem(item);
            }
        }

        /// <summary>
        /// このインベントリのモデルから、指定したアイテムを削除する。
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItemAtModel(GridItem item) => model.RemoveItem(item);

        public Vector2Int GetSlotByPoint(Vector2 point) => view.GetSlotByPoint(point);
        public Vector2 GetPointBySlot(Vector2Int slotPosition) => view.GetPointBySlot(slotPosition);
        public bool CanPlace(Vector2Int slotPosition, GridItem item) => model.CanPlace(slotPosition, item);
        public bool CanPlaceWithSize(Vector2Int slotPosition, GridItem item, out Vector2Int size) => model.CanPlaceWithMaskSize(slotPosition, item, out size);
        public void AddItem(GridItem item, Vector2Int slotPosition) => model.AddItem(item, slotPosition);


        private void InstantiateItem(GridItem item)
        {
            if(item == null)
            {
                return; 
            }

            GridItemPresenter instance = Instantiate(itemPresenterPrefab, itemParent);
            instance.Initialize(item, this);
        }
    }
}
