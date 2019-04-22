//https://gist.github.com/RyanNielson/9879056

using System.Collections.Generic;
using UnityEngine;

namespace Game.Scene.CameraControllers
{
    public class TrackTargetsCamera : MonoBehaviour
    {
        [SerializeField]
        List<Transform> targets = new List<Transform>();

        [SerializeField]
        float boundingBoxPadding = 2f;

        [SerializeField]
        float minimumOrthographicSize = 8f;

        [SerializeField]
        float zoomSpeed = 20f;

        Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
            _camera.orthographic = true;
        }

        void LateUpdate()
        {
            Rect boundingBox = CalculateTargetsBoundingBox();
            transform.position = CalculateCameraPosition(boundingBox);
            _camera.orthographicSize = CalculateOrthographicSize(boundingBox);
        }

        public void AddTarget(Transform target)
        {
            targets.Add(target);
        }

        public void RemoveTarget(Transform target)
        {
            targets.Remove(target);
        }

        /// <summary>
        /// Calculates a bounding box that contains all the targets.
        /// </summary>
        /// <returns>A Rect containing all the targets.</returns>
        Rect CalculateTargetsBoundingBox()
        {
            float minX = Mathf.Infinity;
            float maxX = Mathf.NegativeInfinity;
            float minY = Mathf.Infinity;
            float maxY = Mathf.NegativeInfinity;

            foreach (Transform target in targets)
            {
                Vector3 position = target.position;

                minX = Mathf.Min(minX, position.x);
                minY = Mathf.Min(minY, position.y);
                maxX = Mathf.Max(maxX, position.x);
                maxY = Mathf.Max(maxY, position.y);
            }

            return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
        }

        /// <summary>
        /// Calculates a camera position given the a bounding box containing all the targets.
        /// </summary>
        /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
        /// <returns>A Vector3 in the center of the bounding box.</returns>
        Vector3 CalculateCameraPosition(Rect boundingBox)
        {
            Vector2 boundingBoxCenter = boundingBox.center;

            return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, -10f);
        }

        /// <summary>
        /// Calculates a new orthographic size for the camera based on the target bounding box.
        /// </summary>
        /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
        /// <returns>A float for the orthographic size.</returns>
        float CalculateOrthographicSize(Rect boundingBox)
        {
            float orthographicSize = _camera.orthographicSize;
            Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
            Vector3 topRightAsViewport = _camera.WorldToViewportPoint(topRight);

            if (topRightAsViewport.x >= topRightAsViewport.y)
                orthographicSize = Mathf.Abs(boundingBox.width) / _camera.aspect / 2f;
            else
                orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

            return Mathf.Max(Mathf.Lerp(_camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize);
        }
    }
}
