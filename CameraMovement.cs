using UnityEngine;
using System.Collections;

public class CameraMovement
{
	
	// Pinch To Zoom attribute
	float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
	
	float x =0.0f;
	float y = 0.0f;
	
	Vector3 vertical = new Vector3(0,0,0);
	Vector3 horizontal = new Vector3(0,0,0);
	
	// Orbit Camera
	
	GameObject camera;
	GameObject cube;
	GameObject Hips;
	GameObject Lantai;
	
	public CameraMovement(){
		camera = GameObject.Find("Main Camera");
		Hips = GameObject.Find("Hips");
		cube = GameObject.Find("Pivot");
		Lantai = GameObject.Find("lantai");
		}
	
	public void scanInput(){
		camera.transform.LookAt(Hips.transform);
		if(Input.touchCount > 0){	
		
			if(Input.touchCount == 2){
				// Zoomiing
				PinchZoom();	
			}else{
				// Rotasi
				x = Input.GetTouch(0).deltaPosition.x;
				y = Input.GetTouch(0).deltaPosition.y;
				
				for(int i=0; i< Mathf.Abs(x); i++ ){
					RotateVertical(x);
				}
			
				for(int i=0; i<Mathf.Abs(y); i++){
					RotateHorizontal(y);
				}
			}
		}
	
		
	}
	
	void RotateVertical(float rotateValue){
		if(rotateValue < 0){
			vertical = Vector3.up;
		}else{
			vertical = Vector3.down;
		}
		cube.transform.eulerAngles += vertical * Time.maximumDeltaTime * 2f;
	}
	
	void RotateHorizontal(float rotateValue){
		if(rotateValue < 0){
			horizontal = Vector3.right;
		}else{
			horizontal = Vector3.left;	
		}
		
		cube.transform.Rotate(horizontal);
	}
	/// <summary>
	/// Pinchs the zoom.
	/// 
	/// </summary>
	void PinchZoom(){
			// UNity 
		
		 // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Otherwise change the field of view based on the change in distance between the touches.
            camera.camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

            // Clamp the field of view to make sure it's between 0 and 180.
			// modifying the limit values ​​being 30 to 50
            camera.camera.fieldOfView = Mathf.Clamp(camera.camera.fieldOfView, 30f, 50f);
    }
	
	/// <summary>
	/// Clamps the angle.
	/// </summary>
	/// <param name='angle'>
	/// Angle.
	/// </param>
	/// <param name='min'>
	/// Minimum.
	/// </param>
	/// <param name='max'>
	/// Max.
	/// </param>
	float ClampAngle ( float angle, float  min,  float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
}

