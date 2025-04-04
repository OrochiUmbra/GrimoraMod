﻿using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBanshee = $"{GUID}_Banshee";

	private void Add_Card_Banshee()
	{
		CardInfo banshee = "Banshee".GetCardInfo();
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(banshee.Abilities.ToArray())
			.SetBaseAttackAndHealth(banshee.Attack, banshee.Health)
			.SetBoneCost(banshee.BonesCost)
			.SetDescription("THE SCREAMING TERROR. THEY GO STRAIGHT THROUGH TO ATTACK THEIR PREY.")
			.SetNames(NameBanshee, "Banshee", banshee.portraitTex)
			.Build();
	}
}
