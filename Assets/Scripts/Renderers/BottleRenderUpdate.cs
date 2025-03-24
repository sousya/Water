using UnityEngine;

namespace WaterGame.Renderers
{
    public class BottleRenderUpdate : MonoBehaviour
    {
        private bool _isMoving;
        private Material _bottleMaterial;
        private static readonly int MoveStateProperty = Shader.PropertyToID("_MoveState");

        private void Awake()
        {
            _bottleMaterial = GetComponent<Renderer>().material;
        }

        public void SetMoveBottleRenderState(bool isMoving)
        {
            _isMoving = isMoving;
            _bottleMaterial.SetFloat(MoveStateProperty, isMoving ? 1 : 0);
        }

        private void OnDestroy()
        {
            if (_bottleMaterial != null)
            {
                Destroy(_bottleMaterial);
            }
        }
    }
} 