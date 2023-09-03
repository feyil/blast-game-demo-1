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
        private IGridObject _gridObjectImplementation;

        protected CommonGridObject(GridManager gridManager, GridCell gridCell, GameObject viewPrefab, int offset)
        {
            _gridManager = gridManager;
            _gridCell = gridCell;

            _view = Object.Instantiate(viewPrefab, gridManager.GetObjectContainer());
            _view.transform.position = GetSpawnPosition(_gridCell, offset);

            _view.GetComponent<RectTransform>().sizeDelta = gridCell.GetSize();
        }

        
        public abstract IItemData GetData();

        public abstract void OnInteract();
        public abstract bool IsBlastable();

        public void UpdateCell(GridCell gridCell)
        {
            _gridCell = gridCell;
        }

        private Vector3 GetSpawnPosition(GridCell gridCell, int offset)
        {
            var cord = gridCell.GetCord();
            var topMostCell = _gridManager.GetCell(0, cord.y);
            var topMostCellPosition = topMostCell.GetPosition();
            
            //TODO can be moved to GridConfig
            var defaultOffset = Vector3.up * 100;
            var cordOffset = (_gridManager.GetDimensions().x - cord.x) * Vector3.up * 100;
            var externalOffset = offset * Vector3.up * 100;

            var startPosition = topMostCellPosition + defaultOffset + cordOffset + externalOffset;
            return startPosition;
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