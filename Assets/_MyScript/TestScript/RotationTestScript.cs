using UnityEngine;
using System.Collections;

public class RotationTestScript : MonoBehaviour {

	
	
	
	// Use this for initialization
	void Start () {
		//initiate the turret position on screen
		//turretPos = Camera.main.WorldToScreenPoint(turret.position);
		
		//fake a tap event to initiate the turret rotation and cursor
		Tap tap=new Tap(new Vector2(Screen.width/2, Screen.height/2));
		OnTap(tap);
	}
	
	void OnEnable(){
		Gesture.onMultiTapE += OnTap;
		
		//Gesture.onChargingE += OnCharging;
		//Gesture.onChargeEndE += OnChargeEnd;
		
		//Gesture.onMFChargingE += OnMFCharging;
		//Gesture.onMFChargeEndE += OnMFChargeEnd;
		print("TestOnEneable");
	}
	
	void OnDisable(){
		Gesture.onMultiTapE -= OnTap;
		
		//Gesture.onChargingE -= OnCharging;
		//Gesture.onChargeEndE -= OnChargeEnd;
		
		//Gesture.onMFChargingE -= OnMFCharging;
		//Gesture.onMFChargeEndE -= OnMFChargeEnd;
		print ("TestOndesibale");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//called when a tap is detected
	void OnTap(Tap tap){
		print("Tap");
	}
	
	/*
	//triggered when mouse/single-finger charging event is on-going
	//this is just to simulate 2-fingers charge event using right-mouse-click
	void OnCharging(ChargedInfo cInfo){
		if(cInfo.isMouse){
			if(cInfo.index==1){
				//if this is triggered by right-mouse-button, modified the fingerCount and call OnMFCharging with the same chargeInfo
				cInfo.fingerCount=2;
				OnMFCharging(cInfo);
			}
		}
	}
	
	//triggered when mouse/single-finger charging event is ended
	void OnChargeEnd(ChargedInfo cInfo){
		if(cInfo.isMouse){
			if(cInfo.index==1){
				//if this is triggered by right-mouse-button, modified the fingerCount and call OnMFChargeEnd with the same chargeInfo
				cInfo.fingerCount=2;
				OnMFChargeEnd(cInfo);
			}
		}
	}
	
	//triggered when a multiple finger charge event is on-going
	void OnMFCharging(ChargedInfo cInfo){
		if(cInfo.fingerCount==2){
			//adjust the length of the indicator bar accordingly to the percent
			//bar.pixelInset=new Rect(bar.pixelInset.x, bar.pixelInset.y, cInfo.percent*150, bar.pixelInset.height);
			//adjust the color on the bar
			//bar.color=new Color(cInfo.percent, 1-cInfo.percent, 0);
		}
	}
	
	//triggered when a multiple finger charge event has ended
	void OnMFChargeEnd(ChargedInfo cInfo){
		if(cInfo.fingerCount==2){
			//reset the bullet
			//adjust the position os it's at the tip of the barrel
			//bullet.transform.position=turret.TransformPoint(new Vector3(0, 0, 1.0f));
			//match the bullet rotation to turret's, so that when force is applied, the bullet head in the right direction
			//bullet.transform.rotation=turret.rotation;
			//cancel current force on bullet
			//bullet.rigidbody.velocity=Vector3.zero;
			
			//shoot the bullet based on the charged percent
			//bullet.rigidbody.AddForce(cInfo.percent*maxForce*bullet.transform.forward);
			
			//clear the charge indicator bar
			//bar.pixelInset=new Rect(bar.pixelInset.x, bar.pixelInset.y, 0, bar.pixelInset.height);
		}
	}
	*/
}
