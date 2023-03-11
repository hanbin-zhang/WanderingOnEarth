using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        public float zoom = 0;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

        public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnLeftMouseDown(InputValue value)
        {
			GetComponent<PlayerPlanting>().Plant();
        }

        public void OnZoom(InputValue value)
        {
			if (cursorInputForLook)
			{
				ZoomInput(-value.Get<Vector2>().y);
			}
        }

		public void OnAltCursor(InputValue value)
		{
			if (value.isPressed) 
			{
				Cursor.visible = true;
            	Cursor.lockState = CursorLockMode.None;
				cursorInputForLook = false;
				LookInput(Vector2.zero);
				ZoomInput(0f);
			}
			else 
			{
				Cursor.visible = false;
            	Cursor.lockState = CursorLockMode.Locked;
				cursorInputForLook = true;
			}
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void ZoomInput(float value) {
			zoom = Mathf.Clamp(value, -1, 1);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = newState;
		}
	}
	
}