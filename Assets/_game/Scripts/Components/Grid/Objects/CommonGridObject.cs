using _game.Scripts.Components.Grid.Objects.Data;
using DG.Tweening;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Objects
{
    public abstract class CommonGridObject : IGridObject
    {
        protected readonly GridManager _gridManager;
        protected readonly GameObject _view;

        protected GridCell _gridCell;
        private Tween _moveTween;

        protected CommonGridObject(GridManager gridManager, GridCell gridCell, GameObject viewPrefab, int offset)
        {
            _gridManager = gridManager;
            _gridCell = gridCell;

            _view = Object.Instantiate(viewPrefab, gridManager.GetObjectContainer());

            var cord = _gridCell.GetCord();
            var cell = _gridManager.GetCell(0, cord.y);
            _view.transform.position = cell.transform.position + Vector3.up * 100 + (7 - cord.x) * Vector3.up * 150 +
                                       offset * Vector3.up * 150;
            _view.GetComponent<RectTransform>().sizeDelta = gridCell.GetSize();
        }

        public abstract IItemData GetData();

        public abstract void OnInteract();

        public void UpdateCell(GridCell gridCell)
        {
            _gridCell = gridCell;
        }

        public void SetPosition(Vector3 position)
        {
            _view.transform.SetAsLastSibling();
            _moveTween?.Kill();
            _moveTween = _view.transform.DOMove(position, 2000).SetSpeedBased();
        }

        public virtual void Destroy()
        {
            _gridCell.SetGridObject(null);
            Object.Destroy(_view);
        }
    }
}