﻿// 'Elite - The Sharp Kind' - Andy Hawkins 2023.
// 'Elite - The New Kind' - C.J.Pinder 1999-2001.
// Elite (C) I.Bell & D.Braben 1984.

using Elite.Engine.Enums;
using Elite.Engine.Trader;

namespace Elite.Engine.Views
{
    internal sealed class MarketView : IView
    {
        private readonly Draw _draw;
        private readonly GameState _gameState;
        private readonly IGraphics _graphics;
        private readonly IKeyboard _keyboard;
        private readonly Planet _planet;
        private readonly Trade _trade;
        private StockType _highlightedStock;

        internal MarketView(GameState gameState, IGraphics graphics, Draw draw, IKeyboard keyboard, Trade trade, Planet planet)
        {
            _gameState = gameState;
            _graphics = graphics;
            _draw = draw;
            _keyboard = keyboard;
            _trade = trade;
            _planet = planet;
        }

        public void Draw()
        {
            _draw.ClearDisplay();
            _draw.DrawViewHeader($"{_planet.NamePlanet(_gameState.DockedPlanet, false)} MARKET PRICES");

            _graphics.DrawTextLeft(16, 40, "PRODUCT", Colour.Green1);
            _graphics.DrawTextLeft(166, 40, "UNIT", Colour.Green1);
            _graphics.DrawTextLeft(246, 40, "PRICE", Colour.Green1);
            _graphics.DrawTextLeft(314, 40, "FOR SALE", Colour.Green1);
            _graphics.DrawTextLeft(420, 40, "IN HOLD", Colour.Green1);

            int i = 0;
            foreach (KeyValuePair<StockType, StockItem> stock in _trade._stockMarket)
            {
                int y = (i * 15) + 55;

                if (stock.Key == _highlightedStock)
                {
                    _graphics.DrawRectangleFilled(2, y, 508, 15, Colour.Red2);
                }

                _graphics.DrawTextLeft(16, y, stock.Value.Name, Colour.White1);

                _graphics.DrawTextLeft(180, y, stock.Value.Units, Colour.White1);

                _graphics.DrawTextRight(285, y, $"{stock.Value.CurrentPrice:N1}", Colour.White1);

                _graphics.DrawTextRight(365, y, stock.Value.CurrentQuantity > 0 ? $"{stock.Value.CurrentQuantity}" : "-", Colour.White1);
                _graphics.DrawTextLeft(365, y, stock.Value.CurrentQuantity > 0 ? stock.Value.Units : string.Empty, Colour.White1);

                _graphics.DrawTextRight(455, y, stock.Value.CurrentCargo > 0 ? $"{stock.Value.CurrentCargo,2}" : "-", Colour.White1);
                _graphics.DrawTextLeft(455, y, stock.Value.CurrentCargo > 0 ? stock.Value.Units : string.Empty, Colour.White1);

                i++;
            }

            _graphics.DrawTextLeft(16, 340, "Cash:", Colour.Green1);
            _graphics.DrawTextRight(160, 340, $"{_trade._credits,10:N1} Credits", Colour.White1);
        }

        public void HandleInput()
        {
            if (_keyboard.IsKeyPressed(CommandKey.Up, CommandKey.UpArrow))
            {
                _highlightedStock = (StockType)Math.Clamp((int)_highlightedStock - 1, 0, _trade._stockMarket.Count - 1);
            }

            if (_keyboard.IsKeyPressed(CommandKey.Down, CommandKey.DownArrow))
            {
                _highlightedStock = (StockType)Math.Clamp((int)_highlightedStock + 1, 0, _trade._stockMarket.Count - 1);
            }

            if (_keyboard.IsKeyPressed(CommandKey.Left, CommandKey.LeftArrow))
            {
                _trade.SellStock(_highlightedStock);
            }

            if (_keyboard.IsKeyPressed(CommandKey.Right, CommandKey.RightArrow))
            {
                _trade.BuyStock(_highlightedStock);
            }
        }

        public void Reset() => _highlightedStock = 0;

        public void UpdateUniverse()
        {
        }
    }
}
