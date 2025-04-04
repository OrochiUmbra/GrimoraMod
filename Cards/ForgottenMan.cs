using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameForgottenMan = $"{GUID}_ForgottenMan";

	private void Add_Card_ForgottenMan()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(CreateShipwrecks.ability, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			.SetDescription("A waterlogged sailor who carries his tragic past beside him. I don't remember the details.")
			.SetNames(NameForgottenMan, "Forgotten Man")
			.Build();
	}
}
