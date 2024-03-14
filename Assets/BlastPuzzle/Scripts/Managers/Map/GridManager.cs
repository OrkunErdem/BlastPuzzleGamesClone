using System;
using System.Collections.Generic;
using BlastPuzzle.Scripts.Cubes;
using BlastPuzzle.Scripts.Data;
using BlastPuzzle.Scripts.Services;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BlastPuzzle.Scripts.Managers.Map
{
    [Serializable]
    public struct CubeItemData
    {
        public string id;
        public CubeItem prefab;
    }

    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private LevelContainer levelContainer;
        [SerializeField] private int rows;
        [SerializeField] private int cols;
        [SerializeField] private float cellSize;
        [SerializeField] private Transform map;
        private static GameObject[,] _cells;
        public SpriteRenderer background;
        public Vector3 Pos => background.transform.position;
        private int[,] _objectMatrix;
        [SerializeField] private List<CubeItemData> nodePrefabDataList;
        private readonly Dictionary<string, CubeItem> _prefabDictionary = new Dictionary<string, CubeItem>();
        private LevelData _currentLevelData;


        private int _moveCount;

        private int MoveCount
        {
            get => _moveCount;
            set
            {
                _moveCount = value;
                EventSystem.TriggerEvent(Events.MoveCountChanged, MoveCount, _moveCount);
            }
        }

        private static readonly Vector2Int[] Directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        private void Start()
        {
            var x = levelContainer.Levels[PlayerPrefs.GetInt("ActiveLevel")].MapData;
            _currentLevelData = levelContainer.Levels[PlayerPrefs.GetInt("ActiveLevel")];
            LevelInitialize(x);
        }

        public void LevelComplete()
        {
            SceneManager.LoadScene("MainScene");
            AreaManager.Instance.GoNext();
        }

        public void LevelFailed()
        {
            SceneManager.LoadScene("MainScene");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                LevelComplete();
            }
        }

        private void LevelInitialize(int[,] mapData)
        {
            _objectMatrix = mapData;
            MoveCount = _currentLevelData.moveCount;
            ReverseRows(_objectMatrix);
            rows = _objectMatrix.GetLength(0);
            cols = _objectMatrix.GetLength(1);
            _cells = new GameObject[rows, cols];
            var x = (cols - 1) * cellSize / 2;
            var y = (rows - 1) * cellSize / 2;
            SetCamera(x, y);
            SetBackground(x, y);
            InitializeMap();
        }

        private void SetCamera(float x, float y)
        {
            CameraController.SetCameraPosition(new Vector3(x, 2 * y, -1));
            CameraController.SetCameraSize(rows * cellSize);
        }

        private void SetBackground(float x, float y)
        {
            background.size = new Vector2((cols * cellSize) + (cellSize / 3), (rows * cellSize) + (cellSize / 3));
            background.transform.position = new Vector3(x, y, 0);
        }

        private static void ReverseRows(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows / 2; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    (matrix[i, j], matrix[rows - 1 - i, j]) = (matrix[rows - 1 - i, j], matrix[i, j]);
                }
            }
        }


        private void InitializeMap()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float x = col * cellSize;
                    float y = row * cellSize;
                    Vector3 position = new Vector3(x, y, 0);

                    GameObject cell = PoolingSystem.Instance.Instantiate("BasicNode", position, Quaternion.identity);
                    cell.GetComponent<BasicNode>().gridPos = new Vector2Int(row, col);
                    cell.transform.parent = map;
                    _cells[row, col] = cell;
                }
            }


            Filler();
        }


        private void DamageCube(Vector2Int gridPosition, int damage = 1)
        {
            CubeItem cubeItem = _cells[gridPosition.x, gridPosition.y].GetComponentInChildren<CubeItem>();
            cubeItem.TakeDamage(() => { _objectMatrix[gridPosition.x, gridPosition.y] = -1; }, damage);
        }

        private void InstantiateObjectAtLocation(int row, int col, GameObject nodeObject)
        {
            GameObject cellObject = _cells[row, col];

            if (nodeObject.TryGetComponent(out IPoolable poolObject))
            {
                var o =
                    PoolingSystem.Instance.Instantiate(poolObject.PoolInstanceId, cellObject.transform.position,
                        Quaternion.identity);
                o.GetComponent<CubeSkinController>().SetSpriteOrder(row * 5);
                o.TryGetComponent(out CubeItem cubeItem);

                if (_cubeProperties.TryGetValue(poolObject.PoolInstanceId, out var properties))
                {
                    cubeItem.Health = properties.Health;
                    cubeItem.IsObstacle = properties.IsObstacle;
                    EventSystem.TriggerEvent(Events.ObstacleCreated, poolObject.PoolInstanceId);
                }
                else
                {
                    cubeItem.Health = 1;
                }

                o.transform.parent = cellObject.transform;
            }
        }

        private class CubeProperties
        {
            public int Health { get; set; }
            public bool IsObstacle { get; set; }
        }

        private readonly Dictionary<string, CubeProperties> _cubeProperties = new Dictionary<string, CubeProperties>
        {
            { "VaseItem", new CubeProperties { Health = 2, IsObstacle = true } },
            { "BoxItem", new CubeProperties { Health = 1, IsObstacle = true } },
            { "StoneItem", new CubeProperties { Health = 1, IsObstacle = true } },
        };


        private GameObject InstantiateObject(int row, int col, GameObject nodeObject)
        {
            GameObject cellObject = _cells[row, col];

            if (nodeObject.TryGetComponent(out IPoolable poolObject))
            {
                var o =
                    PoolingSystem.Instance.Instantiate(poolObject.PoolInstanceId, cellObject.transform.position,
                        Quaternion.identity);
                o.GetComponent<CubeSkinController>().SetSpriteOrder(row * 5);
                o.transform.DOMoveY(rows + row, 0);
                o.transform.parent = cellObject.transform;
                return o;
            }

            return null;
        }


        private void Filler()
        {
            foreach (var data in nodePrefabDataList)
            {
                _prefabDictionary.Add(data.id, data.prefab);
            }

            _FillMap();
        }

        private void _FillMap()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var node = _prefabDictionary[_objectMatrix[i, j].ToString()];
                    InstantiateObjectAtLocation(i, j, node.gameObject);
                }
            }

            CheckSprites();
        }


        private void CheckSprites()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _cells[i, j].transform.GetComponentInChildren<CubeSkinController>()
                        .ChangeSprite(GetSameColorNeighbors(new Vector2Int(i, j)).Count > 3);
                }
            }
        }

        private List<Vector2Int> GetSameColorNeighbors(Vector2Int gridPosition)
        {
            List<Vector2Int> sameColorNeighbors = new List<Vector2Int>();
            var targetColor = _objectMatrix[gridPosition.x, gridPosition.y];


            HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int> { gridPosition };

            void FindSameColorNeighborsRecursive(Vector2Int currentPosition)
            {
                foreach (var direction in Directions)
                {
                    Vector2Int neighborPosition = currentPosition + direction;


                    if (IsWithinGridBounds(neighborPosition) &&
                        _objectMatrix[neighborPosition.x, neighborPosition.y] == targetColor &&
                        !visitedCells.Contains(neighborPosition))
                    {
                        sameColorNeighbors.Add(neighborPosition);
                        visitedCells.Add(neighborPosition);
                        FindSameColorNeighborsRecursive(neighborPosition);
                    }
                }
            }

            FindSameColorNeighborsRecursive(gridPosition);

            return sameColorNeighbors;
        }

        public void Blast(Vector2Int gridPosition)
        {
            List<Vector2Int> sameColorNeighbors = GetSameColorNeighbors(gridPosition);
            if (sameColorNeighbors.Count == 0) return;
            MoveCount--;
            sameColorNeighbors.Add(gridPosition);

            foreach (var neighbor in sameColorNeighbors)
            {
                DestroyAdjacentCubes(neighbor);
                DamageCube(neighbor);
            }

            if (sameColorNeighbors.Count > 4)
            {
                InstantiateTnt(gridPosition);
            }

            FillEmptySpaces();
        }


        public void BlastTnt(Vector2Int gridPosition)
        {
            List<Vector2Int> sameColorNeighbors = GetSameColorNeighbors(gridPosition);
            DestroyGrids(gridPosition, sameColorNeighbors.Count > 0 ? 7 : 5);
            FillEmptySpaces();
        }


        private void DestroyGrids(Vector2Int centerGridPosition, int gridSize)
        {
            int minX = Mathf.Max(0, centerGridPosition.x - gridSize / 2);
            int minY = Mathf.Max(0, centerGridPosition.y - gridSize / 2);
            int maxX = Mathf.Min(rows - 1, centerGridPosition.x + gridSize / 2);
            int maxY = Mathf.Min(cols - 1, centerGridPosition.y + gridSize / 2);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector2Int gridPosition = new Vector2Int(x, y);

                    if (_objectMatrix[x, y] != -1)
                    {
                        DamageCube(gridPosition);
                    }
                }
            }

            FillEmptySpaces();
        }

        private void DestroyAdjacentCubes(Vector2Int centerGridPosition)
        {
            foreach (var direction in Directions)
            {
                Vector2Int neighborGridPosition = centerGridPosition + direction;

                if (IsWithinGridBounds(neighborGridPosition))
                {
                    int itemID = _objectMatrix[neighborGridPosition.x, neighborGridPosition.y];

                    if (itemID == 5 || itemID == 6 || itemID == 7)
                    {
                        DamageCube(neighborGridPosition);
                    }
                }
            }
        }


        private void InstantiateTnt(Vector2Int gridPosition)
        {
            var newCube = _prefabDictionary[(4).ToString()];
            InstantiateObjectAtLocation(gridPosition.x, gridPosition.y, newCube.gameObject);
            _objectMatrix[gridPosition.x, gridPosition.y] = 4;
        }

        private bool IsWithinGridBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < rows && position.y >= 0 && position.y < cols;
        }


        private void FillEmptySpaces()
        {
            for (int col = 0; col < cols; col++)
            {
                int newRow = 0;

                for (int row = 0; row < rows; row++)
                {
                    if (_objectMatrix[row, col] != -1)
                    {
                        _objectMatrix[newRow, col] = _objectMatrix[row, col];

                        if (newRow != row)
                        {
                            _objectMatrix[row, col] = -1;
                        }

                        GameObject cubeObject = _cells[row, col].transform.GetChild(0).gameObject;
                        cubeObject.transform.parent = _cells[newRow, col].transform;
                        cubeObject.GetComponent<CubeSkinController>().SetSpriteOrder(row * 5);
                        cubeObject.transform.DOMove(_cells[newRow, col].transform.position, 10).SetSpeedBased(true);

                        newRow++;
                    }
                }

                for (; newRow < rows; newRow++)
                {
                    int newCubeColor = Random.Range(0, 4);
                    var newCube = _prefabDictionary[newCubeColor.ToString()];
                    var o = InstantiateObject(newRow, col, newCube.gameObject);
                    o.transform.DOMove(_cells[newRow, col].transform.position, 10).SetSpeedBased(true);
                    _objectMatrix[newRow, col] = newCubeColor;
                }
            }

            CheckSprites();
        }
    }
}