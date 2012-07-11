/// <summary>
/// GameTime.cs
/// Nov 10, 2010
/// Peter Laliberte
/// 
/// This class is responsible for keeping track of in game time. It will also rotate the suns and moons in the sky based on the current in game time.
/// This class will also change the skybox from the day skybox to the night skybox as time progresses in game.
/// </summary>
using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {
	public enum TimeOfDay {
		Idle,
		SunRise,
		SunSet
	}
	
	public Transform[] sun;							//an array to hold all of our suns
	public float dayCycleInMinutes = 1;				//how many real time minutes an in game day will last
	
	public float sunRise;							//the time of day that we start the sunrise
	public float sunSet;							//the time of day that we start the sunset
	public float skyboxBlendModifier;				//the speed at which the textures in the skybox blend
	
	public Color ambLightMax;						//the ambient light color at full day
	public Color ambLightMin;						//the ambient light color at full night
	
	public float morningLight;
	public float nightLight;
	private bool _isMorning = false;
	
	private Sun[] _sunScript;						//an array to hold all of the Sun.cs scripts attached to our sun
	private float _degreeRotation;					//how many degrees we rotate each unit of time
	private float _timeOfDay;						//track the passage of time through out the day
	
	private float _dayCycleInSeconds;				//the number of real time seconds in an in game day

	private const float SECOND = 1;							//constant for 1 second
	private const float MINUTE = 60 * SECOND;				//constant for how many seconds in a minute
	private const float HOUR = 60 * MINUTE;					//constant for how many seconds in an hour
	private const float DAY = 24 * HOUR;					//constant for how many seconds in a day
	private const float DEGREES_PER_SECOND = 360 / DAY;		//constant for how many degrees we have to rotate per second a day to do 360 degrees
	
	private TimeOfDay _tod;							//our time of day enumeration value;
	private float _noonTime;						//this is the time of day when it is noon
	private float _morningLength;					//the length in seconds the morning last
	private float _eveningLength;					//the length is seconds the night lasts
	
	

	// Use this for initialization
	void Start () {
		_tod = TimeOfDay.Idle;
		
		//get the number of real time seconds in an in game day
		_dayCycleInSeconds = dayCycleInMinutes * MINUTE;
		
		//change our blended skybox to be set to the first skybox in the list
		RenderSettings.skybox.SetFloat("_Blend",0);
		
		//initialize the _sunScript array
		_sunScript = new Sun[sun.Length];
			
		//make sure that all of our suns have the script, if not add it
		for(int cnt = 0; cnt < sun.Length; cnt++) {
			Sun temp = sun[cnt].GetComponent<Sun>();
			
			if(temp == null) {
				Debug.LogWarning("Sun script not found. Adding it.");
				sun[cnt].gameObject.AddComponent<Sun>();
				temp = sun[cnt].GetComponent<Sun>();
			}
			_sunScript[cnt] = temp;
		}
		
		//start the day off at 0 seconds
		_timeOfDay = 0;

		//set the _degreeRotation to the amount of degrees that have to rotate for our day
		_degreeRotation = DEGREES_PER_SECOND * DAY / (_dayCycleInSeconds);
		
		sunRise *= _dayCycleInSeconds;
		sunSet *= _dayCycleInSeconds;
		_noonTime = _dayCycleInSeconds / 2;
		_morningLength = _noonTime - sunRise;			//the length of the morning in seconds
		_eveningLength = sunSet - _noonTime;			//the length of the evening in seconds
		morningLight *= _dayCycleInSeconds;
		nightLight *= _dayCycleInSeconds;

		//setup lighting to minLight values to start
		SetupLighting();
	}
	


	// Update is called once per frame
	void Update () {
		//update the time of day
		_timeOfDay += Time.deltaTime;
		
		//if the day timer is over the limit of how long a day lasts, reset the day timer
		if(_timeOfDay > _dayCycleInSeconds)
			_timeOfDay -= _dayCycleInSeconds;

//		Debug.Log(_timeOfDay);
		
		//control the outside lighting effects according to the time of day
		if(!_isMorning && _timeOfDay > morningLight && _timeOfDay < nightLight) {
			_isMorning = true;
			Messenger<bool>.Broadcast("Morning Light Time", true);
			Debug.Log("Morning");
		}
		else if(_isMorning && _timeOfDay > nightLight) {
			_isMorning = false;
			Messenger<bool>.Broadcast("Morning Light Time", false);
			Debug.Log("Night");
		}
		
		
		//position the sun in the sky by adjusting the angle the flare is shining from
		for(int cnt = 0; cnt < sun.Length; cnt++)
			sun[cnt].Rotate(new Vector3(_degreeRotation, 0, 0) * Time.deltaTime);
		
		if(_timeOfDay > sunRise && _timeOfDay < _noonTime) {
			AdjustLighting(true);
		}
		else if(_timeOfDay > _noonTime && _timeOfDay < sunSet) {
			AdjustLighting(false);
		}
		
		//the sun is past the sunrise point, before the sunset point, and the day skybox has not fully faded in
		if(_timeOfDay > sunRise && _timeOfDay < sunSet && RenderSettings.skybox.GetFloat("_Blend") < 1) {
			_tod = GameTime.TimeOfDay.SunRise;
			BlendSkybox();
		}
		else if(_timeOfDay > sunSet && RenderSettings.skybox.GetFloat("_Blend") > 0) {
			_tod = GameTime.TimeOfDay.SunSet;
			BlendSkybox();
		}
		else
			_tod = GameTime.TimeOfDay.Idle;
		
	}
	
	private void BlendSkybox() {
		float temp = 0;
		
		switch(_tod) {
		case TimeOfDay.SunRise:
			temp =  (_timeOfDay - sunRise) / _dayCycleInSeconds * skyboxBlendModifier;
			break;
		case TimeOfDay.SunSet:
			temp =  (_timeOfDay - sunSet) / _dayCycleInSeconds * skyboxBlendModifier;
			temp = 1 - temp;
			break;
		}
		
//		Debug.Log(temp);
		RenderSettings.skybox.SetFloat("_Blend", temp);
	}
	
	private void SetupLighting() {
		RenderSettings.ambientLight = ambLightMin;
		
		for(int cnt = 0; cnt < _sunScript.Length; cnt++) {
			if(_sunScript[cnt].giveLight) {
				sun[cnt].GetComponent<Light>().intensity = _sunScript[cnt].minLightBrightness;
			}
		}
	}
	
	private void AdjustLighting(bool brighten) {
		float pos = 1;											//get the position of the sun in the morning sky
		
		if(brighten) {
			pos = (_timeOfDay - sunRise) / _morningLength;		//get the position of the sun in the morning sky
		}
		else {
			pos = (sunSet - _timeOfDay) / _eveningLength;		//get the position of the sun in the evening sky
		}
		
//		Debug.Log(pos);
		RenderSettings.ambientLight = new Color(ambLightMin.r + ambLightMax.r * pos,
												ambLightMin.g + ambLightMax.g * pos,
												ambLightMin.b + ambLightMax.b * pos);
//		Debug.Log(RenderSettings.ambientLight);

		for(int cnt = 0; cnt < _sunScript.Length; cnt++) {
			if(_sunScript[cnt].giveLight) {
				_sunScript[cnt].GetComponent<Light>().intensity = _sunScript[cnt].maxLightBrightness * pos;
			}
		}
	}
}
