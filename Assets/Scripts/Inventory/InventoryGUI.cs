﻿using UnityEngine;
using System.Collections.Generic;

using AdamPassey.UserInterface;
using AdamPassey.Inventory.Handler;

namespace AdamPassey.Inventory
{
	[AddComponentMenu("Gameplay/Inventory GUI")]
	public class InventoryGUI : MonoBehaviour
	{
		public Vector2 itemOffset;
		public int tilesize;
		public Rect windowRect = new Rect(50, 50, 220, 200);

		private Inventory parentInventory;
		private GameObject[,] inventory;
		private DraggedItem draggedItem;

		public void Awake() {
			draggedItem = DraggedItem.GetInstance();
		}

		/**
		 *	Render the inventory GUI
		 **/
		void OnGUI() {
			//	using the Instance ID as the window ID
			windowRect = GUI.Window(gameObject.GetInstanceID(), windowRect, OnInventoryWindow, "");
		}

		/**
		 * 	Set the click-event handler
		 * 	Will notify the handler when an item
		 * 	is clicked, dragged, etc.
		 */
		public void SetInventoryEventHandler(Inventory handler) {
			parentInventory = handler;
		}

		/**
		 * 	Draw the inventory in a draggable window
		 **/
		public void OnInventoryWindow(int windowId) {
			Vector2 position = itemOffset;
			GUI.depth = InventoryLayer.MID;

			for (int x = 0; x < inventory.GetLength(0); x++) {
				for (int y = 0; y < inventory.GetLength(1); y++) {
					if (inventory[x, y] != null) {
						//	create a UI.Draggable element
						InventoryItem item = inventory[x, y].GetComponent<InventoryItem>();
						InventoryDraggableHandler draggableHandler = new InventoryDraggableHandler(gameObject, new InventoryPosition(x, y), inventory, item); 
						UI.Draggable(new Rect(position.x, position.y, tilesize, tilesize), item.GetGUIContent(), new GUIStyle("button"), draggableHandler); 
					} else {
						//	create a UI.Slot
						InventorySlotHandler slotHandler = new InventorySlotHandler(gameObject, new InventoryPosition(x, y), inventory);
						UI.Slot(new Rect(position.x, position.y, tilesize, tilesize), new GUIStyle("button"), slotHandler);
					}
					position.x += tilesize;
				}
				position.y += tilesize;
				position.x = itemOffset.y;
			}
			//	if there is no dragged item, 
			//	this window is draggable
			if (draggedItem.item == null) {
				GUI.DragWindow();
			}
		}

		/**
		 * 	Set the objects to render
		 * 	@param gameObjects List<GameObject>
		 **/
		public InventoryGUI SetObjects(GameObject[,] gameObjects) {
			inventory = gameObjects;
			return this;
		}

		/**
		 *	Show the inventory
		 *	@param parentInventory The inventory
		 **/
		public void Show() {
			gameObject.SetActive(true);
		}

		/**
		 * 	Hide the inventory GUI
		 **/
		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}