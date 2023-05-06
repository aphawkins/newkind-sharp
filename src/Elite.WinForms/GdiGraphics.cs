// 'Elite - The Sharp Kind' - Andy Hawkins 2023.
// 'Elite - The New Kind' - C.J.Pinder 1999-2001.
// Elite (C) I.Bell & D.Braben 1984.

using System.Diagnostics;
using System.Numerics;
using Elite.Engine;
using Elite.Engine.Enums;

namespace Elite.WinForms
{
    public class GdiGraphics : IGfx, IDisposable
    {
        private readonly Dictionary<GFX_COL, Brush> _brushes = new()
            {
                { GFX_COL.GFX_COL_BLACK, Brushes.Black },
                { GFX_COL.GFX_COL_WHITE, Brushes.White },
                { GFX_COL.GFX_COL_WHITE_2, Brushes.WhiteSmoke },
                { GFX_COL.GFX_COL_CYAN, Brushes.Cyan },
                { GFX_COL.GFX_COL_GREY_1, Brushes.LightGray },
                { GFX_COL.GFX_COL_GREY_2, Brushes.DimGray },
                { GFX_COL.GFX_COL_GREY_3, Brushes.Gray },
                { GFX_COL.GFX_COL_GREY_4, Brushes.DarkGray },
                { GFX_COL.GFX_COL_BLUE_1, Brushes.DarkBlue },
                { GFX_COL.GFX_COL_BLUE_2, Brushes.Blue },
                { GFX_COL.GFX_COL_BLUE_3, Brushes.MediumBlue },
                { GFX_COL.GFX_COL_BLUE_4, Brushes.LightBlue },
                { GFX_COL.GFX_COL_RED, Brushes.Red },
                { GFX_COL.GFX_COL_RED_3, Brushes.PaleVioletRed },
                { GFX_COL.GFX_COL_RED_4, Brushes.MediumVioletRed },
                { GFX_COL.GFX_COL_DARK_RED, Brushes.DarkRed },
                { GFX_COL.GFX_COL_YELLOW_1, Brushes.Goldenrod },
                { GFX_COL.GFX_COL_GOLD, Brushes.Gold },
                { GFX_COL.GFX_COL_YELLOW_3, Brushes.Yellow },
                { GFX_COL.GFX_COL_YELLOW_4, Brushes.LightYellow },
                { GFX_COL.GFX_COL_YELLOW_5, Brushes.LightGoldenrodYellow },
                { GFX_COL.GFX_COL_ORANGE_1, Brushes.DarkOrange },
                { GFX_COL.GFX_COL_ORANGE_2, Brushes.OrangeRed },
                { GFX_COL.GFX_COL_ORANGE_3, Brushes.Orange },
                { GFX_COL.GFX_COL_GREEN_1, Brushes.DarkGreen },
                { GFX_COL.GFX_COL_GREEN_2, Brushes.Green },
                { GFX_COL.GFX_COL_GREEN_3, Brushes.LightGreen },
                { GFX_COL.GFX_COL_PINK_1, Brushes.Pink },
            };

        private readonly Font _fontLarge = new("Arial", 18, FontStyle.Bold, GraphicsUnit.Pixel);

