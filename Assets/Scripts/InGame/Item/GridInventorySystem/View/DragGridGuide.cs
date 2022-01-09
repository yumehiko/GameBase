using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    /// <summary>
    /// 現在ドラッグ中のアイテムを、配置する場所のガイド。
    /// </summary>
    public class DragGridGuide : UIBehaviour
    {
        [SerializeField] private GridInventoryCursor cursor;
        [SerializeField] private Image image;
        //TODO: 色を変更可能にしておく。
        private Color32 permitColor = new Color32(0, 255, 0, 63);
        private Color32 forbidColor = new Color32(255, 0, 0, 63);
        private Vector2Int prevSlotPosition;
        private GridInventoryPresenter inventory;
        private GridItem gridItem;
        private System.IDisposable cursorObserver;

        protected override void Awake()
        {
            base.Awake();

            _ = cursor.IsDragging
                .Where(isTrue => isTrue)
                .Subscribe(_ => Enable(cursor.TouchingInventory.Value))
                .AddTo(this);

            _ = cursor.IsDragging
                .Where(isTrue => !isTrue)
                .Skip(1)
                .Subscribe(_ => Disable())
                .AddTo(this);

            _ = cursor.TouchingInventory
                .Where(_ => cursor.IsDragging.Value)
                .Where(inventory => inventory != null)
                .Subscribe(inventory => Enable(inventory))
                .AddTo(this);

            _ = cursor.TouchingInventory
                .Where(_ => cursor.IsDragging.Value)
                .Where(inventory => inventory == null)
                .Subscribe(_ => Disable())
                .AddTo(this);

            _ = cursor.CurrentMergeCheckResult
                .Where(_ => cursor.IsDragging.Value)
                .Where(result => result != MergeCheckResult.Permit)
                .Where(_ => cursor.TouchingInventory != null)
                .Subscribe(_ => Enable(cursor.TouchingInventory.Value))
                .AddTo(this);

            _ = cursor.CurrentMergeCheckResult
                .Where(result => result == MergeCheckResult.Permit)
                .Subscribe(_ => Disable())
                .AddTo(this);
        }


        private void Enable(GridInventoryPresenter inventory)
        {
            //Enableにしたタイミングで、inventoryがnullなら無視（inventoryから抜けながらドラッグ開始したりした場合）。
            if (inventory == null)
            {
                return;
            }

            image.enabled = true;
            gridItem = cursor.DraggingItem.Model;
            Vector2Int slotPosition = inventory.GetSlotByPoint(cursor.Position.Value);
            Vector2Int size;
            //bool canPlace = inventory.CanPlace(prevSlotPosition, gridItem);
            bool canPlace = inventory.CanPlaceWithSize(prevSlotPosition, gridItem, out size);
            this.inventory = inventory;
            ResetViewStat(slotPosition, size, canPlace);

            cursorObserver = cursor.Position
                .Subscribe(position => Move(position))
                .AddTo(this);
        }

        private void Disable()
        {
            image.enabled = false;
            cursorObserver?.Dispose();
        }

        private void Move(Vector2 cursorPosition)
        {
            Vector2Int slotPosition = inventory.GetSlotByPoint(cursorPosition);
            //スロット座標が変わらないなら、無視。
            if (slotPosition == prevSlotPosition)
            {
                return;
            }
            Vector2Int size;
            bool canPlace = inventory.CanPlaceWithSize(slotPosition, gridItem, out size);
            ResetViewStat(slotPosition, size, canPlace);
        }

        private void ResetViewStat(Vector2Int slotPosition, Vector2Int size, bool canPlace)
        {
            prevSlotPosition = slotPosition;
            image.color = canPlace ? permitColor : forbidColor;
            image.rectTransform.sizeDelta = (Vector2)size * GridInventory.SlotSize;
            transform.position = inventory.GetPointBySlot(slotPosition);
        }

    }
}
