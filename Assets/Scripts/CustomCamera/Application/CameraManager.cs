using Map.Application;
using UnityEngine;

namespace CustomCamera.Application
{
    public class CameraManager : MonoBehaviour
    {
        public float movementSpeed;
        public float movementShiftMultiplier;
        public float rotationSpeed;
        
        private float _currentMovementSpeed;

        private Camera _cameraComponent;
        
        private TerrainHitter _terrainHitter;

        public void Init(TerrainHitter terrainHitter)
        {
            _terrainHitter = terrainHitter;
        }
        
        private void Awake()
        {
            _cameraComponent = GetComponent<Camera>();
        }

        public void Update()
        {
            //faster speed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _currentMovementSpeed = movementSpeed * movementShiftMultiplier;
            }
            else
            {
                _currentMovementSpeed = movementSpeed;
            }

            //movement
            if (Input.GetKey(KeyCode.W))
            {
                Move(transform.forward);
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                Move(-transform.forward);
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                Move(-transform.right);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                Move(transform.right);
            }
            
            //rotation
            if (Input.GetKey(KeyCode.E))
            {
                Rotate(Vector3.down);
            }
            
            if (Input.GetKey(KeyCode.Q))
            {
                Rotate(Vector3.up);
            }
        }
        
        //needed for rotation
        private Vector3 HitTerrain()
        {
            Vector3 hitPoint = _terrainHitter.Hit(_cameraComponent.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)));
            
            return new Vector3(
                Mathf.Round(hitPoint.x / 1),
                hitPoint.y,
                Mathf.Round(hitPoint.z / 1)
            );
        }
        private void Move(Vector3 direction)
        {
            direction.y = 0;
            direction.Normalize();
            transform.Translate(_currentMovementSpeed * Time.unscaledDeltaTime * direction, Space.World);
        }

        private void Rotate(Vector3 vector)
        {
            transform.RotateAround(HitTerrain(), vector, rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}