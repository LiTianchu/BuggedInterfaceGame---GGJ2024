using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridSystem : MonoBehaviour, IDraggableArea
{
    [TitleGroup("Grid Area Boundaries")]
    [SerializeField] private Vector2 gridLowerLeft = new(-10f,-10f);
    [SerializeField] private Vector2 gridUpperRight = new(10f, 10f);
    [SerializeField] private bool generateBoundColliders = true;

    [TitleGroup("Grid Cell Settings")]
    [SerializeField] private Vector2 cellSize = new(1f, 1f);
    [Required("Cell Prefab is required")]
    [SerializeField] private Grid cellPrefab;
    [SerializeField] private float cellSpacing = 0.05f;

    [TitleGroup("Render Settings")]
    [SerializeField] private bool renderGridWireframe = true;
    [ShowIf("renderGridWireframe", true)]
    [ColorPalette]
    [SerializeField] private Color gridWireframeColor = Color.white;
    [ShowIf("renderGridWireframe", true)]
    [SerializeField] private Material gridWireframeMaterial;
    [SerializeField] private bool renderGridBoundaries = true;
    [ShowIf("renderGridBoundaries", true)]
    [ColorPalette]
    [SerializeField] private Color gridBoundariesColor = Color.cyan;
    [ShowIf("renderGridBoundaries", true)]
    [SerializeField] private Material gridBoundariesMaterial;
    // Start is called before the first frame update

    public int HSize{get; set;}
    public int VSize{get; set;}
    public List<Grid> Grids { get; set; } = new List<Grid>();

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid(){
        for(float i=gridLowerLeft.x; i<gridUpperRight.x; i+=cellSize.x){ // left to right
            VSize = 0;
            for(float j=gridLowerLeft.y; j<gridUpperRight.y; j+=cellSize.y){ // down to top
                Vector2 spawnPos = new Vector2(i + cellSize.x/2, j + cellSize.y/2);
                Grid grid = Instantiate(cellPrefab,spawnPos,Quaternion.identity);
                grid.transform.SetParent(this.transform);
                grid.transform.localScale = new Vector3(cellSize.x - cellSpacing/2,cellSize.y-cellSpacing/2,1);
                grid.GridSystem = this;
                grid.X = HSize;
                grid.Y = VSize;
                grid.name = "Grid (" + HSize + "," + VSize + ")";
                Grids.Add(grid);
                VSize++;
            }
            HSize++;
        }

        if(generateBoundColliders){
            EdgeCollider2D boundCollider = gameObject.AddComponent<EdgeCollider2D>();
            Vector2[] boundPoints = new Vector2[5];
            boundPoints[0] = new Vector2(gridLowerLeft.x, gridLowerLeft.y); // lower left bound
            boundPoints[1] = new Vector2(gridUpperRight.x, gridLowerLeft.y); // lower right bound
            boundPoints[2] = new Vector2(gridUpperRight.x, gridUpperRight.y); // upper right bound
            boundPoints[3] = new Vector2(gridLowerLeft.x, gridUpperRight.y); // upper left bound
            boundPoints[4] = new Vector2(gridLowerLeft.x, gridLowerLeft.y); // close the loop
            boundCollider.points = boundPoints;
        }

        if(renderGridWireframe){
            for(float i=gridLowerLeft.x + cellSize.x; i<gridUpperRight.x; i+=cellSize.x){ // vertical lines
                LineRenderer line = new GameObject().AddComponent<LineRenderer>();
                line.transform.SetParent(this.transform);
                line.positionCount = 2;
                line.SetPositions(new Vector3[]{
                    new(i,gridLowerLeft.y,0),
                    new(i,gridUpperRight.y,0)
                });
                line.startWidth = cellSpacing;
                line.endWidth = cellSpacing;
                line.startColor = gridWireframeColor;
                line.material = gridWireframeMaterial;
                line.gameObject.name = "VerticalGridLine";
            }

            for(float i=gridLowerLeft.y + cellSize.y; i<gridUpperRight.y; i+=cellSize.y){ // horizontal lines
                LineRenderer line = new GameObject().AddComponent<LineRenderer>();
                line.transform.SetParent(this.transform);
                line.positionCount = 2;
                line.SetPositions(new Vector3[]{
                    new(gridLowerLeft.x,i,0),
                    new(gridUpperRight.x,i,0)
                });
                line.startWidth = cellSpacing;
                line.endWidth = cellSpacing;
                line.startColor = gridWireframeColor;
                line.material = gridWireframeMaterial;
                line.gameObject.name = "HorizontalGridLine";
            }
        }

        if(renderGridBoundaries){
            LineRenderer boundLine = new GameObject().AddComponent<LineRenderer>();
            boundLine.transform.SetParent(this.transform);
            boundLine.positionCount = 5;
            boundLine.SetPositions(new Vector3[]{
                new(gridLowerLeft.x,gridLowerLeft.y,0),
                new(gridUpperRight.x,gridLowerLeft.y,0),
                new(gridUpperRight.x,gridUpperRight.y,0),
                new(gridLowerLeft.x,gridUpperRight.y,0),
                new(gridLowerLeft.x,gridLowerLeft.y,0)
            });
            boundLine.startWidth = cellSpacing;
            boundLine.endWidth = cellSpacing;
            boundLine.startColor = gridBoundariesColor;
            boundLine.material = gridBoundariesMaterial;
            boundLine.gameObject.name = "GridBoundaryLine";
        }

    }

    public Vector2[] GetBoundPoints()
    {
        Vector2[] boundPoints = {
            gridLowerLeft,
            gridUpperRight
        };

        return boundPoints;
        
    }

    public Grid GetGrid(int x, int y){
        //Debug.Log("Grid Clicked 2: " + x + "," + y);
        if(x < 0 || y < 0 || x >= HSize || y >= VSize){
            return null;
        }
        return Grids[x*VSize + y];
    }

    public void HighlightDropArea(DropArea dropArea)
    {
        foreach(Grid grid in Grids){
            grid.NormalColor();
        }
        dropArea.Highlight();
    }

    public Vector2 GridCoordToPosition(int x, int y){
        if(GetGrid(x,y) == null){
            throw new System.Exception("Grid does not exist");
        }
        return GetGrid(x,y).transform.position;
    }
}
