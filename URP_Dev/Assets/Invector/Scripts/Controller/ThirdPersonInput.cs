using UnityEngine;

namespace TPS
{
	public class ThirdPersonInput : MonoBehaviour
	{
		[Header("Controller Input")]
		public string horizontalInput = "Horizontal";
		public string verticallInput = "Vertical";
		public KeyCode jumpInput = KeyCode.Space;
		public KeyCode strafeInput = KeyCode.Tab;
		public KeyCode sprintInput = KeyCode.LeftShift;

		[Header("Camera Input")]
		public string rotateCameraXInput = "Mouse X";
		public string rotateCameraYInput = "Mouse Y";

		[HideInInspector]
		public ThirdPersonController tpController;

		[HideInInspector]
		public ThirdPersonCamera tpCamera;

		[HideInInspector]
		public Camera cameraMain;


		protected virtual void Start()
		{
			InitilizeController();
			InitializeTpCamera();
		}

		protected virtual void FixedUpdate()
		{
			// updates the ThirdPersonMotor methods
			tpController.UpdateMotor();
			// handle the controller locomotion type and movespeed
			tpController.ControlLocomotionType();
			// handle the controller rotation type
			tpController.ControlRotationType();
		}

		protected virtual void Update()
		{
			// update the input methods
			InputHandle();
			// updates the Animator Parameters
			tpController.UpdateAnimator();
		}

		public virtual void OnAnimatorMove()
		{
			// handle root motion animations 
			tpController.ControlAnimatorRootMotion();
		}

		protected virtual void InitilizeController()
		{
			tpController = GetComponent<ThirdPersonController>();
			tpController?.Init();
		}

		protected virtual void InitializeTpCamera()
		{
			if (tpCamera == null)
			{
				tpCamera = FindObjectOfType<ThirdPersonCamera>();
				if (tpCamera == null)
				{
					return;
				}

				if (tpCamera)
				{
					tpCamera.SetMainTarget(this.transform);
					tpCamera.Init();
				}
			}
		}

		protected virtual void InputHandle()
		{
			MoveInput();
			CameraInput();
			SprintInput();
			StrafeInput();
			JumpInput();
		}

		public virtual void MoveInput()
		{
			tpController.input.x = Input.GetAxis(horizontalInput);
			tpController.input.z = Input.GetAxis(verticallInput);
		}

		protected virtual void CameraInput()
		{
			if (!cameraMain)
			{
				if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
				else
				{
					cameraMain = Camera.main;
					tpController.rotateTarget = cameraMain.transform;
				}
			}

			if (cameraMain)
			{
				tpController.UpdateMoveDirection(cameraMain.transform);
			}

			if (tpCamera == null)
			{
				return;
			}

			var Y = Input.GetAxis(rotateCameraYInput);
			var X = Input.GetAxis(rotateCameraXInput);

			tpCamera.RotateCamera(X, Y);
		}

		protected virtual void StrafeInput()
		{
			if (Input.GetKeyDown(strafeInput))
				tpController.Strafe();
		}

		protected virtual void SprintInput()
		{
			if (Input.GetKeyDown(sprintInput))
				tpController.Sprint(true);
			else if (Input.GetKeyUp(sprintInput))
				tpController.Sprint(false);
		}

		protected virtual bool JumpConditions()
		{
			return tpController.isGrounded && tpController.GroundAngle() < tpController.slopeLimit && !tpController.isJumping && !tpController.stopMove;
		}

		protected virtual void JumpInput()
		{
			if (Input.GetKeyDown(jumpInput) && JumpConditions())
			{
				tpController.Jump();
			}
		}
	}
}