using UniRx;
using UnityEngine;
using UnityEngine.UI;
using yumehiko.Items.GridInventorySystem.Model;
using yumehiko.Items.GridInventorySystem.Presenter;

namespace yumehiko.Items.GridInventorySystem.View
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
            image.sprite = cursor.DraggingItem.Idea.Sprite;
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
