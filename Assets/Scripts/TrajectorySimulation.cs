using UnityEngine;
using System.Collections.Generic;

public class TrajectorySimulation : MonoBehaviour {
	// Reference to the LineRenderer we will use to display the simulated path
    public LineRenderer sightLine1, sightLine2;
	public Color lineColor;
	//tamanho da linha
	public float startWidth;
	public float endWidth;
	//limites da trajetoria
	public GameObject leftWall;
	public GameObject rightWall;

    // Reference to a Component that holds information about fire strength, location of cannon, etc.
    public DragShotMover playerFire;
	public float myForce;

    // Number of segments to calculate - more gives a smoother line
    public int segmentCount = 20;

    // Length scale for each segment
    public float segmentScale = 1;

    // gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
    private Collider _hitObject;
    public Collider hitObject { get { return _hitObject; } }
	
	public List<List<Vector3>> segmentsList = new List<List<Vector3>>();
	
	private bool mouseDragging;
	
	private Vector3 newPosStore;
	private Vector3 screenPos;
	//private GameObject myView;
	private RaycastHit hit;
	public int sightIndex;
	public int counter, segmentsShare;
	
	//private methods
	private int getTotalSegments() {
		int totalSegments = 0;
		
		for (int i = 0; i < segmentsList.Count; i++) {
			totalSegments += segmentsList[i].Count;
		}
		
		return totalSegments;
	}
	
	void Start(){
		sightLine1.SetWidth(startWidth, endWidth);
		sightLine2.SetWidth(startWidth, endWidth);
		
		//myView = GameObject.FindWithTag("MainCamera");
		//leftWall = GameObject.FindWithTag("LeftWall");
		//rightWall = GameObject.FindWithTag("RightWall");
	}

    void FixedUpdate() {		
		mouseDragging = transform.GetComponent<DragShotMover>().GetMouseDragging();
		sightLine1.enabled = mouseDragging;
		sightLine2.enabled = false;
		
		if (mouseDragging) {
			simulatePath();
		}
    }

    /// <summary>
    /// Simulate the path of a launched object.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    void simulatePath() {
		counter = 0;
		segmentsShare = 0;

        // The first line point is wherever the player's cannon, etc is
		segmentsList.Add(new List<Vector3>());
		segmentsList[segmentsShare].Add(playerFire.transform.position);

        // The initial velocity
        Vector3 segVelocity = playerFire.forceVector * -1 * myForce * Time.deltaTime;

        // reset our hit object
        _hitObject = null;
		
		while (getTotalSegments() < segmentCount) {
			// Time it takes to traverse one segment of length segScale (careful if velocity is zero)
            float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;
			
			// Add velocity from gravity for this segment's timestep
            segVelocity = segVelocity + Physics.gravity * segTime;
			
			// Check to see if we're going to hit a physics object
            if (Physics.Raycast(segmentsList[segmentsShare][counter], segVelocity, out hit, segmentScale)) {
				// remember who we hit
				_hitObject = hit.collider;
				
				if (hit.transform.name == "LeftWall" || hit.transform.name == "RightWall") {
					// set next position to the position where we hit the wall
					newPosStore = segmentsList[segmentsShare][counter] + segVelocity.normalized * hit.distance;
					segmentsList[segmentsShare].Add(newPosStore);
					
					// continue simulating the trajectory from the other side of the screen (the opposite wall)
					newPosStore = segmentsList[segmentsShare][counter] + segVelocity * segTime;
					
					switch (hit.transform.name) {
						case "LeftWall" :
							newPosStore.x = rightWall.transform.position.x;
							break;
						case "RightWall" :
							newPosStore.x = leftWall.transform.position.x;
							break;
					}
					
					// create a new segments share, new array to make a new sightLine
					segmentsShare++;
					segmentsList.Add(new List<Vector3>());
				} else {
					// set next position to the position where we hit the physics object
					newPosStore = segmentsList[segmentsShare][counter] + segVelocity.normalized * hit.distance;
					
					// correct ending velocity, since we didn't actually travel an entire segment
	                segVelocity = new Vector3(0,0,0);
					
					// flip the velocity to simulate a bounce
	                //segVelocity = Vector3.Reflect(segVelocity, hit.normal);
					//break;
				}
            } else {
				// If our raycast hit no objects, then set the next position to the last one plus v*t
				newPosStore = segmentsList[segmentsShare][counter] + segVelocity * segTime;
            }
			
			segmentsList[segmentsShare].Add(newPosStore);
			counter = segmentsList[segmentsShare].Count - 1;
		}

        // At the end, apply our simulations to the LineRenderer
        // Set the colour of our path
        sightLine1.SetColors(lineColor, lineColor);
		sightLine1.SetVertexCount(segmentsList[0].Count);
		
		for (int a = 0; a < segmentsList[0].Count; a++) {
			sightLine1.SetPosition(a, segmentsList[0][a]);
		}
		
		if (segmentsList.Count > 1) {
			sightLine2.enabled = mouseDragging;
			sightLine2.SetColors(lineColor, lineColor);
			sightLine2.SetVertexCount(segmentsList[1].Count);
			
			for (int b = 0; b < segmentsList[1].Count; b++) {
				sightLine2.SetPosition(b, segmentsList[1][b]);
			}
		}
		
		segmentsList.Clear();
    }
}
