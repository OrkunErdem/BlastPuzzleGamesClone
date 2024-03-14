using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BlastPuzzle.Scripts.Data
{
    [CreateAssetMenu(fileName = "LevelData_0", menuName = "ScriptableObjects/LevelData", order = 4)]
    public class LevelData : ScriptableObject
    {
        private static readonly Dictionary<string, int> TileValueMap = new Dictionary<string, int>
        {
            { "r", 0 },
            { "g", 1 },
            { "b", 2 },
            { "y", 3 },
            { "rand", 8 },
            { "t", 4 },
            { "bo", 5 },
            { "s", 6 },
            { "v", 7 },
        };

        public string Id
        {
            get => id;
            set => id = value;
        }

        public int[,] MapData
        {
            get
            {
                string[] tileStrings = data.Split(new[] { ", " }, StringSplitOptions.None);

                int numRows = gridHeight;
                int numCols = gridWidth;

                int[,] mapData = new int[numRows, numCols];

                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        int index = i * numCols + j;
                        if (index < tileStrings.Length)
                        {
                            string tileName = tileStrings[index].Trim('"');
                            TileValueMap.TryGetValue(tileName, out int tileValue);
                            tileValue = tileValue ==8 ? UnityEngine.Random.Range(0, 4) : tileValue;

                            mapData[i, j] = tileValue;
                        }
                        else
                        {
                            mapData[i, j] = 0;
                        }
                    }
                }

                return mapData;
            }
            set
            {
                int numRows = gridWidth;
                int numCols = gridWidth;

                StringBuilder sb = new StringBuilder();
                sb.Append("[");

                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        int tileValue = value[i, j];
                        string tileName = GetTileName(tileValue);
                        sb.Append($"\"{tileName}\"");

                        if (j < numCols - 1)
                        {
                            sb.Append(", ");
                        }
                    }

                    if (i < numRows - 1)
                    {
                        sb.Append(", ");
                    }
                }

                sb.Append("]");
                data = sb.ToString();
            }
        }

        private static string GetTileName(int tileValue)
        {
            foreach (var kvp in TileValueMap)
            {
                if (kvp.Value == tileValue)
                {
                    return kvp.Key;
                }
            }

            return "unknown";
        }

        [Header("LEVEL DATA")] public int levelNumber;
        public int gridWidth;
        public int gridHeight;
        public int moveCount;


        [Header("ID AND MAP DATA")] [SerializeField]
        private string id;

        [SerializeField] private string data;
    }
}