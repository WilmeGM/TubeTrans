# TubeTrans
<img width="1912" height="857" alt="image" src="https://github.com/user-attachments/assets/ac45ad57-2489-4832-8d0b-fcf12fe617f0" />

Get the transcript of any public YouTube video, instantly — no sign-up, no ads, no tracking.

## What it does
<img width="1720" height="847" alt="image" src="https://github.com/user-attachments/assets/da704ecb-0953-43fb-89e2-a6124165fbb9" />

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
