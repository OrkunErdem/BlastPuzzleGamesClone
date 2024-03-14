using UnityEngine;

namespace BlastPuzzle.Scripts.Map
{
    public class Grid : MonoBehaviour
    {
        public Grid(GameObject[,] cells)
        {
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out IPoolable poolable))
                {
                    var id = poolable.PoolInstanceId;
                    PoolingSystem.Instance.Destroy(id, child.gameObject);
                }
            }
        }
    }
}