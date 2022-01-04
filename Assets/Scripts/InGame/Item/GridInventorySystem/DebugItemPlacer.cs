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
            Item item = new Item(0, "Dummy");
            GridItem gridItem1x1 = new GridItem(item, 10, new Vector2Int(1, 1), dummySprites[0]);
            GridItem gridOran1x1 = new GridItem(item, 10, new Vector2Int(1, 1), dummySprites[1]);
            GridItem gridItem3x1 = new GridItem(item, 10, new Vector2Int(3, 1), dummySprites[2]);
            GridItem gridItem1x2 = new GridItem(item, 10, new Vector2Int(1, 2), dummySprites[3]);
            GridItem gridItem2x2 = new GridItem(item, 10, new Vector2Int(2, 2), dummySprites[4]);

            AddItem(gridItem1x1, new Vector2Int(0, 0));
            AddItem(gridItem3x1, new Vector2Int(1, 0));
            AddItem(gridItem1x1, new Vector2Int(0, 1));
            AddItem(gridOran1x1, new Vector2Int(1, 1));
            AddItem(gridItem2x2, new Vector2Int(0, 2));
            AddItem(gridItem1x2, new Vector2Int(3, 1));
            AddItem(gridItem1x1, new Vector2Int(3, 3));
        }

        private void AddItem(GridItem item, Vector2Int slotPosition)
        {
            inventory.GenerateItem(item, slotPosition);
        }
    }
}
