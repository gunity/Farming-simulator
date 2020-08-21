using System.Collections.Generic;
using Data;
using Extension;
using Object;
using Unity.Mathematics;
using UnityEngine;

namespace Manager
{
    public class GroundManager : Singleton<GroundManager>
    {
        public GameObject groundPrefab;
        public GroundData groundData;
        public int numberOfShifts = 1;
        public Vector2 shiftDistance;

        public List<Ground> Grounds { get; } = new List<Ground>();

        private void Start()
        {
            Generate();
        }

        public void Expand()
        {
            numberOfShifts += 1;
            Generate();
        }

        private void Generate()
        {
            if (Grounds.Count > 0)
            {
                Grounds.ForEach(ground => { Destroy(ground.gameObject); });
                Grounds.Clear();
            }

            for (var j = 0; j >= -numberOfShifts + 1; j--)
            for (var i = 0; i < numberOfShifts; i++)
            {
                var yIntPosition = j + numberOfShifts - 1 - i;
                var position = new Vector3((j + i) * shiftDistance.x, shiftDistance.y * yIntPosition);
                var ground = Instantiate(groundPrefab, position, quaternion.identity, transform).GetComponent<Ground>();
                ground.GroundData = groundData;
                ground.groundSpriteRenderer.sortingOrder = -yIntPosition;
                ground.weedSpriteRenderer.sortingOrder = -yIntPosition + 1;
                Grounds.Add(ground);
            }
        }
    }
}