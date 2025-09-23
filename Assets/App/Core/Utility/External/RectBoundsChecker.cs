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
        public static bool IsRectCompletelyInsideWithTolerance(RectTransform cubeRect, RectTransform areaRect,
            float tolerance = 1f)
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

        /// <summary>
        /// Проверяет, пересекаются ли два RectTransform (надежный метод)
        /// </summary>
        /// <param name="rect1">Первый RectTransform</param>
        /// <param name="rect2">Второй RectTransform</param>
        /// <returns>true если пересекаются</returns>
        public static bool AreRectsIntersecting(RectTransform rect1, RectTransform rect2)
        {
            if (rect1 == null || rect2 == null)
                return false;

            // Используем более надежный метод через углы
            return AreRectsIntersectingByCorners(rect1, rect2);
        }

        /// <summary>
        /// Альтернативный метод через Rect.Overlaps (может быть менее точным)
        /// </summary>
        public static bool AreRectsIntersectingByOverlaps(RectTransform rect1, RectTransform rect2)
        {
            if (rect1 == null || rect2 == null)
                return false;

            Rect worldRect1 = GetWorldRect(rect1);
            Rect worldRect2 = GetWorldRect(rect2);

            return worldRect1.Overlaps(worldRect2);
        }

        /// <summary>
        /// Проверка пересечения через углы (самый надежный метод)
        /// </summary>
        public static bool AreRectsIntersectingByCorners(RectTransform rect1, RectTransform rect2)
        {
            if (rect1 == null || rect2 == null)
                return false;

            // Получаем углы обоих прямоугольников в мировых координатах
            Vector3[] corners1 = new Vector3[4];
            Vector3[] corners2 = new Vector3[4];

            rect1.GetWorldCorners(corners1);
            rect2.GetWorldCorners(corners2);

            // Находим границы каждого прямоугольника
            float minX1 = corners1[0].x, maxX1 = corners1[0].x;
            float minY1 = corners1[0].y, maxY1 = corners1[0].y;

            for (int i = 1; i < 4; i++)
            {
                if (corners1[i].x < minX1) minX1 = corners1[i].x;
                if (corners1[i].x > maxX1) maxX1 = corners1[i].x;
                if (corners1[i].y < minY1) minY1 = corners1[i].y;
                if (corners1[i].y > maxY1) maxY1 = corners1[i].y;
            }

            float minX2 = corners2[0].x, maxX2 = corners2[0].x;
            float minY2 = corners2[0].y, maxY2 = corners2[0].y;

            for (int i = 1; i < 4; i++)
            {
                if (corners2[i].x < minX2) minX2 = corners2[i].x;
                if (corners2[i].x > maxX2) maxX2 = corners2[i].x;
                if (corners2[i].y < minY2) minY2 = corners2[i].y;
                if (corners2[i].y > maxY2) maxY2 = corners2[i].y;
            }

            // Проверяем пересечение по осям с небольшим допуском
            const float epsilon = 0.001f;
            bool intersectsX = maxX1 > minX2 + epsilon && maxX2 > minX1 + epsilon;
            bool intersectsY = maxY1 > minY2 + epsilon && maxY2 > minY1 + epsilon;

            return intersectsX && intersectsY;
        }

        /// <summary>
        /// Проверяет пересечение с дополнительными параметрами
        /// </summary>
        /// <param name="rect1">Первый RectTransform</param>
        /// <param name="rect2">Второй RectTransform</param>
        /// <param name="allowTouching">Считать касание пересечением</param>
        /// <returns>true если пересекаются</returns>
        public static bool AreRectsIntersecting(RectTransform rect1, RectTransform rect2, bool allowTouching)
        {
            if (rect1 == null || rect2 == null)
                return false;

            Vector3[] corners1 = new Vector3[4];
            Vector3[] corners2 = new Vector3[4];

            rect1.GetWorldCorners(corners1);
            rect2.GetWorldCorners(corners2);

            // Находим boundaries
            float minX1 = corners1[0].x, maxX1 = corners1[0].x;
            float minY1 = corners1[0].y, maxY1 = corners1[0].y;

            for (int i = 1; i < 4; i++)
            {
                if (corners1[i].x < minX1) minX1 = corners1[i].x;
                if (corners1[i].x > maxX1) maxX1 = corners1[i].x;
                if (corners1[i].y < minY1) minY1 = corners1[i].y;
                if (corners1[i].y > maxY1) maxY1 = corners1[i].y;
            }

            float minX2 = corners2[0].x, maxX2 = corners2[0].x;
            float minY2 = corners2[0].y, maxY2 = corners2[0].y;

            for (int i = 1; i < 4; i++)
            {
                if (corners2[i].x < minX2) minX2 = corners2[i].x;
                if (corners2[i].x > maxX2) maxX2 = corners2[i].x;
                if (corners2[i].y < minY2) minY2 = corners2[i].y;
                if (corners2[i].y > maxY2) maxY2 = corners2[i].y;
            }

            bool intersectsX, intersectsY;

            if (allowTouching)
            {
                // Касание считается пересечением (>=)
                intersectsX = maxX1 >= minX2 && maxX2 >= minX1;
                intersectsY = maxY1 >= minY2 && maxY2 >= minY1;
            }
            else
            {
                // Только реальное перекрытие (>)
                const float epsilon = 0.001f;
                intersectsX = maxX1 > minX2 + epsilon && maxX2 > minX1 + epsilon;
                intersectsY = maxY1 > minY2 + epsilon && maxY2 > minY1 + epsilon;
            }

            return intersectsX && intersectsY;
        }

        /// <summary>
        /// Отладочный метод для проверки различных алгоритмов
        /// </summary>
        public static void DebugIntersectionMethods(RectTransform rect1, RectTransform rect2)
        {
            if (rect1 == null || rect2 == null)
            {
                Debug.Log("One of the RectTransforms is null");
                return;
            }

            bool method1 = AreRectsIntersectingByOverlaps(rect1, rect2);
            bool method2 = AreRectsIntersectingByCorners(rect1, rect2);
            bool method3 = AreRectsIntersecting(rect1, rect2, allowTouching: false);
            bool method4 = AreRectsIntersecting(rect1, rect2, allowTouching: true);

            Debug.Log($"[DEBUG] Intersection methods comparison:");
            Debug.Log($"  Overlaps method: {method1}");
            Debug.Log($"  Corners method: {method2}");
            Debug.Log($"  No touching: {method3}");
            Debug.Log($"  Allow touching: {method4}");

            // Дополнительная информация
            Rect worldRect1 = GetWorldRect(rect1);
            Rect worldRect2 = GetWorldRect(rect2);

            Debug.Log($"  Rect1 bounds: {worldRect1}");
            Debug.Log($"  Rect2 bounds: {worldRect2}");

            Vector3[] corners1 = new Vector3[4];
            Vector3[] corners2 = new Vector3[4];
            rect1.GetWorldCorners(corners1);
            rect2.GetWorldCorners(corners2);

            Debug.Log($"  Rect1 corners: [{corners1[0]}, {corners1[1]}, {corners1[2]}, {corners1[3]}]");
            Debug.Log($"  Rect2 corners: [{corners2[0]}, {corners2[1]}, {corners2[2]}, {corners2[3]}]");
        }

        /// <summary>
        /// Получает область пересечения двух RectTransform
        /// </summary>
        /// <param name="rect1">Первый RectTransform</param>
        /// <param name="rect2">Второй RectTransform</param>
        /// <returns>Rect пересечения или пустой Rect если не пересекаются</returns>
        public static Rect GetIntersectionRect(RectTransform rect1, RectTransform rect2)
        {
            Rect worldRect1 = GetWorldRect(rect1);
            Rect worldRect2 = GetWorldRect(rect2);

            if (!worldRect1.Overlaps(worldRect2))
            {
                return new Rect(0, 0, 0, 0); // Пустой прямоугольник
            }

            float xMin = Mathf.Max(worldRect1.xMin, worldRect2.xMin);
            float xMax = Mathf.Min(worldRect1.xMax, worldRect2.xMax);
            float yMin = Mathf.Max(worldRect1.yMin, worldRect2.yMin);
            float yMax = Mathf.Min(worldRect1.yMax, worldRect2.yMax);

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// Получает площадь пересечения двух RectTransform
        /// </summary>
        /// <param name="rect1">Первый RectTransform</param>
        /// <param name="rect2">Второй RectTransform</param>
        /// <returns>Площадь пересечения в мировых единицах</returns>
        public static float GetIntersectionArea(RectTransform rect1, RectTransform rect2)
        {
            Rect intersectionRect = GetIntersectionRect(rect1, rect2);
            return intersectionRect.width * intersectionRect.height;
        }

        /// <summary>
        /// Получает процент пересечения первого прямоугольника со вторым
        /// </summary>
        /// <param name="rect1">Первый RectTransform (базовый для расчета процента)</param>
        /// <param name="rect2">Второй RectTransform</param>
        /// <returns>Процент от 0 до 1, показывающий какая часть rect1 пересекается с rect2</returns>
        public static float GetIntersectionPercentage(RectTransform rect1, RectTransform rect2)
        {
            Rect worldRect1 = GetWorldRect(rect1);
            float rect1Area = worldRect1.width * worldRect1.height;

            if (rect1Area <= 0)
                return 0f;

            float intersectionArea = GetIntersectionArea(rect1, rect2);
            return intersectionArea / rect1Area;
        }
    }
}