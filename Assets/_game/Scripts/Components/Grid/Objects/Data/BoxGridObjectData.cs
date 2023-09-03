using System;
using System.Collections.Generic;
using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _game.Scripts.Components.Grid.Objects.Data
{
    [Serializable]
    public class BoxGridObjectData : IItemData
    {
        public int Number;
        public Sprite Icon;
        public Color Color = new(1, 0.504717f, 0.504717f);

        public static BoxGridObjectData GetRandomData(GridConfig gridConfig)
        {
            var min = 0;
            var max = gridConfig.NumberOfColors;
            var boxConfigArray = gridConfig.BoxConfigArray;

            var index = Random.Range(min, max);
            var boxConfig = boxConfigArray[Mathf.Clamp(index, min, max)];

            var data = new BoxGridObjectData()
            {
                Number = index,
                Color = boxConfig.DefaultColor
            };

            return data;
        }
    }
}