using System;
using System.Collections.Generic;
using UnityEngine;
using static GameArea;

[RequireComponent(typeof(Grid))]
public class GameArea : MonoBehaviour, IProviderAreaPoints
{
    
    [SerializeField] 
    private Bounds _boundsGameArea;

    private List<AreaPoint> _areaPoints = new List<AreaPoint>();
    private Grid _grid;

    [SerializeField]
    private LayerMask _layerUnwalkable;
    
    [SerializeField]
    private float _closePointRadius;
    private float _closePointDiameter;
    
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    
    private float _rowCount;
    private float _cellCount;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _rowCount = Mathf.RoundToInt(_boundsGameArea.size.y / _grid.cellSize.y);
        _cellCount = Mathf.RoundToInt(_boundsGameArea.size.x / _grid.cellSize.x);
        _closePointDiameter = _closePointRadius * 2;
        // get top left bounds' coordinate
        _startPoint = _boundsGameArea.center + (Vector3.up * _boundsGameArea.size.y / 2) -
                      (Vector3.right * _boundsGameArea.size.x / 2);
        // get bottom right bounds' coordinate
        _endPoint = _boundsGameArea.center - (Vector3.up * _boundsGameArea.size.y / 2) +
                    (Vector3.right * _boundsGameArea.size.x / 2);
        BuildPoints();
    }

    private void BuildPoints()
    {
        Vector3 cellSize = _grid.cellSize;
        for (float y = _startPoint.y; y > _endPoint.y; y -= cellSize.y)
        {
            for (float x = _startPoint.x; x < _endPoint.x; x += cellSize.x)
            {
                var worldPosition = _grid.GetCellCenterWorld(_grid.WorldToCell(new Vector3(x, y)));
                var close = Physics2D.OverlapCircle(worldPosition, _closePointDiameter, _layerUnwalkable) != null;
                var gridCell = GetGridXY(worldPosition);
                var areaPoint = new AreaPoint(worldPosition.x, worldPosition.y, gridCell.x, gridCell.y, close);
                _areaPoints.Add(areaPoint);
            }
        }
        
    }

    public List<AreaPoint> GetAreaPoints()
    {
        return _areaPoints;
    }

    public AreaPoint AreaPointFromWorldPoint(Vector2 worldPosition)
    {
        var gridXY = GetGridXY(worldPosition);
        return _areaPoints.GetPointByXY(gridXY.x, gridXY.y, (int)_cellCount);
    }

    public Vector2Int GetGridXY(Vector2 worldPosition)
    {
        float offsetX = _boundsGameArea.center.x;
        float offsetY = _boundsGameArea.center.y;
        float percentX = (worldPosition.x - offsetX + _boundsGameArea.size.x / 2) / _boundsGameArea.size.x;
        float percentY = (worldPosition.y - offsetY + _boundsGameArea.size.y / 2) / _boundsGameArea.size.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        return new Vector2Int()
        {
            x = Mathf.RoundToInt(_cellCount * percentX),
            y = Mathf.RoundToInt(Mathf.Abs(_rowCount * percentY - _rowCount)) 
        };
    }

    public List<AreaPoint> ClosestAreaPoints(AreaPoint point)
    {
        List<AreaPoint> closestPoints = new List<AreaPoint>();
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int xCell = point.GridX + x;
                int yCell = point.GridY + y;
                if(xCell >= 0 && xCell < _cellCount && yCell >= 0 && yCell < _rowCount)
                {
                    AreaPoint closestPoint = _areaPoints.GetPointByXY(xCell, yCell, (int)_cellCount);
                    closestPoints.Add(closestPoint);
                }
            }
        }

        return closestPoints;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_boundsGameArea.center, _boundsGameArea.size);
        if (_areaPoints != null && _areaPoints.Count > 0)
        {
            foreach (var point in _areaPoints)
            {
                Gizmos.color = point.IsClose ? Color.red : Color.green;
                Gizmos.DrawCube(new Vector3(point.X, point.Y), new Vector3(.1f,.1f));
            }
            
            
        }
    }
#endif

    public class AreaPoint
    {
        public bool IsClose { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        
        public int GridX { get; set; }
        
        public int GridY { get; set; }
        
        public int GCost { get; set; }
        
        public int HCost { get; set; }

        public int FCost => GCost + HCost;

        public AreaPoint prev;

        public AreaPoint(float x, float y, int gridX, int gridY, bool isClose = default)
        {
            IsClose = isClose;
            X = x;
            Y = y;
            GridX = gridX;
            GridY = gridY;
            GCost = 0;
            HCost = 0;
        }
    }
    
    public interface IProviderAreaPoints
    {
        List<AreaPoint> GetAreaPoints();

        AreaPoint AreaPointFromWorldPoint(Vector2 worldPosition);

        List<AreaPoint> ClosestAreaPoints(AreaPoint point);
    }

}

public static class AreaPointsListExtension
{
    public static AreaPoint GetPointByXY(this List<AreaPoint> list, int x, int y, int cellCount)
    {
        return list[y * cellCount + x];
    }
}