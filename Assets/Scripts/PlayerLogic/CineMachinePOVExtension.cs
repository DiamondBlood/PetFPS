using UnityEngine;
using Cinemachine;

public class CineMachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float _horizontalSpeed = 10f;
    [SerializeField] private float _verticalSpeed = 10f;
    [SerializeField] private float _clampAngle = 80f;

    private InputManger _inputManager;
    private Vector3 _startingRotation;
    protected override void Awake()
    {
        _inputManager = InputManger.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (_inputManager != null && vcam.Follow && stage == CinemachineCore.Stage.Aim)
        { 
            if (_startingRotation == null)
            {
                _startingRotation = transform.localRotation.eulerAngles;
            }
            Vector2 deltaInput = _inputManager.GetPlayerMouseDelta();
            _startingRotation.x += deltaInput.x * _verticalSpeed * Time.deltaTime;
            _startingRotation.y += deltaInput.y * _horizontalSpeed * Time.deltaTime;
            _startingRotation.y = Mathf.Clamp(_startingRotation.y, -_clampAngle, _clampAngle);
            state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
        }
    }
}
