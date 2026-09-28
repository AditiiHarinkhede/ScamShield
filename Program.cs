using System.Text.Json;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">

<title>ScamShield</title>

<style>

* {
    box-sizing: border-box;
}

body {
    margin: 0;
    background: #090909;
    color: #e8e8e8;
    font-family: Arial, Helvetica, sans-serif;
}

.container {
    max-width: 1100px;
    margin: auto;
    padding: 35px 22px 60px;
}

/* HEADER */

.header {
    margin-bottom: 30px;
}

.logo {
    color: #7db7ff;
    font-family: monospace;
    font-size: 14px;
    letter-spacing: 2px;
    margin-bottom: 12px;
}

h1 {
    font-size: 44px;
    margin: 0;
    font-weight: 500;
}

.subtitle {
    color: #929292;
    margin-top: 12px;
    font-size: 17px;
    line-height: 1.5;
}

/* INPUT */

.card {
    background: #111111;
    border: 1px solid #202020;
    border-radius: 22px;
    padding: 28px;
    margin-top: 22px;
}

.section-title {
    font-size: 21px;
    font-weight: bold;
    margin-bottom: 18px;
}

.tabs {
    display: flex;
    gap: 10px;
    margin-bottom: 18px;
}

.tab {
    padding: 10px 18px;
    border-radius: 20px;
    border: 1px solid #303030;
    background: #181818;
    color: #aaa;
    cursor: pointer;
}

.tab.active {
    background: #2257c7;
    color: white;
    border-color: #2257c7;
}

textarea,
input {
    width: 100%;
    background: #151515;
    color: #eee;
    border: 1px solid #333;
    border-radius: 14px;
    padding: 16px;
    font-size: 16px;
    outline: none;
}

textarea {
    min-height: 150px;
    resize: vertical;
}

textarea:focus,
input:focus {
    border-color: #367cff;
}

button {
    border: none;
    border-radius: 12px;
    padding: 14px 22px;
    font-size: 16px;
    font-weight: bold;
    cursor: pointer;
}

.analyze {
    margin-top: 15px;
    width: 100%;
    background: #2864df;
    color: white;
}

.analyze:hover {
    background: #3573ed;
}

.samples {
    display: flex;
    gap: 10px;
    flex-wrap: wrap;
    margin-top: 16px;
}

.sample {
    background: #1b1b1b;
    color: #aaa;
    border: 1px solid #303030;
    padding: 9px 13px;
    font-size: 13px;
}

/* RESULT */

.result {
    display: none;
}

.risk-card {
    border-radius: 22px;
    padding: 28px;
    margin-top: 22px;
    border: 1px solid;
}

.risk-critical {
    background: #350000;
    border-color: #ff4949;
}

.risk-high {
    background: #301600;
    border-color: #ff8b32;
}

.risk-medium {
    background: #302800;
    border-color: #e8c94c;
}

.risk-low {
    background: #062d19;
    border-color: #35c77a;
}

.risk-top {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 20px;
}

.badge {
    display: inline-block;
    padding: 10px 20px;
    border-radius: 30px;
    background: #ff4949;
    color: white;
    font-weight: bold;
}

