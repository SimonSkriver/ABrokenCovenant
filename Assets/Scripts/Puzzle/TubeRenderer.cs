// Author: Mathias Soeholm
// Date: 05/10/2016
// No license, do whatever you want with this script

// Project note:
// After getting MultiBeamScript to work with LineRenderer and multiple failed attempts
// at making a material that would look good from both the side and when directly behind it
// Researching a better alternative quickly pointed towards this work by Mathias Soeholm.
// When tested the mesh renderer seemed to work quite well and replacing it with our own
// script became a secondary priority as there was much else work to do. In the end it made 
// it to our final product and we thank Mathias Soeholm for his unknowingly great contribution

using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class TubeRenderer : MonoBehaviour
{
	// Array of points the tube should follow. 
	[SerializeField] Vector3[] _positions;

	// Number of sides used to approximate the tube's circular shape.
	// Higher values = smoother tube, but also more vertices.
	[SerializeField] int _sides;

	// Start radius of the tube.
	[SerializeField] float _radiusOne;

	// End radius of the tube, only used if _useTwoRadii is true.
	[SerializeField] float _radiusTwo;

	// If true, input positions are treated as world-space positions
	// and converted into local space before being assigned to the mesh.
	[SerializeField] bool _useWorldSpace = true;

	// If true, the tube can change radius from _radiusOne to _radiusTwo.
	[SerializeField] bool _useTwoRadii = false;
	
	// Stores the generated mesh vertices.
	private Vector3[] _vertices;

	// The runtime-generated mesh used for the tube.
	private Mesh _mesh;

	// MeshFilter holds the mesh data.
	private MeshFilter _meshFilter;

	// MeshRenderer handles rendering the mesh with a material.
	private MeshRenderer _meshRenderer;

	// Shortcut property for getting/setting the renderer material.
	public Material material
	{
		get { return _meshRenderer.material; }
		set { _meshRenderer.material = value; }
	}

	void Awake()
	{
		// Ensure the object has a MeshFilter component.
		_meshFilter = GetComponent<MeshFilter>();
		if (_meshFilter == null)
		{
			_meshFilter = gameObject.AddComponent<MeshFilter>();
		}

		// Ensure the object has a MeshRenderer component.
		_meshRenderer = GetComponent<MeshRenderer>();
		if (_meshRenderer == null)
		{
			_meshRenderer = gameObject.AddComponent<MeshRenderer>();
		}

		// Create a fresh mesh and assign it to the MeshFilter.
		_mesh = new Mesh();
		_meshFilter.mesh = _mesh;
	}

	private void OnEnable()
	{
		// Make sure the renderer is visible when enabled.
		_meshRenderer.enabled = true;
	}

	private void OnDisable()
	{
		// Hide the renderer when disabled.
		_meshRenderer.enabled = false;
	}

	void Update ()
	{
		// Empty, only updates when SetPostions() is called.
	}

	private void OnValidate()
	{
		// Prevent invalid values.
		// A tube needs at least 3 sides to form a valid closed shape.
		_sides = Mathf.Max(3, _sides);
	}

	// Public method for assigning the tube path.
	// We use this in MultiBeamScript to constantly update the mesh after getting the correct direction
	public void SetPositions(Vector3[] positions)
	{
		_positions = positions;
		GenerateMesh();
	}

	private void GenerateMesh()
	{
		// If the mesh is missing or there are too few points to form a tube,
		// create/reset the mesh and stop.
		if (_mesh == null || _positions == null || _positions.Length <= 1)
		{
			_mesh = new Mesh();
			return;
		}

		// Total number of vertices = number of path points * number of sides per ring.
		var verticesLength = _sides*_positions.Length;

		// Rebuild vertex array and dependent mesh data only if size changed.
		if (_vertices == null || _vertices.Length != verticesLength)
		{
			_vertices = new Vector3[verticesLength];

			// Generate triangle indices and UVs once when topology changes.
			var indices = GenerateIndices();
			var uvs = GenerateUVs();

			// Unity can sometimes complain if triangles reference vertices not yet assigned,
			// so order is switched depending on current mesh size.
			if (verticesLength > _mesh.vertexCount)
			{
				_mesh.vertices = _vertices;
				_mesh.triangles = indices;
				_mesh.uv = uvs;
			}
			else
			{
				_mesh.triangles = indices;
				_mesh.vertices = _vertices;
				_mesh.uv = uvs;
			}
		}

		var currentVertIndex = 0;

		// For each point along the path, generate one circular ring of vertices.
		for (int i = 0; i < _positions.Length; i++)
		{
			var circle = CalculateCircle(i);
			foreach (var vertex in circle)
			{
				// Convert to local space if positions were provided in world space.
				_vertices[currentVertIndex++] = _useWorldSpace ? transform.InverseTransformPoint(vertex) : vertex;
			}
		}

		// Apply final vertex positions to the mesh.
		_mesh.vertices = _vertices;

		// Recalculate lighting normals and bounding box.
		_mesh.RecalculateNormals();
		_mesh.RecalculateBounds();

		// Reassign mesh to ensure MeshFilter uses the updated version.
		_meshFilter.mesh = _mesh;
	}

	private Vector2[] GenerateUVs()
	{
		// UV layout:
		// U = around the circle
		// V = along the tube length
		var uvs = new Vector2[_positions.Length*_sides];

		for (int segment = 0; segment < _positions.Length; segment++)
		{
			for (int side = 0; side < _sides; side++)
			{
				var vertIndex = (segment * _sides + side);
				var u = side/(_sides-1f);
				var v = segment/(_positions.Length-1f);

				uvs[vertIndex] = new Vector2(u, v);
			}
		}

		return uvs;
	}

	private int[] GenerateIndices()
	{
		// Each quad between two rings is made of 2 triangles = 6 indices.
		// One full ring connection contains _sides quads.
		var indices = new int[_positions.Length*_sides*2*3];

		var currentIndicesIndex = 0;

		// Start at segment 1 because each segment connects to the previous one.
		for (int segment = 1; segment < _positions.Length; segment++)
		{
			for (int side = 0; side < _sides; side++)
			{
				var vertIndex = (segment*_sides + side);
				var prevVertIndex = vertIndex - _sides;

				// Triangle one
				indices[currentIndicesIndex++] = prevVertIndex;
				indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
				indices[currentIndicesIndex++] = vertIndex;
				

				// Triangle two
				indices[currentIndicesIndex++] = (side == _sides - 1) ? (prevVertIndex - (_sides - 1)) : (prevVertIndex + 1);
				indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
				indices[currentIndicesIndex++] = prevVertIndex;
			}
		}

		return indices;
	}

	private Vector3[] CalculateCircle(int index)
	{
		var dirCount = 0;
		var forward = Vector3.zero;

		// If this is not the first point, include direction from previous point to current.
		if (index > 0)
		{
			forward += (_positions[index] - _positions[index - 1]).normalized;
			dirCount++;
		}

		// If this is not the last point, include direction from current point to next.
		if (index < _positions.Length-1)
		{
			forward += (_positions[index + 1] - _positions[index]).normalized;
			dirCount++;
		}

		// Average the connected segment directions to get a smoother forward direction.
		forward = (forward/dirCount).normalized;

		// Build a local coordinate frame around the forward direction.
		// The random-looking vector helps avoid a zero cross product if forward aligns with a common axis.
		var side = Vector3.Cross(forward, forward+new Vector3(.123564f, .34675f, .756892f)).normalized;
		var up = Vector3.Cross(forward, side).normalized;

		var circle = new Vector3[_sides];
		var angle = 0f;
		var angleStep = (2*Mathf.PI)/_sides;

		// t is used for radius interpolation along the tube length.
		var t = index / (_positions.Length-1f);

		// Either use one constant radius or interpolate between two radii.
		var radius = _useTwoRadii ? Mathf.Lerp(_radiusOne, _radiusTwo, t) : _radiusOne;

		// Generate points around a circle in the local side/up plane.
		for (int i = 0; i < _sides; i++)
		{
			var x = Mathf.Cos(angle);
			var y = Mathf.Sin(angle);

			circle[i] = _positions[index] + side*x* radius + up*y* radius;

			angle += angleStep;
		}

		return circle;
	}
}