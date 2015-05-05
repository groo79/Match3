using UnityEngine;
using System.Collections;

#if !UNITY_3_5
namespace u2dex
{
#endif
    /// <summary>
    /// The Grid component that's used on a camera to visualize a 2D grid.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("U2DEX/Camera/Grid")]
    public class u2dexGrid : MonoBehaviour
    {
        public Vector2 gridSize = new Vector2(64, 64);

        public Color color = Color.white;

        float lineLength = 100000;

        void OnDrawGizmos()
        {
            Vector3 position = Camera.current.transform.position;
            Gizmos.color = color;

            GameObject sceneCamObj = GameObject.Find("SceneCamera");
            Camera sceneCamera = new Camera();

            var numLines = 0f;
            if (sceneCamObj != null)
            {
                sceneCamera = sceneCamObj.GetComponent<Camera>();
                position = sceneCamera.transform.position;

                numLines = sceneCamera.orthographicSize * 4.0f;
            }

            //Draw horizontal lines
            for (float y = position.y - numLines; y < position.y + numLines; y += gridSize.y)
            {

                Gizmos.DrawLine(new Vector3(-lineLength, Mathf.Floor(y / gridSize.y) * gridSize.y + 0, 0.0f),
                                new Vector3(lineLength, Mathf.Floor(y / gridSize.y) * gridSize.y + 0, 0.0f));
            }

            //Draw vertical lines
            for (float x = position.x - numLines; x < position.x + numLines; x += gridSize.y)
            {

                Gizmos.DrawLine(new Vector3(Mathf.Floor(x / gridSize.x) * gridSize.x + 0, -lineLength, 0.0f),
                                new Vector3(Mathf.Floor(x / gridSize.x) * gridSize.x + 0, lineLength, 0.0f));
            }
        }
    }
#if !UNITY_3_5
}
#endif
