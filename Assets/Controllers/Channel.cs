using UnityEngine;
using UnityEngine.UI;

using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using Colyseus;
using Colyseus.Schema;

using ChannelSchema;

public class Channel : MonoBehaviour
{
    protected int maxMessages = 100;
    protected string endpoint = "ws://localhost:2567";

    protected Client client;
    protected Room<State> channel;
    
    public GameObject chatPanel, textObject;
    public InputField chatInput;

    List<LocalMessage> messages =  new List<LocalMessage>();

    void Start()
    {
        client = new Colyseus.Client("ws://localhost:2567");
        JoinChannel();
    }

    void Update()
    {
        if (chatInput.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SendMessageToServer(chatInput.text);
                chatInput.text = "";
            }
        }

        if (!chatInput.isFocused && Input.GetKeyDown(KeyCode.Return)) {
            chatInput.ActivateInputField();
        }
    }

    async void JoinChannel() {
        channel = await client.JoinOrCreate<State>("channel");

        channel.OnMessage += OnMessage;
    }

    void SendMessageToServer(string content) {
        Message message = new Message();

        message.content = content;

        channel.Send(message);
    }

    void OnMessage(object msg) {
        if (msg is Message) MessageCreate(msg);
    }

    void MessageCreate(object msg) {
        var channelMessage = (Message)msg;
        var message = new LocalMessage();

        GameObject newText =  Instantiate(textObject, chatPanel.transform);

        message.textObject = newText.GetComponent<Text>();
        message.textObject.text = "[" + channelMessage.username + "] " + channelMessage.content;

        if (messages.Count >= maxMessages) {
            Destroy(messages[0].textObject.gameObject);
            messages.Remove(messages[0]);
        }

        messages.Add(message);
    }
}

public class LocalMessage {
    public Text textObject;
}