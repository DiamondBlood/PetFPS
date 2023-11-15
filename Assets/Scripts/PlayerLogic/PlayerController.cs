using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _playerInitialSpeed = 2.0f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;

    private float _playerSpeed;
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private InputManger _inputManager;
    private Transform _cameraTransform;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inputManager = InputManger.Instance;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
         
        Vector2 movement = _inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = _cameraTransform.forward * move.z + _cameraTransform.right * move.x;
        move.y = 0f;
        move = move.normalized;

        if (_inputManager.PlayerSpeedUp())
            _playerSpeed = _playerInitialSpeed * 1.5f;
        else
            _playerSpeed = _playerInitialSpeed;
        _controller.Move(move * Time.deltaTime * _playerSpeed);

        /*if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        // Changes the height position of the player..
        if (_inputManager.PlayerJumpedThisFrame() && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
}
