# TubeTrans

Get the transcript of any public YouTube video, instantly — no sign-up, no ads, no tracking.

## What it does

Paste a YouTube video link and TubeTrans fetches its available captions, then shows the full transcript as plain text that you can copy with one click.

## Tech stack

- ASP.NET Core MVC (.NET)
- [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) — fetches captions without an API key
- Bootstrap 5

## Key characteristics

- No accounts, no login
- No database — nothing is stored between requests
- No translation — transcripts are shown in the video's original language

## Running locally

```bash
git clone https://github.com/WilmeGM/TubeTrans.git
cd TubeTrans
dotnet run --project TubeTrans
```

## Author

**Wilme González** — [@WilmeGM](https://github.com/WilmeGM)
