using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Improbable.Math;
using System.Collections;

namespace Assets.Gamelogic.Organisms {
    public class OrganismMovement : MonoBehaviour {

        protected Evolution.Organism.Mover.Reader OrganismMoverReader;
        //private Evolution.Organism.Mover.Writer OrganismMoverWriter;

        int ticksTravelled = 0;
        float currentSpeed;
        int maxTicks;
        Coordinates currentPosition;
        float currentAngle;
        float tickDelay = 1; //time between Update calls.

        public void OnEnable() {
            changeDirection();
            moveOrganism();
        }

        public void Update() {
            moveOrganism();
        }

        public void moveOrganism() {
            
            
            maxTicks = OrganismMoverReader.Data.timeConstant;
            if (ticksTravelled >= maxTicks) {
                changeDirection();
            }

            currentSpeed = OrganismMoverReader.Data.speed;
            //currentPosition = OrganismMoverReader.Data.position;
            currentAngle = OrganismMoverReader.Data.angle;

            transform.Translate(currentSpeed * tickDelay * Mathf.Cos(currentAngle), currentSpeed * tickDelay * Mathf.Sin(currentAngle), 0, Space.World);
            //OrganismMoverWriter.Data.position.X = currentPosition.X + currentSpeed * tickDelay * Mathf.Cos(currentAngle);
            //OrganismMoverWriter.Data.position.Y = currentPosition.Y + currentSpeed * tickDelay * Mathf.Sin(currentAngle);

            ticksTravelled += ticksTravelled;


        }

        public void changeDirection() {
            currentAngle = Random.Range(1, 360);
        }
    }

 
}