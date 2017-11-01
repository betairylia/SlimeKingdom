using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class DialogShower : eObject
{
    public enum DialogShowerState
    {
        Unused,
        WaitForClick,
        WaitForSelection,
        WaitForShop,
        Showing
    };

    public TextAsset FilePath;
    string[] stringArray;

    Dictionary<string, int> passageDict = new Dictionary<string, int>();

    public GameObject chatBoxRoot, selectionRoot, selectionPrefab, fakeSelectionPrefab;
    public Text chatText, targetName;
    public UIController m_UIController;
    public ShopMenu m_shopMenu;

    eObject dialogParent;
    int m_dialogIndexNext, m_pStart;
    DialogShowerState m_state;

    // Use this for initialization
    void Start ()
    {
        char[] lineSp = new char[2];
        lineSp[0] = '\r';
        lineSp[1] = '\n';

        stringArray = FilePath.text.Split(lineSp, System.StringSplitOptions.RemoveEmptyEntries);

        //Generate passage list and replace [el]s
        for(int i = 0; i < stringArray.Length; i++)
        {
            //if "[passage]" was the start of this line
            if(stringArray[i].Length > 9 && stringArray[i].Substring(0, 9).Equals("[passage]"))
            {
                //add it to the dict
                passageDict.Add(stringArray[i].Substring(9), i);
            }

            stringArray[i] = stringArray[i].Replace("[el]", "\n");
        }

        chatBoxRoot.SetActive(false);
        //chatClicker.SetActive(false);
        selectionRoot.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(m_state == DialogShowerState.WaitForClick)
        {
            if(Input.GetButtonDown("Submit"))
            {
                clicked(1);
            }
        }
	}

    void showChoices()
    {
        string str = stringArray[m_dialogIndexNext];

        foreach (Transform child in selectionRoot.transform)
        {
            Destroy(child.gameObject);
        }
        selectionRoot.SetActive(true);

        int count = 0;

        GameObject selection = null, _first_selection = null;

        while (str.Equals("[endchoice]") == false)
        {
            //todo: showchoices
            //start from 0, 0; then 0, -40; ...
            int offset = int.Parse(str.Split(']')[0].Substring(1));
            selection = Instantiate(selectionPrefab, selectionRoot.transform);
            selection.transform.localPosition = new Vector3(0, 65 * count, 0);

            selection.GetComponentInChildren<Text>().text = str.Split(']')[1];
            selection.GetComponent<Button>().onClick.AddListener(() => { clicked(offset); });

            if(count == 0)
            {
                _first_selection = selection;
            }

            count++;

            m_dialogIndexNext++;
            str = stringArray[m_dialogIndexNext];
        }

        GameObject fakeSelection = Instantiate(fakeSelectionPrefab, selectionRoot.transform);
        fakeSelection.transform.localPosition = new Vector3(-2000, 0, 0);

        fakeSelection.GetComponentInChildren<Text>().text = str.Split(']')[1];

        Navigation navFake = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = selection.GetComponent<Button>(),
            selectOnUp = _first_selection.GetComponent<Button>()
        };
        fakeSelection.GetComponent<Button>().navigation = navFake;

        //Navigation navFirst = new Navigation()
        //{
        //    mode = Navigation.Mode.Explicit,
        //    selectOnDown = selection.GetComponent<Button>(),
        //    selectOnUp = _first_selection.GetComponent<Button>().navigation.selectOnUp
        //};
        //_first_selection.GetComponent<Button>().navigation = navFirst;

        //Navigation navFinal = new Navigation()
        //{
        //    mode = Navigation.Mode.Explicit,
        //    selectOnDown = selection.GetComponent<Button>().navigation.selectOnDown,
        //    selectOnUp = _first_selection.GetComponent<Button>()
        //};
        //selection.GetComponent<Button>().navigation = navFinal;

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(fakeSelection);

        //chatClicker.SetActive(false);
        m_state = DialogShowerState.WaitForSelection;
    }

    void ShowDialog()
    {
        bool cont = false;

        string str = stringArray[m_dialogIndexNext];

        if(str[0] == '[')
        {
            string[] strArr = str.Split(']');
            string[] inside = strArr[0].Substring(1).Split(' ');
            switch(inside[0])
            {
                case "passage":
                    cont = true;
                    break;
                case "name":
                    targetName.text = strArr[1];
                    cont = true;
                    break;
                case "choice":
                    m_dialogIndexNext++;
                    showChoices();
                    break;
                case "end":
                    TriggerEvent(this, "dialogEnd");
                    break;
                case "trig":
                    string strTrig = "";
                    for (int i = 1; i < inside.Length; i++)
                    {
                        if (i < inside.Length - 1)
                        {
                            strTrig = strTrig + inside[i] + " ";
                        }
                        else
                        {
                            strTrig = strTrig + inside[i];
                        }
                    }
                    TriggerEvent(this, strTrig);
                    cont = true;
                    break;
                case "jump":
                    m_dialogIndexNext = m_pStart + int.Parse(inside[1]) - 1;
                    cont = true;
                    break;
                case "jumpRelative":
                    m_dialogIndexNext += int.Parse(inside[1]) - 1;
                    cont = true;
                    break;
                case "hide":
                    chatBoxRoot.SetActive(false);
                    cont = true;
                    break;
                case "show":
                    chatBoxRoot.SetActive(true);
                    cont = true;
                    break;
                case "wait":
                    TriggerEvent(this, "dialogContinue", float.Parse(inside[1]));
                    break;
                case "blackScreenOn":
                    m_UIController.BlackScreenOn(float.Parse(inside[1]));
                    cont = true;
                    break;
                case "blackScreenOff":
                    m_UIController.BlackScreenOff(float.Parse(inside[1]));
                    cont = true;
                    break;
                case "shopMenu":
                    m_shopMenu.Show(int.Parse(inside[1]));
                    m_state = DialogShowerState.WaitForShop;
                    break;
            }
        }
        else
        {
            chatText.text = str;
            m_state = DialogShowerState.Showing;
            TriggerEvent(this, "wait4click", 0.5f);
        }

        m_dialogIndexNext++;

        //if next is choice then continue
        if(m_dialogIndexNext < stringArray.Length)
        {
            str = stringArray[m_dialogIndexNext].Split(' ')[0];
            if (str.Equals("[choice]"))
            {
                cont = true;
            }
        }

        if (cont)
        {
            ShowDialog();
        }
        return;
    }

    public void clicked(int offset = 1)
    {
        switch(m_state)
        {
            case DialogShowerState.Unused:
                selectionRoot.SetActive(false);
                //chatClicker.SetActive(false);
                chatBoxRoot.SetActive(false);
                return;
                break;

            case DialogShowerState.WaitForClick:
                selectionRoot.SetActive(false);
                break;

            case DialogShowerState.WaitForSelection:
                //chatClicker.SetActive(true);
                selectionRoot.SetActive(false);
                break;
        }

        m_dialogIndexNext += offset - 1;
        TriggerEvent(this, "dialogContinue");
    }

    bool isDialogShowing;
    public bool IsDialogShowing()
    {
        return isDialogShowing;
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "dialog":
                if(passageDict.ContainsKey(signal[1]))
                {
                    isDialogShowing = true;
                    dialogParent = source;
                    chatBoxRoot.SetActive(true);
                    //chatClicker.SetActive(true);
                    m_dialogIndexNext = passageDict[signal[1]];
                    m_pStart = m_dialogIndexNext;
                    ShowDialog();
                }
                break;
            case "dialogContinue":
                ShowDialog();
                break;
            case "wait4click":
                if(m_state == DialogShowerState.Showing)
                {
                    m_state = DialogShowerState.WaitForClick;
                }
                break;
            case "dialogEnd":
                isDialogShowing = false;
                m_state = DialogShowerState.Unused;
                chatBoxRoot.SetActive(false);
                //chatClicker.SetActive(false);
                selectionRoot.SetActive(false);

                TriggerEvent(dialogParent, "dialogEnd");

                dialogParent = null;
                break;
            default:
                Debug.Log(signal[0]);
                string str = "";
                for(int i = 0; i < signal.Length; i++)
                {
                    if(i < signal.Length - 1)
                    {
                        str = str + signal[i] + " ";
                    }
                    else
                    {
                        str = str + signal[i];
                    }
                }
                TriggerEvent(dialogParent, str);
                break;

        }
    }
}
