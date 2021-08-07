using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace kake
{
	[Library( "trigger_red" )]
	public partial class TriggerRed : TriggerMultiple
	{
		public override void OnTriggered( Entity other )
		{
			if ( other is KakePlayer client )
			{
				client.ToTeamRed();
			}

			base.StartTouch( other );
		}
	}
}
