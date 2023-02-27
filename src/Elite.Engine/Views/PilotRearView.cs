﻿namespace Elite.Engine.Views
{
    using Elite.Engine.Enums;

    internal class PilotRearView : PilotView
    {
        private readonly Stars _stars;

        internal PilotRearView(IGfx gfx, IKeyboard keyboard, Stars stars) : base(gfx, keyboard)
        {
            _stars = stars;
        }

        public override void Draw()
        {
            base.Draw();
            DrawViewName("Rear View");
            DrawLaserSights(elite.cmdr.front_laser);
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void UpdateUniverse()
        {
            base.UpdateUniverse();
            _stars.rear_starfield();
        }
    }
}
