using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    public class DebugItemPlacer : MonoBehaviour
    {
        [SerializeField] private GridInventoryPresenter inventory;
        [SerializeField] private List<Sprite> dummySprites;

        private void Start()
        {
            Item item0 = new Item(0, "Dummy0");
            Item item1 = new Item(1, "Dummy1");
            Item item2 = new Item(2, "Dummy2");
            Item item3 = new Item(3, "Dummy3");
            Item item4 = new Item(4, "Dummy4");
            GridItem gridItem1x1 = new GridItem(item0, 10, 25, new Vector2Int(1, 1), dummySprites[0]);
            GridItem gridItem1x1_2 = new GridItem(item0, 10, 25, new Vector2Int(1, 1), dummySprites[0]);
            GridItem gridItem1x1_3 = new GridItem(item0, 10, 25, new Vector2Int(1, 1), dummySprites[0]);
            GridItem gridItem2x2 = new GridItem(item4, 1, 2, new Vector2Int(2, 2), dummySprites[4]);
            GridItem gridItem2x2_2 = new GridItem(item4, 2, 2, new Vector2Int(2, 2), dummySprites[4]);

            AddItem(gridItem1x1, new Vector2Int(0, 0));
            AddItem(gridItem1x1_2, new Vector2Int(1, 0));
            AddItem(gridItem1x1_3, new Vector2Int(2, 0));
            AddItem(gridItem2x2, new Vector2Int(0, 2));
            AddItem(gridItem2x2_2, new Vector2Int(2, 2));
        }

        private void AddItem(GridItem item, Vector2Int slotPosition)
        {
            inventory.GenerateItem(item, slotPosition);
        }
    }
}
