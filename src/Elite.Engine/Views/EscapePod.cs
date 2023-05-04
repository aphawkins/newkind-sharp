﻿namespace Elite.Engine.Views
{
    using System.Numerics;
    using Elite.Common.Enums;
    using Elite.Engine.Enums;
    using Elite.Engine.Ships;

    internal class EscapePod : IView
    {
        private readonly GameState _gameState;
        private readonly IGfx _gfx;
        private readonly Audio _audio;
        private readonly Stars _stars;
        private readonly PlayerShip _ship;
        private readonly Trade _trade;
        private readonly Combat _combat;

        private int _newship;
        private int _i;

        internal EscapePod(GameState gameState, IGfx gfx, Audio audio, Stars stars, PlayerShip ship, Trade trade, Combat combat)
        {
            _gameState = gameState;
            _gfx = gfx;
            _audio = audio;
            _stars = stars;
            _ship = ship;
            _trade = trade;
            _combat = combat;
        }

        public void Draw()
        {
            if (_i < 90)
            {
                _gfx.DrawTextCentre(358, "Escape pod launched - Ship auto-destuct initiated.", 120, GFX_COL.GFX_COL_WHITE);
            }
        }

        public void HandleInput()
        {
        }

        public void Reset()
        {
            _ship.speed = 1;
            _ship.roll = 0;
            _ship.climb = 0;
            Vector3[] rotmat = VectorMaths.GetInitialMatrix();
            rotmat[2].Z = 1;
            _newship = _combat.AddNewShip(ShipType.CobraMk3, new(0, 0, 200), rotmat, -127, -127);
            Space.universe[_newship].velocity = 7;
            _audio.PlayEffect(SoundEffect.Launch);
            _i = 0;
        }

        public void UpdateUniverse()
        {
            if (_i < 90)
            {
                if (_i == 40)
                {
                    Space.universe[_newship].flags |= FLG.FLG_DEAD;
                    _audio.PlayEffect(SoundEffect.Explode);
                }
                
                _stars.FrontStarfield();
                Space.universe[_newship].location.X = 0;
                Space.universe[_newship].location.Y = 0;
                Space.universe[_newship].location.Z += 2;
                _i++;
            }
            else if ((Space.ship_count[ShipType.Coriolis] == 0) && (Space.ship_count[ShipType.Dodec] == 0))
            {
                _ship.AutoDock();

                if ((MathF.Abs(_ship.roll) < 3) && (MathF.Abs(_ship.climb) < 3))
                {
                    for (int i = 0; i < EliteMain.MAX_UNIV_OBJECTS; i++)
                    {
                        if (Space.universe[i].type != 0)
                        {
                            Space.universe[i].location.Z -= 1500;
                        }
                    }
                }

                Stars.warp_stars = true;
                _stars.FrontStarfield();
            }
            else
            {
                _ship.hasEscapePod = false;
                _gameState.cmdr.LegalStatus = 0;
                _ship.fuel = _ship.maxFuel;
                _trade.ClearCurrentCargo();
                _gameState.SetView(SCR.SCR_DOCKING);
            }
        }
    }
}