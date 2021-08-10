
using kake.Entities;
using kake.UI;
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static kake.KakeEnums;

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

		// 0 Pre-Game; 1 Choose Time; 2 In-Game; 3 Post-Game
		[Net] public static int GameState { get; set; } = 0;

		[Net] public static int PrizePool { get; set; } = 0;

		[Net] private static List<kake.KakePlayer> RedPlayers { get; set; } = new();

		[Net] private static List<kake.KakePlayer> BluePlayers { get; set; } = new();

		public static int ChooseTime { get; set; } = 15;

		public static int PostGameTime { get; set; } = 10;

		public static int PreGameTime { get; set; } = 5;

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

				GameTimer = PreGameTime;
			}

			if ( IsClient )
			{
				Log.Info( "Kake Has Created Clientside!" );
			}

		}

		internal static void changePlayerTeam( KakePlayer kakePlayer, Team team )
		{
			if ( team == Team.Red )
			{
				BluePlayers.Remove( kakePlayer );
				if ( !RedPlayers.Contains( kakePlayer ) )
				{
					RedPlayers.Add( kakePlayer );
				}
			}
			else if ( team == Team.Blue )
			{
				RedPlayers.Remove( kakePlayer );
				if ( !BluePlayers.Contains( kakePlayer ) )
				{
					BluePlayers.Add( kakePlayer );
				}
			}
			else if ( team == Team.NoTeam )
			{
				RedPlayers.Remove( kakePlayer );
				BluePlayers.Remove( kakePlayer );
			}
		}

		[Event.Tick]
		public void Tick()
		{
			if ( IsServer )
			{
				if ( GameState == 0 ) //Pre-Game
				{
					GameTimer -= 1.0f / 60.0f;
					EventHud.updateText( $"Pre-Game {  MathF.Ceiling( GameTimer ).ToString()  }" );
					if ( MathF.Ceiling( GameTimer ) <= 0 )
					{
						ResetHUD();
						GameState = 1;
						GameTimer = ChooseTime;
					}
				}
				else if ( GameState == 1 ) //Choose Time
				{
					GameTimer -= 1.0f / 60.0f;
					PrizePool = (BluePlayers.Count + RedPlayers.Count) * 5;
					EventHud.updateText( $"Choose your side: { MathF.Ceiling( GameTimer ).ToString() }" );
					EventSub.updateText( $"Prize Pool: { PrizePool }" );
					if ( MathF.Ceiling( GameTimer ) <= 0 )
					{
						ResetHUD();
						GameState = 2;
						GameTimer = 0;
						StartRound();
					}
					//EventHud.updateText( $"Blue: { BluePlayers.Count } Red: { RedPlayers.Count }" );
				}
				else if ( GameState == 2 ) //In-Game
				{
					GameTimer += 1.0f / 60.0f;
					EventHud.updateText( $"Fighting: { MathF.Ceiling( GameTimer ).ToString() }" );
					if ( MathF.Ceiling( GameTimer ) >= 10 ) //Dummy
					{
						ResetHUD();
						ResetMap();
						GameState = 3;
						GameTimer = PostGameTime;
					}
				}
				else if ( GameState == 3 ) //Post-Game
				{
					GameTimer -= 1.0f / 60.0f;
					EventHud.updateText( $"Post-Game: { MathF.Ceiling( GameTimer ).ToString() }" );
					if ( MathF.Ceiling( GameTimer ) <= 0 )
					{
						ResetHUD();
						ResetTeams();
						GameState = 1; //To Choose Time
						GameTimer = ChooseTime;
					}
				}
			}

		}

		private static void StartRound()
		{

			InfoNPCStart redSpawn = Entity.All.OfType<InfoNPCStart>().ToList().Find( x => x.EntityName == "spawn_red" );
			InfoNPCStart blueSpawn = Entity.All.OfType<InfoNPCStart>().ToList().Find( x => x.EntityName == "spawn_blue" );

			PlatformEntity seperatorStart1 = Entity.All.OfType<PlatformEntity>().ToList().Find( x => x.EntityName == "seperator_1" );
			PlatformEntity seperatorStart2 = Entity.All.OfType<PlatformEntity>().ToList().Find( x => x.EntityName == "seperator_2" );
			seperatorStart1.StartMovingForward();
			seperatorStart2.StartMovingForward();

		}

		private static void ResetMap()
		{
			PlatformEntity seperatorStart1 = Entity.All.OfType<PlatformEntity>().ToList().Find( x => x.EntityName == "seperator_1" );
			PlatformEntity seperatorStart2 = Entity.All.OfType<PlatformEntity>().ToList().Find( x => x.EntityName == "seperator_2" );
			seperatorStart1.StartMovingBackwards();
			seperatorStart2.StartMovingBackwards();
		}

		private void ResetTeams()
		{
			//foreach(KakePlayer Player in RedPlayers){
			//	Player.ResetTeam();
			//}

			//foreach ( KakePlayer Player in BluePlayers )
			//{
			//	Player.ResetTeam();
			//}

			//RedPlayers.Clear();
			//BluePlayers.Clear();
		}

		private static void ResetHUD()
		{
			EventHud.updateText( "" );
			EventSub.updateText( "" );
			TeamHud.updateText( "" );
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
