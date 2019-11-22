using PolyWars.API;
using System;

namespace PolyWars.Logic {
    class InputController {

        /// <summary>
        /// Grabs input from a player
        /// </summary>
        private Input input;

        /// <summary>
        /// Initializes input
        /// </summary>
        public void initInput() {
            input = new Input();
        }

        public IShape applyInput( IShape player ) {
            // TODO Grab input int user and covert to forwardVelocity, backwardsVelocity and rotationVelocity
            int Movement = input.Movement;

            int forwardVelocity = 0;
            int backwardVelocity = 0;
            int turningLeftVelocity = 0;
            int turningRightVelocity = 0;


            if( Movement == 2 ) {
                // Accelerating Forward
                forwardVelocity = 10;
                backwardVelocity = 0;

            } else if( Movement == 8 ) {
                // Accelerating Backwards
                backwardVelocity = 10;
                forwardVelocity = 0;

            } else if( Movement == 4 ) {
                // Turn Left
                turningLeftVelocity = 10;
                turningRightVelocity = 0;

            } else if( Movement == 1 ) {
                // Turn Right
                turningRightVelocity = 10;
                turningLeftVelocity = 0;

            } else if( Movement == 3 ) {
                // Turn Right and forward
                turningRightVelocity = 10;
                turningLeftVelocity = 0;
                forwardVelocity = 10;
                backwardVelocity = 0;

            } else if( Movement == 6 ) {
                // Turn Left and forward
                turningRightVelocity = 0;
                turningLeftVelocity = 10;
                forwardVelocity = 10;
                backwardVelocity = 0;

            } else if( Movement == 9 ) {
                // Turn Right and backwards
                turningRightVelocity = 10;
                turningLeftVelocity = 0;
                forwardVelocity = 0;
                backwardVelocity = 10;

            } else if( Movement == 12 ) {
                // Turn Left and backwards
                turningRightVelocity = 0;
                turningLeftVelocity = 10;
                forwardVelocity = 0;
                backwardVelocity = 10;

            } else if( Movement == 0 ) {
                // Stop Moving
                turningRightVelocity = 0;
                turningLeftVelocity = 0;
                forwardVelocity = 0;
                backwardVelocity = 0;

            } else {
                // Invalid keys pressed
            }

            // Apply Velocities to Player

            player.ForwardVelocity = forwardVelocity;
            player.BackwardVelocity = backwardVelocity;
            player.TurnRightVelocity = turningRightVelocity;
            player.TurnLeftVelocity = turningLeftVelocity;

            return player;
        }
    }
}