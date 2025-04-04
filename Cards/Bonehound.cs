using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonehound = $"{GUID}_Bonehound";

	private void Add_Card_Bonehound()
	{
		Sprite pixelSprite = "Bonehound".GetCardInfo().pixelPortrait;

		CardInfo bonehound = "Bonehound".GetCardInfo();
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(bonehound.Abilities.ToArray())
			.SetBaseAttackAndHealth(bonehound.Attack, bonehound.Health)
			.SetBoneCost(bonehound.BonesCost)
			.SetDescription("USED TO BE A VERY LOYAL DOG BACK IN THE DAY. WELL, ONE DAY HIS OWNER DIED.")
			.SetNames(NameBonehound, "Bonehound", bonehound.portraitTex)
			.Build().pixelPortrait = pixelSprite;
	}
}
