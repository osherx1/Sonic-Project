using UnityEngine;
using System.Collections;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        private float distanceTravelled;
        private bool isWaiting = false;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribe to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null && !isWaiting)
            {
                distanceTravelled += speed * Time.deltaTime;

                // Get the total length of the path
                float pathLength = pathCreator.path.length;

                // Check if the object is at the start or end of the path
                if (distanceTravelled >= pathLength || distanceTravelled <= 0)
                {
                    StartCoroutine(WaitAtEdge());
                }
                else
                {
                    // Move the object along the path
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                }
            }
        }

        private IEnumerator WaitAtEdge()
        {
            isWaiting = true;

            // Adjust the distance if it's at the edge
            if (distanceTravelled >= pathCreator.path.length)
            {
                distanceTravelled = pathCreator.path.length;
            }
            else if (distanceTravelled <= 0)
            {
                distanceTravelled = 0;
            }

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Reverse direction if the instruction is set to reverse
            if (endOfPathInstruction == EndOfPathInstruction.Reverse)
            {
                speed = -speed; // Reverse the speed
            }

            isWaiting = false;
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}
