using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace kake
{
	[Library( "trigger_blue" )]
	public partial class TriggerBlue : TriggerMultiple
	{
		public override void OnTriggered( Entity other )
		{
			if ( other is KakePlayer client )
			{
				client.ToTeamBlue();
			}

			base.StartTouch( other );
		}

		public override void StartTouch( Entity other )
		{
			if ( other is KakePlayer client )
			{
				client.ToTeamBlue();
			}
			base.StartTouch( other );
		}

		public override void EndTouch( Entity other )
		{
			if ( other is KakePlayer client )
			{
				client.ToNoTeam();
			}
			base.EndTouch( other );
		}
	}
}
