using System.Linq;
using DV.UI;
using DV.UserManagement;
using DV.Utils;
using HarmonyLib;

namespace dv_quickstart;

[HarmonyPatch(typeof(MainMenu))]
[HarmonyPatch(nameof(MainMenu.Start))]
public class MainMenu_Patch
{
	private static void Postfix(MainMenu __instance)
	{
		var args = System.Environment.GetCommandLineArgs();
		
		//find quickstart argument
		if (args.All(argument => argument != "-quickstart")) return;
		
		var gameSession = SingletonBehaviour<UserManager>.Instance.CurrentUser.CurrentSession;
		if (gameSession == null)
		{
			Main.Error("There are no sessions");
			return;
		}

		if (gameSession.LatestSave == null)
		{
			Main.Error("Session " + gameSession.Name + " doesn't have any saves");
			return;
		}
			
		__instance.OnContinueGameRequested(gameSession.LatestSave);
	}
}