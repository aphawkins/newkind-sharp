﻿// 'Elite - The Sharp Kind' - Andy Hawkins 2023.
// 'Elite - The New Kind' - C.J.Pinder 1999-2001.
// Elite (C) I.Bell & D.Braben 1984.

using System.Diagnostics;
using System.Numerics;

namespace EliteSharp.Graphics
{
    internal class SoftwareGraphics : IGraphics
    {
        private readonly int[,] _buffer;

        internal SoftwareGraphics(int[,] buffer)
        {
            _buffer = buffer;
            ScreenWidth = buffer.GetLength(0);
            ScreenHeight = buffer.GetLength(1);
            Scale = 2;
        }

        public float ScreenHeight { get; }

        public float Scale { get; }

        public float ScreenWidth { get; }

        public void ClearArea(Vector2 position, float width, float height)
        {
            for (float y = position.Y; y < position.Y + width; y++)
            {
                for (float x = position.X; x < position.X + height; x++)
                {
                    _buffer[(int)x, (int)y] = 0;
                }
            }
        }

        public void Dispose()
        {
        }

        public void DrawCircle(Vector2 centre, float radius, Colour colour)
        {
            float diameter = radius * 2;
            float x = radius - 1;
            float y = 0;
            float tx = 1;
            float ty = 1;
            float error = tx - diameter;

            while (x >= y)
            {
                _buffer[(int)(centre.X + x), (int)(centre.Y + y)] = (int)colour;
                _buffer[(int)(centre.X + x), (int)(centre.Y - y)] = (int)colour;
                _buffer[(int)(centre.X - x), (int)(centre.Y + y)] = (int)colour;
                _buffer[(int)(centre.X - x), (int)(centre.Y - y)] = (int)colour;
                _buffer[(int)(centre.X + y), (int)(centre.Y + x)] = (int)colour;
                _buffer[(int)(centre.X + y), (int)(centre.Y - x)] = (int)colour;
                _buffer[(int)(centre.X - y), (int)(centre.Y + x)] = (int)colour;
                _buffer[(int)(centre.X - y), (int)(centre.Y - x)] = (int)colour;

                if (error <= 0)
                {
                    y++;
                    error += ty;
                    ty += 2;
                }

                if (error > 0)
                {
                    x--;
                    tx += 2;
                    error += tx - diameter;
                }
            }
        }

        public void DrawCircleFilled(Vector2 centre, float radius, Colour colour)
        {
            float diameter = radius * 2;
            float x = MathF.Floor(radius - 1);
            float y = 0;
            float tx = 1;
            float ty = 1;
            float error = tx - diameter;

            while (x >= y)
            {
                Debug.WriteLine($"{x},{y}");

                // Top of top half
                DrawLine(new(centre.X - y, centre.Y - x), new(centre.X + y, centre.Y - x), colour);

                // Bottom of top half
                DrawLine(new(centre.X - x, centre.Y - y), new(centre.X + x, centre.Y - y), colour);

                // Top of bottom half
                DrawLine(new(centre.X - x, centre.Y + y), new(centre.X + x, centre.Y + y), colour);

                // Bottom of bottom half
                DrawLine(new(centre.X - y, centre.Y + x), new(centre.X + y, centre.Y + x), colour);

                //for (float i = y; i <= x; i++)
                //{
                //    _buffer[(int)(centre.X + i), (int)(centre.Y + y)] = (int)colour;
                //    _buffer[(int)(centre.X + i), (int)(centre.Y - y)] = (int)colour;
                //    _buffer[(int)(centre.X + y), (int)(centre.Y + i)] = (int)colour;
                //    _buffer[(int)(centre.X - y), (int)(centre.Y + i)] = (int)colour;
                //    _buffer[(int)(centre.X - i), (int)(centre.Y + y)] = (int)colour;
                //    _buffer[(int)(centre.X - i), (int)(centre.Y - y)] = (int)colour;
                //    _buffer[(int)(centre.X + y), (int)(centre.Y - i)] = (int)colour;
                //    _buffer[(int)(centre.X - y), (int)(centre.Y - i)] = (int)colour;
                //}
                if (error <= 0)
                {
                    y++;
                    error += ty;
                    ty += 2;
                }

                if (error > 0)
                {
                    x--;
                    tx += 2;
                    error += tx - diameter;
                }
            }
        }

        public void DrawImage(Image image, Vector2 position) => throw new NotImplementedException();

        public void DrawImageCentre(Image image, float y) => throw new NotImplementedException();

        public void DrawLine(Vector2 lineStart, Vector2 lineEnd, Colour colour)
        {
            float dx = MathF.Abs(lineStart.X - lineEnd.X);
            float dy = MathF.Abs(lineStart.Y - lineEnd.Y);
            int sx = lineStart.X < lineEnd.X ? 1 : -1;
            int sy = lineEnd.X < lineEnd.Y ? 1 : -1;
            float err = dx - dy;

            while (true)
            {
                _buffer[(int)lineStart.X, (int)lineStart.Y] = (int)colour;

                if ((int)lineStart.X == (int)lineEnd.X && (int)lineStart.Y == (int)lineEnd.Y)
                {
                    break;
                }

                float err2 = 2 * err;
                if (err2 > -dy)
                {
                    err -= dy;
                    lineStart.X += sx;
                }

                if (err2 < dx)
                {
                    err += dx;
                    lineStart.Y += sy;
                }
            }
        }

        public void DrawLine(Vector2 lineStart, Vector2 lineEnd) => DrawLine(lineStart, lineEnd, Colour.White);

        public void DrawPixel(Vector2 position, Colour colour) => _buffer[(int)position.X, (int)position.Y] = (int)colour;

        public void DrawPixelFast(Vector2 position, Colour colour) => _buffer[(int)position.X, (int)position.Y] = (int)colour;

        public void DrawPolygon(Vector2[] pointList, Colour lineColour) => throw new NotImplementedException();

        public void DrawPolygonFilled(Vector2[] pointList, Colour faceColour) => throw new NotImplementedException();

        public void DrawRectangle(Vector2 position, float width, float height, Colour colour) => throw new NotImplementedException();

        public void DrawRectangleCentre(float y, float width, float height, Colour colour) => throw new NotImplementedException();

        public void DrawRectangleFilled(Vector2 position, float width, float height, Colour colour) => throw new NotImplementedException();

        public void DrawTextCentre(float y, string text, FontSize fontSize, Colour colour) => throw new NotImplementedException();

        public void DrawTextLeft(Vector2 position, string text, Colour colour) => throw new NotImplementedException();

        public void DrawTextRight(float x, float y, string text, Colour colour) => throw new NotImplementedException();

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Colour colour) => throw new NotImplementedException();

        public void DrawTriangleFilled(Vector2 a, Vector2 b, Vector2 c, Colour colour) => throw new NotImplementedException();

        public void LoadBitmap(Image imgType, byte[] bitmapBytes) => throw new NotImplementedException();

        public void ScreenAcquire() => throw new NotImplementedException();

        public void ScreenRelease() => throw new NotImplementedException();

        public void ScreenUpdate() => throw new NotImplementedException();

        public void SetClipRegion(Vector2 position, float width, float height) => throw new NotImplementedException();
    }
}
