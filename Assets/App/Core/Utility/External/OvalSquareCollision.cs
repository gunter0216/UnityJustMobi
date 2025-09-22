using UnityEngine;

namespace App.Core.Utility.External
{
    public static class OvalSquareCollision
    {
        /// <summary>
        /// Проверяет пересечение квадрата с овалом
        /// </summary>
        /// <param name="squareRect">RectTransform квадрата</param>
        /// <param name="ovalRect">RectTransform овала</param>
        /// <returns>true если пересекаются</returns>
        public static bool IsSquareIntersectingOval(RectTransform squareRect, RectTransform ovalRect)
        {
            // Получаем параметры овала
            Vector2 ovalCenter = GetWorldCenter(ovalRect);
            Vector2 ovalSize = GetWorldSize(ovalRect);
            float radiusX = ovalSize.x * 0.5f;
            float radiusY = ovalSize.y * 0.5f;

            // Получаем параметры квадрата
            Rect squareWorldRect = GetWorldRect(squareRect);

            return IsRectIntersectingEllipse(squareWorldRect, ovalCenter, radiusX, radiusY);
        }

        /// <summary>
        /// Проверяет пересечение прямоугольника с эллипсом
        /// </summary>
        private static bool IsRectIntersectingEllipse(Rect rect, Vector2 ellipseCenter, float radiusX, float radiusY)
        {
            // 1. Проверяем, есть ли углы квадрата внутри овала
            Vector2[] corners =
            {
                new Vector2(rect.xMin, rect.yMin), // bottom-left
                new Vector2(rect.xMax, rect.yMin), // bottom-right
                new Vector2(rect.xMax, rect.yMax), // top-right
                new Vector2(rect.xMin, rect.yMax) // top-left
            };

            foreach (Vector2 corner in corners)
            {
                if (IsPointInEllipse(corner, ellipseCenter, radiusX, radiusY))
                {
                    return true;
                }
            }

            // 2. Проверяем, пересекают ли стороны квадрата овал
            Vector2[] edges =
            {
                new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin), // bottom edge
                new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, rect.yMax), // right edge
                new Vector2(rect.xMax, rect.yMax), new Vector2(rect.xMin, rect.yMax), // top edge
                new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMin, rect.yMin) // left edge
            };

            for (int i = 0; i < edges.Length; i += 2)
            {
                if (IsLineIntersectingEllipse(edges[i], edges[i + 1], ellipseCenter, radiusX, radiusY))
                {
                    return true;
                }
            }

            // 3. Проверяем, находится ли центр овала внутри квадрата
            if (rect.Contains(ellipseCenter))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет, находится ли точка внутри эллипса
        /// </summary>
        private static bool IsPointInEllipse(Vector2 point, Vector2 center, float radiusX, float radiusY)
        {
            float dx = point.x - center.x;
            float dy = point.y - center.y;

            return (dx * dx) / (radiusX * radiusX) + (dy * dy) / (radiusY * radiusY) <= 1.0f;
        }

        /// <summary>
        /// Проверяет пересечение линии с эллипсом (упрощенная версия)
        /// </summary>
        private static bool IsLineIntersectingEllipse(Vector2 lineStart, Vector2 lineEnd, Vector2 center, float radiusX,
            float radiusY)
        {
            // Находим ближайшую точку на линии к центру эллипса
            Vector2 closestPoint = GetClosestPointOnLine(lineStart, lineEnd, center);

            // Проверяем, находится ли эта точка внутри эллипса
            return IsPointInEllipse(closestPoint, center, radiusX, radiusY);
        }

        /// <summary>
        /// Находит ближайшую точку на линии к заданной точке
        /// </summary>
        private static Vector2 GetClosestPointOnLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 line = lineEnd - lineStart;
            float lineLength = line.magnitude;

            if (lineLength == 0)
                return lineStart;

            Vector2 lineDirection = line / lineLength;
            Vector2 toPoint = point - lineStart;

            float projectionLength = Vector2.Dot(toPoint, lineDirection);
            projectionLength = Mathf.Clamp(projectionLength, 0, lineLength);

            return lineStart + lineDirection * projectionLength;
        }

        /// <summary>
        /// Альтернативный метод через проверку расстояния
        /// </summary>
        public static bool IsSquareIntersectingOvalByDistance(RectTransform squareRect, RectTransform ovalRect)
        {
            Vector2 squareCenter = GetWorldCenter(squareRect);
            Vector2 ovalCenter = GetWorldCenter(ovalRect);

            Vector2 squareSize = GetWorldSize(squareRect);
            Vector2 ovalSize = GetWorldSize(ovalRect);

            float squareRadius = Mathf.Max(squareSize.x, squareSize.y) * 0.5f;
            float ovalRadius = Mathf.Max(ovalSize.x, ovalSize.y) * 0.5f;

            float distance = Vector2.Distance(squareCenter, ovalCenter);

            // Быстрая проверка: если расстояние больше суммы радиусов, пересечения нет
            if (distance > squareRadius + ovalRadius)
                return false;

            // Если один объект внутри другого
            if (distance < Mathf.Abs(squareRadius - ovalRadius))
                return true;

            // Для более точной проверки используем основной метод
            return IsSquareIntersectingOval(squareRect, ovalRect);
        }

        /// <summary>
        /// Проверяет, полностью ли квадрат внутри овала
        /// </summary>
        public static bool IsSquareCompletelyInsideOval(RectTransform squareRect, RectTransform ovalRect)
        {
            Vector2 ovalCenter = GetWorldCenter(ovalRect);
            Vector2 ovalSize = GetWorldSize(ovalRect);
            float radiusX = ovalSize.x * 0.5f;
            float radiusY = ovalSize.y * 0.5f;

            // Получаем все углы квадрата
            Vector3[] corners = new Vector3[4];
            squareRect.GetWorldCorners(corners);

            // Проверяем, что все углы внутри овала
            foreach (Vector3 corner in corners)
            {
                Vector2 point = new Vector2(corner.x, corner.y);
                if (!IsPointInEllipse(point, ovalCenter, radiusX, radiusY))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Получает процент перекрытия квадрата с овалом (приблизительно)
        /// </summary>
        public static float GetOverlapPercentage(RectTransform squareRect, RectTransform ovalRect, int samples = 10)
        {
            Rect squareWorldRect = GetWorldRect(squareRect);
            Vector2 ovalCenter = GetWorldCenter(ovalRect);
            Vector2 ovalSize = GetWorldSize(ovalRect);
            float radiusX = ovalSize.x * 0.5f;
            float radiusY = ovalSize.y * 0.5f;

            int totalSamples = samples * samples;
            int samplesInside = 0;

            // Семплируем точки внутри квадрата
            for (int x = 0; x < samples; x++)
            {
                for (int y = 0; y < samples; y++)
                {
                    float px = Mathf.Lerp(squareWorldRect.xMin, squareWorldRect.xMax, (x + 0.5f) / samples);
                    float py = Mathf.Lerp(squareWorldRect.yMin, squareWorldRect.yMax, (y + 0.5f) / samples);

                    Vector2 samplePoint = new Vector2(px, py);

                    if (IsPointInEllipse(samplePoint, ovalCenter, radiusX, radiusY))
                    {
                        samplesInside++;
                    }
                }
            }

            return (float)samplesInside / totalSamples;
        }

        // Утилитарные методы
        private static Vector2 GetWorldCenter(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector3 center = (corners[0] + corners[2]) * 0.5f;
            return new Vector2(center.x, center.y);
        }

        private static Vector2 GetWorldSize(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            float width = Vector3.Distance(corners[0], corners[3]);
            float height = Vector3.Distance(corners[0], corners[1]);

            return new Vector2(width, height);
        }

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
    }
}
