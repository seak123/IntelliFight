using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapGridState
{
    InAttackRange = 1, //可到达的
    Reachable = 1 << 1, //在攻击范围内
    InSkillRange = 1 << 2, //在技能范围内
    SelectedPath = 1 << 3, //被选中路径
    SelectedEnemy = 1 << 4, //被选中敌对单位
    SelectedFriend = 1 << 5, //被选中友方单位
}

public class MapGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public MapCell cellPrefab;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    MapCell[] cells;
    int[] cellStates;  //地图单位状态(以二进制位标识)
    private bool mapDirty = false; //用以刷新地图

    public Text cellLabelPrefab;
    Canvas gridCanvas;
    MapMesh mapMesh;
    GameObject mapCellRoot;
    GameObject mapUnitRoot;

    private readonly Color[] gridColors = new Color[6]
    {
        new Color(0.73f,0.55f,0.56f),
        new Color(0.77f,0.9f,0.95f),
        new Color(0.81f,0.83f,0.58f),
        new Color(0.94f,0.89f,0.38f),
        new Color(0.81f,0.83f,0.58f),
        new Color(0.58f,0.83f,0.72f)
    };

    public GameObject MapUnitRoot
    {
        get
        {
            return mapUnitRoot;
        }
    }

    private void Awake()
    {
        mapCellRoot = transform.Find("MapCellRoot").gameObject;

        mapUnitRoot = transform.Find("MapUnitRoot").gameObject;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (mapDirty)
        {
            RefreshMapCells();
            mapDirty = false;
        }
    }

    public Vector3 World2MapPostion(Vector3 pos)
    {
        return transform.InverseTransformPoint(pos);

    }

    public void MarkDirty()
    {
        mapDirty = true;
    }

    /*
     *@param add true为添加状态 false为剔除状态 
     */
    public void ChangeCellState(MapCoordinates pos, MapGridState state, bool add = true)
    {
        if (!IsMapCoordValid(pos)) return;
        int index = pos.X + pos.Z * width;
        if (add)
        {
            cellStates[index] |= (int)state;
        }
        else
        {
            cellStates[index] &= ~(int)state;
        }
        MarkDirty();
    }


    public void CreateMapCells()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        mapMesh = GetComponentInChildren<MapMesh>();

        cells = new MapCell[height * width];
        cellStates = new int[height * width];

        for (int z = 0, i = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                cellStates[i] = 0;
                CreateCell(x, z, i++);
            }
        }
        mapMesh.Triangulate(cells);
    }

    private void ResetCells()
    {
        for (int z = 0, i = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                MapCell cell = cells[i++];
                cell.color = defaultColor;
            }
        }
        mapMesh.Triangulate(cells);
    }

    public void RefreshMapCells()
    {
        for (int z = 0, i = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                MapCell cell = cells[i];
                cell.color = defaultColor;
                if (cellStates[i] > 0) {
                    for (int p = 5; p >= 0; --p)
                    {
                        if ((cellStates[i] & (1 << p)) > 0)
                        {
                            cell.color = gridColors[p];
                            break;
                        }
                    }
                }
                ++i;
            }
        }
        mapMesh.Triangulate(cells);
    }


    //public void ColorCell(MapCoordinates coordinates, Color color)
    //{

    //    int index = coordinates.X + coordinates.Z * width;
    //    MapCell cell = cells[index];
    //    cell.color = color;
    //    mapMesh.Triangulate(cells);
    //}

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + 0.5f) * MapConstant.sideLength;
        position.y = 0;
        position.z = (z + 0.5f) * MapConstant.sideLength;

        MapCell cell = cells[i] = Instantiate<MapCell>(cellPrefab);
        cell.transform.SetParent(mapCellRoot.transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = MapCoordinates.FromPosition(position);
        cell.color = defaultColor;

        Text label = Instantiate<Text>(cellLabelPrefab, gridCanvas.transform, false);
        label.rectTransform.SetParent(gridCanvas.transform);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    public bool IsMapCoordValid(MapCoordinates coord)
    {
        return coord.X >= 0 && coord.Z >= 0 && coord.X < width && coord.Z < height;
    }
}
