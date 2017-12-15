﻿using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public LayerMask movementMask;
	public Interactable focus;

	Camera cam;
	PlayerMotor motor;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		motor = GetComponent<PlayerMotor> ();
	}
	
	// Update is called once per frame
	void Update () {


		//to check if the mouse was clicked over the ui
		if (EventSystem.current.IsPointerOverGameObject()){
			return;
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100, movementMask  )) 
			{
				//move player to what we hit
				motor.MoveToPoint(hit.point);
				//stop focusing objects
				RemoveFocus();
			}
		}
		else if (Input.GetMouseButtonDown (1)) 
		{
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100)) 
			{
				Interactable interactable = hit.collider.GetComponent<Interactable> ();
				if (interactable != null) 
				{
					SetFocus (interactable);
				}
			}
		}
	}

	void SetFocus(Interactable newFocus){
		if (newFocus != focus){

			if (focus != null) 
				focus.OnDefocused ();
			
			focus = newFocus;
			motor.FollowTarget (newFocus);
			
		}

		newFocus.OnFocused (transform);
	}

	void RemoveFocus(){
		if (focus != null) 
			focus.OnDefocused ();

		focus = null; 
		motor.stopFollowingTarget ();
	}

}
