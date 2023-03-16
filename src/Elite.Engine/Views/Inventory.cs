/*
 * Elite - The New Kind.
 *
 * Reverse engineered from the BBC disk version of Elite.
 * Additional material by C.J.Pinder.
 *
 * The original Elite code is (C) I.Bell & D.Braben 1984.
 * This version re-engineered in C by C.J.Pinder 1999-2001.
 *
 * email: <christian@newkind.co.uk>
 *
 *
 */

namespace Elite.Engine.Views
{
    using Elite.Engine.Enums;

    internal class Inventory : IView
    {
        private readonly GameState _gameState;
        private readonly IGfx _gfx;
        private readonly Draw _draw;

        internal Inventory(GameState gameState, IGfx gfx, Draw draw)
        {
            _gameState = gameState;
            _gfx = gfx;
            _draw = draw;
        }

        public void Draw()
        {
            _draw.ClearDisplay();
            _draw.DrawViewHeader("INVENTORY");

            _gfx.DrawTextLeft(16, 50, "Fuel:", GFX_COL.GFX_COL_GREEN_1);
            _gfx.DrawTextLeft(70, 50, $"{_gameState.cmdr.fuel:N1} Light Years", GFX_COL.GFX_COL_WHITE);

            _gfx.DrawTextLeft(16, 66, "Cash:", GFX_COL.GFX_COL_GREEN_1);
            _gfx.DrawTextLeft(70, 66, $"{_gameState.cmdr.credits:N1} Credits", GFX_COL.GFX_COL_WHITE);

            int y = 98;
            for (int i = 0; i < _gameState.cmdr.current_cargo.Length; i++)
            {
                if (_gameState.cmdr.current_cargo[i] > 0)
                {
                    _gfx.DrawTextLeft(16, y, _gameState.stock_market[i].name, GFX_COL.GFX_COL_WHITE);
                    _gfx.DrawTextLeft(180, y, $"{_gameState.cmdr.current_cargo[i]}{_gameState.stock_market[i].units}", GFX_COL.GFX_COL_WHITE);
                    y += 16;
                }
            }
        }

        public void HandleInput()
        {
        }

        public void Reset()
        {
        }

        public void UpdateUniverse()
        {
        }
    }
}