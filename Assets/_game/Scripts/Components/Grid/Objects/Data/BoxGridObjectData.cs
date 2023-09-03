using System;
using _game.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _game.Scripts.Components.Grid.Objects.Data
{
    [Serializable]
    public class BoxGridObjectData : IItemData
    {
        public static int MIN_VALUE = 2;
        public static int MAX_VALUE = 8;

        public int Number;
        public Color Color = new(1, 0.504717f, 0.504717f);

        public static BoxGridObjectData GetRandomData()
        {
            var data = new BoxGridObjectData()
            {
                Number = (int)Mathf.Pow(MIN_VALUE, Random.Range(1, (int)Mathf.Log(MAX_VALUE, 2) + 1)),
            };

            var color = GameManager.Instance.GetColor(data.Number);
            data.Color = color;
            return data;
        }
        
    }
}