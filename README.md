# slack-app-boilerplate

## Useful links

https://api.slack.com/apps

https://api.slack.com/methods/chat.postMessage 

https://api.slack.com/docs/messages/builder

https://api.slack.com/web#methods

## How to execute a command

```
curl -X POST \
  http://localhost:5001/api/slack2 \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -H 'cache-control: no-cache' \
  -H 'x-slack-request-timestamp: 1561931414' \
  -H 'x-slack-signature: v0=283155a3328d9cfd2a3308c566ca68fe4bd59088f14a8c1497662b52d633ecaf' \
  -d 'token=8O4s3rgvVFjHTv3aAVtbo2FP&team_id=TKVHCH21L&team_domain=luciandev&channel_id=CKSUPQ31A&channel_name=general&user_id=UKG1CTVU3&user_name=luciansr&command=%2Flucian&text=list&response_url=https%3A%2F%2Fhooks.slack.com%2Fcommands%2FTKVHCH21L%2F682573638535%2FnJeeLss5of7MRLAkdPn9sm3C&trigger_id=669321780995.675590580054.5a199315fdefcc23c84eb3042b4100d1'
  ```