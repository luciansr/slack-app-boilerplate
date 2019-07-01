# slack-app-boilerplate

## How it works

Each command is redirected to a Mvc Controller. 

For instance,
 
1. If you send to slack:
```
/[command] list
```
It will be redirected to `/api/list`

2. If you send:

```
/[command] list something
```
It will be redirected to `/api/list/something`

3. If you send extra parameters:

```
/[command] list something extra things
```
It will be redirected to `/api/list/something&extraParameters=extra+things`

Easy peasy.

## Authentication/Authorization

Using `[SlackAuthentication]`: 

All your requests are validated, so you know that they are really from Slack.

Using: 
```
[SlackAuthorize(authorizedUsernames: new[] {"luciansr"})]
``` 
You can ensure that only "luciansr" is allowed to execute your route.

```
[SlackAuthorize(authorizedChannels: new[] {"general"})]
``` 

You can ensure that only requests from the channel "general" are allowed to execute your route.


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
  
  All you have to do it to start coding.