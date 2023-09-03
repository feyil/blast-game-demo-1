using System.Collections.Generic;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Components.Grid.Objects.View;

namespace _game.Scripts.Components.Grid.Objects
{
    public class BoxGridObject : CommonGridObject
    {
        private readonly BoxGridObjectData _data;
        private readonly BoxGridObjectView _viewSpecific;

        public BoxGridObject(GridManager gridManager, GridCell gridCell, BoxGridObjectView viewPrefab,
            BoxGridObjectData data, int offset) : base(gridManager, gridCell, viewPrefab.gameObject, offset)
        {
            _data = data;
            _viewSpecific = _view.GetComponent<BoxGridObjectView>();
            Refresh();
        }

        private void Refresh()
        {
            _viewSpecific.Render(_data);
        }

        public override IItemData GetData()
        {
            return _data;
        }

        public override void OnInteract()
        {
            var affectedGridObjectSet = new HashSet<BoxGridObject>();
            CheckNeighbouringCells(affectedGridObjectSet);

            affectedGridObjectSet.Add(this);
            if (affectedGridObjectSet.Count < _gridManager.GetBlastableGroupCount()) return;

            foreach (var affectedGridObject in affectedGridObjectSet)
            {
                affectedGridObject.Destroy();

                var affectedGridCell = affectedGridObject._gridCell;
                var upCell = affectedGridCell.GetUpCell();

                // Top of the board
                if (upCell == null)
                {
                    var topCord = affectedGridCell.GetCord();
                    GridObjectSpawner.Instance.SpawnBoxGridObject(_gridManager, topCord.x, topCord.y);
                    continue;
                }

                upCell.Fall(affectedGridCell);
            }
        }

        private void CheckNeighbouringCells(HashSet<BoxGridObject> affectedGridObjectSet)
        {
            var cord = _gridCell.GetCord();
            for (var i = -1; i <= 1; i += 2)
            {
                CheckNeighboringCell(cord.x, cord.y + i, affectedGridObjectSet);
            }

            for (var i = -1; i <= 1; i += 2)
            {
                CheckNeighboringCell(cord.x + i, cord.y, affectedGridObjectSet);
            }
        }

        private void CheckNeighboringCell(int x, int y, HashSet<BoxGridObject> affectedGridObjectSet)
        {
            var gridCell = _gridManager.GetCell(x, y);
            if (gridCell == null) return;

            var gridObject = gridCell.GetGridObject();
            if (gridObject == null) return;

            var boxGridObject = gridObject as BoxGridObject;
            if (boxGridObject == null) return;
            if (boxGridObject.GetNumber() == GetNumber())
            {
                if (affectedGridObjectSet.Contains(boxGridObject)) return;
                affectedGridObjectSet.Add(boxGridObject);
                boxGridObject.CheckNeighbouringCells(affectedGridObjectSet);
            }
        }

        private int GetNumber()
        {
            return _data.Number;
        }
    }
}