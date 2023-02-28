﻿namespace Elite.Engine.Views
{
    using Elite.Common.Enums;
    using Elite.Engine.Enums;

    internal class Hyperspace : IView
    {
        private readonly GameState _gameState;
        private readonly IGfx _gfx;
        private readonly Audio _audio;
        private readonly BreakPattern _breakPattern;

        internal Hyperspace(GameState gameState, IGfx gfx, Audio audio)
        {
            _gameState = gameState;
            _gfx = gfx;
            _audio = audio;
            _breakPattern = new(_gfx);
        }

        public void Draw()
        {
            _breakPattern.Draw();
        }

        public void HandleInput()
        {
        }

        public void Reset()
        {
            _breakPattern.Reset();
            _audio.PlayEffect(SoundEffect.Hyperspace);
        }

        public void UpdateUniverse()
        {
            _breakPattern.Update();

            if (_breakPattern.IsComplete)
            {
                _gameState.SetView(SCR.SCR_FRONT_VIEW);
            }
        }
    }
}