.risk-critical .badge { background: #ff4949; }
.risk-high .badge { background: #ff8b32; }
.risk-medium .badge { background: #c9a91f; }
.risk-low .badge { background: #35c77a; color: #04210f; }

.score-label {
    color: #aaa;
    font-style: italic;
    font-size: 15px;
}

.score {
    font-size: 48px;
    font-weight: 300;
}

.score span {
    color: #777;
    font-size: 20px;
}

.detected {
    font-family: monospace;
    font-size: 22px;
    margin-top: 28px;
    word-break: break-word;
}

/* GRID */

.grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 22px;
    margin-top: 22px;
}

@media(max-width: 750px) {
    .grid {
        grid-template-columns: 1fr;
    }

    h1 {
        font-size: 34px;
    }

    .risk-top {
        align-items: flex-start;
    }
}

/* VECTOR */

.bar-row {
    display: grid;
    grid-template-columns: 120px 1fr 40px;
    gap: 12px;
    align-items: center;
    margin: 20px 0;
}

.bar-label {
    color: #ccc;
}

.bar {
    height: 14px;
    background: #292929;
    border-radius: 20px;
    overflow: hidden;
}

.fill {
    height: 100%;
    background: #ff4e4e;
    border-radius: 20px;
}

/* BREAKDOWN */

.breakdown-row {
    display: flex;
    justify-content: space-between;
    gap: 20px;
    padding: 16px 0;
    border-bottom: 1px solid #252525;
}

.breakdown-row:last-child {
    border-bottom: none;
}

.value {
    color: #ff6868;
    font-family: monospace;
    text-align: right;
}

/* FLAGS */

.flag {
    padding: 16px;
    margin: 12px 0;
    background: #171717;
    border-left: 4px solid #ff5151;
    border-radius: 8px;
}

.flag-title {
    font-weight: bold;
    margin-bottom: 6px;
}

.flag-description {
    color: #999;
    line-height: 1.5;
}

/* ADVICE */

.advice {
    color: #bbb;
    line-height: 1.7;
}

.footer-note {
    color: #666;
    text-align: center;
    margin-top: 35px;
    font-size: 13px;
}

.loading {
    text-align: center;
    color: #888;
    padding: 30px;
}

</style>
</head>

<body>

<div class="container">

    <div class="header">

        <div class="logo">
            SCAMSHIELD // THREAT ANALYSIS
        </div>

        <h1>ScamShield</h1>

        <div class="subtitle">
            AI-powered analysis for suspicious messages, links,
            job offers, investments and online fraud.
        </div>

    </div>


    <!-- INPUT -->

    <div class="card">

        <div class="section-title">
            Analyze suspicious content
        </div>

        <div class="tabs">

            <button class="tab active" onclick="setMode('message', this)">
                Message
            </button>

            <button class="tab" onclick="setMode('url', this)">
                URL / Link
            </button>

            <button class="tab" onclick="setMode('offer', this)">
                Offer
            </button>

        </div>

        <textarea
            id="input"
            placeholder="Paste a suspicious SMS, email or online message here..."
        ></textarea>

        <button class="analyze" onclick="analyze()">
            Analyze Threat
        </button>

        <div class="samples">

            <button class="sample" onclick="loadPhishing()">
                Phishing Scam
            </button>

            <button class="sample" onclick="loadJob()">
                Job Scam
            </button>

            <button class="sample" onclick="loadInvestment()">
                Investment Scam
            </button>

            <button class="sample" onclick="loadSafe()">
                Safe Message
            </button>

        </div>

    </div>


    <!-- RESULTS -->

    <div id="result" class="result">

        <div id="riskCard" class="risk-card">

            <div class="risk-top">

                <div id="badge" class="badge">
                    SCAM DETECTED
                </div>

                <div>
                    <div class="score-label">
                        RISK SCORE
                    </div>

                    <div class="score">
                        <span id="score">94</span>
                        <span>/100</span>
                    </div>
                </div>

            </div>

            <div id="detected" class="detected"></div>

        </div>


        <div class="grid">

            <!-- VECTOR ANALYSIS -->

            <div class="card">

                <div class="section-title">
                    Vector Risk Analysis
                </div>

                <div id="vectors"></div>

            </div>


            <!-- BREAKDOWN -->

            <div class="card">

                <div class="section-title">
                    Analytical Breakdown
                </div>

                <div id="breakdown"></div>

            </div>

        </div>


        <!-- RED FLAGS -->

        <div class="card">

            <div class="section-title">
                Warning Signs Detected
            </div>

            <div id="flags"></div>

        </div>


        <!-- ADVICE -->

        <div class="card">

            <div class="section-title">
                What should you do?
            </div>

            <div class="advice">

                • Don't share OTPs, passwords or financial information.<br>
                • Don't send money based only on an unexpected message.<br>
                • Verify the sender using an official website or phone number.<br>
                • Avoid clicking suspicious links.<br>
                • If an offer seems unusually good, verify it independently.<br><br>

                <strong>
                    ScamShield identifies potential warning signs.
                    It does not guarantee that a message is fraudulent.
                </strong>

            </div>

        </div>

    </div>


    <div class="footer-note">
        ScamShield uses Gemini AI to identify potential fraud patterns.
        Results are estimates and should be independently verified.
    </div>

</div>


<script>

let mode = "message";


// Escapes text before it is placed into innerHTML (prevents XSS).
function esc(value) {

    return String(value ?? "").replace(/[&<>"']/g, function (c) {
        return {
            "&": "&amp;",
            "<": "&lt;",
            ">": "&gt;",
            '"': "&quot;",
            "'": "&#39;"
        }[c];
    });
}


function setMode(newMode, button) {

    mode = newMode;

    document
        .querySelectorAll(".tab")
        .forEach(x => x.classList.remove("active"));

    button.classList.add("active");

    const input = document.getElementById("input");

    if (newMode === "url") {

        input.placeholder =
            "Paste a suspicious URL or domain here...";

    } else if (newMode === "offer") {

        input.placeholder =
            "Paste a job, investment, shopping or other offer here...";

    } else {

        input.placeholder =
            "Paste a suspicious SMS, email or online message here...";

    }
}


async function analyze() {

    const message =
        document.getElementById("input").value;

    if (!message.trim()) {

        alert("Please enter something to analyze.");

        return;
    }

    document.getElementById("result").style.display = "block";

    document.getElementById("flags").innerHTML =
        '<div class="loading">Analyzing threat...</div>';

    try {

        const response = await fetch("/api/analyze", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                message: message,
                mode: mode
            })

        });

        const data = await response.json();

        if (!response.ok) {
            alert(data.detail || data.error || "Analysis failed. Please try again.");
            return;
        }

        displayResults(data);

    } catch (err) {

        alert("Could not reach the server. Please try again.");
    }
}


