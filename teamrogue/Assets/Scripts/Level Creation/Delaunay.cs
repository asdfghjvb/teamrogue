using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Delaunay
{
    public HashSet<Triangle> triangles { get; private set; }

    public List<Edge> uniqueEdges { get; private set; }

    private Vector3 origin;

    public class Edge
    {
        public Vector2 A;
        public Vector2 B;

        public Edge(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        public static bool operator ==(Edge edge1, Edge edge2)
        {
            if (edge1.A == edge2.A && edge1.B == edge2.B)
                return true;
            else if (edge1.A == edge2.B && edge1.B == edge2.A)
                return true;
            else
                return false;
        }

        public static bool operator !=(Edge edge1, Edge edge2)
        {
            if (edge1.A == edge2.A && edge1.B == edge2.B)
                return false;
            else if (edge1.A == edge2.B && edge1.B == edge2.A)
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Edge)
                return false;

            Edge other = (Edge)obj;
            if (this == other)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                unchecked
                {
                    int hash = 53;

                    /*Gets the lower hash value and adds it to the hash first. This ensures 
                    that the Edges containing the same two points but in different vars will
                    still have the same hash, since they are effectivly the same edge*/

                    Vector2 smaller = A;
                    Vector2 larger = B;
                    if (B.GetHashCode() < A.GetHashCode())
                    {
                        smaller = B;
                        larger = A;
                    }

                    hash = hash * 17 + smaller.GetHashCode();
                    hash = hash * 17 + larger.GetHashCode();

                    return hash;
                }
            }
        }

        public bool ContainsVertex(Vector2 vertex)
        {
            if (A == vertex || B == vertex)
                return true;
            else
                return false;
        }

        public void DebugDrawEdge()
        {
            Debug.DrawLine(new Vector3(A.x, 0, A.y), new Vector3(B.x, 0, B.y));
        }
    }

    public class CircumCircle
    {
        public Vector2 center;
        public float radius;

        public CircumCircle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 midAB = (p1 + p2) / 2;
            Vector2 midBC = (p2 + p3) / 2;

            float slopeAB = (p2.y - p1.y) / (p2.x - p1.x);
            float slopeBC = (p3.y - p2.y) / (p3.x - p2.x);

            float centerX = (midAB.y - midBC.y + slopeBC * midBC.x - slopeAB * midAB.x) / (slopeAB - slopeBC);
            float centerY = midAB.y - slopeAB * (centerX - midAB.x);

            center = new Vector2(centerX, centerY);
            radius = Vector2.Distance(center, p1);
        }

        public static bool operator==(CircumCircle cir1, CircumCircle cir2)
        {
            //uses a "basically equal" with epsilon
            const float epsilon = 0.001f;
            if (Mathf.Abs(cir1.center.x - cir2.center.x) < epsilon &&
                Mathf.Abs(cir1.center.y - cir2.center.y) < epsilon &&
                Mathf.Abs(cir1.radius - cir2.radius) < epsilon)
                return true;
            else
                return false;
        }

        public static bool operator !=(CircumCircle cir1, CircumCircle cir2)
        {
            const float epsilon = 0.001f;
            if (Mathf.Abs(cir1.center.x - cir2.center.x) < epsilon &&
                Mathf.Abs(cir1.center.y - cir2.center.y) < epsilon &&
                Mathf.Abs(cir1.radius - cir2.radius) < epsilon)
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is not CircumCircle)
                return false;

            CircumCircle other = (CircumCircle)obj;
            if (this == other)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 61;
                hash = hash * 11 + center.GetHashCode();
                hash = hash * 11 + radius.GetHashCode();

                return hash;
            }
        }

        public bool Contains(Vector2 p)
        {
            if (p.x < center.x + radius
                && p.x > center.x - radius
                && p.y < center.y + radius
                && p.y > center.y - radius)
                return true;
            else
                return false;
        }
    }

    public class Triangle
    {
        public Edge AB;
        public Edge BC;
        public Edge CA;

        public CircumCircle circumCircle { get; }

        public Triangle(Vector2 vA, Vector2 vB, Vector2 vC)
        {
            AB = new Edge(vA, vB);
            BC = new Edge(vB, vC);
            CA = new Edge(vC, vA);

            circumCircle = new CircumCircle(vA, vB, vC);
        }

        public Triangle(Edge ab, Edge bc, Edge ca)
        {
            if(ab.A != ca.B)
            {
                Debug.LogError("Invalid triangle creation attempted");
            }

            AB = ab;
            BC = bc;
            CA = ca;

            circumCircle = new CircumCircle(ab.A, ab.B, bc.B);
        }

        public Triangle(Edge ab, Vector2 c)
        {
            AB = ab;
            BC = new Edge(ab.B, c);
            CA = new Edge(c, ab.A);

            circumCircle = new CircumCircle(ab.A, ab.B, c);
        }

        public static bool operator==(Triangle tri1, Triangle tri2)
        {
            if ((tri1.AB == tri2.AB || tri1.AB == tri2.BC || tri1.AB == tri2.CA) &&
                (tri1.BC == tri2.AB || tri1.BC == tri2.BC || tri1.BC == tri2.CA) &&
                (tri1.CA == tri2.AB || tri1.CA == tri2.BC || tri1.CA == tri2.CA))
                return true;
            else
                return false;
        }

        public static bool operator !=(Triangle tri1, Triangle tri2)
        {
            if ((tri1.AB == tri2.AB || tri1.AB == tri2.BC || tri1.AB == tri2.CA) &&
                (tri1.BC == tri2.AB || tri1.BC == tri2.BC || tri1.BC == tri2.CA) &&
                (tri1.CA == tri2.AB || tri1.CA == tri2.BC || tri1.CA == tri2.CA))
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Triangle)
                return false;

            Triangle other = (Triangle)obj;
            if (this == other)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashAB = AB.GetHashCode();
                int hashBC = BC.GetHashCode();
                int hashCA = CA.GetHashCode();

                int[] hashes = { hashAB, hashBC, hashCA };
                Array.Sort(hashes);

                int hash = 43;
                foreach(int num in hashes)
                {
                    hash = hash * 59 + num;
                }

                return hash;
            }
        }

        public HashSet<Edge> SharedEdges(Triangle other)
        {
            HashSet<Edge> sharedEdges = new();

            if (AB == other.AB || AB == other.BC || AB == other.CA)
                sharedEdges.Add(AB);

            if (BC == other.AB || BC == other.BC || BC == other.CA)
                sharedEdges.Add(BC);

            if (CA == other.AB || CA == other.BC || CA == other.CA)
                sharedEdges.Add(CA);

            return sharedEdges;
        }

        public bool ContainsVertex(Vector2 vertex)
        {
            if (AB.ContainsVertex(vertex) || BC.ContainsVertex(vertex) || CA.ContainsVertex(vertex))
                return true;
            else
                return false;
        }

        public bool HasEdge(Edge edge)
        {
            if (AB == edge || BC == edge || CA == edge)
                return true;
            else
                return false;
        }

        public List<Vector2> GetVertices()
        {
            List<Vector2> vertices = new()
            {
                AB.A,
                AB.B,
                CA.A
            };

            return vertices;
        }

        public List<Edge> GetEdges()
        {
            List<Edge> edges = new()
            {
                AB,
                BC,
                CA
            };

            return edges;
        }

        public void DebugDrawTriangle()
        {
            AB.DebugDrawEdge();
            BC.DebugDrawEdge();
            CA.DebugDrawEdge();
        }
    }

    public Delaunay(List<Vector2> points, Vector2 areaSize, Vector3 graphOrigin)
    {
        triangles = new();
        origin = graphOrigin;

        Triangle superTriangle = CreateSuperTriangle(areaSize, new Vector2(origin.x, origin.z));
        triangles.Add(superTriangle);

        Debug.Log("Amount of triangles before entering loop: " + triangles.Count);

        foreach (Vector2 point in points)
        {
            HashSet<Triangle> newTriangles = new();
            List<Triangle> badTriangles = new();

            /* Find bad triangles */

            foreach (Triangle triangle in triangles)
            {
                if (triangle.circumCircle.Contains(point))
                {
                    badTriangles.Add(triangle);
                }
            }

            /* Remove bad triangles */

            foreach (Triangle triangle in badTriangles)
            {
                triangles.Remove(triangle);
            }

            /* Find polygonal hole */

            HashSet<Vector2> badTriangleVertices = new();

            foreach (Triangle triangle in badTriangles)
            {
                List<Vector2> vertices = triangle.GetVertices();
                foreach (Vector2 vertex in vertices)
                {
                    badTriangleVertices.Add(vertex);
                }
            }

            /* Connect point to polgonal hole */

            List<Vector2> badTriangleVerticesList = new List<Vector2>(badTriangleVertices);
            for (int i = 0; i < badTriangleVertices.Count; i++)
            {
                for (int j = i + 1; j < badTriangleVertices.Count; j++)
                {
                    newTriangles.Add(new Triangle(point, badTriangleVerticesList[i], badTriangleVerticesList[j]));
                }
            }

            /* Add new triagles to the main triangle list */

            triangles.UnionWith(newTriangles);

            //Debug.Log("Point: " + point + ", Bad Triangles: " + badTriangles.Count + ", New triangles: " + newTriangles.Count + "\nTotal triangles: " + triangles.Count);

        }

        /* Remove the super triangle */

        List<Vector2> superVertices = superTriangle.GetVertices();
        List<Triangle> superTriangleConnections = new();

        foreach (Triangle triangle in triangles)
        {
            foreach (Vector2 vertex in superVertices)
            {
                if (triangle.ContainsVertex(vertex))
                {
                    superTriangleConnections.Add(triangle);
                }
            }
        }

        foreach (Triangle triangle in superTriangleConnections)
        {
            triangles.Remove(triangle);
        }

        //Debug Code
        foreach (Triangle triangle in triangles)
        {
            Debug.Log("Center: " + triangle.circumCircle.center + " Radius: " + triangle.circumCircle.radius);
        }

        //Debug.Log("After removing super triangle connections: " + triangles.Count);
    }

    private Triangle CreateSuperTriangle(Vector2 areaSize, Vector2 areaOrigin)
    {
        const float scaler = 2; //Increase half diagonal by this amount to ensure large enough super triangle. 2 should work fine but for future proofing it's a var
        Vector2 areaCenter = new Vector2(areaOrigin.x + (areaSize.x / 2f), areaOrigin.y + (areaSize.y / 2f));

        Vector2 v1 = new Vector2(areaCenter.x, areaOrigin.y + (areaSize.y * scaler)); 
        Vector2 v2 = new Vector2(areaOrigin.x - (areaSize.x * scaler), areaOrigin.y); 
        Vector2 v3 = new Vector2(areaOrigin.x + areaSize.x + (areaSize.x * scaler), areaOrigin.y);

        Debug.DrawLine(new Vector3(v1.x, origin.y, v1.y), new Vector3(v2.x, origin.y, v2.y), Color.green, 1000f);
        Debug.DrawLine(new Vector3(v2.x, origin.y, v2.y), new Vector3(v3.x, origin.y, v3.y), Color.green, 1000f);
        Debug.DrawLine(new Vector3(v3.x, origin.y, v3.y), new Vector3(v1.x, origin.y, v1.y), Color.green, 1000f);

        return new Triangle(v1, v2, v3);
    }

    public void DebugDrawDelauny()
    {
       foreach(Triangle tri in triangles)
        {
            tri.DebugDrawTriangle();
        }
    }
}
