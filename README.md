# Online Voting System (C#)

A console-based online voting system developed using C# and .NET 8.

Features:
- Voter account management
- Candidate management
- Voting functionality
- Gmail API integration for email notifications

## Screenshots

![Voting System](https://github.com/Asrorbek-Abrorov/OnlineVotingSystem/assets/138248626/08694a8d-d940-4c89-80d7-0ea09e107258)

![Voting System](https://github.com/Asrorbek-Abrorov/OnlineVotingSystem/assets/138248626/d6135839-fd09-4676-a3b5-598fd3e79bdc)

## Gmail API Configuration

The repository includes sanitized example credential files:

- `Credentials/examples/client_secret.example.json`
- `Credentials/examples/token.example.json`

To enable Gmail API:

1. Create your own Google OAuth Desktop application credentials.
2. Copy `client_secret.example.json` to `Credentials/client_secret.json`.
3. Replace the placeholder values with your own credentials.
4. Run the application and complete Google authorization.
5. Keep generated token files private. Never commit real credentials or tokens.