function displayResults(data) {

    const result =
        document.getElementById("result");

    result.style.display = "block";

    const riskCard =
        document.getElementById("riskCard");

    riskCard.className =
        "risk-card risk-" + String(data.risk).toLowerCase();

    document.getElementById("score").innerText =
        data.score;

    let badge = "SAFE";

    if (data.risk === "CRITICAL")
        badge = "SCAM DETECTED";

    else if (data.risk === "HIGH")
        badge = "HIGH RISK";

    else if (data.risk === "MEDIUM")
        badge = "SUSPICIOUS";

    document.getElementById("badge").innerText =
        badge;


    document.getElementById("detected").innerText =
        data.subject;


    // VECTOR BARS

    const vectors = data.vectors || {};

    const vectorNames = [
        ["Urgency", vectors.urgency || 0],
        ["Sensitive Data", vectors.sensitive || 0],
        ["Link Risk", vectors.link || 0],
        ["Impersonation", vectors.impersonation || 0],
        ["Fraud Pattern", vectors.fraud || 0]
    ];

    document.getElementById("vectors").innerHTML =
        vectorNames.map(v => `

            <div class="bar-row">

                <div class="bar-label">
                    ${esc(v[0])}
                </div>

                <div class="bar">
                    <div
                        class="fill"
                        style="width:${Number(v[1])}%"
                    ></div>
                </div>

                <div>${Number(v[1])}</div>

            </div>

        `).join("");


    const flags = data.flags || [];


    // BREAKDOWN

    document.getElementById("breakdown").innerHTML = `

        <div class="breakdown-row">
            <span>Analysis Type</span>
            <span class="value">${esc(data.analysisType)}</span>
        </div>

        <div class="breakdown-row">
            <span>Warning Signs</span>
            <span class="value">${flags.length}</span>
        </div>

        <div class="breakdown-row">
            <span>Risk Score</span>
            <span class="value">${esc(data.score)}/100</span>
        </div>

        <div class="breakdown-row">
            <span>Detection Mode</span>
            <span class="value">${esc(data.detectionMode || "Gemini AI")}</span>
        </div>

        <div class="breakdown-row">
            <span>Category</span>
            <span class="value">${esc(data.category || "Unknown")}</span>
        </div>

        <div class="breakdown-row">
            <span>AI Summary</span>
            <span class="value">${esc(data.summary)}</span>
        </div>

    `;


    // FLAGS

    if (flags.length === 0) {

        document.getElementById("flags").innerHTML = `

            <div class="flag"
                 style="border-left-color:#35c77a">

                <div class="flag-title">
                    No obvious warning signs detected
                </div>

                <div class="flag-description">
                    This does not guarantee that the content is safe.
                    Always verify unexpected requests independently.
                </div>

            </div>

        `;

        return;
    }


    document.getElementById("flags").innerHTML =
        flags.map(flag => `

            <div class="flag">

                <div class="flag-title">
                    ${esc(flag.title)}
                </div>

                <div class="flag-description">
                    ${esc(flag.description)}
                </div>

            </div>

        `).join("");
}


function loadPhishing() {

    mode = "message";

    document.getElementById("input").value =
`URGENT: Your SBI account will be blocked within 24 hours.

Verify your KYC immediately by clicking this link:
https://example.com/verify

Enter your OTP to complete verification.`;

}


function loadJob() {

    mode = "offer";

    document.getElementById("input").value =
`Congratulations! You have been selected for a work-from-home position.

Earn ₹50,000 per month with no experience required.

To activate your employee account, pay a refundable registration fee of ₹1,999 immediately.`;

}


