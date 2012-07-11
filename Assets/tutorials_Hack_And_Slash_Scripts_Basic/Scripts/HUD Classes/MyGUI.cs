/// <summary>
/// MyGUI.cs
/// Jan 10, 2010
/// Peter Laliberte
/// 
/// This class will display all of the gui elements while the game is playing.
/// This includes:
	/// Looting Window
	/// Inventory Window
	/// Character Panels (equipment, attributes, skils)
	/// Healthbars, Staminabars, Manabars
///
/// This script needs to be attached to a gameobject.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Hack And Slash Tutorial/GUI/HUD")]
public class MyGUI : MonoBehaviour {
	public GUISkin mySkin;								//create a public variable for us to reference our custom guiskin
	
	public string emptyInventorySlotStyle;				//the style name for an epty inventory slot
	public string closeButtonStyle;						//the style name for a close button
	public string inventorySlotCommonStlye;				//the stlye name for a common item
	
	
	public float lootWindowHeight = 90;					//define how tall our loot window will be
	
	public float buttonWidth = 40;						//define the width of all of the buttons we will be using for items
	public float buttonHeight = 40;						//define the height of all of the buttons we wll be using for items
	public float closeButtonWidth = 20;					//the width of a close button (small x in a corner)
	public float closeButtonHeight = 20;				//the heght of a close button (small x in the corner)
	
//	private List<Item> _lootItems;

	private float _offset = 10;							//the default offset that is going to be used when spacing gui elements

	private string _toolTip = "";						//the tooltip to display

	
	#region Loot Window Variables
	/***************************************************
	* Loot window variables
	* 
	* These variables are only used fot the loot window
	***************************************************/
	private bool _displayLootWindow = false;			//toggle to control if the loot window should be displayed or not
	private const int LOOT_WINDOW_ID = 0;				//the unique id of the loot window
	private Rect _lootWindowRect = new Rect(0,0,0,0);	//the starting location of the loot window
	private Vector2 _lootWindowSlider = Vector2.zero;	//the scrollview of the loot window
	public static Chest chest;							//the reference to the lootable object that we are currently using
	#endregion
	

	#region Inventory Window Variables
	/*******************************************************
	* Inventory window variables
	* 
	* These variables are only used fot the inventory window
	*******************************************************/
	private bool _displayInventoryWindow = false;						//toggle the display of the inventory window.
	private const int INVENTORY_WINDOW_ID = 1;							//the unique id of the loot window
	private Rect _inventoryWindowRect = new Rect(10, 10, 170, 265);		//the starting location of the inventory window
	private int _inventoryRows = 6;										//the number of rows of items in our inventory
	private int _inventoryCols = 4;										//the number of columns in our inventory
	
	private float _doubleClickTimer = 0;								//allow us to track when clicks happen
	private const float DOUBLE_CLICK_TIMER_THRESHHOLD = .5f;			//the default threshhold for how fast a click has to occur be be considered a double click
	private Item _selectedItem;											//the item that we clicked on
	#endregion
	

	#region Character Window Variables
	/*******************************************************
	* Character window variables
	* 
	* These variables are only used fot the character window
	*******************************************************/
	private bool _displayCharacterWindow = false;							//toggle the display of the character window
	private const int CHARACTER_WINDOW_ID = 2;								//the unique id of the character window
	private Rect _characterWindowRect = new Rect(10, 10, 170, 265);			//the default placement of the character window
	private int _characterPanel = 0;										//the current tab we have selected from toolbar
	private string[] _characterPanelNames = new string[] {"Equipment",		//the name of the tabs in the toolbar
														  "Attributes",
														  "Skills"
														 };
	#endregion

	

	/// <summary>
	/// Start this instance.
	/// 
	/// This function is called before the first Update is called. Use this function to set tings up before we start.
	/// </summary>
	void Start() {
		PC.Instance.Awake();
//		_lootItems = new List<Item>();
	}
	

	
	/// <summary>
	/// Raises the enable event.
	/// 
	/// Make sure you add all of the listeners that are going to be needed for this script
	/// </summary>
	private void OnEnable() {
//		Messenger<int>.AddListener("PopulateChest", PopulateChest);
		Messenger.AddListener("DisplayLoot", DisplayLoot);							//display the loot window
		Messenger.AddListener("ToggleInventory", ToggleInventoryWindow);			//display the inventory
		Messenger.AddListener("ToggleCharacterWindow", ToggleCharacterWindow);		//display the character window
		Messenger.AddListener("CloseChest", ClearWindow);							//close the loot window.
	}
	
	
	
