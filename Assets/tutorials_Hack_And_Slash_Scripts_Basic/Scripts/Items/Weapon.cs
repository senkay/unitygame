/// <summary>
/// Weapon.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// Our weapon class!
/// 
/// This script does not get attached to anything
/// </summary>
using UnityEngine;

[AddComponentMenu("Hack And Slash Tutorial/Item/Weapon")]
public class Weapon : BuffItem {
	private int _maxDamage;
	private float _dmgVar;
	private float _maxRange;
	public DamageType _dmgType;
	
	public Weapon() {
		_maxDamage = 0;
		_dmgVar = 0;
		_maxRange = 0;
		_dmgType = DamageType.Bludgeon;
	}
	
	public Weapon(int mDmg, float dmgV, float mRange, DamageType dt) {
		_maxDamage = mDmg;
		_dmgVar = dmgV;
		_maxRange = mRange;
		_dmgType = dt;
	}
	
	public int MaxDamage {
		get {return _maxDamage; }
		set { _maxDamage = value; }
	}
	
	public float DamageVariance {
		get { return _dmgVar; }
		set { _dmgVar = value; }
	}
	
	public float MaxRange {
		get { return _maxRange; }
		set { _maxRange = value; }
	}
	
	public DamageType TypeOfDamage {
		get { return _dmgType; }
		set { _dmgType = value; }
	}
	
	public override string ToolTip() {
		return Name + "\n" +
				"Value " + Value + "\n" +
				"Durability " + CurDurability + "/" + MaxDurability + "\n" +
				MaxDamage * DamageVariance + " - " + MaxDamage;
	}
}

public enum DamageType {
	Bludgeon,
	Pierce,
	Slash,
	Fire,
	Ice,
	Lightning,
	Acid
}
