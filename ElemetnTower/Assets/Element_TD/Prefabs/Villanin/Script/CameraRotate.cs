using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour 
{
	public GameObject cam;
	public Vector2 rotationVelocity ;
	public int zoom = 22;
	public int normal = 40;
	public	float smooth = 5f;
	private Vector3 pivot = new Vector3 (0f,1f,0f);
	private bool clickingButton = false;
	private float PlusY;
	private bool isZoomed =false;



	void Update () 
	{
		cam.transform.LookAt(pivot);
		if (!clickingButton && (Input.GetMouseButton (0) || Input.GetMouseButton (1))) 
		{
			rotationVelocity.x += Mathf.Pow(Mathf.Abs(Input.GetAxis("Mouse X")),1.5f) * Mathf.Sign(Input.GetAxis("Mouse X"));
			rotationVelocity.y -= Input.GetAxis("Mouse Y") * 0.04f;
		}

		rotationVelocity = Vector2.Lerp(rotationVelocity, Vector2.zero, Time.deltaTime * 10.0f);
		cam.transform.RotateAround(Vector3.zero, Vector3.up, rotationVelocity.x );
	

		if(Input.GetMouseButtonDown(1))
		{
			isZoomed = !isZoomed; 
		}

		if(isZoomed == true){
			cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView,zoom,Time.deltaTime*smooth);
		}
		else{
			cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView,normal,Time.deltaTime*smooth);
		}
	}
}
