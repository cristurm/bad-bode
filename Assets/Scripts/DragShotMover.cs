using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]

public class DragShotMover : MonoBehaviour {
	//public vars
	public float magBase = 2; // this is the base magnitude and the maximum length of the line drawn in the user interface
	public float magMultiplier = 5; // multiply the line length by this to allow for higher force values to be represented by shorter lines
	public Vector3 dragPlaneNormal = Vector3.up; // a vector describing the orientation of the drag plan relative to world-space but centered on the target
	public SnapDir snapDirection = SnapDir.away; // force is applied either toward or away from the mouse on release
	public ForceMode forceTypeToApply = ForceMode.VelocityChange;
	public moveStatus movementStatus = moveStatus.jump0; // controlling double jumps
	public enum SnapDir {toward, away};
	public enum moveStatus {jump0, jump1, jump2};
	public Vector3 forceVector;
	public Transform bodeAnimation;
	public float dragFar, maxTimeDrink, maxTimeGravityControl;
	
	// private vars
	private bool mouseDragging = false;
	private Vector3 mousePos3D, prevLocalPosition;
	private float dragDistance;
	private Plane dragPlane;
	private Ray mouseRay;
	private float timer, timerDrink, timerGravityControl, timerReset, actualForce;
	private bool hasDrinked, hasGravityControl;
	private GameObject player, gameCam;
	
	// Private Methods
	void  Start (){
		Time.timeScale = 1.0F; //volta ao normal
		player = GameObject.FindWithTag("Player");
		gameCam = GameObject.FindWithTag("MainCamera");

		// create the dragplane
		dragPlane = new Plane(dragPlaneNormal, transform.position);
	}
	
	void Update() {
		 //pula mais alto
		if(hasDrinked){
			timerDrink += Time.deltaTime;
			if(timerDrink >= maxTimeDrink - 10 && timerDrink <= maxTimeDrink - 9){
				transform.GetComponent<BadBodeAnimationandSound>().ssj3.enabled = false;
				transform.GetComponent<BadBodeAnimationandSound>().ssj2.enabled = true;
				transform.GetComponent<BadBodeAnimationandSound>().ssjKI.particleSystem.startSize = 0.5f;
			}
			if(timerDrink >= maxTimeDrink - 5 && timerDrink <= maxTimeDrink - 4){
				transform.GetComponent<BadBodeAnimationandSound>().ssjKI.particleSystem.startSize = 0.1f;
			}
			
			if(timerDrink > maxTimeDrink){
				transform.GetComponent<BadBodeAnimationandSound>().ssjKI.audio.Stop();
				transform.GetComponent<BadBodeAnimationandSound>().ssj2.enabled = false;
				hasDrinked = false;
				timerDrink = 0;
				magMultiplier = 10; //volta ao normal
				transform.GetComponent<BadBodeAnimationandSound>().ssjKI.particleSystem.enableEmission = false;
			}
		}
		
		//cai mais devagar (tudo fica mais devagar, bulletTime)
		if(hasGravityControl){
			timerGravityControl += Time.deltaTime;
			if(timerGravityControl > maxTimeGravityControl){
				transform.GetComponent<BadBodeAnimationandSound>().ToogleMatrixGlasses();//retira matrixGlasses
				hasGravityControl = false;
				timerGravityControl = 0;
				transform.GetComponent<GravityTrail>().Emit = false;
				Time.timeScale = 1.0F; //volta ao normal
			}
		}
		
		//verificar ultima velocidade do rigidbody //zerado pelo OnDrag
		if (forceVector.x > 0) {
			if (actualForce > rigidbody.velocity.x) {
				actualForce = rigidbody.velocity.x;
			}
		} else {
			if (actualForce < rigidbody.velocity.x) {
				actualForce = rigidbody.velocity.x;
			}
		}
		
		//resetar o movementStatus para jump0 quando o bode estiver parado - correção de bug com pulo para baixo
		if (transform.rigidbody.velocity == new Vector3(0,0,0) && movementStatus != moveStatus.jump0) {			
			timerReset += Time.deltaTime;
			
			if (timerReset > 0.3f) {
				//////////animation//////////
				transform.GetComponent<BadBodeAnimationandSound>().Idle();
				//////////animation//////////
				
				movementStatus = moveStatus.jump0;
				timerReset = 0;
			}
		}
	}
	
