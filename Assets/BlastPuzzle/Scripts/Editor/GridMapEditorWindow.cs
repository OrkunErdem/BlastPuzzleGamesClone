using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BlastPuzzle.Scripts.Data;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class GridMapEditorWindow : EditorWindow
{
    private const int CELL_SIZE = 20;
    private const int MAX_GRID_SIZE = 20;

    private int[,] gridData;
    private int[] possibleValues = new int[] { 0, 1, 2, 3, 4, 5, 6 };

    private Dictionary<int, Color> possibleColors = new Dictionary<int, Color>()
    {
        { 0, new Color(150, 150, 150, 255) },
        { 1, Color.blue },
        { 2, Color.yellow },
        { 3, Color.red },
        { 4, Color.magenta },
        { 5, Color.green },
        { 6, Color.cyan }
    };

    private Dictionary<int, Texture2D> possibleTextures = new();

    private LevelContainer _levelContainer;
    private LevelData _levelData;

    private string _levelId;
    private int width;
    private int height;
    private int selectedPaletteIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < possibleColors.Count; i++)
        {
            possibleTextures.Add(i, GetTexture2D(1, 1, possibleColors[i]));
        }
    }

    [MenuItem("Window/Grid Map Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GridMapEditorWindow));
    }

    private void OnEnable()
    {
        //CreateGridData();
        _levelContainer =
            AssetDatabase.LoadAssetAtPath<LevelContainer>(
                "/Resources/LevelSystemData/LevelContainer.asset");
       // CreateGridData();
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Map Editor", EditorStyles.boldLabel);
        GUILayout.TextArea("blue: bo, red: r, green: g, yellow: y, random: rand", EditorStyles.helpBox);
        _levelId = EditorGUILayout.TextField("Level Id", _levelId);
        width = EditorGUILayout.IntSlider("Width", width, 1, MAX_GRID_SIZE);
        height = EditorGUILayout.IntSlider("height", height, 1, MAX_GRID_SIZE);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Regenerate Grid"))
        {
            CreateGridData();
        }

        EditorGUILayout.Space(10);
        DrawPalette();
        EditorGUILayout.Space(10);

        if (GUILayout.Button("Load"))
        {
            if (_levelContainer.TryGetLevelData(_levelId, out _levelData))
            {
                var data = _levelData.MapData;
                width = _levelData.gridWidth;
                height = _levelData.gridHeight;

                RegenerateGridData(data);
            }
        }

        if (GUILayout.Button("Save"))
        {
            var levelData = CreateNewLevel(_levelId, _levelId);
            _levelData = levelData;
            Save(levelData);
        }

        EditorGUILayout.EndHorizontal();

        // Draw the grid map
        GUILayout.Space(10);
        DrawGrid();

        GUILayout.Space(10);
    }

    private void CreateGridData()
    {
        gridData = new int[width, height];

        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                gridData[x, y] = 0;
            }
        }
    }


    private void RegenerateGridData(int[,] data)
    {
        gridData = data;
    }

    private void DrawPalette()
    {
        var defaultGuiColor = GUI.color;
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < possibleColors.Count; i++)
        {
            possibleColors.TryGetValue(i, out Color c);
            GUI.color = c;
            if (GUILayout.Button(i.ToString(), GUILayout.Width(CELL_SIZE), GUILayout.Height(CELL_SIZE)))
            {
                selectedPaletteIndex = i;
            }
        }

        EditorGUILayout.EndHorizontal();
        GUI.color = defaultGuiColor;
    }

    private void DrawGrid()
    {
        var defaultGuiColor = GUI.color;

        for (int x = 0; x < width; x++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < height; y++)
            {
                int cellValue = gridData[x, y];


                if (possibleTextures.TryGetValue(cellValue, out var tex))

                    if (possibleColors.TryGetValue(cellValue, out Color c))
                    {
                        GUI.color = c;
                    }

                if (GUILayout.Button(cellValue.ToString(), GUILayout.Width(CELL_SIZE), GUILayout.Height(CELL_SIZE)))
                {
                    int newValueIndex = (Array.IndexOf(possibleValues, cellValue) + 1) % possibleValues.Length;
                    gridData[x, y] = selectedPaletteIndex;
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        GUI.color = defaultGuiColor;
    }

    private Texture2D GetTexture2D(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, width);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        return texture;
    }


    private static readonly string LevelsPath = "/Resources/LevelSystemData/LevelsData";
    private static readonly string LevelAssetsPath = Path.Combine("Assets", LevelsPath);

    public LevelData CreateNewLevel(string levelName, string id)
    {
        var resourcePath = Path.Combine("Data/Levels", $"{levelName}");
        var levelData = Resources.Load<LevelData>(resourcePath);
        if (levelData) return levelData;

        levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.Id = id;
        levelData.name = levelName;
        var path = Path.Combine(LevelAssetsPath, $"{levelName}.asset");
        AssetDatabase.CreateAsset(levelData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return levelData;
    }


    private string ConvertListToString(List<(int, int)> list)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in list)
        {
            sb.Append("(" + item.Item1 + ", " + item.Item2 + ")");
        }

        return sb.ToString();
    }

    private Texture2D GetOutlinedTexture(int width, int height, Color color, Color outlineColor)
    {
        Texture2D texture = new Texture2D(width, width);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    texture.SetPixel(i, j, outlineColor);
                }
                else
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }

        texture.Apply();

        return texture;
    }

    public int[,] ReverseRows(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        var matrix2 = new int[rows, cols];

        for (int i = 0; i < rows / 2; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                (matrix2[i, j], matrix2[rows - 1 - i, j]) = (matrix[rows - 1 - i, j], matrix[i, j]);
            }
        }

        return matrix2;
    }


    public void Save(LevelData levelData)
    {
        EditorUtility.SetDirty(levelData);
        levelData.gridHeight = height;
        levelData.gridWidth = width;
        levelData.MapData = gridData;

        _levelContainer.AddLevelData(levelData);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}