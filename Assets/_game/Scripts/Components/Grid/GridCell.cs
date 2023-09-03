using _game.Scripts.Components.Grid.Objects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _game.Scripts.Components.Grid
{
    public class GridCell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField, ReadOnly] private Vector2Int m_cord;

        private GridManager _gridManager;
        private IGridObject _gridObject;

        [Button]
        public void Initialize(GridManager gridManager, Vector2Int cord, Vector2 localPosition)
        {
            name = GetIndex(cord.x, cord.y);
            m_rectTransform.anchoredPosition = localPosition;

            m_cord = cord;

            _gridManager = gridManager;
        }

        public static string GetIndex(int x, int y)
        {
            return $"x_{x}_y_{y}";
        }
        
        public Vector2 GetSize()
        {
            var rect = m_rectTransform.rect;
            return new Vector2(rect.width, rect.height);
        }

        public string GetIndex()
        {
            return name;
        }

        public Vector2Int GetCord()
        {
            return m_cord;
        }

        public bool IsFilled()
        {
            return _gridObject != null;
        }

        public IGridObject GetGridObject()
        {
            return _gridObject;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_gridObject != null)
            {
                _gridObject.OnInteract();
                _gridManager.CheckAndHandleDeadlock();
            }
        }

        public void SetGridObject(IGridObject newGridObject)
        {
            newGridObject?.UpdateCell(this);
            newGridObject?.SetPosition(transform.position);

            _gridObject = newGridObject;
        }

        public GridCell GetUpCell()
        {
            return _gridManager.GetCell(m_cord.x - 1, m_cord.y);
        }
        
        public void Fall(GridCell downCell)
        {
            if (downCell.GetGridObject() != null) return;
            downCell.SetGridObject(_gridObject);
            SetGridObject(null);

            var upCell = GetUpCell();
            if (upCell != null)
            {
                upCell.Fall(this);
            }
            else
            {
                // Top of the board
                GridObjectSpawner.Instance.SpawnBoxGridObject(_gridManager, m_cord.x, m_cord.y);
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}