function loadInvestment() {

    mode = "offer";

    document.getElementById("input").value =
`Limited investment opportunity!

Invest ₹5,000 today and receive guaranteed returns of ₹25,000 within 7 days.

Only 10 slots remain. Send your payment immediately to reserve your position.`;

}


function loadSafe() {

    mode = "message";

    document.getElementById("input").value =
`Hi, your library book is due for return on Friday.

Please return it at the library counter during regular hours.

Thank you.`;

}

</script>

</body>
</html>
""", "text/html"));


app.MapPost("/api/analyze", async (
    AnalysisRequest request,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory) =>
{
    if (string.IsNullOrWhiteSpace(request.Message))
        return Results.BadRequest(new { error = "Please enter something to analyze." });

    string mode = string.IsNullOrWhiteSpace(request.Mode) ? "message" : request.Mode.ToLowerInvariant();

    string? apiKey = configuration["GeminiApiKey"];

    // Change the model in appsettings / user-secrets with "GeminiModel" if needed.
    string model = configuration["GeminiModel"] ?? "gemini-3.8-flash";

    // This text is shown on the page if Gemini fails, so you can see the reason.
    string geminiProblem = "No GeminiApiKey was found in your settings.";

    Console.WriteLine($"[Gemini] API key loaded: {!string.IsNullOrWhiteSpace(apiKey)} | model: {model}");

    // ONLINE MODE: Try Gemini first.
    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        try
        {
            geminiProblem = "Gemini replied, but the answer could not be understood.";

            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(60);

            // NOTE: $$""" means single { } are literal braces,
            // and interpolation is written as {{ ... }}.
            string prompt = $$"""
You are ScamShield, a cybersecurity assistant that analyzes suspicious online content.

Analyze the following {{mode}}:

--- CONTENT START ---
{{request.Message}}
--- CONTENT END ---

Return ONLY valid JSON with exactly these fields:
{
  "risk": "LOW" | "MEDIUM" | "HIGH" | "CRITICAL",
  "score": integer from 0 to 100,
  "subject": short uppercase analysis title,
  "analysisType": "{{mode.ToUpperInvariant()}}",
  "summary": short plain-English explanation,
  "category": short scam category, e.g. "Phishing", "Fake Job", "Investment Fraud", "Legitimate",
  "vectors": {
    "urgency": integer 0-100,
    "sensitive": integer 0-100,
    "link": integer 0-100,
    "impersonation": integer 0-100,
    "fraud": integer 0-100
  },
  "flags": [
    {
      "title": short warning-sign title,
      "description": concise explanation
    }
  ]
}

Rules:
- Analyze the actual content; do not assume it is a scam merely because it contains a link.
- A legitimate message can receive a LOW score.
- Do not call something fraudulent with certainty. Use "potential", "appears", or similar language when appropriate.
- Score based on the evidence present in the supplied content.
- Identify phishing, fake jobs, investment fraud, shopping scams, impersonation, credential theft, suspicious links, urgency, advance-fee requests, and similar patterns when supported.
""";

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json"
                }
            };

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent");

            httpRequest.Headers.Add("x-goog-api-key", apiKey);
            httpRequest.Content = JsonContent.Create(body);

            using var response = await client.SendAsync(httpRequest);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using var geminiJson = JsonDocument.Parse(responseText);

                var generatedText =
                    geminiJson.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                if (!string.IsNullOrWhiteSpace(generatedText))
                {
                    var cleanJson = generatedText.Trim();

                    if (cleanJson.StartsWith("```"))
                    {
                        cleanJson = cleanJson
                            .Replace("```json", "")
                            .Replace("```", "")
                            .Trim();
                    }

                    var aiResult = JsonSerializer.Deserialize<GeminiResult>(
                        cleanJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (aiResult != null)
                    {
                        return Results.Ok(new
                        {
                            risk = aiResult.Risk.ToUpperInvariant(),
                            score = Math.Clamp(aiResult.Score, 0, 100),
                            subject = aiResult.Subject,
                            analysisType = mode.ToUpperInvariant(),
                            summary = aiResult.Summary,
                            category = aiResult.Category,
                            vectors = aiResult.Vectors,
                            flags = aiResult.Flags ?? new List<GeminiFlag>(),
                            detectionMode = "Gemini AI"
                        });
                    }
                }
            }
            else
            {
                geminiProblem = $"Gemini returned HTTP {(int)response.StatusCode} ({response.StatusCode}).";
                Console.WriteLine($"[Gemini] HTTP {(int)response.StatusCode}: {responseText}");
            }
        }
        catch (Exception ex)
        {
            geminiProblem = "Gemini call failed: " + ex.Message;
            Console.WriteLine($"[Gemini] Exception: {ex.Message}");
        }
    }

    // OFFLINE MODE: local heuristic fallback.
    return Results.Ok(AnalyzeOffline(request.Message, mode, geminiProblem));
});

