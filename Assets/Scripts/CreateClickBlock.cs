    using UnityEngine;
    using UnityEngine.InputSystem;

    public class SpawnCubeOnMouseClick : MonoBehaviour
    {
        [SerializeField]
        private GameObject cubePrefab;

        [SerializeField]
        private LayerMask groundLayer;

        private void Start()
        {
            InputManager.Instance.RegisterOnClickInput(OnClick, true);
        }

        private void OnDisable()
        {
            InputManager.Instance.RegisterOnClickInput(OnClick, false);
        }

        private void OnClick(InputAction.CallbackContext callbackContext)
        {
            SpawnCubeAtMousePosition();
        }

        private void SpawnCubeAtMousePosition()
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer) && cubePrefab != null)
            {
                Instantiate(cubePrefab, hitInfo.point, Quaternion.identity);
            }
        }
    }