	void OnCollisionEnter(Collision what) {
		Vector3 contact = what.contacts[0].normal;
		
		if (contact.y > 0.5f) {
			//////////animation//////////
			transform.GetComponent<BadBodeAnimationandSound>().Idle();
			//////////animation//////////
			
			movementStatus = moveStatus.jump0;
		}
	}

	Mesh MakeDiscMeshBrute ( float r  ){
		Mesh discMesh;
		Vector3[] dmVerts = new Vector3[18];
		Vector3[] dmNorms = new Vector3[18];
		Vector2[] dmUVs = new Vector2[18];
		int[] dmTris = new int[48];
		int i = 0;

		discMesh = new Mesh();

		dmVerts[0] = new Vector3(0,0,0);
		dmVerts[1] = new Vector3(0,0,r);
		dmVerts[2] = new Vector3(1,0,1).normalized * r; // find the vector at the correct distance the hacky-hillbilly way!
		dmVerts[3] = new Vector3(r,0,0);
		dmVerts[4] = new Vector3(1,0,-1).normalized * r;
		dmVerts[5] = new Vector3(0,0,-r);
		dmVerts[6] = new Vector3(-1,0,-1).normalized * r;
		dmVerts[7] = new Vector3(-r,0,0);
		dmVerts[8] = new Vector3(-1,0,1).normalized * r;

		// set the other side to the same points
		for (i = 0; i<dmVerts.Length/2; i++) {
			dmVerts[dmVerts.Length/2 + i] = dmVerts[i];
		}
		for (i = 0; i<dmNorms.Length; i++) {
			if (i<dmNorms.Length/2) dmNorms[i] = Vector3.up; // set side one to face up
			else dmNorms[i] = -Vector3.up; // set side two to face down
		}

		dmUVs[0] = new Vector2(0,0);
		dmUVs[1] = new Vector2(0,r);
		dmUVs[2] = new Vector2(1,1).normalized * r;;
		dmUVs[3] = new Vector2(r,0);
		dmUVs[4] = new Vector2(1,-1).normalized * r;;
		dmUVs[5] = new Vector2(0,-r);
		dmUVs[6] = new Vector2(-1,-1).normalized * r;;
		dmUVs[7] = new Vector2(-r,0);
		dmUVs[8] = new Vector2(-1,1).normalized * r;;

		// set the other side to the same points
		for (i = 0; i<dmUVs.Length/2; i++) {
			dmUVs[dmUVs.Length/2 + i] = dmUVs[i];
		}

		dmTris[0] = 0;
		dmTris[1] = 1;
		dmTris[2] = 2;

		dmTris[3] = 0;
		dmTris[4] = 2;
		dmTris[5] = 3;

		dmTris[6] = 0;
		dmTris[7] = 3;
		dmTris[8] = 4;

		dmTris[9] = 0;
		dmTris[10] = 4;
		dmTris[11] = 5;

		dmTris[12] = 0;
		dmTris[13] = 5;
		dmTris[14] = 6;

		dmTris[15] = 0;
		dmTris[16] = 6;
		dmTris[17] = 7;

		dmTris[18] = 0;
		dmTris[19] = 7;
		dmTris[20] = 8;

		dmTris[21] = 0;
		dmTris[22] = 8;
		dmTris[23] = 1;

		// side two
		dmTris[24] = 9;
		dmTris[25] = 11;
		dmTris[26] = 10;

		dmTris[27] = 9;
		dmTris[28] = 12;
		dmTris[29] = 11;

		dmTris[30] = 9;
		dmTris[31] = 13;
		dmTris[32] = 12;

		dmTris[33] = 9;
		dmTris[34] = 14;
		dmTris[35] = 13;

		dmTris[36] = 9;
		dmTris[37] = 15;
		dmTris[38] = 14;

		dmTris[39] = 9;
		dmTris[40] = 16;
		dmTris[41] = 15;

		dmTris[42] = 9;
		dmTris[43] = 17;
		dmTris[44] = 16;

		dmTris[45] = 9;
		dmTris[46] = 10;
		dmTris[47] = 17;

		discMesh.vertices = dmVerts;
		discMesh.uv = dmUVs;
		discMesh.normals = dmNorms;
		discMesh.triangles = dmTris;

		return discMesh;
	}
	
