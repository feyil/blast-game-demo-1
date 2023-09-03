using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Data
{
    [Serializable]
    public class GridConfig
    {
        [InfoBox("M")] public int NumberOfRows;
        [InfoBox("N")] public int NumberOfColumns;
        [InfoBox("K")] public int NumberOfColors;
        [InfoBox("Spacing between cells.")] public Vector2Int Spacing;


        [PropertySpace(30)] public int ConditionALimit;
        public int ConditionBLimit;
        public int ConditionCLimit;

        [PropertySpace(30)] public int BlastableGroupCount;

        [PropertySpace(30)] public BoxConfig[] BoxConfigArray;
    }
}