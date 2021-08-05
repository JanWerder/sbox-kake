﻿using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kake.Entities
{
	[Library( "info_npc_start" )]
	[Hammer.EditorModel( "models/editor/playerstart.vmdl" )]
	[Hammer.EntityTool( "NPC Spawn", "Player", "Defines a point where NPCs are spawned" )]
	public class InfoNPCStart : Entity
	{
		public enum Team
		{
			Red,
			Blue
		}

		[Property( Title = "NPC Team" )] public Team NPCTeam { get; set; } = Team.Blue;
	}
}
