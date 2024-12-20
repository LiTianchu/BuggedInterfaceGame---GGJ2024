using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mineplanter : MonoBehaviour
{
    [Header("Grids")]
    [SerializeField] private GridLayoutGroup puzzleGrid;
    [SerializeField] private GridLayoutGroup referenceGrid;

    [Header("Prefabs")]
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private MinePuzzleGrid minePuzzleGridPrefab;

    [Header("UI")]
    [SerializeField] private GameObject levelClearPrompt;
    [SerializeField] private GameObject levelClearPrompt1;


    private List<List<bool>> mines;
    private List<List<int>> currentMineNums;
    private List<List<int>> neighborMineCounts;
    private static readonly float PUZZLE_GRID_SPACING = 5f;
    private static readonly float REFERENCE_GRID_SPACING = 5f;

    private int level = 1;

    private static List<List<bool>> MINES = new()
    {
        new() {true,false,true},
        new() {false,true,false},
        new() {true,false,true},
    };

    // 4*4
    // private static readonly List<List<bool>> MINES = new()
    // {
    //     new() {true,false,true,false},
    //     new() {false,true,false,true},
    //     new() {true,false,true,false},
    //     new() {false,true,false,true},
    // };

    public List<List<int>> CurrentMineNums { get => currentMineNums; set => currentMineNums = value; }

    // Start is called before the first frame update
    public void Start()
    {
        mines = new();
        currentMineNums = new();
        neighborMineCounts = new();
        SetupLevel();
    }

    public void SetupLevel()
    {
        int rows = MINES.Count;
        int cols = MINES[0].Count;

        // Calculate the cell size and spacing for the grids
        float puzzleWidth = puzzleGrid.GetComponent<RectTransform>().rect.width;
        float puzzleHeight = puzzleGrid.GetComponent<RectTransform>().rect.height;
        float refWidth = referenceGrid.GetComponent<RectTransform>().rect.width;
        float refHeight = referenceGrid.GetComponent<RectTransform>().rect.height;

        float puzzleCellHeight = (puzzleHeight - PUZZLE_GRID_SPACING * (rows - 1)) / rows;
        float puzzleCellWidth = (puzzleWidth - PUZZLE_GRID_SPACING * (cols - 1)) / cols;
        puzzleGrid.cellSize = new Vector2(puzzleCellWidth, puzzleCellHeight);
        puzzleGrid.spacing = new Vector2(PUZZLE_GRID_SPACING, PUZZLE_GRID_SPACING);

        float refCellHeight = (refHeight - REFERENCE_GRID_SPACING * (rows - 1)) / rows;
        float refCellWidth = (refWidth - REFERENCE_GRID_SPACING * (cols - 1)) / cols;
        referenceGrid.cellSize = new Vector2(refCellWidth, refCellHeight);
        referenceGrid.spacing = new Vector2(REFERENCE_GRID_SPACING, REFERENCE_GRID_SPACING);

        // Initialize the grids
        for (int i = 0; i < rows; i++)
        {
            mines.Add(new List<bool>());
            currentMineNums.Add(new List<int>());
            neighborMineCounts.Add(new List<int>());
            for (int j = 0; j < cols; j++)
            {
                mines[i].Add(MINES[i][j]);
                currentMineNums[i].Add(0);
                int neighborMines = GetNeighborMineCount(i, j);
                neighborMineCounts[i].Add(neighborMines);

                // spawn the mine references
                GameObject mine = Instantiate(minePrefab, referenceGrid.transform);
                if (!MINES[i][j])
                {
                    // set as transparent
                    mine.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }

                // spawn the mine puzzle grids
                MinePuzzleGrid minePuzzleGrid = Instantiate(minePuzzleGridPrefab, puzzleGrid.transform);
                minePuzzleGrid.Mineplanter = this;
                minePuzzleGrid.X = i;
                minePuzzleGrid.Y = j;
            }
        }
    }

    public bool CheckLevelClear()
    {
        int rows = MINES.Count;
        int cols = MINES[0].Count;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (MINES[i][j] && currentMineNums[i][j] != 0)
                {
                    return false;
                }
                else if (MINES[i][j])
                {
                    continue;
                }

                if (currentMineNums[i][j] != neighborMineCounts[i][j])
                {
                    return false;
                }
            }
        }
        if (level == 1)
        {
            levelClearPrompt.SetActive(true);
        }
        else
        {
            levelClearPrompt1.SetActive(true);
        }
        
        return true;
    }

    public int GetNeighborMineCount(int x, int y)
    {
        int count = 0;
        int rows = MINES.Count;
        int cols = MINES[0].Count;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == x && j == y) { continue; } // Skip the current cell

                int xDist = Mathf.Abs(i - x);
                int yDist = Mathf.Abs(j - y);
                if (xDist > 1 || yDist > 1) { continue; } // Skip cells that are not adjacent

                if (MINES[i][j])
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void NextPuzzle()
    {
        MINES = new List<List<bool>>()
        {
            new() {true,false,true},
            new() {false,true,false},
            new() {false,true,false},
            new() {true,false,true},
        };

        foreach (Transform child in puzzleGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in referenceGrid.transform)
        {
            Destroy(child.gameObject);
        }
        level += 1;
        mines.Clear();
        currentMineNums.Clear();
        neighborMineCounts.Clear();
        levelClearPrompt.SetActive(false);
        SetupLevel();
    }

    public void NextLevel() {
        SceneManager.LoadScene("MineplanterLevel2");
    }
}
