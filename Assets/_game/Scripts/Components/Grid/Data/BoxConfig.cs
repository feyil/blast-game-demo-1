using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Data
{
    [Serializable]
    public class BoxConfig
    {
        public Color DefaultColor;
        [PreviewField] public Sprite DefaultIcon;
        [PreviewField] public Sprite AIcon;
        [PreviewField] public Sprite BIcon;
        [PreviewField] public Sprite CIcon;
    }
}