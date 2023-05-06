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

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elite.Engine.Enums;
using Elite.Engine.Ships;
using Elite.Engine.Types;

namespace Elite.Engine.Save
{
    internal class SaveFile
    {
        private const string fileExtension = ".cmdr";
        private readonly GameState _state;
        private readonly PlayerShip _ship;
        private readonly Trade _trade;
        private readonly Planet _planet;
        private SaveState _lastSaved;

        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        internal SaveFile(GameState state, PlayerShip ship, Trade trade, Planet planet)
        {
            _state = state;
            _ship = ship;
            _trade = trade;
            _planet = planet;

#if DEBUG
            _lastSaved = CommanderFactory.Max();
#else
		    _lastSaved = CommanderFactory.Jameson();
#endif
        }

        /// <summary>
        /// Write the save file.
        /// </summary>
        /// <param name="state">The game state to save.</param>
        internal async Task<bool> SaveCommanderAsync(string newName)
        {
            try
            {
                SaveState save = GameStateToSaveState(newName);

                string path = save.CommanderName + fileExtension;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using FileStream stream = File.OpenWrite(path);
                await JsonSerializer.SerializeAsync(stream, save, options);

                _lastSaved = save;
                return true;
            }
            catch (Exception ex)
            {
                //TODO: handle error message better
                Debug.WriteLine("Failed to save commander.\n" + ex);
                return false;
            }
        }

