using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    /// <summary>
    /// グリッドインベントリを管理するUI。
    /// </summary>
    public class GridInventoryUI : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GridInventoryCursor cursor;
        [SerializeField] private GridInventoryPresenter playerInventory;
        [SerializeField] private GridInventoryPresenter inventoryPrefab;
        [SerializeField] private Transform otherInventoryParent;

        private bool isOpen => canvas.gameObject.activeSelf;
        private readonly Vector2 inventoryOffsetToFrame = new Vector2(47.0f, -47.0f);

        private void Awake()
        {
            var playerInventorySize = new Vector2Int(7, 13);
            var model = new GridInventory(playerInventorySize);
            playerInventory.Initialize(model, cursor);
        }

        public void ToggleOpenUI()
        {
            if(!isOpen)
            {
                OpenUI();
            }
            else
            {
                CloseUI();
            }
        }

        public void OpenUI()
        {
            canvas.gameObject.SetActive(true);
        }

        public void CloseUI()
        {
            DiscardOtherInventory();
            canvas.gameObject.SetActive(false);
        }

        public void OpenWithOtherInventory(GridInventory model)
        {
            //すでにインベントリを開いている場合は無視。
            if (isOpen)
            {
                return;
            }

            InstanciateOtherInventory(model);
            OpenUI();
        }

        /// <summary>
        /// プレイヤーでない側のインベントリを生成する。
        /// </summary>
        private void InstanciateOtherInventory(GridInventory model)
        {
            DiscardOtherInventory();
            var instance = Instantiate(inventoryPrefab, otherInventoryParent);
            instance.GetComponent<RectTransform>().anchoredPosition = inventoryOffsetToFrame;
            instance.Initialize(model, cursor);
        }

        /// <summary>
        /// プレイヤーでない側のインベントリを消す。
        /// </summary>
        private void DiscardOtherInventory()
        {
            foreach (Transform child in otherInventoryParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