	/// <summary>
	/// Raises the disable event.
	/// 
	/// Make sure you add the remove listeners for all of the events that you are listenning for
	/// </summary>
	private void OnDisable() {
//		Messenger<int>.RemoveListener("PopulateChest", PopulateChest);
		Messenger.RemoveListener("DisplayLoot", DisplayLoot);						//display the loot window
		Messenger.RemoveListener("ToggleInventory", ToggleInventoryWindow);			//display the inventory window
		Messenger.RemoveListener("ToggleCharacterWindow", ToggleCharacterWindow);	//display the character window
		Messenger.RemoveListener("CloseChest", ClearWindow);						//close the loot window
	}

	
	
	/// <summary>
	/// Raises the GUI event.
	/// 
	/// This function draws all of the GUI elements on the screen. This functions can be called more then once per frame.
	/// </summary>
	void OnGUI() {
		//define the skin that we are going to be using for our window
		GUI.skin = mySkin;
		
		
		//dispplay the character window if we are set to.
		if(_displayCharacterWindow)
			_characterWindowRect = GUI.Window(CHARACTER_WINDOW_ID, _characterWindowRect, CharacterWindow, "Character");

		//dispplay the inventory window if we are set to.
		if(_displayInventoryWindow)
			_inventoryWindowRect = GUI.Window(INVENTORY_WINDOW_ID, _inventoryWindowRect, InventoryWindow, "Inventory");

		//dispplay the loot window if we are set to.
		if(_displayLootWindow)
			_lootWindowRect = GUI.Window(LOOT_WINDOW_ID, new Rect(_offset, Screen.height - (_offset + lootWindowHeight), Screen.width - (_offset * 2), lootWindowHeight), LootWindow, "Loot Window", "box");

		//display the tooltip that we have
		DisplayToolTip();	
	}

	
	
	/// <summary>
	/// Display the loot window that will display all of the loot that is in the object we are looting.
	/// </summary>
	/// <param name='id'>
	/// Identifier for the window we are going to use.
	/// </param>
	private void LootWindow(int id) {
		//define the skin that we are going to be using for the elements in the loot window
		GUI.skin = mySkin;
			
		//add a close button to the window
		if(GUI.Button(new Rect(_lootWindowRect.width - _offset * 2, 0, closeButtonWidth, closeButtonHeight), "x", closeButtonStyle))
			ClearWindow();
		
		if(chest == null)
			return;
		
		if(chest.loot.Count == 0) {
			ClearWindow();
			return;
		}
			
		
//		_lootWindowSlider = GUI.BeginScrollView(new Rect(_offset * .5f, 15, _lootWindowRect.width - _offset, 70), _lootWindowSlider, new Rect(0, 0, (_lootItems.Count * buttonWidth) + _offset, buttonHeight + _offset));
		//create a scroll view for our loot window
		_lootWindowSlider = GUI.BeginScrollView(new Rect(_offset * .5f, 15, _lootWindowRect.width - _offset, 70), _lootWindowSlider, new Rect(0, 0, (chest.loot.Count * buttonWidth) + _offset, buttonHeight + _offset));
		
		
		//iterate though the items in the lootable object and display them in the scroll view as a styled button
		for(int cnt = 0; cnt < chest.loot.Count; cnt++) {
			if(GUI.Button(new Rect(_offset * .5f + (buttonWidth * cnt), _offset, buttonWidth, buttonHeight), new GUIContent(chest.loot[cnt].Icon , chest.loot[cnt].ToolTip()), inventorySlotCommonStlye)) {
//				Debug.Log(chest.loot[cnt].ToolTip());
				PlayerCharacter.Inventory.Add(chest.loot[cnt]);
				chest.loot.RemoveAt(cnt);
			}
		}
		
		//make sure we define where we are going to end the scroll view
		GUI.EndScrollView();
		
		//set the tooltip to display if we have one.
		SetToolTip();
	}
	

	
	/// <summary>
	/// Activate the loot window.
	/// </summary>
	private void DisplayLoot() {
		_displayLootWindow = true;
	}


	
//	private void PopulateChest(int x) {
//		for(int cnt = 0; cnt < x; cnt++)
//			_lootItems.Add(new Item());
//		
//		_displayLootWindow = true;
//	}
	

	
	/// <summary>
	/// Close the loot window, and tell the lootable object that we have open that we are closing it.
	/// </summary>
	private void ClearWindow() {
		_displayLootWindow = false;			//toggle the loot window display to off
//		_lootItems.Clear();
		
		chest.OnMouseUp();					//let the lootable object that we currently have open to close
		
		chest = null;						//clear the reference to a lootable object
	}
	
	
	
