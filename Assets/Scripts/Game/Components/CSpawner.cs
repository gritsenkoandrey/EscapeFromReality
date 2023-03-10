using CodeBase.ECSCore;
using UnityEngine;

namespace CodeBase.Game.Components
{
    public sealed class CSpawner : EntityComponent<CSpawner>
    {
        [SerializeField] private GameObject _prefab;

        public void CreatePrefab() => Instantiate(_prefab, transform.position, Quaternion.identity, transform.parent);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawSphere(transform.position, 0.25f);
        }

        protected override void OnEntityCreate() { }
        protected override void OnEntityEnable() { }
        protected override void OnEntityDisable() { }
    }
}