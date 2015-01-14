using UnityEngine;
using System.Collections;

public class LineBuilder : MonoBehaviour {

	public GameObject template;

	// Use this for initialization
	void Start () {
		BuildEdges ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void BuildEdges() {
		ArrayList edges = new ArrayList();

		foreach (Transform child in transform) {
			MeshFilter filter = child.GetComponent<MeshFilter>();
			Vector3[] vertices = filter.mesh.vertices;
			int[] triangles = filter.mesh.triangles;

			for(int index=0; index<triangles.Length-1; index+=3) {
				Vector3 v1 = child.transform.TransformPoint(vertices[triangles[index]]);
				Vector3 v2 = child.transform.TransformPoint(vertices[triangles[index+1]]);
				Vector3 v3 = child.transform.TransformPoint(vertices[triangles[index+2]]);
				edges.Add(new Edge(v1, v2));
				edges.Add(new Edge(v2, v3));
				edges.Add(new Edge(v3, v1));
			}
			//child.transform.TransformPoint()
			// Collect edges from triangles (world-space translated)
			// Compare edges for overlap 
			//   - eliminate shared edges for coplanar triangles
			//   - de-dup shared edges for non-coplanar triangles
			// Build LineRenderers for all remaining edges
			
		}

		foreach (Edge edge in edges) {
			GameObject n = Instantiate(template) as GameObject;
			LineRenderer l = n.GetComponent<LineRenderer>();
			l.SetPosition(0, edge.start);
			l.SetPosition(1, edge.end);
		}
	}
}

public class Edge {
	public Vector3 start;
	public Vector3 end;

	public Edge(Vector3 s, Vector3 e) {
		start = s; end = e;
	}

	public bool Equals(Edge other) {
		if (other.start.Equals(start) && other.end.Equals(end)) {
			return true;
		} else {
			return false;
		}
	}

	public Vector3 direction() {
		return (end - start).normalized;
	}

	public bool parallel(Edge other) {
		return direction().Equals(other.direction());
	}
}
