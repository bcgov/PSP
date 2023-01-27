import groovy.json.JsonOutput
import java.util.regex.Pattern

def version = "1.0"

// ------------------
// Notifications
// ------------------

def success(appName, version, environment, webhook_url = "") {
  sendNotification(
    webhook_url,
    "${appName}, release: [${version}] was deployed to **${environment}** 🚀",
    "#1ee321",  // green
    "https://i.imgur.com/zHzPWXr.png"
  )
}

def failure(appName, version, environment, webhook_url = "") {
  sendNotification(
    webhook_url,
    "Could not deploy ${appName}, release: [${version}] to **${environment}** 🤕",
    "#e3211e",  // red
    "https://i.imgflip.com/1czxka.jpg"
  )
}

def createPayload(text) {
  def jenkinsAvatar = "https://wiki.jenkins.io/download/attachments/2916393/headshot.png"

  def message = text
  def payload = JsonOutput.toJson([
    type: "message",
    attachments: [
      [
        contentType: "application/vnd.microsoft.card.adaptive",
        contentUrl: null,
        content: [
          '$schema': "http://adaptivecards.io/schemas/adaptive-card.json",
          type: "AdaptiveCard",
          version: "1.2",
          body: [
            [
              type: "TextBlock",
              text: message
            ]
          ]
        ]
      ]
    ]
  ])
  return payload
}

/*
 * Sends a notification to MS Teams (via webhook URL)
 */
def sendNotification(webhook_url, text, color, image_url = "") {
  if (webhook_url != "") {
    def message = text
    def payload = createPayload(message)

    writeFile(file: "post.json", text: payload)
    sh "curl -X POST -H 'Content-Type: application/json' --data @post.json ${webhook_url}"
  } else {
    echo text
  }
}

return this;
