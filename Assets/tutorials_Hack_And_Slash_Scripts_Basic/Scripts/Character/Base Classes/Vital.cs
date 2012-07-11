/// <summary>
/// Vital.cs
/// Sept 10, 2010
/// Peter Laliberte
/// 
/// This class contain all the extra functions for a characters vitals
/// 
/// THis class can not be attached to anything
/// </summary>
public class Vital : ModifiedStat {
	private int _curValue;				//this is the current value if this vital
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Vital"/> class.
	/// </summary>
	public Vital() {
		_curValue = 0;
		ExpToLevel = 40;
		LevelModifier = 1.1f;
	}
	
	
	/// <summary>
	/// When getting the _curValue, make sure that it is not greater than our AdjustedBaseValue.
	/// If it is, make it the same as our AdjustedBaseValue
	/// </summary>
	/// <value>
	/// The current value.
	/// </value>
	public int CurValue {
		get{
			if(_curValue > AdjustedBaseValue)
				_curValue = AdjustedBaseValue;
			
			return _curValue;
		}
		set{ _curValue = value; }
	}
}

/// <summary>
/// This enumerations is just a list of the vitals our character will have.
/// </summary>
public enum VitalName {
	Health,
	Energy,
	Mana
}