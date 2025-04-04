using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(PlayerHand))]
public class PlayerHandPatches
{

	public static int cardsPlayedThisCombatForFuse =0;
	private static int lastRegisteredTurn;
	[HarmonyPostfix, HarmonyPatch(nameof(PlayerHand.PlayCardOnSlot))]
	public static IEnumerator HandleMarchingDeadLogic(
		IEnumerator enumerator,
		PlayerHand __instance,
		PlayableCard card,
		CardSlot slot
	)
	{
		GrimoraPlugin.Log.LogInfo($"Pre change lit turn {cardsPlayedThisCombatForFuse}");
		if (GrimoraSaveUtil.IsGrimoraModRun && __instance.CardsInHand.Contains(card) && card.HasAbility(MarchingDead.ability))
		{
			card.GetComponent<MarchingDead>().SetAdjCardsToPlay(__instance.CardsInHand);
		}

		yield return enumerator;

		if (card.EnergyCost > 0 && card.name != "skeleton")
		{

			if (ConfigHelper.Instance.EnergyMode == true && ResourcesManager.Instance.PlayerMaxEnergy >= card.EnergyCost) yield return ResourcesManager.Instance.PlayerMaxEnergy -= card.EnergyCost;


		}

			if (lastRegisteredTurn > TurnManager.Instance.TurnNumber)
			{
				cardsPlayedThisCombatForFuse = 0;
				lastRegisteredTurn = 0;
			}
			lastRegisteredTurn = TurnManager.Instance.TurnNumber;
			cardsPlayedThisCombatForFuse++;
			if (cardsPlayedThisCombatForFuse == 2 && SaveFile.IsAscension && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.RoyalsRevenge))
			{
				yield return TextDisplayer.Instance.ShowUntilInput("Careful, the life of your next card will be on a timer.");
			}
			if (cardsPlayedThisCombatForFuse >= 3)
			{
				if (SaveFile.IsAscension && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.RoyalsRevenge))
				{ 
				yield return TextDisplayer.Instance.ShowUntilInput("I look forward to the [c:brnO]explosive[c:] results!");
				ViewManager.Instance.SwitchToView(View.Board);
				cardsPlayedThisCombatForFuse = 0;
				ChallengeActivationUI.TryShowActivation(ChallengeManagement.RoyalsRevenge);
				yield return new WaitForSeconds(0.2f);
				if (card != null && !card.Dead) { 
					if (card.AllAbilities().Count < 5) { 
				card.AddTemporaryMod(new CardModificationInfo(LitFuse.ability));
				card.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.2f);
				cardsPlayedThisCombatForFuse = 0;
					}
					else
					{
						if (card != null && !card.Dead)
						{
							yield return TextDisplayer.Instance.ShowUntilInput("Your card cannot explode, how dissapointing.");
							card.TakeDamage(1, null);
							cardsPlayedThisCombatForFuse = 0;
						}
						else yield break;
					}
				}

				CardInfo Skeleton = GrimoraPlugin.NameSkeleton.GetCardInfo();
				Skeleton.mods.Add(new CardModificationInfo(Anchored.ability));

				if (GrimoraRunState.CurrentRun.riggedDraws.Contains("Boon_Pirates")) yield return BoardManager.Instance.GetPlayerOpenSlots().GetRandomItem().CreateCardInSlot(Skeleton);



			}
		}
		GrimoraPlugin.Log.LogInfo($"after change lit turn {cardsPlayedThisCombatForFuse}");


		if (SaveFile.IsAscension && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.PlaceBones))
		{
			if (card.Info.bonesCost == 0)
			{ 
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.PlaceBones);
			yield return ResourcesManager.Instance.AddBones(1);
			}
		}

	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayerHand.AddCardToHand))]
	public static void RerenderCard(ref PlayableCard card, Vector3 spawnOffset, float onDrawnTriggerDelay)
	{
		if (card.InfoName() == GrimoraPlugin.NameSpectrabbit)
		{
			card.RenderCard();
		}
	}
}
