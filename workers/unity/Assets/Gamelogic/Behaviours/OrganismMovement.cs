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
        float tickDelay = 1; //time between Update calls.

        public void OnEnable() {
            changeDirection();
            moveOrganism();
        }

        public void Update() {
            moveOrganism();
        }

        public void moveOrganism() {
            maxTicks = OrganismMoverWriter.Data.timeConstant;
            if (ticksTravelled >= maxTicks) {
                changeDirection();
            }

            var currentSpeed = OrganismMoverWriter.Data.speed;
            var currentAngle = OrganismMoverWriter.Data.angle;

            transform.Translate(
                currentSpeed * tickDelay * Mathf.Cos(currentAngle),
                0,
                currentSpeed * tickDelay * Mathf.Sin(currentAngle),
                Space.World
            );

            ticksTravelled += ticksTravelled;
        }

        public void changeDirection() {
            OrganismMoverWriter.Send(new Mover.Update().SetAngle(Random.Range(1, 360)));
        }
    }

}
