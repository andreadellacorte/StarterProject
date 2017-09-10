using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using System;
using System.Collections;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class DyingBehaviour : MonoBehaviour
    {
        // Inject access to the entity's Health component
        [Require] private Health.Writer HealthWriter;
        [Require] private Position.Writer PositionWriter;

        private PlayerMover playerMover;

        public bool isDead = false;

        public int currentHealth;

        private void OnEnable() {
            isDead = false;
            InitializeDyingAnimation();

            // Register callback for when components change
            HealthWriter.HealthUpdated.Add(OnHealthUpdated);

            currentHealth = HealthWriter.Data.health;

            playerMover = GetComponent<PlayerMover>();
            if (playerMover == null) {
              Debug.LogError("PlayerInputSender not found.");
            }
        }

        private void OnDisable() {
            // Deregister callback for when components change
            HealthWriter.HealthUpdated.Remove(OnHealthUpdated);
        }

        private void Update() {
            if (PositionWriter.Data.coords.ToUnityVector().y < - 50) {
                Die();
            }
        }

        private void InitializeDyingAnimation() {
            /*
             * DyingAnimation is triggered when the ship is first killed. But a worker which checks out
             * the entity after this time (for example, a client connecting to the game later)
             * must not visualize the ship as still alive.
             *
             * Therefore, on checkout, any sunk ships jump to the end of the sinking animation.
             */
            if (HealthWriter.Data.health <= 0) {
                isDead = true;
            }
        }

        // Callback for whenever the CurrentHealth property of the Health component is updated
        private void OnHealthUpdated(int newHealth)
        {
            currentHealth = newHealth;

            if (!isDead && currentHealth <= 0) {
                Die();
                isDead = true;
            }
        }

        private void Die() {
            // Lock controls
            if (playerMover != null) {
                playerMover.HasControl(false);
            }

            // Respawn and regain controls after x secs
            StartCoroutine(DelayedAction(Respawn, 4f));
        }

        private IEnumerator DelayedAction(Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action();
        }

        private void Respawn() {
            // Initialise player
            isDead = false;
            HealthWriter.Send(new Health.Update().SetHealth(SimulationSettings.PlayerSpawnHealth));

            // Respawn character
            playerMover.Respawn();

            // Regain controls
            playerMover.HasControl(true);
        }
    }
}