﻿// 'Elite - The Sharp Kind' - Andy Hawkins 2023.
// 'Elite - The New Kind' - C.J.Pinder 1999-2001.
// Elite (C) I.Bell & D.Braben 1984.

using System.Diagnostics;
using System.Numerics;
using Elite.Engine.Enums;
using Elite.Engine.Lasers;
using Elite.Engine.Types;

namespace Elite.Engine.Ships
{
    internal sealed class PlayerShip
    {
        internal PlayerShip() => Reset();

        internal float Altitude { get; set; }
        internal float CabinTemperature { get; set; }
        internal int CargoCapacity { get; set; }
        internal float Climb { get; set; }
        internal int EcmActive { get; set; }
        internal float Energy { get; set; }
        internal EnergyUnit EnergyUnit { get; set; }
        internal float Fuel { get; set; }
        internal bool HasDockingComputer { get; set; }
        internal bool HasECM { get; set; }
        internal bool HasEnergyBomb { get; set; }
        internal bool HasEscapeCapsule { get; set; }
        internal bool HasFuelScoop { get; set; }
        internal bool HasGalacticHyperdrive { get; set; }
        internal bool IsClimbing { get; set; }
        internal bool IsRolling { get; set; }
        internal ILaser LaserFront { get; set; } = new LaserNone();
        internal ILaser LaserLeft { get; set; } = new LaserNone();
        internal ILaser LaserRear { get; set; } = new LaserNone();
        internal ILaser LaserRight { get; set; } = new LaserNone();
        internal float MaxClimb { get; } = 8;
        internal float MaxFuel { get; } = 7;
        internal float MaxRoll { get; } = 31;

        // 0.27 Light Mach
        internal float MaxSpeed { get; } = 40;

        internal int MissileCount { get; set; }
        internal float Roll { get; set; }
        internal float ShieldFront { get; set; }
        internal float ShieldRear { get; set; }
        internal float Speed { get; set; }

        internal void AutoDock()
        {
            UniverseObject ship = new()
            {
                Rotmat = VectorMaths.GetInitialMatrix(),
                Location = Vector3.Zero
            };

            ship.Rotmat[2].Z = 1;
            ship.Rotmat[0].X = -1;
            ship.Type = (ShipType)(-96);
            ship.Velocity = Speed;
            ship.Acceleration = 0;
            ship.Bravery = 0;
            ship.RotZ = 0;
            ship.RotX = 0;

            Pilot.AutoPilotShip(ref ship);

            Speed = ship.Velocity > 22 ? 22 : ship.Velocity;

            if (ship.Acceleration > 0)
            {
                Speed++;
                if (Speed > 22)
                {
                    Speed = 22;
                }
            }

            if (ship.Acceleration < 0)
            {
                Speed--;
                if (Speed < 1)
                {
                    Speed = 1;
                }
            }

            if (ship.RotX == 0)
            {
                Climb = 0;
            }

            if (ship.RotX < 0)
            {
                IncreaseClimb();

                if (ship.RotX < -1)
                {
                    IncreaseClimb();
                }
            }

            if (ship.RotX > 0)
            {
                DecreaseClimb();

                if (ship.RotX > 1)
                {
                    DecreaseClimb();
                }
            }

            if (ship.RotZ == 127)
            {
                Roll = -14;
            }
            else
            {
                if (ship.RotZ == 0)
                {
                    Roll = 0;
                }
                else if (ship.RotZ > 0)
                {
                    IncreaseRoll();

                    if (ship.RotZ > 1)
                    {
                        IncreaseRoll();
                    }
                }
                else if (ship.RotZ < 0)
                {
                    DecreaseRoll();

                    if (ship.RotZ < -1)
                    {
                        DecreaseRoll();
                    }
                }
            }
        }

        /// <summary>
        /// Deplete the shields.  Drain the energy banks if the shields fail.
        /// </summary>
        /// <param name="damage">Amount of damage.</param>
        /// <param name="front">True if front, false if rear.</param>
        internal void DamageShip(float damage, bool front)
        {
            Debug.Assert(damage > 0);

            float shield = front ? ShieldFront : ShieldRear;

            shield -= damage;
            if (shield < 0)
            {
                DecreaseEnergy(shield);
                shield = 0;
            }

            if (front)
            {
                ShieldFront = shield;
            }
            else
            {
                ShieldRear = shield;
            }
        }

        internal void DecreaseClimb() => Climb = Math.Clamp(Climb - 1, -MaxClimb, MaxClimb);

        internal void DecreaseEnergy(float amount) => Energy += amount;

        internal void DecreaseRoll() => Roll = Math.Clamp(Roll - 1, -MaxRoll, MaxRoll);

        internal void DecreaseSpeed() => Speed = Math.Clamp(Speed - 1, 0, MaxSpeed);

        internal void IncreaseClimb() => Climb = Math.Clamp(Climb + 1, -MaxClimb, MaxClimb);

        internal void IncreaseRoll() => Roll = Math.Clamp(Roll + 1, -MaxRoll, MaxRoll);

        internal void IncreaseSpeed() => Speed = Math.Clamp(Speed + 1, 0, MaxSpeed);

        internal bool IsEnergyLow() => Energy < 50;

        internal void LevelOut()
        {
            if (!IsRolling)
            {
                if (Roll > 0)
                {
                    DecreaseRoll();
                }
                else if (Roll < 0)
                {
                    IncreaseRoll();
                }
            }

            if (!IsClimbing)
            {
                if (Climb > 0)
                {
                    DecreaseClimb();
                }
                else if (Climb < 0)
                {
                    IncreaseClimb();
                }
            }
        }

        /// <summary>
        /// Regenerate the shields and the energy banks.
        /// </summary>
        internal void RegenerateShields()
        {
            if (Energy > 127)
            {
                if (ShieldFront < 255)
                {
                    ShieldFront++;
                    Energy = Math.Clamp(Energy - 1, 0, 255);
                }

                if (ShieldRear < 255)
                {
                    ShieldRear++;
                    Energy = Math.Clamp(Energy - 1, 0, 255);
                }
            }

            Energy = Math.Clamp(Energy + 1 + (int)EnergyUnit, 0, 255);
        }

        internal void Reset()
        {
            Altitude = 255;
            CabinTemperature = 30;
            Roll = 0;
            Climb = 0;
            Speed = 0;
            Energy = 255;
            ShieldFront = 255;
            ShieldRear = 255;
        }
    }
}