app.Run();


static object AnalyzeOffline(string rawMessage, string mode, string reason)
{
    string message = rawMessage.ToLowerInvariant();

    var flags = new List<object>();

    int urgency = 0;
    int sensitive = 0;
    int link = 0;
    int impersonation = 0;
    int fraud = 0;

    if (message.Contains("urgent") ||
        message.Contains("immediately") ||
        message.Contains("act now") ||
        message.Contains("limited time") ||
        message.Contains("24 hours") ||
        message.Contains("today only"))
    {
        urgency = 90;
        flags.Add(new
        {
            title = "⏰ Urgency or pressure",
            description = "The message pressures the recipient to act quickly instead of giving them time to verify the request."
        });
    }

    if (message.Contains("otp") ||
        message.Contains("password") ||
        message.Contains("cvv") ||
        message.Contains("pin") ||
        message.Contains("bank details"))
    {
        sensitive = 95;
        flags.Add(new
        {
            title = "🔐 Sensitive information request",
            description = "The message appears to request credentials, OTPs or other information that should not normally be shared."
        });
    }

    if (message.Contains("http://") ||
        message.Contains("https://") ||
        message.Contains("click this") ||
        message.Contains("click the link"))
    {
        link = 80;
        flags.Add(new
        {
            title = "🔗 Suspicious link activity",
            description = "The message encourages the recipient to follow a link or provides a web address that should be independently verified."
        });
    }

    if (message.Contains("sbi") ||
        message.Contains("bank") ||
        message.Contains("paypal") ||
        message.Contains("government") ||
        message.Contains("tax department") ||
        message.Contains("account will be blocked"))
    {
        impersonation = 85;
        flags.Add(new
        {
            title = "🏦 Possible impersonation",
            description = "The message references a financial institution, organization or authority in a way that may be attempting to establish trust."
        });
    }

    if (message.Contains("registration fee") ||
        message.Contains("pay") ||
        message.Contains("guaranteed returns") ||
        message.Contains("guaranteed profit") ||
        message.Contains("earn ₹") ||
        message.Contains("work-from-home"))
    {
        fraud = 90;
        flags.Add(new
        {
            title = "💰 Potential financial fraud pattern",
            description = "The content contains patterns commonly associated with advance-fee, fake job or unrealistic investment offers."
        });
    }

    if (message.Contains("prize") ||
        message.Contains("winner") ||
        message.Contains("lottery") ||
        message.Contains("free gift") ||
        message.Contains("reward"))
    {
        fraud = Math.Max(fraud, 80);
        flags.Add(new
        {
            title = "🎁 Unexpected reward",
            description = "Unexpected prizes and rewards can be used to encourage people to click links or provide personal information."
        });
    }

    int score = (urgency + sensitive + link + impersonation + fraud) / 5;

    if (flags.Count == 0)
        score = 5;

    string risk =
        score >= 75 ? "CRITICAL" :
        score >= 50 ? "HIGH" :
        score >= 20 ? "MEDIUM" :
        "LOW";

    string subject =
        mode == "url" ? "URL / DOMAIN ANALYSIS" :
        mode == "offer" ? "ONLINE OFFER ANALYSIS" :
        "MESSAGE ANALYSIS";

    return new
    {
        risk,
        score,
        subject,
        analysisType = mode.ToUpperInvariant(),
        summary = "Offline analysis used. Reason: " + reason,
        category = flags.Count == 0 ? "No pattern detected" : "Heuristic match",
        vectors = new
        {
            urgency,
            sensitive,
            link,
            impersonation,
            fraud
        },
        flags,
        detectionMode = "Offline"
    };
}

public class GeminiResult
{
    public string Risk { get; set; } = "LOW";
    public int Score { get; set; }
    public string Subject { get; set; } = "MESSAGE ANALYSIS";
    public string Summary { get; set; } = "";
    public string Category { get; set; } = "Unknown";
    public GeminiVectors Vectors { get; set; } = new();
    public List<GeminiFlag>? Flags { get; set; }
}

public class GeminiVectors
{
    public int Urgency { get; set; }
    public int Sensitive { get; set; }
    public int Link { get; set; }
    public int Impersonation { get; set; }
    public int Fraud { get; set; }
}

public class GeminiFlag
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}


record AnalysisRequest(
    string Message,
    string Mode
);