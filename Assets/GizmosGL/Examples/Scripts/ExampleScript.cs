using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGL;

namespace GGL.Examples
{
    [System.Serializable]
    public class CircleSettings
    {
        public Vector3 position = new Vector3(-15, 0, 0);
        public Vector3 up = Vector3.forward;
        public float radius = 0.5f;
        public int segments = 16;
        public Color color = Color.red;
    }

    [System.Serializable]
    public class SquareSettings
    {
        public Vector3 position = new Vector3(-10, 0, 0);
        public Vector2 scale = Vector3.one;
        public Quaternion rotation;
        public Color color = Color.green;
    }

    [System.Serializable]
    public class CubeSettings
    {
        public Vector3 position = new Vector3(-5, 0, 0);
        public Vector3 scale = Vector3.one * 0.5f;
        public Quaternion rotation;
        public Color color = Color.blue;
    }

    [System.Serializable]
    public class SphereSettings
    {
        public Vector3 position = new Vector3(0, 0, 0);
        public float radius = 0.5f;
        public Quaternion rotation;
        public Color color = Color.cyan;
    }

    [System.Serializable]
    public class CylinderSettings
    {
        public Vector3 position = new Vector3(5, 0, 0);
        public float radius = 0.5f;
        public Vector3 scale = Vector3.one;
        public Quaternion rotation;
        public float halfLength = 1f;
        public int segments = 16;
        public Color color = Color.magenta;
    }

    [System.Serializable]
    public class RingSettings
    {
        public Vector3 position = new Vector3(10, 0, 0);
        public Vector3 up = Vector3.up;
        public float innerRadius = 0.5f;
        public float outerRadius = 1;
        public Vector2 scale = Vector2.one;
        public Vector3 rotation;
        public int segments = 16;
        public Color color = Color.yellow;
    }

    [System.Serializable]
    public class ArcSettings
    {
        public Vector3 position = new Vector3(15, 0, 0);
        public Vector3 up = Vector3.up;
        public float radius = 1f;
        public Quaternion rotation;
        public float halfAngle = 90f;
        public int segments = 16;
        public Color color = Color.gray;
    }


    [System.Serializable]
    public class LineSettings
    {
        public Vector3 start = new Vector3(-15, 5, 0);
        public Vector3 end = new Vector3(15, 5, 0);
        public float startWidth = 0.5f;
        public float endWidth = 0.5f;
        public Color startColor = Color.red;
        public Color endColor = Color.blue;
    }

    public class ExampleScript : MonoBehaviour
    {
        public CircleSettings circle;
        public SquareSettings square;
        public CubeSettings cube;
        public SphereSettings sphere;
        public CylinderSettings cylinder;
        public RingSettings ring;
        public ArcSettings arc;
        public LineSettings line;

        private void Update()
        {

            //if (Input.GetKey(KeyCode.R))
            //{
            //    Line spawn = GizmosGL.AddLine(Vector3.zero, Vector3.one * 10f, 0.1f, 0.1f);
            //}

            for (int i = 0; i < 500; i++)
            {
                Vector3 pos = new Vector3(Mathf.Sin(i) * 9f, i * 0.1f, Mathf.Cos(i) * 9f);
                Cube spawn = GizmosGL.AddCube(pos);
                spawn.isRigidbodyEnabled = true;
                spawn.isCollisionEnabled = true;
                //spawn.rigidbody.useGravity = false;
                //spawn.isCollisionEnabled = true;
                spawn.color = Color.Lerp(Color.red, Color.blue, (float)i / 800f);
            }

            Vector3 prevPos = transform.position;
            for (int i = 0; i < 200; i++)
            {
                Vector3 pos = prevPos + new Vector3(Mathf.Tan(i) * 2f, i * 0.1f);
                Line line = GizmosGL.AddLine(prevPos, pos, 1f, 1f);
                line.color = Color.Lerp(Color.red, Color.blue, (float)i / 200f);
                line.startColor = Color.red;
                line.endColor = Color.blue;
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;

                prevPos = pos;
            }
            for (int i = 0; i < 200; i++)
            {
                Circle spawn = GizmosGL.AddCircle(new Vector3(Mathf.Sin(i) * 2f, i * 0.1f), circle.radius, Quaternion.identity, circle.segments, circle.color);
                spawn.isRigidbodyEnabled = true;
                spawn.name = "FixedUpdate - Circle";
            }

            for (int i = 0; i < 200; i++)
            {
                Square spawn = GizmosGL.AddSquare(new Vector3(Mathf.Sin(i) * 2f, i * 1f), new Vector2(1, 1));
                spawn.color = Color.blue;
                //spawn.isRigidbodyEnabled = true;
                spawn.name = "FixedUpdate - Circle";
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //  GizmosGL.color = Color.blue;
            //GizmosGL.AddCube(Vector3.right * 2, new Vector3(1, 0.5f, 1), Quaternion.identity, Color.red);
            GizmosGL.AddRing(ring.position, ring.innerRadius, ring.outerRadius, Quaternion.Euler(ring.rotation), ring.scale, ring.segments, ring.color);
            GizmosGL.AddArc(arc.position, arc.radius, arc.halfAngle, arc.rotation, arc.segments, arc.color);
            GizmosGL.AddSphere(sphere.position, sphere.radius, sphere.rotation, sphere.color);
            GizmosGL.AddSquare(square.position, square.scale, square.rotation);
            GizmosGL.AddCircle(circle.position, circle.radius, Quaternion.identity, circle.segments, circle.color);
            GizmosGL.AddCube(cube.position, cube.scale, cube.rotation);
            GizmosGL.AddCube(cube.position, cube.scale, cube.rotation);
            GizmosGL.AddCylinder(cylinder.position, cylinder.radius, cylinder.scale, cylinder.rotation, cylinder.halfLength, cylinder.segments, cylinder.color);
            GizmosGL.AddLine(line.start, line.end, line.startWidth, line.endWidth, line.startColor, line.endColor);

        }
    }
}