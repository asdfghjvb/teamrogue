using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay : MonoBehaviour
{
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
    }

    public class Triangle
    {
        public Edge AB;
        public Edge BC;
        public Edge CA;

        CircumCircle circumCircle;

        public Triangle(Vector2 vA, Vector2 vB, Vector2 vC)
        {
            AB = new Edge(vA, vB);
            BC = new Edge(vB, vC);
            CA = new Edge(vC, vA);

            circumCircle = new CircumCircle(vA, vB, vC);
        }
    }
}
