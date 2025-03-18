using System;
using System.Linq;
using UnityEngine;

namespace BuildingSystem
{
    public interface IBuilderStrategy
    {
        bool CanBuild(Building buildable);
        void Build(Building buildable, IPredicate condition = null);
    }

    public class FixedPositionBuilder : IBuilderStrategy
    {
        private readonly GameObject _builderGameObject;
        private readonly BoxCollider _builderColliderCache;
        private readonly LayerMask _targetLayers;

        public FixedPositionBuilder(GameObject origin, LayerMask targetLayers)
        {
            _builderGameObject = origin;
            _builderColliderCache = origin.GetComponent<BoxCollider>();

            _targetLayers = targetLayers;
        }

        public bool CanBuild(Building buildable)
        {
            var buildingCollider = buildable.GetComponents<BoxCollider>().FirstOrDefault(c => !c.isTrigger);

            if (buildingCollider == null)
                return false;

            var centerPos = _builderColliderCache.bounds.center + 
                Vector3.up * _builderColliderCache.bounds.extents.y * 0.5f + 
                Vector3.up * (buildingCollider.size.y * buildingCollider.transform.localScale.y * 0.5f);

            var buildingSize = Vector3.Scale(buildingCollider.size, buildingCollider.transform.localScale) * 0.5f;

            var isHit = Physics.CheckBox(centerPos, buildingSize, _builderGameObject.transform.rotation, _targetLayers);

            return !isHit;
        }

        public void Build(Building buildable, IPredicate condition = null)
        {
            if (condition == null || condition.Evaluate())
            {
                var go = GameObject.Instantiate(buildable);

                //go.transform.position =
                //    _builderGameObject.transform.position + Vector3.up * go.GetComponent<Collider>().bounds.extents.y * 0.5f;
                go.transform.position =
                    _builderGameObject.transform.position + Vector3.down *
                    (_builderColliderCache.size.y * _builderColliderCache.transform.localScale.y) * 0.5f +
                    Vector3.up * go.GetComponent<Collider>().bounds.extents.y * 0.5f;
                go.transform.rotation = _builderColliderCache.transform.rotation;
            }
        }
    }
}

