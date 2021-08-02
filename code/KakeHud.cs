using kake.UI;
using Sandbox.UI;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace kake
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class KakeHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public KakeHudEntity()
		{
			if ( IsClient )
			{
				RootPanel.StyleSheet.Load("/UI/KakeHud.scss");

				RootPanel.AddChild<EventHud>();
				RootPanel.AddChild<EventSub>();
			}
		}
	}

}
