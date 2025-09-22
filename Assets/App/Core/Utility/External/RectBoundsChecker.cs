using UnityEngine;

namespace App.Core.Utility.External
{
    public static class RectBoundsChecker
    {
        /// <summary>
        /// Проверяет, полностью ли кубик находится внутри области
        /// </summary>
        /// <param name="cubeRect">RectTransform кубика</param>
        /// <param name="areaRect">RectTransform области</param>
        /// <returns>true если кубик полностью внутри области</returns>
        public static bool IsRectCompletelyInside(RectTransform cubeRect, RectTransform areaRect)
        {
            // Получаем границы в мировых координатах
            Rect cubeWorldRect = GetWorldRect(cubeRect);
            Rect areaWorldRect = GetWorldRect(areaRect);
        
            return IsRectInsideRect(cubeWorldRect, areaWorldRect);
        }
    
        /// <summary>
        /// Альтернативный способ через углы
        /// </summary>
        public static bool IsRectCompletelyInsideByCorners(RectTransform cubeRect, RectTransform areaRect)
        {
            // Получаем все 4 угла кубика в мировых координатах
            Vector3[] cubeCorners = new Vector3[4];
            cubeRect.GetWorldCorners(cubeCorners);
        
            // Получаем все 4 угла области в мировых координатах  
            Vector3[] areaCorners = new Vector3[4];
            areaRect.GetWorldCorners(areaCorners);
        
            // Проверяем, что все углы кубика внутри области
            foreach (Vector3 cubeCorner in cubeCorners)
            {
                if (!IsPointInsideRect(cubeCorner, areaCorners))
                {
                    return false;
                }
            }
        
            return true;
        }
    
        /// <summary>
        /// Проверяет пересечение и полное вхождение
        /// </summary>
        public static bool IsRectCompletelyInsideWithOverlap(RectTransform cubeRect, RectTransform areaRect)
        {
            Rect cubeWorldRect = GetWorldRect(cubeRect);
            Rect areaWorldRect = GetWorldRect(areaRect);
        
            // Проверяем, что есть пересечение
            if (!cubeWorldRect.Overlaps(areaWorldRect))
            {
                return false;
            }
        
            // Проверяем полное вхождение
            return IsRectInsideRect(cubeWorldRect, areaWorldRect);
        }
    
        /// <summary>
        /// Получает Rect в мировых координатах из RectTransform
        /// </summary>
        private static Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
        
            float minX = corners[0].x;
            float maxX = corners[0].x;
            float minY = corners[0].y;
            float maxY = corners[0].y;
        
            for (int i = 1; i < 4; i++)
            {
                if (corners[i].x < minX) minX = corners[i].x;
                if (corners[i].x > maxX) maxX = corners[i].x;
                if (corners[i].y < minY) minY = corners[i].y;
                if (corners[i].y > maxY) maxY = corners[i].y;
            }
        
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
    
        /// <summary>
        /// Проверяет, находится ли один прямоугольник полностью внутри другого
        /// </summary>
        private static bool IsRectInsideRect(Rect inner, Rect outer)
        {
            return inner.xMin >= outer.xMin &&
                   inner.xMax <= outer.xMax &&
                   inner.yMin >= outer.yMin &&
                   inner.yMax <= outer.yMax;
        }
    
        /// <summary>
        /// Проверяет, находится ли точка внутри прямоугольника (заданного углами)
        /// </summary>
        private static bool IsPointInsideRect(Vector3 point, Vector3[] corners)
        {
            // corners[0] = bottom-left
            // corners[1] = top-left  
            // corners[2] = top-right
            // corners[3] = bottom-right
        
            float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
            float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
            float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
            float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        
            return point.x >= minX && point.x <= maxX && 
                   point.y >= minY && point.y <= maxY;
        }
    
        /// <summary>
        /// Получает процент перекрытия кубика с областью (0-1)
        /// </summary>
        public static float GetOverlapPercentage(RectTransform cubeRect, RectTransform areaRect)
        {
            Rect cubeWorldRect = GetWorldRect(cubeRect);
            Rect areaWorldRect = GetWorldRect(areaRect);
        
            // Находим пересечение
            float intersectXMin = Mathf.Max(cubeWorldRect.xMin, areaWorldRect.xMin);
            float intersectXMax = Mathf.Min(cubeWorldRect.xMax, areaWorldRect.xMax);
            float intersectYMin = Mathf.Max(cubeWorldRect.yMin, areaWorldRect.yMin);
            float intersectYMax = Mathf.Min(cubeWorldRect.yMax, areaWorldRect.yMax);
        
            // Если нет пересечения
            if (intersectXMin >= intersectXMax || intersectYMin >= intersectYMax)
            {
                return 0f;
            }
        
            // Площадь пересечения
            float intersectArea = (intersectXMax - intersectXMin) * (intersectYMax - intersectYMin);
        
            // Площадь кубика
            float cubeArea = cubeWorldRect.width * cubeWorldRect.height;
        
            return intersectArea / cubeArea;
        }
    
        /// <summary>
        /// Проверяет с допуском (полезно для UI с погрешностями)
        /// </summary>
        public static bool IsRectCompletelyInsideWithTolerance(RectTransform cubeRect, RectTransform areaRect, float tolerance = 1f)
        {
            Rect cubeWorldRect = GetWorldRect(cubeRect);
            Rect areaWorldRect = GetWorldRect(areaRect);
        
            // Расширяем область на величину допуска
            areaWorldRect.xMin -= tolerance;
            areaWorldRect.xMax += tolerance;
            areaWorldRect.yMin -= tolerance;
            areaWorldRect.yMax += tolerance;
        
            return IsRectInsideRect(cubeWorldRect, areaWorldRect);
        }
    }
}