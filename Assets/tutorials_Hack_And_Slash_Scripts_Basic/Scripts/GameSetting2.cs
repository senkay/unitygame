using UnityEngine;
using System.Collections;
using System;

public static class GameSetting2 {
	#region PlayerPrefs Constants
	private const string HAIR_COLOR = "Hair Color";
	private const string HAIR_MESH = "Hair Mesh";
	private const string NAME = "Player Name";
	private const string BASE_VALUE = " - BASE VALUE";
	private const string EXP_TO_LEVEL = " - EXP TO LEVEL";
	private const string CUR_VALUE = " - Cur Value";
	private const string CHARACTER_WIDTH = "Char Width";
	private const string CHARACTER_HEIGHT = "Char Height";
	#endregion
	
	#region Resource Paths
	public const string MELEE_WEAPON_ICON_PATH = "Item/Icon/Weapon/Melee/";
	public const string MELEE_WEAPON_MESH_PATH = "Item/Mesh/Weapon/Melee/";
	#endregion
	
	public static PlayerCharacter pc;

	//index 0 = mainmenu
	//index 1 = character creation screen
	//index 2 = character customization screen
	//index 3 = tutorial level
	public static string[] levelNames = {
		"Main Menu",
		"Character Generation",
		"Character Customization",
		"Tutorial"
	};
	
	/// <summary>
	/// Default Constructor
	/// </summary>
	static GameSetting2() {
	}
	
	public static void SaveCharacterWidth( float width ) {
		PlayerPrefs.SetFloat( CHARACTER_WIDTH , width );
	}
	
	public static void SaveCharacterHeight( float height ) {
		PlayerPrefs.SetFloat( CHARACTER_HEIGHT , height );
	}
	
	public static void SaveCharacterScale( float width, float height ) {
		SaveCharacterWidth( width );
		SaveCharacterHeight( height );
	}
	
	public static float LoadCharacterWidth() {
		return PlayerPrefs.GetFloat( CHARACTER_WIDTH, 1 );
	}

	public static float LoadCharacterHeight() {
		return PlayerPrefs.GetFloat( CHARACTER_HEIGHT, 1 );
	}
	
	public static float[] LoadCharacterScale() {
		float[] scale = new float[2];
		
		scale[0] = PlayerPrefs.GetFloat( CHARACTER_WIDTH, 1 );
		scale[1] = PlayerPrefs.GetFloat( CHARACTER_HEIGHT, 1 );

		return scale;
	}

	
	/// <summary>
	/// Store the index of the hair color as an int
	/// </summary>
	/// <param name="index">
	/// A <see cref="System.Int32"/>
	/// </param>
	public static void SaveHairColor( int index ) {
		PlayerPrefs.SetInt( HAIR_COLOR , index );
	}
	

	/// <summary>
	/// Load the players selected index for the hair color they have selected
	/// </summary>
	/// <returns>
	/// A <see cref="System.Int32"/>
	/// </returns>
	public static int LoadHairColor() {
		return PlayerPrefs.GetInt( HAIR_COLOR, 0 );
	}
	
	
	/// <summary>
	/// Save the selected index for the hair mesh as an int
	/// </summary>
	/// <param name="index">
	/// A <see cref="System.Int32"/>
	/// </param>
	public static void SaveHairMesh( int index ) {
		PlayerPrefs.SetInt( HAIR_MESH , index );
		Debug.Log( HAIR_MESH + " : " + index  );
	}
	

	/// <summary>
	/// Load both the hair color and the hair mesh the player as selected, both as an int
	/// </summary>
	/// <param name="index">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Int32"/>
	/// </returns>
	public static int LoadHairMesh() {
		return PlayerPrefs.GetInt( HAIR_MESH, 0 );
	}
	
	
	/// <summary>
	/// Save both the hair color and the hair mesh the player as selected from the playerprefs
	/// </summary>
	/// <param name="mesh">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="color">
	/// A <see cref="System.Int32"/>
	/// </param>
	public static void SaveHair( int mesh, int color ) {
		SaveHairColor( color );
		SaveHairMesh( mesh );
	}
	

	public static void SaveName( string name ) {
		PlayerPrefs.SetString( NAME, name);
	}
	

	public static string LoadName() {
		return PlayerPrefs.GetString(NAME, "Anon");
	}
	

	public static void SaveAttribute( AttributeName name, Attribute attribute ) {
		PlayerPrefs.SetInt(((AttributeName)name).ToString() + BASE_VALUE, attribute.BaseValue );
		PlayerPrefs.SetInt(((AttributeName)name).ToString() + EXP_TO_LEVEL, attribute.ExpToLevel );
	}
	

