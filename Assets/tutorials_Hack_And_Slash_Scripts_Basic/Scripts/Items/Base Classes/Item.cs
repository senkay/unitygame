/// <summary>
/// Item.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// This is the base class for all items in our game. In order for an item to be placed in to our
/// inventory, it will have to be of a type that is dirived from this class
/// 
/// Ths script does not get attached to anyhing
/// </summary>
using UnityEngine;

public class Item {
	private string _name;
	private int _value;
	private RarityTypes _rarity;
	private int _curDur;
	private int _maxDur;
	public Texture2D _icon;
	
//	void Awake() {
//		Init();
//	}
	
	public Item() {
		_name = "Need Name";
		_value = 0;
		_rarity = RarityTypes.Common;
		_maxDur = 50;
		_curDur = _maxDur;
	}
	
	public Item(string name, int value, RarityTypes rare, int maxDur, int curDur) {
		_name = name;
		_value = value;
		_rarity = rare;
		_maxDur = maxDur;
		_curDur = curDur;
	}
	
	
	public string Name {
		get { return _name;  }
		set { _name = value; }
	}
	
	public int Value {
		get { return _value; }
		set { _value = value; }
	}
	
	public RarityTypes Rarity {
		get { return _rarity; }
		set { _rarity = value; }
	}
	
	public int MaxDurability {
		get { return _maxDur; }
		set { _maxDur = value;}
	}
	
	public int CurDurability {
		get { return _curDur; }
		set { _curDur = value;}
	}
	
	public Texture2D Icon {
		get { return _icon; }
		set { _icon = value; }
	}
	
	public virtual string ToolTip() {
		return Name + "\n" +
				"Value " + Value + "\n" +
				"Durability " + CurDurability + "/" + MaxDurability + "\n";
	}
}

public enum RarityTypes {
	Common,
	Uncommon,
	Rare
}