using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UEventHandler;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{

    public string[] devices;

    public Camera playerCamera { get; private set; }

    PlayerInput input;

    private void Awake()
    {


        input = GetComponent<PlayerInput>();

        if (input.camera == null)
            playerCamera = Camera.main;
        else
            playerCamera = input.camera;
        devices = input.devices.Select(x => x.name).ToArray();
    }

    public InputDevice GetMainInput() => input.devices.Count <= 0 ? null : input.devices[0];
    public void ChangeInputDevice(int inputId)
    {
        var device = InputSystem.GetDeviceById(inputId);

        if (device == null) return;

        bool isKeyboard = device.name.Contains("Keyboard");

        input.SwitchCurrentControlScheme(isKeyboard ? "Keyboard&Mouse" : "Gamepad", device);
        devices = input.devices.Select(x => x.name).ToArray();
    }


    // public BufferedButton input_bufferedJump = new BufferedButton { bufferTime = 2 };
    public Button<Vector2> input_move = new Button<Vector2>();
    public Button<Vector2> input_look = new Button<Vector2>();
    public Button<float> input_jump = new Button<float>();
    public Button<float> input_sprint = new Button<float>();
    public Button<float> input_pause = new Button<float>();
    public Button<float> input_interact = new Button<float>();

    #region Button Base stuff
    //public delegate void ClickAction();

    public class Button<TValue>  //Suported data types--> float | Vector2 | Vector3 | Vector4
    {
        public TValue value { get; set; }
        //public event ClickAction Onpressed;
        //public event ClickAction Onreleased;
        public UEvent Onpressed = new UEvent();
        public UEvent Onreleased = new UEvent();

        public void Pressed() => Onpressed.TryInvoke();
        //public void Pressed() => Onpressed?.Invoke();
        public void Released() => Onreleased.TryInvoke();
        //public void Released() => Onreleased?.Invoke();
    }

    public class BufferedButton : MonoBehaviour  //Suported data types--> bool
    {
        public bool isPressed { get; set; }
        public float bufferTime { get; set; }

        Coroutine bufferRoutine;

        public void ClearButtonBuffer()
        {
            if (bufferRoutine != null) StopCoroutine(bufferRoutine);
            bufferRoutine = null;
        }
        public void SetButtonPress()
        {
            if (bufferRoutine != null) StopCoroutine(bufferRoutine);
            bufferRoutine = StartCoroutine(WaitToClearInput());
        }

        IEnumerator WaitToClearInput()
        {
            yield return new WaitForSeconds(bufferTime);
            isPressed = false;
        }
    }
    #endregion

    private void OnMove(InputValue inputValue) => SetInputInfo(input_move, inputValue);
    private void OnLook(InputValue inputValue) => SetInputInfo(input_look, inputValue);
    private void OnJump(InputValue inputValue) => SetInputInfo(input_jump, inputValue);
    private void OnSprint(InputValue inputValue) => SetInputInfo(input_sprint, inputValue);
    private void OnPause(InputValue inputValue) => SetInputInfo(input_pause, inputValue);
    private void OnInteract(InputValue inputValue) => SetInputInfo(input_interact, inputValue);


    #region Info Setters
    void SetInputInfo(Button<float> button, InputValue inputValue)
    {
        var value = inputValue.Get<float>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value == 0)
            button.Released();
        else if (oldValue == 0)
            button.Pressed();
    }

    void SetInputInfo(Button<Vector2> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector2>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    void SetInputInfo(Button<Vector3> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector3>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    void SetInputInfo(Button<Vector4> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector4>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    #endregion

}
