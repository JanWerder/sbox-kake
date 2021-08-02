
using kake.UI;
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace kake
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class KakeGame : Sandbox.Game
	{
		[Net] public static float GameTimer { get; set; } = 30;

		// 0 Pre-Game; 1 In-Game; 2-Post-Game
		[Net] public static int GameState { get; set; } = 0;

		public KakeGame()
		{


			if ( IsServer )
			{
				Log.Info( "Kake Has Created Serverside!" );

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new KakeHudEntity();
			}

			if ( IsClient )
			{
				Log.Info( "Kake Has Created Clientside!" );
			}
		}

		[Event.Tick]
		public void Tick()
		{
			if ( IsServer )
			{
				if ( GameState == 0 )
				{
					GameTimer -= 1.0f / 60.0f;
					EventHud.updateText( MathF.Ceiling( GameTimer ).ToString() );
					if ( MathF.Ceiling( GameTimer ) <= 0 )
					{
						GameState = 1;
						GameTimer = 30;
						EventHud.updateText("");
					}
				}
			}
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new KakePlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}

}
