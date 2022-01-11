using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    public class GridInventoryCursor : UIBehaviour
    {
        public GridItemPresenter DraggingItem { get; private set; }
        public IReadOnlyReactiveProperty<bool> IsDragging => isDragging;
        public IReadOnlyReactiveProperty<Vector2> Position => position;
        public IReadOnlyReactiveProperty<GridInventoryPresenter> TouchingInventory => touchingInventory;
        public IReadOnlyReactiveProperty<MergeCheckResult> CurrentMergeCheckResult => currentMergeCheckResult;

        private ReactiveProperty<GridInventoryPresenter> touchingInventory = new ReactiveProperty<GridInventoryPresenter>();
        private Vector2ReactiveProperty position = new Vector2ReactiveProperty();
        private BoolReactiveProperty isDragging = new BoolReactiveProperty();
        private ReactiveProperty<MergeCheckResult> currentMergeCheckResult = new ReactiveProperty<MergeCheckResult>();
        private System.IDisposable cursorUpdateObserver;

        protected override void OnDisable()
        {
            base.OnDisable();
            EndDrag();
        }


        public void StartDrag(GridItemPresenter gridItem, PointerEventData eventData)
        {
            DraggingItem = gridItem;
            position.Value = eventData.position;
            cursorUpdateObserver = Observable.EveryUpdate()
                .Subscribe(_ => position.Value = eventData.position)
                .AddTo(this);
            isDragging.Value = true;
            Cursor.visible = false;
        }

        public void EndDrag()
        {
            DraggingItem = null;
            cursorUpdateObserver?.Dispose();
            isDragging.Value = false;
            Cursor.visible = true;
        }

        /// <summary>
        /// ドラッグアンドドロップで、ドラッグ中のアイテムをアイテムにドロップする。
        /// </summary>
        public void DropToItem(GridItemPresenter dropTarget)
        {
            //ドラッグ元とドロップ先が同じアイテムなら、無視。
            if (DraggingItem == dropTarget)
            {
                return;
            }

            //異なるアイテムはスタック不能なので、無視。
            if (DraggingItem.Model.Idea != dropTarget.Model.Idea)
            {
                return;
            }

            //スタックをマージする。
            int overflow = dropTarget.Model.TryMergeStack(DraggingItem.Model);
            DraggingItem.Model.SetStackAmount(overflow);
            ExitItem(dropTarget);
            EndDrag();
        }

        /// <summary>
        /// ドラッグアンドロップで、アイテムをインベントリにドロップする。
        /// </summary>
        /// <param name="dropTarget"></param>
        public void DropToInventory(GridInventoryPresenter dropTarget)
        {
            Vector2Int dropSlotPosition = dropTarget.GetSlotByPoint(position.Value);
            bool canPlace = dropTarget.CanPlace(dropSlotPosition, DraggingItem.Model);

            if (!canPlace)
            {
                return;
            }

            DraggingItem.MoveTo(dropTarget, dropSlotPosition);
            dropTarget.AddItem(DraggingItem.Model, dropSlotPosition);
        }

        /// <summary>
        /// インベントリの中にカーソルが入った時の処理。
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="point"></param>
        public void EnterInventory(GridInventoryPresenter inventory)
        {
            touchingInventory.Value = inventory;
        }

        /// <summary>
        /// インベントリからカーソルが出たときの処理。
        /// </summary>
        public void ExitInventory()
        {
            touchingInventory.Value = null;
        }

        public void EnterItem(GridItemPresenter item)
        {
            if(!IsDragging.Value)
            {
                return;
            }
            MergeCheckResult result = CheckCanMergeItems(item);
            currentMergeCheckResult.Value = result;
            item.ChangeColorByMergeResult(result);
        }

        public void ExitItem(GridItemPresenter item)
        {
            currentMergeCheckResult.Value = MergeCheckResult.None;
            item.ResetColorToWhite();
        }


        /// <summary>
        /// 指定したアイテムに、現在ドラッグ中のアイテムがマージ可能か確認し、その結果を返す。
        /// </summary>
        private MergeCheckResult CheckCanMergeItems(GridItemPresenter mergeTarget)
        {
            //ターゲットがnullならnone。
            if (mergeTarget == null)
            {
                return MergeCheckResult.None;
            }

            //まったく同じもの同士（ドラッグ元＝確認先）の場合
            if (mergeTarget == DraggingItem)
            {
                return MergeCheckResult.SameItem;
            }

            //同種のアイテムではない場合
            if (mergeTarget.Model.Idea != DraggingItem.Model.Idea)
            {
                return MergeCheckResult.Forbid;
            }

            //すでに最大量の場合
            if (mergeTarget.Model.Idea.MaxStack == mergeTarget.Model.Stack.Value)
            {
                return MergeCheckResult.Forbid;
            }

            //マージ可能な場合。
            return MergeCheckResult.Permit;
        }
    }
}
