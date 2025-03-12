using UnityEngine;

namespace BuildingSystem
{
    public interface IBuilderStrategy<T> where T : Component
    {
        bool CanBuild(T buildable);
        void Build(T buildable, IPredicate condition);
    }

    public class FixedPositionBuilder : IBuilderStrategy<Building>
    {
        private GameObject _builderGameObject;
        private BoxCollider _builderColliderCache;
        private LayerMask _targetLayers;

        public FixedPositionBuilder(GameObject origin, LayerMask targetLayers)
        {
            _builderGameObject = origin;
            _builderColliderCache = origin.GetComponent<BoxCollider>();

            _targetLayers = targetLayers;
        }

        public bool CanBuild(Building buildable)
        {
            BoxCollider buildingCollider = buildable.GetComponent<BoxCollider>();

            var centerPos = _builderColliderCache.bounds.center + 
                Vector3.up * _builderColliderCache.bounds.extents.y * 0.5f + 
                Vector3.up * (buildingCollider.size.y * buildingCollider.transform.localScale.y * 0.5f);

            var buildingSize = Vector3.Scale(buildingCollider.size, buildingCollider.transform.localScale) * 0.5f;

            var isHit = Physics.CheckBox(centerPos, buildingSize, Quaternion.identity, _targetLayers);

            return !isHit;
        }

        public void Build(Building buildable, IPredicate condition)
        {
            if (condition.Evaluate())
            {
                var go = GameObject.Instantiate(buildable);

                go.transform.position =
                    _builderGameObject.transform.position + Vector3.up * go.GetComponent<Collider>().bounds.extents.y;
                go.transform.rotation = _builderColliderCache.transform.rotation;
            }
        }
    }
}

