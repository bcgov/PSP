# Teams Notification Action

Reusable local GitHub Action to send Microsoft Teams notifications via incoming webhook.

## Inputs

- `webhook-uri` (required): Teams incoming webhook URL.
- `summary` (required): Main notification text.
- `title` (optional, default `GitHub Actions Notification`): Card title.
- `color` (optional, default `17a2b8`): Message card color (hex without `#`).
- `timezone` (optional, default `UTC`): Timezone used in the card timestamp.

## Usage

```yaml
- name: Notify Teams
  uses: ./.github/actions/teams-notification
  with:
    webhook-uri: ${{ secrets.MS_TEAMS_WEBHOOK_URI_BUILD_CHANNEL }}
    summary: Deployment started.
    title: PIMS Deployment
    color: 17a2b8
    timezone: America/Los_Angeles
```
