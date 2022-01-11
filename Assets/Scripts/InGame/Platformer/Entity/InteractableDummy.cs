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
        [SerializeField] private List<GridItemIdea> itemIdeas;
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
            Vector2Int slotSize = new Vector2Int(4, 6);
            inventoryModel = new GridInventory(slotSize);
            inventoryModel.AddItemBasedObject(itemIdeas[0], 2, new Vector2Int(0, 0));
            inventoryModel.AddItemBasedObject(itemIdeas[0], 2, new Vector2Int(2, 0));
            inventoryModel.AddItemBasedObject(itemIdeas[0], 2, new Vector2Int(1, 2));
        }
    }
}
