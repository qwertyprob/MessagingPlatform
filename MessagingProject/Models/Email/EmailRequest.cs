using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class EmailRequest
{
    [JsonProperty("mail")]
    public Mail Mail { get; set; }

    [JsonProperty("sendEmailOnDate")]
    public DateTime SendEmailOnDate { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }
}

public class Mail
{
    [JsonProperty("to")]
    public List<string> To { get; set; }

    [JsonProperty("cc")]
    public List<string> Cc { get; set; }

    [JsonProperty("noReply")]
    public bool NoReply { get; set; }

    [JsonProperty("replyTo")]
    public string ReplyTo { get; set; } = "";

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("isHtmlBody")]
    public bool IsHtmlBody { get; set; }

    [JsonProperty("attachments")]
    public List<Attachment> Attachments { get; set; }
}

public class Attachment
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mediaType")]
    public string MediaType { get; set; }

    [JsonProperty("file")]
    public string File { get; set; }
}
