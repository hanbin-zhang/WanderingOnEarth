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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cursorInputForLook = true;

            GetComponent<PlayerPlanting>().HandlePlanting(true);
			
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

		public void OnItemChange(InputValue value)
		{			
			if (Keyboard.current.digit1Key.isPressed)
			{
				GetComponent<PlayerPlanting>().SwitchSlot(0);
			}
            if (Keyboard.current.digit2Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(1);
            }
            if (Keyboard.current.digit3Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(2);
            }
            if (Keyboard.current.digit4Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(3);
            }
			if (Keyboard.current.digit5Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(4);
            }
			if (Keyboard.current.digit6Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(5);
            }
			if (Keyboard.current.digit7Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(6);
            }
			if (Keyboard.current.digit8Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(7);
            }
			if (Keyboard.current.digit9Key.isPressed)
            {
                GetComponent<PlayerPlanting>().SwitchSlot(8);
            }

        }

		public void OnLandPrep(InputValue value)
		{
            Manager.EventController.Get<OnLandPrepEvent>()?.Notify(transform.position);
        }

		public void OnCollect(InputValue value)
		{
			//GetComponent<CollectableManager>().CheckCollectable(true);
            GetComponent<PlayerPlanting>().HandleCollectable(true);
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