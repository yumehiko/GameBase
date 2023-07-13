using System.Collections.Generic;
using UniRx;
using UnityEngine;
using yumehiko.Items.GridInventorySystem.Model;
using yumehiko.Items.GridInventorySystem.View;
using yumehiko.Platformer.Entity.Parts;

namespace yumehiko.Platformer.Entity
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
                UnityEngine.Debug.LogWarning("Inventoryがない");
                return;
            }

            interactable.OnTouchEnter
                .Subscribe(_ => UnityEngine.Debug.Log($"Enter"))
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
