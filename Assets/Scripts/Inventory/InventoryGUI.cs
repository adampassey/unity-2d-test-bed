﻿using UnityEngine;
using System.Collections.Generic;

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

		/**
		 *	Render the inventory GUI
		 **/
		void OnGUI() {
			windowRect = GUI.Window(0, windowRect, OnInventoryWindow, "Inventory");
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

			for (int x = 0; x < inventory.GetLength(0); x++) {
				for (int y = 0; y < inventory.GetLength(1); y++) {
					if (inventory[x, y] != null) {
						DrawInventoryItem(new Vector2(x, y), position, tilesize, inventory[x, y].GetComponent<InventoryItem>());
					}
					position.x += tilesize;
				}
				position.y += tilesize;
				position.x = itemOffset.y;
			}
			GUI.DragWindow();
		}

		/**
		 * 	Draw the specific inventory item
		 * 	@param position The [x, y] coordinates of the item
		 * 		in inventory. Used to send messages to Inventory
		 * 		with item location.
		 * 	@param guiPosition The [x, y] to draw the item at
		 * 	@param tilesize The size the inventory item should be.
		 * 		Assumed to be a square
		 * 	@param inventoryItem the inventory item itself
		 **/
		private void DrawInventoryItem(Vector2 position, Vector2 guiPosition, int tilesize, InventoryItem inventoryItem) {
			GUIContent guiContent = inventoryItem.GetGUIContent();

			if (GUI.Button(new Rect(guiPosition.x, guiPosition.y, tilesize, tilesize), guiContent)) {
				GameObject droppedObject = parentInventory.GetObject(position);
				//	dropping object manually for now
				Vector2 pos = transform.position;
				pos.x += 1f;
				droppedObject.transform.position = pos;
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