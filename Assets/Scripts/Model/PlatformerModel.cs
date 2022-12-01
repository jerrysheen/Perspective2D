using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    /// <summary>
    /// The main model containing needed data to implement a platformer style 
    /// game. This class should only contain data, and methods that operate 
    /// on the data. It is initialised with data in the GameController class.
    /// </summary>
    [System.Serializable]
    public class PlatformerModel : MonoBehaviour
    {
        /// <summary>
        /// The virtual camera in the scene.
        /// </summary>
        public Cinemachine.CinemachineVirtualCamera virtualCamera;

        /// <summary>
        /// The main component which controls the player sprite, controlled 
        /// by the user.
        /// </summary>
        public PlayerController player;

        /// <summary>
        /// The spawn point in the scene.
        /// </summary>
        public Transform spawnPoint;

        /// <summary>
        /// A global jump modifier applied to all initial jump velocities.
        /// </summary>
        public float jumpModifier = 1.5f;

        /// <summary>
        /// A global jump modifier applied to slow down an active jump when 
        /// the user releases the jump input.
        /// </summary>
        public float jumpDeceleration = 0.5f;
        
        /// <summary>
        /// 全局重力
        /// </summary>
        public float gForce = 9.8f;
        
        
        /// The force to apply vertically at all times
        [Tooltip("The force to apply vertically at all times")]
        public float Gravity = -30f;
        /// a multiplier applied to the character's gravity when going down
        [Tooltip("a multiplier applied to the character's gravity when going down")]
        public float FallMultiplier = 5f;
        /// a multiplier applied to the character's gravity when going up
        [Tooltip("a multiplier applied to the character's gravity when going up")]
        public float AscentMultiplier = 0.5f;

    }
}