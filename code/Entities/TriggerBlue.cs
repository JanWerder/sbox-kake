using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace kake
{
	[Library( "trigger_blue" )]
	public partial class TriggerBlue: BaseTrigger
	{
		public override void StartTouch( Entity other )
		{
			if ( other is KakePlayer client )
			{
				client.ToTeamBlue();
			}

			base.StartTouch( other );
		}
	}
}
