using UnityEngine;

public static class ItemGenerator {
	public const int BASE_MELEE_RANGE = 1;
	public const int BASE_RANGED_RANGE = 5;
	
	public static Item CreateItem() {
		//decide what type of item to make
		
		//call the method to create that base item type
		Item item = CreateWeapon();

//	private string _name;

		item.Value = Random.Range(1, 101);
		
		item.Rarity = RarityTypes.Common;
		
		item.MaxDurability = Random.Range(50, 61);
		item.CurDurability = item.MaxDurability;

		//return the new Item
		return item;
	}
	
	private static Weapon CreateWeapon() {
		//decide if we make a melee or ranged weapon
		Weapon weapon = CreateMeleeWeapon();

		//return the weapon created
		return weapon;
	}
	
	private static Weapon CreateMeleeWeapon() {
		Weapon meleeWeapon = new Weapon();
		
		string[] weaponNames = new string[3];
		weaponNames[0] = "Sword";
		weaponNames[1] = "Morningstar";
		weaponNames[2] = "Silifi";

		//fill in all of the values for that item type
		meleeWeapon.Name = weaponNames[Random.Range(0, weaponNames.Length)];

		//assign the max damage of the weapon
		meleeWeapon.MaxDamage = Random.Range(5, 11);
		meleeWeapon.DamageVariance = Random.Range(.2f, .76f);
		meleeWeapon.TypeOfDamage = DamageType.Slash;
		
		//assign the max range of this weapon
		meleeWeapon.MaxRange = BASE_MELEE_RANGE;
		
		//assign the icon for the weapon
		meleeWeapon.Icon = Resources.Load(GameSetting2.MELEE_WEAPON_ICON_PATH + meleeWeapon.Name) as Texture2D;
		
		//return the melee weapon
		return meleeWeapon;
	}
}

public enum ItemType {
	Armor,
	Weapon,
	Potion,
	Scroll
}