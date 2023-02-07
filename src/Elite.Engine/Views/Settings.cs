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

namespace Elite.Engine
{
	using Elite.Engine.Enums;

	internal class Settings : IView
	{
        private readonly IGfx _gfx;
        private readonly IKeyboard _keyboard;
        private int _highlightedItem;

        private readonly (string Name, string[] Values)[] setting_list = 
		{
			new("Graphics:", new [] {"Solid", "Wireframe", "", "", ""}),
			new("Anti Alias:", new [] {"Off", "On", "", "", ""}),
			new("Planet Style:", new [] {"Wireframe", "Green", "SNES", "Fractal", ""}),
			new("Planet Desc.:", new [] {"BBC", "MSX", "", "", ""}),
			new("Instant Dock:", new [] {"Off", "On", "", "", ""}),
			new("Save Settings", new [] {"", "", "", "", ""})
		};

        internal Settings(IGfx gfx, IKeyboard keyboard)
        {
            _gfx = gfx;
            _keyboard = keyboard;
        }

        public void Reset()
        {
			_highlightedItem = 0;
        }

        public void UpdateUniverse()
        {
        }

        public void Draw()
        {
            elite.draw.ClearDisplay();
            _gfx.DrawTextCentre(20, "GAME SETTINGS", 140, GFX_COL.GFX_COL_GOLD);
            _gfx.DrawLine(new(0f, 36f), new(511f, 36f));

            for (int i = 0; i < setting_list.Length; i++)
            {
                float x;
                int y;
                if (i == (setting_list.Length - 1))
                {
                    y = ((setting_list.Length + 1) / 2 * 30) + 96 + 32;
                    if (i == _highlightedItem)
                    {
                        x = gfx.GFX_X_CENTRE - 200;
                        _gfx.DrawRectangleFilled(x, y - 7, 400, 15, GFX_COL.GFX_COL_DARK_RED);
                    }

                    _gfx.DrawTextCentre(y, setting_list[i].Name, 120, GFX_COL.GFX_COL_WHITE);
                    return;
                }

                int v = i switch
                {
                    0 => elite.config.UseWireframe ? 1 : 0,
                    1 => elite.config.AntiAliasWireframe ? 1 : 0,
                    2 => (int)elite.config.PlanetRenderStyle,
                    3 => elite.config.PlanetDescriptions == PlanetDescriptions.HoopyCasinos ? 1 : 0,
                    4 => elite.config.InstantDock ? 1 : 0,
                    _ => 0,
                };
                x = ((i & 1) * 250) + 32;
                y = (i / 2 * 30) + 96;

                if (i == _highlightedItem)
                {
                    _gfx.DrawRectangleFilled(x, y, 100, 15, GFX_COL.GFX_COL_DARK_RED);
                }
                _gfx.DrawTextLeft(x, y, setting_list[i].Name, GFX_COL.GFX_COL_WHITE);
                _gfx.DrawTextLeft(x + 120, y, setting_list[i].Values[v], GFX_COL.GFX_COL_WHITE);
            }
        }

        public void HandleInput()
        {
            if (_keyboard.IsKeyPressed(CommandKey.Up))
            {
                SelectUp();
            }
            if (_keyboard.IsKeyPressed(CommandKey.Down))
            {
                SelectDown();
            }
            if (_keyboard.IsKeyPressed(CommandKey.Left))
            {
                SelectLeft();
            }
            if (_keyboard.IsKeyPressed(CommandKey.Right))
            {
                SelectRight();
            }
            if (_keyboard.IsKeyPressed(CommandKey.Enter))
            {
                ToggleSetting();
            }
        }

        private void SelectLeft()
        {
            if (_highlightedItem.IsOdd())
            {
                _highlightedItem--;
            }
        }

        private void SelectRight()
        {
            if (!_highlightedItem.IsOdd() && (_highlightedItem < (setting_list.Length - 1)))
            {
                _highlightedItem++;
            }
        }

        private void SelectUp()
        {
            if (_highlightedItem == (setting_list.Length - 1))
            {
                _highlightedItem = setting_list.Length - 2;
            }

            if (_highlightedItem > 1)
            {
                _highlightedItem -= 2;
            }
        }

        private void SelectDown()
        {
            if (_highlightedItem == (setting_list.Length - 2))
            {
                _highlightedItem = setting_list.Length - 1;
            }

            if (_highlightedItem < (setting_list.Length - 2))
            {
                _highlightedItem += 2;
            }
        }

        private void ToggleSetting()
        {
            if (_highlightedItem == (setting_list.Length - 1))
            {
                ConfigFile.WriteConfigAsync(elite.config);
                elite.SetView(SCR.SCR_OPTIONS);
                return;
            }

            switch (_highlightedItem)
            {
                case 0:
                    elite.config.UseWireframe = !elite.config.UseWireframe;
                    break;

                case 1:
                    elite.config.AntiAliasWireframe = !elite.config.AntiAliasWireframe;
                    break;

                case 2:
                    elite.config.PlanetRenderStyle = (PlanetRenderStyle)((int)(elite.config.PlanetRenderStyle + 1) % 4);
                    break;

                case 3:
                    elite.config.PlanetDescriptions = (PlanetDescriptions)((int)(elite.config.PlanetDescriptions + 1) % 2);
                    break;

                case 4:
                    elite.config.InstantDock = !elite.config.InstantDock;
                    break;
            }
        }
    }
}