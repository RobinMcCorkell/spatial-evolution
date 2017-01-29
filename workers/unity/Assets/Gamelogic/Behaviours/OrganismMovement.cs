using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Improbable.Math;
using System.Collections;
using Evolution.Organism;
using Improbable.Unity;
using Improbable.Unity.Visualizer;

namespace Assets.Gamelogic.Organisms {
    public class OrganismMovement : MonoBehaviour {

        [Require]
        private Mover.Writer OrganismMoverWriter;

        int ticksTravelled = 0;
        int maxTicks;
        float tickDelay;

        public void onStart()
        {
            tickDelay = Time.deltaTime; //time between Update calls.
        }
        public void OnEnable() {
            changeDirection();
            moveOrganism();
        }

        public void Update() {
            moveOrganism();
        }

        public void moveOrganism()
        {

            maxTicks = OrganismMoverWriter.Data.timeConstant;
            if ((float)ticksTravelled / maxTicks > Random.Range(0.3f, 1.0f) * 2.0f)
            {
                changeDirection();
                ticksTravelled = 0;
            }

            var currentSpeed = OrganismMoverWriter.Data.speed;
            var currentAngle = OrganismMoverWriter.Data.angle;

            var xTranslation = currentSpeed * tickDelay * Mathf.Cos(currentAngle);
            var zTranslation = currentSpeed * tickDelay * Mathf.Sin(currentAngle);

            transform.Translate(xTranslation, 0, zTranslation, Space.World);

            Coordinates oldCoordinates = OrganismMoverWriter.Data.position;
            Coordinates newCoordinates = new Coordinates(Mathf.Clamp((float)oldCoordinates.X + xTranslation, 0, 200), 
                                                         oldCoordinates.Y,
                                                         Mathf.Clamp((float)oldCoordinates.Z + zTranslation, 0, 200));

            OrganismMoverWriter.Send(new Mover.Update().SetPosition(newCoordinates));
            ticksTravelled++;
        }

        public void changeDirection() {
            OrganismMoverWriter.Send(new Mover.Update().SetAngle(Random.Range(0f, 2 * Mathf.PI)));
        }
    }

}
