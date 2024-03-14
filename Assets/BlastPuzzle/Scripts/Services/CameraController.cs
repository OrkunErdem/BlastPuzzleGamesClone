using UnityEngine;

namespace BlastPuzzle.Scripts.Services
{
    public class CameraController : MonoBehaviour
    {
        private static Camera Camera => Camera.main;


        public static void SetCameraSize(float size)
        {
            Camera.orthographicSize = size;
        }
        public static void SetCameraPosition(Vector3 position)
        {
            Camera.transform.position = position;
        }
    }
}