        // Fonts
        private readonly Font _fontSmall = new("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);

        // Images
        private readonly Dictionary<Common.Enums.Image, Bitmap> _images = new();

        private readonly Dictionary<GFX_COL, Pen> _pens = new()
            {
                { GFX_COL.GFX_COL_BLACK, Pens.Black },
                { GFX_COL.GFX_COL_WHITE, Pens.White },
                { GFX_COL.GFX_COL_WHITE_2, Pens.WhiteSmoke },
                { GFX_COL.GFX_COL_CYAN, Pens.Cyan },
                { GFX_COL.GFX_COL_GREY_1, Pens.LightGray },
                { GFX_COL.GFX_COL_GREY_2, Pens.DimGray },
                { GFX_COL.GFX_COL_GREY_3, Pens.Gray },
                { GFX_COL.GFX_COL_GREY_4, Pens.DarkGray },
                { GFX_COL.GFX_COL_BLUE_1, Pens.DarkBlue },
                { GFX_COL.GFX_COL_BLUE_2, Pens.Blue },
                { GFX_COL.GFX_COL_BLUE_3, Pens.MediumBlue },
                { GFX_COL.GFX_COL_BLUE_4, Pens.LightBlue },
                { GFX_COL.GFX_COL_RED, Pens.Red },
                { GFX_COL.GFX_COL_RED_3, Pens.PaleVioletRed },
                { GFX_COL.GFX_COL_RED_4, Pens.MediumVioletRed },
                { GFX_COL.GFX_COL_DARK_RED, Pens.DarkRed },
                { GFX_COL.GFX_COL_YELLOW_1, Pens.Goldenrod },
                { GFX_COL.GFX_COL_GOLD, Pens.Gold },
                { GFX_COL.GFX_COL_YELLOW_3, Pens.Yellow },
                { GFX_COL.GFX_COL_YELLOW_4, Pens.LightYellow },
                { GFX_COL.GFX_COL_YELLOW_5, Pens.LightGoldenrodYellow },
                { GFX_COL.GFX_COL_ORANGE_1, Pens.DarkOrange },
                { GFX_COL.GFX_COL_ORANGE_2, Pens.OrangeRed },
                { GFX_COL.GFX_COL_ORANGE_3, Pens.Orange },
                { GFX_COL.GFX_COL_GREEN_1, Pens.DarkGreen },
                { GFX_COL.GFX_COL_GREEN_2, Pens.Green },
                { GFX_COL.GFX_COL_GREEN_3, Pens.LightGreen },
                { GFX_COL.GFX_COL_PINK_1, Pens.Pink },
            };

        // Actual screen
        private readonly Bitmap _screen;

        // Screen buffer
        private readonly Bitmap _screenBuffer;
        private readonly System.Drawing.Graphics _screenBufferGraphics;
        private readonly System.Drawing.Graphics _screenGraphics;
        private bool _isDisposed;

        //private volatile int frame_count;
        //private readonly object frameCountLock = new();
        //private readonly System.Windows.Forms.Timer _frameTimer;
        public GdiGraphics(ref Bitmap screen)
        {
            Debug.Assert(screen.Width == 512);
            Debug.Assert(screen.Height == 512);
            Debug.Assert(_pens.Count == Enum.GetNames(typeof(GFX_COL)).Length);
            Debug.Assert(_brushes.Count == Enum.GetNames(typeof(GFX_COL)).Length);

            _screen = screen;
            _screenGraphics = System.Drawing.Graphics.FromImage(_screen);
            _screenBuffer = new Bitmap(screen.Width, screen.Height);
            _screenBufferGraphics = System.Drawing.Graphics.FromImage(_screenBuffer);
            _screenBufferGraphics.Clear(Color.Black);
        }
        public void ClearArea(float x, float y, float width, float height) => _screenBufferGraphics.FillRectangle(Brushes.Black, x + Engine.Graphics.GFX_X_OFFSET, y + Engine.Graphics.GFX_Y_OFFSET, width + Engine.Graphics.GFX_X_OFFSET, height + Engine.Graphics.GFX_Y_OFFSET);

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public virtual void DrawCircle(Vector2 centre, float radius, GFX_COL colour) => _screenBufferGraphics.DrawEllipse(_pens[colour], centre.X + Engine.Graphics.GFX_X_OFFSET - radius, centre.Y + Engine.Graphics.GFX_Y_OFFSET - radius, 2 * radius, 2 * radius);

        public void DrawCircleFilled(Vector2 centre, float radius, GFX_COL colour) => _screenBufferGraphics.FillEllipse(_brushes[colour], centre.X + Engine.Graphics.GFX_X_OFFSET - radius, centre.Y + Engine.Graphics.GFX_Y_OFFSET - radius, 2 * radius, 2 * radius);

        public void DrawImage(Common.Enums.Image image, Vector2 position)
        {
            Bitmap sprite = _images[image];

            if (position.X < 0)
            {
                position.X = ((256 * Engine.Graphics.GFX_SCALE) - sprite.Width) / 2;
            }

            _screenBufferGraphics.DrawImage(sprite, position.X + Engine.Graphics.GFX_X_OFFSET, position.Y + Engine.Graphics.GFX_Y_OFFSET);
        }

        public virtual void DrawLine(Vector2 start, Vector2 end) => _screenBufferGraphics.DrawLine(_pens[GFX_COL.GFX_COL_WHITE], start.X + Engine.Graphics.GFX_X_OFFSET, start.Y + Engine.Graphics.GFX_Y_OFFSET, end.X + Engine.Graphics.GFX_X_OFFSET, end.Y + Engine.Graphics.GFX_Y_OFFSET);

        public void DrawLine(Vector2 start, Vector2 end, GFX_COL line_colour) => _screenBufferGraphics.DrawLine(_pens[line_colour], start.X + Engine.Graphics.GFX_X_OFFSET, start.Y + Engine.Graphics.GFX_Y_OFFSET, end.X + Engine.Graphics.GFX_X_OFFSET, end.Y + Engine.Graphics.GFX_Y_OFFSET);

        public void DrawPixel(Vector2 position, GFX_COL col)
        {
            //TODO: Fix SNES planet colour issues
            Color colour = _pens.TryGetValue(col, out Pen? value) ? value.Color : Color.Magenta;

            Debug.Assert(colour != Color.Magenta);

            //TODO: fix bad values from explosion
            if (position.X < 0 || position.X >= 512 || position.Y < 0 || position.Y >= 512)
            {
                return;
            }

            _screenBuffer.SetPixel((int)(position.X + Engine.Graphics.GFX_X_OFFSET), (int)(position.Y + Engine.Graphics.GFX_Y_OFFSET), colour);
        }

        public void DrawPixelFast(Vector2 position, GFX_COL col) =>
            // Is there a faster way of doing this?
            DrawPixel(position, col);

        public void DrawPolygon(Vector2[] vectors, GFX_COL lineColour)
        {
            PointF[] points = new PointF[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
            {
                points[i] = new PointF(vectors[i].X + Engine.Graphics.GFX_X_OFFSET, vectors[i].Y + Engine.Graphics.GFX_Y_OFFSET);
            }

            _screenBufferGraphics.DrawPolygon(_pens[lineColour], points);
        }

        public void DrawPolygonFilled(Vector2[] vectors, GFX_COL faceColour)
        {
            PointF[] points = new PointF[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
            {
                points[i] = new PointF(vectors[i].X + Engine.Graphics.GFX_X_OFFSET, vectors[i].Y + Engine.Graphics.GFX_Y_OFFSET);
            }

            _screenBufferGraphics.FillPolygon(_brushes[faceColour], points);
        }

        public void DrawRectangle(float x, float y, float width, float height, GFX_COL colour) => _screenBufferGraphics.DrawRectangle(_pens[colour], x + Engine.Graphics.GFX_X_OFFSET, y + Engine.Graphics.GFX_Y_OFFSET, width + Engine.Graphics.GFX_X_OFFSET, height + Engine.Graphics.GFX_Y_OFFSET);

        public void DrawRectangleFilled(float x, float y, float width, float height, GFX_COL colour) => _screenBufferGraphics.FillRectangle(_brushes[colour], x + Engine.Graphics.GFX_X_OFFSET, y + Engine.Graphics.GFX_Y_OFFSET, width + Engine.Graphics.GFX_X_OFFSET, height + Engine.Graphics.GFX_Y_OFFSET);

        public void DrawTextCentre(float y, string text, int psize, GFX_COL colour)
        {
            StringFormat stringFormat = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            PointF point = new((128 * Engine.Graphics.GFX_SCALE) + Engine.Graphics.GFX_X_OFFSET, (y / (2 / Engine.Graphics.GFX_SCALE)) + Engine.Graphics.GFX_Y_OFFSET);
            _screenBufferGraphics.DrawString(
                text,
                psize == 140 ? _fontLarge : _fontSmall,
                _brushes[colour],
                point,
                stringFormat);
        }

        public void DrawTextLeft(float x, float y, string text, GFX_COL colour)
        {
            PointF point = new((x / (2 / Engine.Graphics.GFX_SCALE)) + Engine.Graphics.GFX_X_OFFSET, (y / (2 / Engine.Graphics.GFX_SCALE)) + Engine.Graphics.GFX_Y_OFFSET);
            _screenBufferGraphics.DrawString(text, _fontSmall, _brushes[colour], point);
        }

        public void DrawTextRight(float x, float y, string text, GFX_COL colour)
        {
            StringFormat stringFormat = new()
            {
                Alignment = StringAlignment.Far,
            };

            PointF point = new((x / (2 / Engine.Graphics.GFX_SCALE)) + Engine.Graphics.GFX_X_OFFSET, (y / (2 / Engine.Graphics.GFX_SCALE)) + Engine.Graphics.GFX_Y_OFFSET);
            _screenBufferGraphics.DrawString(text, _fontSmall, _brushes[colour], point, stringFormat);
        }

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, GFX_COL colour)
        {
            PointF[] points = new PointF[3]
            {
                new(a.X += Engine.Graphics.GFX_X_OFFSET, a.Y += Engine.Graphics.GFX_Y_OFFSET),
                new(b.X += Engine.Graphics.GFX_X_OFFSET, b.Y += Engine.Graphics.GFX_Y_OFFSET),
                new(c.X += Engine.Graphics.GFX_X_OFFSET, c.Y += Engine.Graphics.GFX_Y_OFFSET)
            };

            _screenBufferGraphics.DrawLines(_pens[colour], points);
        }

        public void DrawTriangleFilled(Vector2 a, Vector2 b, Vector2 c, GFX_COL colour)
        {
            PointF[] points = new PointF[3]
            {
                new(a.X += Engine.Graphics.GFX_X_OFFSET, a.Y += Engine.Graphics.GFX_Y_OFFSET),
                new(b.X += Engine.Graphics.GFX_X_OFFSET, b.Y += Engine.Graphics.GFX_Y_OFFSET),
                new(c.X += Engine.Graphics.GFX_X_OFFSET, c.Y += Engine.Graphics.GFX_Y_OFFSET)
            };

            _screenBufferGraphics.FillPolygon(_brushes[colour], points);
        }

        public void LoadBitmap(Common.Enums.Image imgType, Stream bitmapStream) => _images[imgType] = (Bitmap)Image.FromStream(bitmapStream);

        public void ScreenAcquire()
        {
            //acquire_bitmap(gfx_screen);
        }

        public void ScreenRelease()
        {
            //release_bitmap(gfx_screen);
        }

        /// <summary>
        /// Blit the back buffer to the screen.
        /// </summary>
		public void ScreenUpdate()
        {
            // TODO: find a better way of doing multithreading
            Application.DoEvents();

            lock (_screen)
            {
                _screenGraphics.DrawImage(_screenBuffer, Engine.Graphics.GFX_X_OFFSET, Engine.Graphics.GFX_Y_OFFSET);
            }

            Application.DoEvents();
        }
        public void SetClipRegion(float x, float y, float width, float height) => _screenBufferGraphics.Clip = new Region(new RectangleF(x + Engine.Graphics.GFX_X_OFFSET, y + Engine.Graphics.GFX_Y_OFFSET, width + Engine.Graphics.GFX_X_OFFSET, height + Engine.Graphics.GFX_Y_OFFSET));
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _screenBufferGraphics.Dispose();
                    _screenBuffer.Dispose();
                    _screenGraphics.Dispose();
                    _screen.Dispose();
                    _fontSmall.Dispose();
                    _fontLarge.Dispose();

                    // Images
                    foreach (KeyValuePair<Common.Enums.Image, Bitmap> image in _images)
                    {
                        image.Value.Dispose();
                    }
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~alg_gfx()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
    }
}