        /// <summary>
        /// Read the save file.
        /// </summary>
        internal async Task<bool> LoadCommanderAsync(string name)
        {
            try
            {
                using FileStream stream = File.OpenRead(name + fileExtension);
                SaveState? save = await JsonSerializer.DeserializeAsync<SaveState>(stream, options);
                if (save != null)
                {
                    _lastSaved = save;
                    SaveStateToGameState();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //TODO: handle error message better
                Debug.WriteLine("Failed to load commander.\n" + ex);
                _lastSaved = CommanderFactory.Jameson();
            }

            return false;
        }

        internal void GetLastSave()
        {
            SaveStateToGameState();
            RestoreSavedCommander();
        }

        private void RestoreSavedCommander()
        {
            _state.DockedPlanet = _planet.FindPlanet(_state.Cmdr.Galaxy, new(_state.DockedPlanet.D, _state.DockedPlanet.B));
            _state.PlanetName = _planet.NamePlanet(_state.DockedPlanet, false);
            _state.HyperspacePlanet = (GalaxySeed)_state.DockedPlanet.Clone();
            _state.CurrentPlanetData = Planet.GeneratePlanetData(_state.DockedPlanet);
            _trade.GenerateStockMarket(_state.CurrentPlanetData);
            _trade.SetStockQuantities();
        }

        private SaveState GameStateToSaveState(string newName)
        {
            SaveState save = new()
            {
                CargoCapacity = _ship.cargoCapacity,
                CommanderName = newName,
                Credits = _trade.credits,
                CurrentCargo = _trade.stockMarket.Values.Select(x => x.currentCargo).ToArray(),
                EnergyUnit = _ship.energyUnit.ToString(),
                Fuel = _ship.fuel,
                GalaxyNumber = _state.Cmdr.GalaxyNumber,
                GalaxySeed = new int[6]
                {
                    _state.Cmdr.Galaxy.A,
                    _state.Cmdr.Galaxy.B,
                    _state.Cmdr.Galaxy.C,
                    _state.Cmdr.Galaxy.D,
                    _state.Cmdr.Galaxy.E,
                    _state.Cmdr.Galaxy.F
                },
                HasDockingComputer = _ship.hasDockingComputer,
                HasECM = _ship.hasECM,
                HasEnergyBomb = _ship.hasEnergyBomb,
                HasEscapePod = _ship.hasEscapePod,
                HasFuelScoop = _ship.hasFuelScoop,
                HasGalacticHyperdrive = _ship.hasGalacticHyperdrive,
                Lasers = new string[4]
                {
                    _ship.laserFront.Type.ToString(),
                    _ship.laserRear.Type.ToString(),
                    _ship.laserRight.Type.ToString(),
                    _ship.laserLeft.Type.ToString()
                },
                LegalStatus = _state.Cmdr.LegalStatus,
                MarketRandomiser = _trade.marketRandomiser,
                Missiles = _ship.missileCount,
                Mission = _state.Cmdr.Mission,
                Saved = _state.Cmdr.Saved,
                Score = _state.Cmdr.Score,
                ShipLocation = new int[2]
                {
                    _state.DockedPlanet.D,
                    _state.DockedPlanet.B,
                },
                StationStock = _trade.stockMarket.Values.Select(x => x.stationStock).ToArray()
            };

            return save;
        }

        private void SaveStateToGameState()
        {
            _ship.cargoCapacity = _lastSaved.CargoCapacity;
            _state.Cmdr.Name = _lastSaved.CommanderName;
            _trade.credits = _lastSaved.Credits;
            for (int i = 0; i < _trade.stockMarket.Count; i++)
            {
                _trade.stockMarket[(StockType)i + 1].currentCargo = _lastSaved.CurrentCargo[i];
            }
            _ship.energyUnit = Enum.Parse<EnergyUnit>(_lastSaved.EnergyUnit);
            _ship.fuel = _lastSaved.Fuel;
            _state.Cmdr.GalaxyNumber = _lastSaved.GalaxyNumber;
            _state.Cmdr.Galaxy.A = _lastSaved.GalaxySeed[0];
            _state.Cmdr.Galaxy.B = _lastSaved.GalaxySeed[1];
            _state.Cmdr.Galaxy.C = _lastSaved.GalaxySeed[2];
            _state.Cmdr.Galaxy.D = _lastSaved.GalaxySeed[3];
            _state.Cmdr.Galaxy.E = _lastSaved.GalaxySeed[4];
            _state.Cmdr.Galaxy.F = _lastSaved.GalaxySeed[5];
            _ship.hasDockingComputer = _lastSaved.HasDockingComputer;
            _ship.hasECM = _lastSaved.HasECM;
            _ship.hasEnergyBomb = _lastSaved.HasEnergyBomb;
            _ship.hasEscapePod = _lastSaved.HasEscapePod;
            _ship.hasFuelScoop = _lastSaved.HasFuelScoop;
            _ship.hasGalacticHyperdrive = _lastSaved.HasGalacticHyperdrive;
            _ship.laserFront = LaserFactory.GetLaser(Enum.Parse<LaserType>(_lastSaved.Lasers[0]));
            _ship.laserRear = LaserFactory.GetLaser(Enum.Parse<LaserType>(_lastSaved.Lasers[1]));
            _ship.laserRight = LaserFactory.GetLaser(Enum.Parse<LaserType>(_lastSaved.Lasers[2]));
            _ship.laserLeft = LaserFactory.GetLaser(Enum.Parse<LaserType>(_lastSaved.Lasers[3]));
            _state.Cmdr.LegalStatus = _lastSaved.LegalStatus;
            _trade.marketRandomiser = _lastSaved.MarketRandomiser;
            _ship.missileCount = _lastSaved.Missiles;
            _state.Cmdr.Mission = _lastSaved.Mission;
            _state.Cmdr.Saved = _lastSaved.Saved;
            _state.Cmdr.Score = _lastSaved.Score;
            _state.DockedPlanet.D = _lastSaved.ShipLocation[0];
            _state.DockedPlanet.B = _lastSaved.ShipLocation[1];
            for (int i = 0; i < _trade.stockMarket.Count; i++)
            {
                _trade.stockMarket[(StockType)i + 1].stationStock = _lastSaved.StationStock[i];
            }
        }

        //static int checksum(unsigned char* block)
        //{
        //	int acc, carry;
        //	int i;

        //	acc = 0x49;
        //	carry = 0;
        //	for (i = 0x49; i > 0; i--)
        //	{
        //		acc += block[i - 1] + carry;
        //		carry = acc >> 8;
        //		acc &= 255;
        //		acc ^= block[i];
        //	}

        //	return acc;
        //}

    }
}
