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

        public override bool CanMerge(IGridObject gridObject)
        {
            if (gridObject is not BoxGridObject applianceGridObject) return false;

            return applianceGridObject._data.Number == _data.Number;
        }

        public override bool Merge(IGridObject gridObject)
        {
            var applianceGridObject = gridObject as BoxGridObject;
            if (applianceGridObject == null) return false;

            var gridObjectNumber = applianceGridObject._data.Number;
            if (gridObjectNumber == BoxGridObjectData.MAX_VALUE) return false;

            var cord = _gridCell.GetCord();
            gridObject.Destroy();
            Destroy();

            GridObjectSpawner.Instance.SpawnApplianceGridObject(_gridManager, cord.x, cord.y,
                new BoxGridObjectData()
                {
                    Number = _data.Number * 2
                });

            return true;
        }

        public override void OnInteract()
        {
            var affectedCellList = new HashSet<BoxGridObject>();
            CheckNeighbouringCells(affectedCellList);
            if (affectedCellList.Count < 2) return;

            foreach (var affectedCell in affectedCellList)
            {
                affectedCell.Destroy();
                
                var upCell = affectedCell._gridCell.GetUpCell();
                if (upCell == null)
                {
                    var cord = affectedCell._gridCell.GetCord();
                    GridObjectSpawner.Instance.SpawnApplianceGridObject(_gridManager, cord.x, cord.y,
                        BoxGridObjectData.GetRandomData());
                    continue;
                }
                upCell.Fall(affectedCell._gridCell);
            }
        }

        private void CheckNeighbouringCells(HashSet<BoxGridObject> affectedCellList)
        {
            var cord = _gridCell.GetCord();
            for (var i = -1; i <= 1; i += 2)
            {
                var gridCell = _gridManager.GetCell(cord.x, cord.y + i);
                if (gridCell == null) continue;

                var gridObject = gridCell.GetGridObject();
                if (gridObject == null) continue;

                var applianceGridObject = gridObject as BoxGridObject;
                if (applianceGridObject == null) continue;
                if (applianceGridObject.GetNumber() == GetNumber())
                {
                    if(affectedCellList.Contains(applianceGridObject)) continue;
                    affectedCellList.Add(applianceGridObject);
                    applianceGridObject.CheckNeighbouringCells(affectedCellList);
                }
            }
            
            
            for (var i = -1; i <= 1; i += 2)
            {
                var gridCell = _gridManager.GetCell(cord.x + i, cord.y);
                if (gridCell == null) continue;

                var gridObject = gridCell.GetGridObject();
                if (gridObject == null) continue;

                var applianceGridObject = gridObject as BoxGridObject;
                if (applianceGridObject == null) continue;
                if (applianceGridObject.GetNumber() == GetNumber())
                {
                    if(affectedCellList.Contains(applianceGridObject)) continue;
                    affectedCellList.Add(applianceGridObject);
                    applianceGridObject.CheckNeighbouringCells(affectedCellList);
                }
            }
        }

        private int GetNumber()
        {
            return _data.Number;
        }
    }
}