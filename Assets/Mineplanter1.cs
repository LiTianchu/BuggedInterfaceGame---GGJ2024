using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mineplanter1 : MonoBehaviour
{
    [Header("Grids")]
    [SerializeField] private GridLayoutGroup puzzleGrid;

    [Header("Prefabs")]
    [SerializeField] private MinePuzzleGrid1 minePuzzleGridPrefab;

    [Header("UI")]
    [SerializeField] private GameObject levelClearPrompt;


    private List<List<bool>> mines;
    private List<List<int>> currentMineNums;
    private List<List<int>> neighborMineCounts;
    private static readonly float PUZZLE_GRID_SPACING = 5f;
    private static readonly float REFERENCE_GRID_SPACING = 5f;


    private static List<List<int>> MINES = new()
    {
        new() {0,0,0,0},
        new() {0,1,2,0},
        new() {0,0,0,0},
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

        float puzzleCellHeight = (puzzleHeight - PUZZLE_GRID_SPACING * (rows - 1)) / rows;
        float puzzleCellWidth = (puzzleWidth - PUZZLE_GRID_SPACING * (cols - 1)) / cols;
        puzzleGrid.cellSize = new Vector2(puzzleCellWidth, puzzleCellHeight);
        puzzleGrid.spacing = new Vector2(PUZZLE_GRID_SPACING, PUZZLE_GRID_SPACING);   

        // Initialize the grids
        for (int i = 0; i < rows; i++)
        {
            mines.Add(new List<bool>());
            currentMineNums.Add(new List<int>());
            neighborMineCounts.Add(new List<int>());
            for (int j = 0; j < cols; j++)
            {
                currentMineNums[i].Add(0);
                // spawn the mine puzzle grids
                MinePuzzleGrid1 minePuzzleGrid = Instantiate(minePuzzleGridPrefab, puzzleGrid.transform);
                minePuzzleGrid.Mineplanter = this;
                minePuzzleGrid.X = i;
                minePuzzleGrid.Y = j;
            }
        }   
    }

    public void NextLevel() {
        SceneManager.LoadScene("MineplanterLevel4");
    }

    // Method to check if inputted numbers match the MINES list
    private bool CheckIfInputMatchesMines(List<List<int>> input)
    {
        Debug.Log(input);
        if (input.Count != MINES.Count)
            return false;

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i].Count != MINES[i].Count)
                return false;

            for (int j = 0; j < input[i].Count; j++)
            {
                if (input[i][j] != MINES[i][j])
                    return false;
            }
        }

        return true;
    }

    // Example usage of the CheckIfInputMatchesMines method
    public void ValidateInput(List<List<int>> input)
    {
        if (CheckIfInputMatchesMines(input))
        {
            levelClearPrompt.SetActive(true);
            Debug.Log("Input matches the MINES list!");
            // Add logic for when the input matches the MINES list
        }
        else
        {
            Debug.Log("Input does not match the MINES list.");
            // Add logic for when the input does not match the MINES list
        }
    }
}
