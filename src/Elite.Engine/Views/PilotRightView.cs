﻿namespace Elite.Engine.Views
{
    using Elite.Engine.Ships;

    internal class PilotRightView : IView
    {
        private readonly PilotView _pilotView;
        private readonly GameState _gameState;
        private readonly Stars _stars;
        private readonly PlayerShip _ship;

        internal PilotRightView(GameState gameState, IGfx gfx, IKeyboard keyboard, Stars stars, pilot pilot, PlayerShip ship)
        {
            _pilotView = new(gameState, gfx, keyboard, pilot, ship);
            _gameState = gameState;
            _stars = stars;
            _ship = ship;
        }

        public void Draw()
        {
            _pilotView.Draw();
            _pilotView.DrawViewName("Right View");
            _pilotView.DrawLaserSights(_ship.laserFront.Type);
        }

        public void HandleInput()
        {
            _pilotView.HandleInput();
        }

        public void Reset()
        {
            _pilotView.Reset();
        }

        public void UpdateUniverse()
        {
            _pilotView.UpdateUniverse();
            _stars.RightStarfield();
        }
    }
}
