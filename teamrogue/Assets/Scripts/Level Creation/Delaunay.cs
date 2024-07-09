using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay : MonoBehaviour
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
    }

    public Delaunay(List<Vector2> points, Vector2 areaSize, Vector3 graphOrigin)
    {
        triangles = new();
        origin = graphOrigin;

        Triangle superTriangle = CreateSuperTriangle(areaSize, new Vector2(origin.x, origin.z));
        triangles.Add(superTriangle);

        foreach(Vector2 point in points)
        {
            HashSet<Triangle> newTriangles = new();
            List<Triangle> badTriangles = new();

            foreach(Triangle tri in triangles)
            {
                if (tri.circumCircle.Contains(point))
                {
                    badTriangles.Add(tri);

                    HashSet<Edge> polygonalHoleEdges = new();

                    foreach (Triangle potNeighbor in triangles)
                    {
                        HashSet<Edge> sharedEdges = tri.SharedEdges(potNeighbor);
                        polygonalHoleEdges.UnionWith(sharedEdges);
                    }

                    foreach (Edge edge in polygonalHoleEdges)
                    {
                        newTriangles.Add(new Triangle(edge, point));
                    }
                }
            }

            foreach(Triangle triangle in badTriangles)
            {
                triangles.Remove(triangle);
            }
            triangles.UnionWith(newTriangles);
        }

        foreach(Triangle triangle in triangles)
        {
            //remove connections formed with super triangle
        }
    }

    private Triangle CreateSuperTriangle(Vector2 areaSize, Vector2 areaOrigin)
    {
        const float scaler = 2.5f; //Increase half diagonal by this amount to ensure large enough super triangle. 2 should work fine but for future proofing it's a var
        Vector2 areaCenter = new Vector2(areaOrigin.x + (areaSize.x / 2f), areaOrigin.y + (areaSize.y / 2f));
        float halfDiagonal = Mathf.Sqrt((areaSize.x * areaSize.x) + (areaSize.y * areaSize.y)) / 2; // (sqrt of (W^2 + H^2)) / 2. basically the radius of a rect

        float size = halfDiagonal * scaler;

        Vector2 v1 = new Vector2(areaCenter.x, areaCenter.y + size); 
        Vector2 v2 = new Vector2(areaCenter.x - size,areaCenter.y - size); 
        Vector2 v3 = new Vector2(areaCenter.x + size, areaCenter.y - size);

        Debug.DrawLine(new Vector3(v1.x, origin.y, v1.y), new Vector3(v2.x, origin.y, v2.y), Color.green, 1000f);
        Debug.DrawLine(new Vector3(v2.x, origin.y, v2.y), new Vector3(v3.x, origin.y, v3.y), Color.green, 1000f);
        Debug.DrawLine(new Vector3(v3.x, origin.y, v3.y), new Vector3(v1.x, origin.y, v1.y), Color.green, 1000f);

        return new Triangle(v1, v2, v3);
    }
}
