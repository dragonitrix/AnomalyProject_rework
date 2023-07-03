using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroChatPanelController : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public RectTransform dialogueRect;

    public GameObject dialogue_prefab_player;
    public GameObject dialogue_prefab_bot;

    public DialogueChat mainDialogue;
    public int dialogueIndex = 0;

    public List<DialoguePiece_chat2> dialoguePieces = new List<DialoguePiece_chat2>();

    [Header("typing")]

    public ScrollRect scroll;
    public float typingHeightOffset = 100f;
    public CanvasGroup typingCanvasGroup;

    public bool isTyping = false;
    public float typeDuration = 0.75f;
    public float typeElapsed = 0f;
    public CanvasGroup simpleClickArea;

    [Header("input")]
    public CanvasGroup inputCanvasGroup;
    public Button inputSubmitButton;
    public TMP_InputField inputField;
    string currentCommandID = "";
    public bool isInputWaiting = false;

    string respond_player_nickname;

    public Dialogue_chat currentDialogue;


    [ContextMenu("TestSpawn")]
    public void TestChat()
    {
        StartCoroutine(_TestChat());
    }
    IEnumerator _TestChat()
    {
        ClearAll();
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < mainDialogue.dialogue_Chats.Count; i++)
        {
            SetDialogue(i);
        }

        Resize(true);
        UpdateProfileVisibility();
    }

    [ContextMenu("InitIntroChat")]
    public void InitIntroChat()
    {
        ClearAll();
        dialogueIndex = 0;

        ProceedCurrentDialogue();

        inputSubmitButton.onClick.RemoveAllListeners();
        inputSubmitButton.onClick.AddListener(OnButtonSubmitInputClicked);

    }

    public void ProceedCurrentDialogue()
    {
        currentDialogue = mainDialogue.dialogue_Chats[dialogueIndex];

        switch (currentDialogue.type)
        {
            case DialogueType._0_bot:
                StartTypingAnim();
                break;
            case DialogueType._1_playerchoice:
                break;
            case DialogueType._2_playerinput:
                currentCommandID = currentDialogue.command_id;
                EnableInputfield();
                break;
            default:
                break;
        }
    }

    public void StartTypingAnim()
    {
        LayoutRebuilder.MarkLayoutForRebuild(dialogueRect);
        scroll.enabled = false;
        dialogueRect.anchoredPosition = new Vector2(0, typingHeightOffset);
        typeElapsed = 0f;
        isTyping = true;
        typingCanvasGroup.ShowAll();
        simpleClickArea.ShowAll();
    }

    public void EndTypingAnim()
    {
        scroll.enabled = true;
        dialogueRect.anchoredPosition = new Vector2(0, 0);
        isTyping = false;
        typingCanvasGroup.HideAll();
        simpleClickArea.HideAll();
        SetDialogue(currentDialogue);
    }

    public void EnableInputfield()
    {
        inputField.text = ""; // clear
        inputCanvasGroup.ShowAll();
        isInputWaiting = true;
    }

    public void DisableInputfield()
    {
        inputField.text = ""; // clear
        inputCanvasGroup.HideAll();
        inputCanvasGroup.alpha = 0.2f;
        isInputWaiting = false;
    }

    void OnButtonSubmitInputClicked()
    {
        var inputtext = inputField.text;
        Regex r = new Regex("^\\s +$");
        if (r.IsMatch(inputtext))
        {
            return;
        }
        ProceedInput(currentCommandID, inputtext);
        DisableInputfield();
    }

    void ProceedInput(string commandID, string input)
    {
        switch (commandID)
        {
            case "player_name_nickname":
                respond_player_nickname = input;
                break;
            case "player_detail_salary":
                break;
            case "player_response_01_searchresult":
                break;
        }

        //currentDialogue.text = input;
        Dialogue_chat dialogue = new Dialogue_chat();
        dialogue.text = input;
        dialogue.type = currentDialogue.type;

        SetDialogue(dialogue);
    }

    private void Update()
    {
        if (isTyping)
        {
            typeElapsed += Time.deltaTime;
            if (typeElapsed >= typeDuration)
            {
                EndTypingAnim();
            }
        }

        if (isInputWaiting)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnButtonSubmitInputClicked();
            }
        }
    }

    public void OnSimpleClickArea()
    {
        if (isTyping)
        {
            EndTypingAnim();
        }
    }

    public void ClearAll()
    {
        foreach (Transform item in dialogueRect)
        {
            Destroy(item.gameObject);
        }
        dialoguePieces.Clear();
    }

    public void UpdateProfileVisibility()
    {
        var lastIndex = 0;
        for (int i = 0; i < dialoguePieces.Count; i++)
        {
            if (dialoguePieces[i].chatSide == ChatSide._BOT)
            {
                lastIndex = i;
                dialoguePieces[i].SetProfile(false);
            }
        }
        dialoguePieces[lastIndex].SetProfile(true);
    }

    public void SetDialogue(int index)
    {
        SetDialogue(mainDialogue.dialogue_Chats[index]);
    }

    public void SetDialogue(string id)
    {

        for (int i = 0; i < mainDialogue.dialogue_Chats.Count; i++)
        {
            if (mainDialogue.dialogue_Chats[i].id == id)
            {
                SetDialogue(mainDialogue.dialogue_Chats[i]);
                return;
            }
        }
    }

    public void SetDialogue(Dialogue_chat dialogue)
    {
        StartCoroutine(_SetDialogue(dialogue));
    }

    IEnumerator _SetDialogue(Dialogue_chat dialogue)
    {

        ChatSide side;
        switch (dialogue.type)
        {
            case DialogueType._0_bot:
                side = ChatSide._BOT;
                break;
            case DialogueType._1_playerchoice:
            case DialogueType._2_playerinput:
                side = ChatSide._PLAYER;
                break;
            default:
                side = ChatSide._BOT;
                break;
        }

        GameObject prefab;
        switch (side)
        {
            case ChatSide._PLAYER:
                prefab = dialogue_prefab_player;
                break;
            case ChatSide._BOT:
                prefab = dialogue_prefab_bot;
                break;
            default:
                prefab = dialogue_prefab_bot;
                break;
        }

        var text = RefineDialogueText(dialogue.text);

        var clone = Instantiate(prefab, dialogueRect);
        var script = clone.GetComponent<DialoguePiece_chat2>();
        script.SetDialogue(this, text, side);
        dialoguePieces.Add(script);
        UpdateProfileVisibility();

        yield return StartCoroutine(_Resize(true));

        //Debug.Log("fnished set");

        switch (dialogue.type)
        {
            case DialogueType._0_bot:
                yield return new WaitForSeconds(0.5f);
                NextDialogue();
                break;
            case DialogueType._1_playerchoice:
                break;
            case DialogueType._2_playerinput:
                currentCommandID = "";// clear command id
                NextDialogue();
                break;
        }
    }

    string RefineDialogueText(string input)
    {
        input = input.Replace("$nickname", respond_player_nickname);
        return input;
    }

    public void NextDialogue()
    {

        if (dialogueIndex < mainDialogue.dialogue_Chats.Count - 1)
        {
            Debug.Log("NextDialogue");
            dialogueIndex++;
            ProceedCurrentDialogue();
        }
        else
        {
            IntroFinished();
        }

    }

    public void IntroFinished()
    {
        StartCoroutine(_IntroFinished(2));
    }

    IEnumerator _IntroFinished(float delay)
    {
        yield return new WaitForSeconds(delay);

        IntroManager.instance.GotoMainMenu();
    }

    void Resize(bool jumptolast = false)
    {
        StartCoroutine(_Resize(jumptolast));
    }
    IEnumerator _Resize(bool jumptolast)
    {

        yield return new WaitForEndOfFrame();
        var totalY = 0f;
        for (int i = 0; i < dialogueRect.childCount; i++)
        {
            totalY += dialogueRect.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }

        var layoutGroup = dialogueRect.GetComponent<VerticalLayoutGroup>();

        totalY += layoutGroup.spacing * (dialogueRect.childCount - 1) + layoutGroup.padding.top + layoutGroup.padding.bottom;

        dialogueRect.sizeDelta = new Vector2(
            dialogueRect.sizeDelta.x,
            totalY
        );
        if (jumptolast)
        {
            dialogueRect.anchoredPosition = new Vector2(
                dialogueRect.anchoredPosition.x,
                0
                );
        }

        LayoutRebuilder.MarkLayoutForRebuild(dialogueRect);
    }
}