	public static Attribute LoadAttribute( AttributeName name ) {
		Attribute att = new Attribute();
		att.BaseValue = PlayerPrefs.GetInt(((AttributeName)name).ToString() + BASE_VALUE, 0);
		att.ExpToLevel = PlayerPrefs.GetInt(((AttributeName)name).ToString() + EXP_TO_LEVEL, Attribute.STARTING_EXP_COST);
		return att;
	}
	

	public static void SaveAttributes( Attribute[] attribute ) {
		for( int cnt = 0; cnt < attribute.Length; cnt++ )
			SaveAttribute( (AttributeName)cnt, attribute[cnt] );
	}
	

	public static Attribute[] LoadAttributes() {
		Attribute[] att = new Attribute[Enum.GetValues(typeof(AttributeName)).Length];
		
		for(int cnt = 0; cnt < Enum.GetValues(typeof(AttributeName)).Length; cnt++)
			att[cnt] = LoadAttribute( (AttributeName)cnt );
		
		return att;
	}


	public static void SaveVital( VitalName name, Vital vital ) {
		PlayerPrefs.SetInt(((VitalName)name).ToString() + BASE_VALUE, vital.BaseValue);
		PlayerPrefs.SetInt(((VitalName)name).ToString() + EXP_TO_LEVEL, vital.ExpToLevel);
		PlayerPrefs.SetInt(((VitalName)name).ToString() + CUR_VALUE, vital.CurValue);
	}
	

	public static Vital LoadVital( VitalName name ) {
		pc.GetVital( (int)name ).BaseValue = PlayerPrefs.GetInt(((VitalName)name).ToString() + BASE_VALUE, 0);
		pc.GetVital( (int)name ).ExpToLevel = PlayerPrefs.GetInt(((VitalName)name).ToString() + EXP_TO_LEVEL, 0);
		
		//make sure you call this so that the AjustedBaseValue will be updated before you try to call to get the curValue
		pc.GetVital( (int)name ).Update();

		//get the stored value for the curValue for each vital
		pc.GetVital( (int)name ).CurValue = PlayerPrefs.GetInt(((VitalName)name).ToString() + CUR_VALUE, 1);

		return pc.GetVital( (int)name );
		
//		Vital temp = new Vital();
		
//		temp.BaseValue = PlayerPrefs.GetInt(((VitalName)name).ToString() + BASE_VALUE, 0);
//		temp.ExpToLevel = PlayerPrefs.GetInt(((VitalName)name).ToString() + EXP_TO_LEVEL, 0);
		
		//make sure you call this so that the AjustedBaseValue will be updated before you try to call to get the curValue
//		temp.Update();

		//get the stored value for the curValue for each vital
//		Debug.Log( "CUR: " + PlayerPrefs.GetInt(((VitalName)name).ToString() + CUR_VALUE, 1) );
//		temp.CurValue = PlayerPrefs.GetInt(((VitalName)name).ToString() + CUR_VALUE, 1);
//		Debug.LogError( "Cur: " + temp.CurValue );

//		return temp;
	}
	

	public static void SaveVitals( Vital[] vital ) {
		for( int cnt = 0; cnt < vital.Length; cnt++ )
			SaveVital( (VitalName)cnt, vital[cnt] );
	}
	

	public static Vital[] LoadVitals() {
		Vital[] vital = new Vital[Enum.GetValues(typeof(VitalName)).Length];
		
		for(int cnt = 0; cnt < vital.Length; cnt++)
			vital[cnt] = LoadVital( (VitalName)cnt );
		
		return vital;
	}
	

	public static void SaveSkill( SkillName name, Skill skill ) {
		PlayerPrefs.SetInt(((SkillName)name).ToString() + BASE_VALUE, skill.BaseValue);
		PlayerPrefs.SetInt(((SkillName)name).ToString() + EXP_TO_LEVEL, skill.ExpToLevel);
	}
	

	public static Skill LoadSkill( SkillName name ) {
		Skill skill = new Skill();
		
		skill.BaseValue = PlayerPrefs.GetInt(((SkillName)name).ToString() + BASE_VALUE, 0);
		skill.ExpToLevel = PlayerPrefs.GetInt(((SkillName)name).ToString() + EXP_TO_LEVEL, 0);

		return skill;
	}
	

	public static void SaveSkills( Skill[] skill ) {
		for( int cnt = 0; cnt < skill.Length; cnt++ )
			SaveSkill( (SkillName)cnt, skill[cnt] );
	}
	

	public static Skill[] LoadSkills() {
		Skill[] skill = new Skill[Enum.GetValues(typeof(SkillName)).Length];
		
		for(int cnt = 0; cnt < skill.Length; cnt++)
			skill[cnt] = LoadSkill( (SkillName)cnt );
	
		return skill;
	}
}