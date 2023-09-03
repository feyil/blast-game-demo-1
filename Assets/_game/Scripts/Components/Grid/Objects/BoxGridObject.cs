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

            if (affectedGridObjectSet.Count < 2) return;

            foreach (var affectedGridObject in affectedGridObjectSet)
            {
                affectedGridObject.Destroy();

                var affectedGridCell = affectedGridObject._gridCell;
                var upCell = affectedGridCell.GetUpCell();

                // Top of the board
                if (upCell == null)
                {
                    var topCord = affectedGridCell.GetCord();
                    GridObjectSpawner.Instance.SpawnApplianceGridObject(_gridManager, topCord.x, topCord.y,
                        BoxGridObjectData.GetRandomData());
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

            var applianceGridObject = gridObject as BoxGridObject;
            if (applianceGridObject == null) return;
            if (applianceGridObject.GetNumber() == GetNumber())
            {
                if (affectedGridObjectSet.Contains(applianceGridObject)) return;
                affectedGridObjectSet.Add(applianceGridObject);
                applianceGridObject.CheckNeighbouringCells(affectedGridObjectSet);
            }
        }

        private int GetNumber()
        {
            return _data.Number;
        }
    }
}