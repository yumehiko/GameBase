﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    /// <summary>
    /// ドラッグ中に表示される、アイテムのゴースト。
    /// </summary>
    public class DragItemGhostGuide : MonoBehaviour
    {
        [SerializeField] private GridInventoryCursor cursor;
        [SerializeField] private Image image;

        private System.IDisposable cursorObserver;
        private readonly Vector2 halfSlotOffset = new Vector2(-0.5f, 0.5f) * GridInventory.SlotSize;

        private void Awake()
        {
            _ = cursor.IsDragging
                .Where(isTrue => isTrue)
                .Subscribe(_ => EnableGuide())
                .AddTo(this);

            _ = cursor.IsDragging
                .Where(isTrue => !isTrue)
                .Skip(1)
                .Subscribe(_ => DisableGuide())
                .AddTo(this);
        }

        private void EnableGuide()
        {
            image.enabled = true;
            image.sprite = cursor.DraggingItem.Model.Sprite;
            image.SetNativeSize();
            transform.position = cursor.Position.Value + halfSlotOffset;

            cursorObserver = cursor.Position
                .Subscribe(position => UpdateGuidePosition(position))
                .AddTo(this);
        }

        private void DisableGuide()
        {
            cursorObserver.Dispose();
            image.enabled = false;
        }

        private void UpdateGuidePosition(Vector2 position)
        {
            transform.position = position + halfSlotOffset;
        }
    }
}