	/// <summary>
	/// Display the contents of the Inventory Window.
	/// </summary>
	/// <param name='id'>
	/// Identifier of the window that we are displaying the inventory elements in.
	/// </param>
	public void InventoryWindow(int id) {
		//create a counter to keep track of what item we are on.
		int cnt = 0;
		
		for(int y = 0; y < _inventoryRows; y++) {
			for(int x = 0; x < _inventoryCols; x++) {
				if(cnt < PlayerCharacter.Inventory.Count) {
					if(GUI.Button(new Rect(5 + (x * buttonWidth), 20 + (y * buttonHeight), buttonWidth, buttonHeight), new GUIContent(PlayerCharacter.Inventory[cnt].Icon, PlayerCharacter.Inventory[cnt].ToolTip()), inventorySlotCommonStlye)) {
						if(_doubleClickTimer != 0 && _selectedItem != null) {
							if(Time.time - _doubleClickTimer < DOUBLE_CLICK_TIMER_THRESHHOLD) {
//								Debug.Log("Double Click: " + PlayerCharacter.Inventory[cnt].Name);
								
								if(PlayerCharacter.EquipedWeapon == null) {
									PlayerCharacter.EquipedWeapon = PlayerCharacter.Inventory[cnt];
									PlayerCharacter.Inventory.RemoveAt(cnt);
								}
								else {
									Item temp = PlayerCharacter.EquipedWeapon;
									PlayerCharacter.EquipedWeapon = PlayerCharacter.Inventory[cnt];
									PlayerCharacter.Inventory[cnt] = temp;
								}
								

								_doubleClickTimer = 0;
								_selectedItem = null;
							}
							else {
//								Debug.Log("Reset the double click timer");
								_doubleClickTimer = Time.time;
							}
						}
						else {
							_doubleClickTimer = Time.time;
							_selectedItem = PlayerCharacter.Inventory[cnt];
						}
					}
				}
				else {
					GUI.Label(new Rect(5 + (x * buttonWidth), 20 + (y * buttonHeight), buttonWidth, buttonHeight), (x + y * _inventoryCols).ToString(), emptyInventorySlotStyle);
				}
				
				cnt++;
			}
		}

		SetToolTip();
		GUI.DragWindow();
	}
	

	
	
	public void ToggleInventoryWindow() {
		_displayInventoryWindow = !_displayInventoryWindow;
	}


	
	
	public void CharacterWindow(int id) {
		_characterPanel = GUI.Toolbar(new Rect(5, 25, _characterWindowRect.width - 10, 50), _characterPanel, _characterPanelNames);
		
		switch(_characterPanel) {
		case 0:
			DisplayEquipment();
			break;
		case 1:
			DisplayAttributes();
			break;
		case 2:
			DisplaySkills();
			break;
		}
		
		GUI.DragWindow();
	}
	

	
	
	public void ToggleCharacterWindow() {
		_displayCharacterWindow = !_displayCharacterWindow;
	}
	

	
	
	private void DisplayEquipment() {
//		GUI.skin = mySkin;
		
//		Debug.Log("Displaying Equipment");
		if(PlayerCharacter.EquipedWeapon == null) {
			GUI.Label(new Rect(5, 100, 40, 40), "", emptyInventorySlotStyle);
		}
		else {
			if(GUI.Button(new Rect(5, 100, 40, 40), new GUIContent(PlayerCharacter.EquipedWeapon.Icon, PlayerCharacter.EquipedWeapon.ToolTip()))) {
				PlayerCharacter.Inventory.Add(PlayerCharacter.EquipedWeapon);
				PlayerCharacter.EquipedWeapon = null;
			}
		}
		
		SetToolTip();
	}
	

	
	
	private void DisplayAttributes() {
//		Debug.Log("Displaying Attributes");
	}


	
	
	private void DisplaySkills() {
//		Debug.Log("Displaying Skills");
	}
	
	

	
	private void SetToolTip() {
		if(Event.current.type == EventType.Repaint && GUI.tooltip != _toolTip) {
			if(_toolTip != "")
				_toolTip = "";
			
			if(GUI.tooltip != "")
				_toolTip = GUI.tooltip;
		}
	}
	

	
	/// <summary>
	/// Display the tool tip if we have one.
	/// </summary>
	private void DisplayToolTip() {
		if(_toolTip != "")
			GUI.Box(new Rect(Screen.width / 2 - 100, 10, 200, 100), _toolTip);
	}
}
