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
    using Elite.Engine.Types;

    internal static class Equipment
    {
        private static int hilite_item;
        private static EquipmentItem[] EquipmentStock = new EquipmentItem[]
        {
            new(false, true,   1, 0.2f, " Fuel",                EquipmentType.EQ_FUEL),
            new(false, true,   1,   30, " Missile",             EquipmentType.EQ_MISSILE),
            new(false, true,   1,  400, " Large Cargo Bay",     EquipmentType.EQ_CARGO_BAY),
            new(false, true,   2,  600, " E.C.M. System",       EquipmentType.EQ_ECM),
            new(false, true,   5,  525, " Fuel Scoops",         EquipmentType.EQ_FUEL_SCOOPS),
            new(false, true,   6, 1000, " Escape Pod",          EquipmentType.EQ_ESCAPE_POD),
            new(false, true,   7,  900, " Energy Bomb",         EquipmentType.EQ_ENERGY_BOMB),
            new(false, true,   8, 1500, " Extra Energy Unit",   EquipmentType.EQ_ENERGY_UNIT),
            new(false, true,   9, 1500, " Docking Computers",   EquipmentType.EQ_DOCK_COMP),
            new(false, true,  10, 5000, " Galactic Hyperdrive", EquipmentType.EQ_GAL_DRIVE),
            new(false, false,  3,  400, "+Pulse Laser",         EquipmentType.EQ_PULSE_LASER),
            new(false, true,   3,    0, "-Pulse Laser",         EquipmentType.EQ_PULSE_LASER),
            new(false, true,   3,  400, ">Front",               EquipmentType.EQ_FRONT_PULSE),
            new(false, true,   3,  400, ">Rear",                EquipmentType.EQ_REAR_PULSE),
            new(false, true,   3,  400, ">Left",                EquipmentType.EQ_LEFT_PULSE),
            new(false, true,   3,  400, ">Right",               EquipmentType.EQ_RIGHT_PULSE),
            new(false, true,   4, 1000, "+Beam Laser",          EquipmentType.EQ_BEAM_LASER),
            new(false, false,  4,    0, "-Beam Laser",          EquipmentType.EQ_BEAM_LASER),
            new(false, false,  4, 1000, ">Front",               EquipmentType.EQ_FRONT_BEAM),
            new(false, false,  4, 1000, ">Rear",                EquipmentType.EQ_REAR_BEAM),
            new(false, false,  4, 1000, ">Left",                EquipmentType.EQ_LEFT_BEAM),
            new(false, false,  4, 1000, ">Right",               EquipmentType.EQ_RIGHT_BEAM),
            new(false, true,  10,  800, "+Mining Laser",        EquipmentType.EQ_MINING_LASER),
            new(false, false, 10,    0, "-Mining Laser",        EquipmentType.EQ_MINING_LASER),
            new(false, false, 10,  800, ">Front",               EquipmentType.EQ_FRONT_MINING),
            new(false, false, 10,  800, ">Rear",                EquipmentType.EQ_REAR_MINING),
            new(false, false, 10,  800, ">Left",                EquipmentType.EQ_LEFT_MINING),
            new(false, false, 10,  800, ">Right",               EquipmentType.EQ_RIGHT_MINING),
            new(false, true,  10, 6000, "+Military Laser",      EquipmentType.EQ_MILITARY_LASER),
            new(false, false, 10,    0, "-Military Laser",      EquipmentType.EQ_MILITARY_LASER),
            new(false, false, 10, 6000, ">Front",               EquipmentType.EQ_FRONT_MILITARY),
            new(false, false, 10, 6000, ">Rear",                EquipmentType.EQ_REAR_MILITARY),
            new(false, false, 10, 6000, ">Left",                EquipmentType.EQ_LEFT_MILITARY),
            new(false, false, 10, 6000, ">Right",               EquipmentType.EQ_RIGHT_MILITARY)
        };

        private static bool equip_present(EquipmentType type)
        {
            return type switch
            {
                EquipmentType.EQ_FUEL => elite.cmdr.fuel >= 70,
                EquipmentType.EQ_MISSILE => elite.cmdr.missiles >= 4,
                EquipmentType.EQ_CARGO_BAY => elite.cmdr.cargo_capacity > 20,
                EquipmentType.EQ_ECM => elite.cmdr.ecm,
                EquipmentType.EQ_FUEL_SCOOPS => elite.cmdr.fuel_scoop,
                EquipmentType.EQ_ESCAPE_POD => elite.cmdr.escape_pod,
                EquipmentType.EQ_ENERGY_BOMB => elite.cmdr.energy_bomb,
                EquipmentType.EQ_ENERGY_UNIT => elite.cmdr.energy_unit != 0,
                EquipmentType.EQ_DOCK_COMP => elite.cmdr.docking_computer,
                EquipmentType.EQ_GAL_DRIVE => elite.cmdr.galactic_hyperdrive,
                EquipmentType.EQ_FRONT_PULSE => elite.cmdr.front_laser == elite.PULSE_LASER,
                EquipmentType.EQ_REAR_PULSE => elite.cmdr.rear_laser == elite.PULSE_LASER,
                EquipmentType.EQ_LEFT_PULSE => elite.cmdr.left_laser == elite.PULSE_LASER,
                EquipmentType.EQ_RIGHT_PULSE => elite.cmdr.right_laser == elite.PULSE_LASER,
                EquipmentType.EQ_FRONT_BEAM => elite.cmdr.front_laser == elite.BEAM_LASER,
                EquipmentType.EQ_REAR_BEAM => elite.cmdr.rear_laser == elite.BEAM_LASER,
                EquipmentType.EQ_LEFT_BEAM => elite.cmdr.left_laser == elite.BEAM_LASER,
                EquipmentType.EQ_RIGHT_BEAM => elite.cmdr.right_laser == elite.BEAM_LASER,
                EquipmentType.EQ_FRONT_MINING => elite.cmdr.front_laser == elite.MINING_LASER,
                EquipmentType.EQ_REAR_MINING => elite.cmdr.rear_laser == elite.MINING_LASER,
                EquipmentType.EQ_LEFT_MINING => elite.cmdr.left_laser == elite.MINING_LASER,
                EquipmentType.EQ_RIGHT_MINING => elite.cmdr.right_laser == elite.MINING_LASER,
                EquipmentType.EQ_FRONT_MILITARY => elite.cmdr.front_laser == elite.MILITARY_LASER,
                EquipmentType.EQ_REAR_MILITARY => elite.cmdr.rear_laser == elite.MILITARY_LASER,
                EquipmentType.EQ_LEFT_MILITARY => elite.cmdr.left_laser == elite.MILITARY_LASER,
                EquipmentType.EQ_RIGHT_MILITARY => elite.cmdr.right_laser == elite.MILITARY_LASER,
                _ => false,
            };
        }

        internal static void select_next_equip()
        {
            if (hilite_item == EquipmentStock.Length - 1)
            {
                return;
            }

            for (int i = hilite_item + 1; i < EquipmentStock.Length; i++)
            {
                if (EquipmentStock[i].Show)
                {
                    hilite_item = i;
                    break;
                }
            }

            elite.draw.DrawEquipShip(EquipmentStock, hilite_item, elite.cmdr.credits);
        }

        internal static void select_previous_equip()
        {
            if (hilite_item == 0)
            {
                return;
            }

            for (int i = hilite_item - 1; i >= 0; i--)
            {
                if (EquipmentStock[i].Show)
                {
                    hilite_item = i;
                    break;
                }
            }

            elite.draw.DrawEquipShip(EquipmentStock, hilite_item, elite.cmdr.credits);
        }

        private static void list_equip_prices()
        {
            int tech_level = elite.current_planet_data.techlevel + 1;

            EquipmentStock[0].Price = (7 - elite.cmdr.fuel) * 2;

            for (int i = 0; i < EquipmentStock.Length; i++)
            {
                EquipmentStock[i].CanBuy = !equip_present(EquipmentStock[i].Type) && EquipmentStock[i].Price <= elite.cmdr.credits;
                EquipmentStock[i].Show = EquipmentStock[i].Show && tech_level >= EquipmentStock[i].TechLevel;
            }

            hilite_item = 0;
            elite.draw.DrawEquipShip(EquipmentStock, hilite_item, elite.cmdr.credits);
        }

        private static void collapse_equip_list()
        {
            for (int i = 0; i < EquipmentStock.Length; i++)
            {
                char ch = EquipmentStock[i].Name[0];
                EquipmentStock[i].Show = ch is ' ' or '+';
            }
        }

        private static float laser_refund(int laser_type)
        {
            return laser_type switch
            {
                elite.PULSE_LASER => 400,
                elite.BEAM_LASER => 1000,
                elite.MILITARY_LASER => 6000,
                elite.MINING_LASER => 800,
                _ => 0,
            };
        }

        internal static void buy_equip()
        {
            if (EquipmentStock[hilite_item].Name[0] == '+')
            {
                collapse_equip_list();
                EquipmentStock[hilite_item].Show = false;
                hilite_item++;
                for (int i = 0; i < 5; i++)
                {
                    EquipmentStock[hilite_item + i].Show = true;
                }

                list_equip_prices();
                return;
            }

            if (!EquipmentStock[hilite_item].CanBuy)
            {
                return;
            }

            switch (EquipmentStock[hilite_item].Type)
            {
                case EquipmentType.EQ_FUEL:
                    elite.cmdr.fuel = elite.myship.max_fuel;
                    space.update_console();
                    break;

                case EquipmentType.EQ_MISSILE:
                    elite.cmdr.missiles++;
                    space.update_console();
                    break;

                case EquipmentType.EQ_CARGO_BAY:
                    elite.cmdr.cargo_capacity = 35;
                    break;

                case EquipmentType.EQ_ECM:
                    elite.cmdr.ecm = true;
                    break;

                case EquipmentType.EQ_FUEL_SCOOPS:
                    elite.cmdr.fuel_scoop = true;
                    break;

                case EquipmentType.EQ_ESCAPE_POD:
                    elite.cmdr.escape_pod = true;
                    break;

                case EquipmentType.EQ_ENERGY_BOMB:
                    elite.cmdr.energy_bomb = true;
                    break;

                case EquipmentType.EQ_ENERGY_UNIT:
                    elite.cmdr.energy_unit = 1;
                    break;

                case EquipmentType.EQ_DOCK_COMP:
                    elite.cmdr.docking_computer = true;
                    break;

                case EquipmentType.EQ_GAL_DRIVE:
                    elite.cmdr.galactic_hyperdrive = true;
                    break;

                case EquipmentType.EQ_FRONT_PULSE:
                    elite.cmdr.credits += laser_refund(elite.cmdr.front_laser);
                    elite.cmdr.front_laser = elite.PULSE_LASER;
                    break;

                case EquipmentType.EQ_REAR_PULSE:
                    elite.cmdr.credits += laser_refund(elite.cmdr.rear_laser);
                    elite.cmdr.rear_laser = elite.PULSE_LASER;
                    break;

                case EquipmentType.EQ_LEFT_PULSE:
                    elite.cmdr.credits += laser_refund(elite.cmdr.left_laser);
                    elite.cmdr.left_laser = elite.PULSE_LASER;
                    break;

                case EquipmentType.EQ_RIGHT_PULSE:
                    elite.cmdr.credits += laser_refund(elite.cmdr.right_laser);
                    elite.cmdr.right_laser = elite.PULSE_LASER;
                    break;

                case EquipmentType.EQ_FRONT_BEAM:
                    elite.cmdr.credits += laser_refund(elite.cmdr.front_laser);
                    elite.cmdr.front_laser = elite.BEAM_LASER;
                    break;

                case EquipmentType.EQ_REAR_BEAM:
                    elite.cmdr.credits += laser_refund(elite.cmdr.rear_laser);
                    elite.cmdr.rear_laser = elite.BEAM_LASER;
                    break;

                case EquipmentType.EQ_LEFT_BEAM:
                    elite.cmdr.credits += laser_refund(elite.cmdr.left_laser);
                    elite.cmdr.left_laser = elite.BEAM_LASER;
                    break;

                case EquipmentType.EQ_RIGHT_BEAM:
                    elite.cmdr.credits += laser_refund(elite.cmdr.right_laser);
                    elite.cmdr.right_laser = elite.BEAM_LASER;
                    break;

                case EquipmentType.EQ_FRONT_MINING:
                    elite.cmdr.credits += laser_refund(elite.cmdr.front_laser);
                    elite.cmdr.front_laser = elite.MINING_LASER;
                    break;

                case EquipmentType.EQ_REAR_MINING:
                    elite.cmdr.credits += laser_refund(elite.cmdr.rear_laser);
                    elite.cmdr.rear_laser = elite.MINING_LASER;
                    break;

                case EquipmentType.EQ_LEFT_MINING:
                    elite.cmdr.credits += laser_refund(elite.cmdr.left_laser);
                    elite.cmdr.left_laser = elite.MINING_LASER;
                    break;

                case EquipmentType.EQ_RIGHT_MINING:
                    elite.cmdr.credits += laser_refund(elite.cmdr.right_laser);
                    elite.cmdr.right_laser = elite.MINING_LASER;
                    break;

                case EquipmentType.EQ_FRONT_MILITARY:
                    elite.cmdr.credits += laser_refund(elite.cmdr.front_laser);
                    elite.cmdr.front_laser = elite.MILITARY_LASER;
                    break;

                case EquipmentType.EQ_REAR_MILITARY:
                    elite.cmdr.credits += laser_refund(elite.cmdr.rear_laser);
                    elite.cmdr.rear_laser = elite.MILITARY_LASER;
                    break;

                case EquipmentType.EQ_LEFT_MILITARY:
                    elite.cmdr.credits += laser_refund(elite.cmdr.left_laser);
                    elite.cmdr.left_laser = elite.MILITARY_LASER;
                    break;

                case EquipmentType.EQ_RIGHT_MILITARY:
                    elite.cmdr.credits += laser_refund(elite.cmdr.right_laser);
                    elite.cmdr.right_laser = elite.MILITARY_LASER;
                    break;
            }

            elite.cmdr.credits -= EquipmentStock[hilite_item].Price;
            list_equip_prices();
        }

        internal static void equip_ship()
        {
            elite.current_screen = SCR.SCR_EQUIP_SHIP;

            collapse_equip_list();

            hilite_item = 0;

            list_equip_prices();
        }
    }
}