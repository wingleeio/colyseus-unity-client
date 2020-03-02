using UnityEngine;
using UnityEngine.UI;

using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using Colyseus;
using Colyseus.Schema;

public class Channel : MonoBehaviour
{
    protected int maxMessages = 100;
    protected string endpoint = "ws://localhost:2567";

    protected Client client;
    protected Room<ChannelState> channel;
    
    public GameObject chatPanel, textObject;
    public InputField chatInput;

    List<Message> messages =  new List<Message>();

    void Start()
    {
        client = new Colyseus.Client("ws://localhost:2567");
        JoinChannel();
    }

    void Update()
    {
        if (chatInput.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SendMessage(chatInput.text);
                chatInput.text = "";
            }
        }

        if (!chatInput.isFocused && Input.GetKeyDown(KeyCode.Return)) {
            chatInput.ActivateInputField();
        }
    }

    async void JoinChannel() {
        channel = await client.JoinOrCreate<ChannelState>("channel");

        channel.OnMessage += OnMessage;
    }

    void SendMessage(string content) {
        ChannelMessage message = new ChannelMessage();

        message.content = content;

        channel.Send(message);
    }

    void OnMessage(object msg) {
        if (msg is ChannelMessage) MessageCreate(msg);
    }

    void MessageCreate(object msg) {
        var channelMessage = (ChannelMessage)msg;
        var message = new Message();

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

public class Message {
    public Text textObject;
}