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
        float tickDelay; //time between Update calls.

        public void OnEnable() {
            changeDirection();
            moveOrganism();
        }

        public void Update() {
            moveOrganism();
        }

        public void moveOrganism() {
            maxTicks = OrganismMoverWriter.Data.timeConstant;
            if (ticksTravelled/maxTicks > Random.Range(0,1) * 2) {
                changeDirection();
                ticksTravelled = 0;
            }

            var currentSpeed = OrganismMoverWriter.Data.speed;
            var currentAngle = OrganismMoverWriter.Data.angle;

            tickDelay = Time.deltaTime;
            xTranslation = currentSpeed * tickDelay * Mathf.Cos(currentAngle);
            yTranslation = currentSpeed * tickDelay * Mathf.Sin(currentAngle);

            transform.Translate(xTranslation, yTranslation, 0, Space.World);

            Coordinates oldCoordinates = OrganismMoverWriter.Data.position;
            Coordinates newCoordinates = new Coordinates(oldCoordinates.x + xTranslation, oldCoordinates.y + yTranslation, oldCoordinates.z);

            OrganismMoverWriter.Send(new Mover.Update().SetPosition(newCoordinates));

            ticksTravelled ++;
        }

        public void changeDirection() {
            OrganismMoverWriter.Send(new Mover.Update().SetAngle(Random.Range(0f, 2 * Mathf.PI)));
        }
    }

}
