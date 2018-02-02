using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsButtonController : ButtonsPanelController {
    public static string COMMAND_REMOVE = "remove";
    public static string COMMAND_HARVEST = "harvest";

    public static string GetCommand () {
        if (currentSelectedButton) {
            ToolsButton tools = currentSelectedButton.GetComponent<ToolsButton>();
            if (tools) {
                return tools.command;
            }
        }

        return null;
    }
}
