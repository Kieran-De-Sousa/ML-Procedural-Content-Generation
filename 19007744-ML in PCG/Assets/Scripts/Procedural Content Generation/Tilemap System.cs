// Base
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG.Tilemaps
{
    /// <summary>
    ///
    /// </summary>
    public class TilemapSystem : Singleton<TilemapSystem>
    {
        [Header("Tilemaps")]
        public List<Tilemap> tilemaps;

        public Tilemap tilemap;
        // Rigidbody, Composite collider, tilemap collider (NOT TRIGGERS)
        public Tilemap collidable;
        // Rigidbody, Composite collider, tilemap collider (TRIGGERS)
        public Tilemap entities;

        protected override void Awake()
        {
            base.Awake();

            // Additional initialisation logic here...
        }

        /// <summary>
        /// Add the tilemaps to the list whenever we change the PCG System in Editor.
        /// </summary>
        private void OnValidate()
        {
            AddTilemapToList(tilemap);
            AddTilemapToList(collidable);
            AddTilemapToList(entities);
        }

        /// <summary>
        /// Adds the tilemap to the list of <c>tilemaps</c> if not already.
        /// </summary>
        /// <param name="tilemapToAdd">Tilemap to be added to list.</param>
        private void AddTilemapToList(Tilemap tilemapToAdd)
        {
            if (tilemapToAdd != null && !tilemaps.Contains(tilemapToAdd))
            {
                tilemaps.Add(tilemapToAdd);
            }
        }
    }
}