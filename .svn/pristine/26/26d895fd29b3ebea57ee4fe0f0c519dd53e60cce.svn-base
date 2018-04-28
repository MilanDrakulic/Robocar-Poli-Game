using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenItem : MonoBehaviour {

    public Text ItemText;
    public Text ItemValueText;

    public void setEndScreenItem(string itemName, int itemValInt, string itemValue) {
        ItemText.text = itemName;
        //if the string val is not empty it will be set else the int val
        ItemValueText.text = itemValue == "" ? (itemValInt + "") : itemValue;

    }
}
