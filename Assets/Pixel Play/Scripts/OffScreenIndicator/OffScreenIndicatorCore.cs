using UnityEngine;

namespace PixelPlay.OffScreenIndicator
{
    public class OffScreenIndicatorCore
    {
        /// <summary>
        /// Gets the position of the target mapped to screen cordinates.
        /// </summary>
        /// <param name="mainCamera">Refrence to the main camera</param>
        /// <param name="targetPosition">Target position</param>
        /// <returns></returns>
        public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            return screenPosition;
        }

        /// <summary>
        /// Gets if the target is within the view frustrum.
        /// </summary>
        /// <param name="screenPosition">Position of the target mapped to screen cordinates</param>
        /// <returns></returns>
        public static bool IsTargetVisible(Vector3 screenPosition)
        {
            bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
            return isTargetVisible;
        }

        /// <summary>
        /// Gets the screen position and angle for the arrow indicator. 
        /// </summary>
        /// <param name="screenPosition">Position of the target mapped to screen cordinates</param>
        /// <param name="angle">Angle of the arrow</param>
        /// <param name="screenCentre">The screen  centre</param>
        /// <param name="screenBounds">The screen bounds</param>
        public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
        {
            screenPosition -= screenCentre;

            if(screenPosition.z < 0)
            {
                screenPosition *= -1;
            }

            angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            float slope = Mathf.Tan(angle);
            if(screenPosition.x > 0)
            {
                screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
            }
            else
            {
                screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
            } 
            if(screenPosition.y > screenBounds.y)
            {
                screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
            }
            else if(screenPosition.y < -screenBounds.y)
            {
                screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
            }
            screenPosition += screenCentre;
        }
    }
}
