/// <summary>
/// Attribute.cs
/// Sept 6, 2010
/// Peter Laliberte
/// 
/// This holds all of the methods and properties that are specific to to a characters attributes.
/// 
/// This class can not, and should not be attached to anything
/// </summary>

public class Attribute : BaseStat {
	new public const int STARTING_EXP_COST = 50;	//this is the starting cost for all of our attributes
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Attribute"/> class.
	/// </summary>
	public Attribute() {
//		UnityEngine.Debug.Log("Attribute Created");
		ExpToLevel = STARTING_EXP_COST;
		LevelModifier = 1.05f;
	}
}


/// <summary>
/// This is a list of all the attributes that we will have in-game for our characters
/// </summary>
public enum AttributeName {
	Might = 0,
	Constituion = 1,
	Nimbleness = 2,
	Speed = 3,
	Concentration = 4,
	Willpower = 5,
	Charisma = 6
}