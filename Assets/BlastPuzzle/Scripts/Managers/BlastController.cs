using BlastPuzzle.Scripts.Cubes;
using BlastPuzzle.Scripts.Managers.Map;
using UnityEngine;

namespace BlastPuzzle.Scripts.Managers
{
    public class BlastController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.TryGetComponent(out BasicNode node))
                    {
                        if (node.transform.childCount > 0)
                        {
                            var x = node.GetComponentInChildren<CubeItem>();
                            if (x.PoolInstanceId == "TntItem")
                            {
                                GridManager.Instance.BlastTnt(node.gridPos);
                            }
                            else
                            {
                                if (x.IsObstacle)
                                {
                                    Debug.Log("It is Obstacle");
                                }
                                else
                                {
                                    GridManager.Instance.Blast(node.gridPos);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Did not hit anything");
                }
            }
        }
    }
}