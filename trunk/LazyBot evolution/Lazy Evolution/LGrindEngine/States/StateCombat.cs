﻿/*
This file is part of LazyBot.

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/
using System.Linq;
using LazyEvo.Public;
using LazyLib.FSM;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.States
{
    internal class StateCombat : MainState
    {
        private PUnit _unit;

        public override int Priority
        {
            get { return Prio.Combat; }
        }

        public override bool NeedToRun
        {
            get
            {
                _unit = null;
                _unit = DefendAgainst();
                return _unit != null;
            }
        }

        public override string Name()
        {
            return "Pull";
        }

        public override void DoWork()
        {
            GrindingEngine.Navigator.Stop();
            CombatHandler.StartCombat(_unit);
            GrindingEngine.UpdateStats(0, 1, 0);
            GrindingEngine.Navigation.UseNextNearestWaypoint();
        }

        private static PUnit DefendAgainst()
        {
            PUnit defendUnit = null;
            if (ObjectManager.ShouldDefend)
            {
                if (!PBlackList.IsBlacklisted(ObjectManager.GetClosestAttacker))
                {
                    defendUnit = ObjectManager.GetClosestAttacker;
                }
                else
                {
                    foreach (PUnit un in ObjectManager.GetAttackers.Where(un => !PBlackList.IsBlacklisted(un)))
                    {
                        defendUnit = un;
                    }
                }
                return defendUnit;
            }
            return null;
        }
    }
}