	// Public Methods
	public void  MouseDown (){
		if (!gameCam.GetComponent<CamView>().isPaused && !player.GetComponent<BadBode>().GetIsDead() && movementStatus != moveStatus.jump2) {
			mouseDragging = true;
			
			if(movementStatus == moveStatus.jump0){
				//////////animation//////////
				transform.GetComponent<BadBodeAnimationandSound>().Down();
				//////////animation//////////
			}
				
			// update the dragplane
			dragPlane = new Plane(dragPlaneNormal, transform.position);
		}
	}
	
	public void  MouseDrag () {
		actualForce = 0;
		if (!gameCam.GetComponent<CamView>().isPaused && !player.GetComponent<BadBode>().GetIsDead() && movementStatus != moveStatus.jump2) {
			// update the plane if the target object has left it
			if (dragPlane.GetDistanceToPoint(transform.position) != 0) {
				// update dragplane by constructing a new one -- I should check this with a profiler
				dragPlane = new Plane(dragPlaneNormal, transform.position);
			}
	
			// create a ray from the camera, through the mouse position in 3D space
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	
			// if mouseRay intersects with dragPlane
			float intersectDist = 0.0f;
	
			if (dragPlane.Raycast(mouseRay, out intersectDist)) {
				// update the world space point for the mouse position on the dragPlane
				mousePos3D = mouseRay.GetPoint(intersectDist);
	
				// calculate the distance between the 3d mouse position and the object position
				dragDistance = Mathf.Clamp((mousePos3D - transform.position).magnitude*dragFar, 0, magBase);
	
				// calculate the force vector
				if (dragDistance*magMultiplier < 1) dragDistance = 0; // this is to allow for a "no move" buffer close to the object
				forceVector = mousePos3D - transform.position;
				forceVector.Normalize();
				forceVector *= dragDistance * magMultiplier;
			}
		}
	}

	public void MouseUp (){
		if (!gameCam.GetComponent<CamView>().isPaused && !player.GetComponent<BadBode>().GetIsDead() && movementStatus != moveStatus.jump2) {
			Vector3 rot = bodeAnimation.eulerAngles;
			mouseDragging = false;
			
			if (forceVector.x > 0) {
				rot.y = 270;
				bodeAnimation.eulerAngles = rot;
			} else if (forceVector.x < 0) {
				rot.y = 90;
				bodeAnimation.eulerAngles = rot;
			}
			
			switch (movementStatus) {
				case moveStatus.jump0:
					//////////animation//////////
					transform.GetComponent<BadBodeAnimationandSound>().Jump();
					//////////animation//////////
					
					movementStatus = moveStatus.jump1;
				break;
				case moveStatus.jump1:
					//////////animation//////////
					transform.GetComponent<BadBodeAnimationandSound>().DoubleJump();
					//////////animation//////////

					movementStatus = moveStatus.jump2;
				break;
			}
			
			// cancel existing velocity
			rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
	
			// add new force
			int snapD = 1;
			if (snapDirection == SnapDir.away) snapD = -1; // if snapdirection is "away" set the force to apply in the opposite direction
			rigidbody.AddForce(snapD * forceVector, forceTypeToApply);
		}
	}
	
	// called by an Enemy Bode
	public float GetActualForce() {
		return actualForce;
	}
	
	// called by TrajectorySimulation
	public bool GetMouseDragging () {
		return mouseDragging;
	}
	
	public void Drink(){
		transform.GetComponent<BadBodeAnimationandSound>().ssj3.enabled = true;
		transform.GetComponent<BadBodeAnimationandSound>().ssjKI.particleSystem.startSize = 1f;
		timerDrink = 0;
		magMultiplier = 15;
		hasDrinked = true;
		transform.GetComponent<BadBodeAnimationandSound>().ssjKI.particleSystem.enableEmission = true;
		transform.GetComponent<BadBodeAnimationandSound>().ssjKI.audio.Play();
	}
	
	public void GravityControl(){
		timerGravityControl = 0;
		Time.timeScale = 0.7F;
		hasGravityControl = true;
	}
}