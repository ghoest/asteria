﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;

namespace HD
{
    public class StuckCommand : Command
    {
        public override string Description
        {
            get { return "/Stuck: Sends you back to the start of the overworld."; }
        }

        public override string Execute(Player player, string args)
        {
            if (player == null)
                return "No character";

            player.StartPosition = Vector2.Zero;
            player.SendTo(World.Overworld);

            if (player.GetMiningTier() == 0)
                player.GiveItem(ItemId.MiningTool1);

            return "Stay Calm and Respawn!";
        }
    }
}