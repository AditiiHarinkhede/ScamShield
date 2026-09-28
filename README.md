# ScamShield

AI-powered scam detection built with ASP.NET Core and the Gemini API.

## How it works
- Online mode: Gemini analyzes a message, link or offer and returns a risk score, warning signs and a summary.
- Offline mode: if there is no API key or no connection, simple keyword rules are used instead.

## Run it
1. Install the .NET SDK.
2. Open appsettings.json and paste the Gemini API key which is (AQ.Ab8RN6JTLLKVNWs5TQkIR9XHKNugiRGya3G-SEpb0tRJ-YJ40A) into "Enter_Your_API_Key_Here".
3. Run: dotnet run
4. Open the address shown in the terminal.

Without a key, the app still runs in offline mode.
