using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using yumehiko.ItemSystem;
using yumehiko.ItemSystem.GridInventory;

namespace yumehiko.Platformer
{
    public class InteractableDummy : MonoBehaviour
    {
        [SerializeField] private Interactable interactable;
        [SerializeField] private Sprite itemSprite;
        private GridInventoryUI gridInventoryUI;
        private GridInventory inventoryModel;

        private void Awake()
        {
            GenerateInventoryModel();

            gridInventoryUI = GameObject.FindWithTag("Inventory")?.GetComponent<GridInventoryUI>();
            if (gridInventoryUI == null)
            {
                Debug.LogWarning("Inventoryがない");
                return;
            }

            interactable.OnTouchEnter
                .Subscribe(_ => Debug.Log($"Enter"))
                .AddTo(this);

            interactable.OnTouchExit
                .Subscribe(_ => gridInventoryUI.CloseUI())
                .AddTo(this);

            interactable.OnInteract
                .Subscribe(_ => gridInventoryUI.OpenWithOtherInventory(inventoryModel))
                .AddTo(this);
        }

        private void GenerateInventoryModel()
        {
            //TODO:アイテムマネージャーみたいなのがいる。
            Item item01 = new Item(0, "Dummy");
            GridItem gridItem01 = new GridItem(item01, 5, 25, itemSprite, new Vector2Int(2, 2), Vector2Int.zero);
            GridItem gridItem02 = new GridItem(item01, 10, 25, itemSprite, new Vector2Int(2, 2), new Vector2Int(2, 0));
            GridItem gridItem03 = new GridItem(item01, 22, 25, itemSprite, new Vector2Int(2, 2), new Vector2Int(1, 2));
            Vector2Int slotSize = new Vector2Int(4, 6);
            inventoryModel = new GridInventory(slotSize);
            inventoryModel.AddItem(gridItem01, gridItem01.SlotPosition);
            inventoryModel.AddItem(gridItem02, gridItem02.SlotPosition);
            inventoryModel.AddItem(gridItem03, gridItem03.SlotPosition);
        }
    }
}
