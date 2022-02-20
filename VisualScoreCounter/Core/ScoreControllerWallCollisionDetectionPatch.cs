using HarmonyLib;
using System;

namespace VisualScoreCounter.Core
{
	[HarmonyPatch(typeof(ScoreController))]
	[HarmonyPatch("Update")]
	class ScoreControllerWallCollisionDetectionPatch
	{
		public static event Action? wallCollisionEvent;
		private static bool playerHeadWasInObstacle = false;
		internal static void Postfix(PlayerHeadAndObstacleInteraction ____playerHeadAndObstacleInteraction)
		{
			if (____playerHeadAndObstacleInteraction.intersectingObstacles.Count > 0)
			{
				if (!playerHeadWasInObstacle)
				{
					wallCollisionEvent?.Invoke();
				}
				playerHeadWasInObstacle = true;
			}
			else
			{
				playerHeadWasInObstacle = false;
			}
		}
	}
}
