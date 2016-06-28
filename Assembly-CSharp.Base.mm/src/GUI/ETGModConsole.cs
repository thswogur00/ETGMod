﻿#pragma warning disable 0626
#pragma warning disable 0649

using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class ETGModConsole : IETGModMenu {

    /// <summary>
    /// All commands supported by the ETGModConsole. Add your own commands here!
    /// </summary>
    public static Dictionary<string, System.Action<string[]>> Commands = new Dictionary<string, System.Action<string[]>>();
    /// <summary>
    /// All console logged text lines. Feel free to add your lines here!
    /// </summary>
    public static List<string> LoggedText = new List<string>();
    /// <summary>
    /// The currently typed in command in the text box.
    /// </summary>
    public static string CurrentCommand = "";

    public static Vector2 ScrollPos;

    private Rect mainBoxRect = new Rect(16,                 16, Screen.width - 32, Screen.height - 32);
    private Rect inputBox =    new Rect(16, Screen.height - 32, Screen.width - 32,                 32);
    private Rect viewRect =    new Rect(16,                 16, Screen.width - 32, Screen.height - 32);

    public void Start() {
        Commands["exit"] = Commands["hide"] = Commands["quit"] = (string[] args) => ETGModGUI.CurrentMenu = ETGModGUI.MenuOpened.None;
        Commands["log"] = Commands["echo"] = Echo;
        Commands["rollDistance"] = DodgeRollDistance;
        Commands["rollSpeed"] = DodgeRollSpeed;
        Commands["tp"] = Commands["teleport"] = Teleport;
    }

    public void Update() {
        
    }

    public void OnGUI() {
        bool ranCommand = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return && CurrentCommand.Length > 0;
        if (ranCommand) {
            RunCommand();
        }

        mainBoxRect = new Rect(16,                      16, Screen.width - 32, Screen.height - 32 -  8);
        inputBox =    new Rect(16, Screen.height - 32 -  8, Screen.width - 32,                      24);

        GUI.Box(mainBoxRect, string.Empty);
        CurrentCommand = GUI.TextArea(inputBox, CurrentCommand);
        viewRect = new Rect(0, 0, mainBoxRect.width, 34 * LoggedText.Count);
        ScrollPos = GUI.BeginScrollView(mainBoxRect, ScrollPos, viewRect);

        for (int i = 0; i < LoggedText.Count; i++) {
            GUILayout.Label(LoggedText[i]);
        }

        GUI.EndScrollView();

        if (ranCommand || Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
            // No new line when we ran a command.
            CurrentCommand = "";
        }
    }

    public void OnDestroy() {
    
    }
    
    /// <summary>
    /// Runs the currently typed in command.
    /// </summary>
    public static void RunCommand() {
        RunCommand(CurrentCommand);
        CurrentCommand = string.Empty;
    }

    private readonly static string[] a_string_0 = new string[0];
    /// <summary>
    /// Runs a given command.
    /// </summary>
    /// <param name="command">Command to run.</param>
    public static void RunCommand(string command) {
        string[] parts;
        string[] args;

        if (command.Contains(" ")) {
            parts = command.Split(' ');
            args = new string[parts.Length - 1];

            for (int i = 1; i < parts.Length; i++) {
                args[i - 1] = parts[i];
            }
        } else {
            parts = new string[] { command };
            args = a_string_0;
        }

        if (Commands.ContainsKey(parts[0])) {
            Commands[parts[0]](args);
            LoggedText.Add("Executed command " + parts[0]);
        } else {
            LoggedText.Add("Command " + parts[0] + " not found.");
        }
    }

    // Example commands

    void Echo(string[] args) {
        StringBuilder combined = new StringBuilder();
        for (int i = 0; i < args.Length; i++) {
            combined.Append(args[i]);
            if (i < args.Length - 1) {
                combined.Append(' ');
            }
        }
        string str = combined.ToString();
        Debug.Log(str);
        LoggedText.Add(str);
    }

    void DodgeRollDistance(string[] args) {
        if (args.Length != 1) {
            return;
        }
        Debug.Log(args[0]);

        if (GameManager.GameManager_0 != null && GameManager.GameManager_0.PlayerController_1 != null) {
            GameManager.GameManager_0.PlayerController_1.dodgeRollStats_0.distance = float.Parse(args[0]);
        }
    }

    void DodgeRollSpeed(string[] args) {
        if (args.Length != 1) {
            return;
        }
        Debug.Log(args[0]);

        if (GameManager.GameManager_0 != null && GameManager.GameManager_0.PlayerController_1 != null) {
            GameManager.GameManager_0.PlayerController_1.dodgeRollStats_0.time = float.Parse(args[0]);
        }
    }

    void Teleport(string[] args) {
        if (args.Length != 3) {
            return;
        }

        if (GameManager.GameManager_0 != null && GameManager.GameManager_0.PlayerController_1 != null) {
            GameManager.GameManager_0.PlayerController_1.transform.position = new Vector3(
                float.Parse(args[0]),
                float.Parse(args[1]),
                float.Parse(args[2])
            );
        }
    }

}
