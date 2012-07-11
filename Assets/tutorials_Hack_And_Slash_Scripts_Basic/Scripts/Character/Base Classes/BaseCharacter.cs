/// <summary>
/// BaseCharacter.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// This is the base class for all characters that will be in your game. It holds all of the generalize
/// methods and properties every character will need. This class is ment to be inherited from and not used
/// directly.
/// 
/// This class should not be attached to anything
/// </summary>
using UnityEngine;
using System.Collections;
using System;					//added to access the enum class

public class BaseCharacter : MonoBehaviour {
	public GameObject weaponMount;
	public GameObject hairMount;
	public GameObject helmetMount;
	public GameObject characterMaterialMesh;
	
	private string _name;
	private int _level;
	private uint _freeExp;
	
	public Attribute[] primaryAttribute;
	public Vital[] vital;
	public Skill[] skill;
	
	/*
	 * The Awake method is called before any Start() methods. We are going to use this to make sure our
	 * variables have a default value, as well makng sure that everything that needs a reference to 
	 * something else, has it
	 */
	public virtual void Awake() {
		_name = string.Empty;
		_level = 0;
		_freeExp = 0;
		
		primaryAttribute = new Attribute[Enum.GetValues(typeof(AttributeName)).Length];
		vital = new Vital[Enum.GetValues(typeof(VitalName)).Length];
		skill = new Skill[Enum.GetValues(typeof(SkillName)).Length];
		
		SetupPrimaryAttributes();
		SetupVitals();
		SetupSkills();
	}
	
	#region Setters and Getters
	public string Name {
		get{ return _name; }
		set { _name = value; }
	}
	
	public int Level {
		get{ return _level; }
		set{ _level = value; }
	}
	
	public uint FreeExp {
		get{ return _freeExp; }
		set{ _freeExp = value; }
	}
	
	#endregion

	
	/*
	 * Add a certain amount of exp to our characters free exp pool. We are using an uint as the value
	 * for _freeExp can never be negative and this allows us to get a larger number then using its
	 * signed version
	 */
	public void AddExp(uint exp) {
		_freeExp += exp;
		
		CalculateLevel();
	}

	
	//take avg of all of the players skills and assign that as the player level
	public void CalculateLevel() {
	}
	

	/*
	 * iterate though all of the characters primary attributes and set them up for use
	 */
	private void SetupPrimaryAttributes() {
		for(int cnt = 0; cnt < primaryAttribute.Length; cnt++) {
			primaryAttribute[cnt] = new Attribute();
			primaryAttribute[cnt].Name = ((AttributeName)cnt).ToString();
		}
	}
	
	/*
	 * iterate though all of the characters vitals and set them up for use
	 */
	private void SetupVitals() {
		for(int cnt = 0; cnt < vital.Length; cnt++)
			vital[cnt] = new Vital();
		
		SetupVitalModifiers();
	}

	
	/*
	 * iterate though all of the characters skills and make sure they are set up for use
	 */
	private void SetupSkills() {
		for(int cnt = 0; cnt < skill.Length; cnt++)
			skill[cnt] = new Skill();
		
		SetupSkillModifiers();
	}

	
	/*
	 * return a certain primary attribute in our array given the index
	 * TODO:
	 * make sure the index we are passed is within range of the size of the array
	 * instead of passing back a null on error, pass back an empty attribute (no values)
	 */
	public Attribute GetPrimaryAttribute(int index) {
		return primaryAttribute[index];
	}

	
	/*
	 * return a vital given the index
	 * TODO:
	 * make sure the index we are passed is within range of the size of the array
	 * instead of passing back a null on error, pass back an empty vital (no values)
	 */
	public Vital GetVital(int index) {
		return vital[index];
	}
	
	
	/*
	 * return a skill given the index
	 * TODO:
	 * make sure the index we are passed is within range of the size of the array
	 * instead of passing back a null on error, pass back an empty skill (no values)
	 */
	public Skill GetSkill(int index) {
		return skill[index];
	}

	
	
	/*
	 * Setup the moodifiers that our vitals will have based on the primary attributes
	 */
	private void SetupVitalModifiers() {
		GetVital((int)VitalName.Health).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constituion), .5f));	//health
		GetVital((int)VitalName.Energy).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constituion), 1));	//energy
		GetVital((int)VitalName.Mana).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Willpower), 1));		//mana

	}
	

	/*
	 * Setup the modifiers that our skills will have based n the promary attributes
	 */
	private void SetupSkillModifiers() {
		//melee offence
		GetSkill((int)SkillName.Melee_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Might), .33f));
		GetSkill((int)SkillName.Melee_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Nimbleness), .33f));
		//melee defence
		GetSkill((int)SkillName.Melee_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		GetSkill((int)SkillName.Melee_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constituion), .33f));
		//magic offence
		GetSkill((int)SkillName.Magic_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Magic_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Willpower), .33f));
		//magic defence
		GetSkill((int)SkillName.Magic_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Magic_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Willpower), .33f));
		//ranged offence
		GetSkill((int)SkillName.Ranged_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Ranged_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		//ranged defence
		GetSkill((int)SkillName.Ranged_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		GetSkill((int)SkillName.Ranged_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Nimbleness), .33f));
	}
	

	/*
	 * A wrapper method that is used to call all of the updates on all of your skills and vitals
	 */
	public void StatUpdate() {
		for(int cnt = 0; cnt < vital.Length; cnt++)
			vital[cnt].Update();
		
		for(int cnt = 0; cnt < skill.Length; cnt++)
			skill[cnt].Update();
	}
}
