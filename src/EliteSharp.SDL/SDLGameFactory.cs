// 'Elite - The Sharp Kind' - Andy Hawkins 2023.
// 'Elite - The New Kind' - C.J.Pinder 1999-2001.
// Elite (C) I.Bell & D.Braben 1984.

using EliteSharp.Assets;
using EliteSharp.Audio;
using EliteSharp.Graphics;
using static SDL2.SDL;

namespace EliteSharp.SDL
{
    internal sealed class SDLGameFactory : IDisposable
    {
        private readonly IGraphics _graphics;
        private readonly SDLKeyboard _keyboard;
        private readonly int _screenHeight;
        private readonly int _screenWidth;
        private readonly ISound _sound;
        private readonly nint _renderer;
        private readonly nint _window;
        private bool _isDisposed;

        internal SDLGameFactory(int screenWidth, int screenHeight, string type = "SDL")
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _keyboard = new();

            if (type == "SOFTWARE")
            {
                (_renderer, _window) = SDLGraphics.CreateRenderer(_screenWidth, _screenHeight);

                SoftwareAssetLoader assetLoader = new(new SoftwareAssetLocator());
                _graphics = new SoftwareGraphics(
                    _screenWidth,
                    _screenHeight,
                    assetLoader,
                    SoftwareScreenUpdate);
                _sound = new SoftwareSound(assetLoader);
            }
            else
            {
                // When running C# applications under the Visual Studio debugger, native code that
                // names threads with the 0x406D1388 exception will silently exit. To prevent this
                // exception from being thrown by SDL, add this line before your SDL_Init call:
                SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
                SDLAssetLoader assetLoader = new(new AssetLocator());
                _graphics = new SDLGraphics(_screenWidth, _screenHeight, assetLoader);
                _sound = new SDLSound(assetLoader);
            }

            Game = new(_graphics, _sound, _keyboard);
        }

        internal EliteMain Game { get; }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _graphics?.Dispose();
                    _sound?.Dispose();

                    SDL_DestroyRenderer(_renderer);
                    SDL_DestroyWindow(_window);
                    SDL_Quit();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _isDisposed = true;
            }
        }

        private void SoftwareScreenUpdate(FastBitmap bitmap)
        {
            IntPtr surface = BitConverter.IsLittleEndian
                ? SDL_CreateRGBSurfaceFrom(
                    bitmap.BitmapHandle,
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.BitsPerPixel,
                    bitmap.Width * 4,
                    0x00FF0000,
                    0x0000FF00,
                    0x000000FF,
                    0xFF000000)
                : SDL_CreateRGBSurfaceFrom(
                    bitmap.BitmapHandle,
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.BitsPerPixel,
                    bitmap.Width * 4,
                    0x0000FF00,
                    0x00FF0000,
                    0xFF000000,
                    0x000000FF);

            if (surface == IntPtr.Zero)
            {
                SDLHelper.Throw(nameof(SDL_CreateRGBSurfaceFrom));
            }

            IntPtr texture = SDL_CreateTextureFromSurface(_renderer, surface);
            if (texture == IntPtr.Zero)
            {
                SDLHelper.Throw(nameof(SDL_CreateTextureFromSurface));
            }

            SDL_FreeSurface(surface);

            SDL_Rect dest = new()
            {
                x = 0,
                y = 0,
                w = bitmap.Width,
                h = bitmap.Height,
            };

            if (SDL_RenderCopy(_renderer, texture, nint.Zero, ref dest) < 0)
            {
                SDLHelper.Throw(nameof(SDL_RenderCopy));
            }

            SDL_DestroyTexture(texture);

            SDL_RenderPresent(_renderer);
        }
    }
}
