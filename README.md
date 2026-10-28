# TubeTrans
<img width="1916" height="952" alt="image" src="https://github.com/user-attachments/assets/0a142d50-3c3f-42af-bcbf-14c813c1acad" />

Get the transcript of any public YouTube video, instantly: no sign-up, no ads, no tracking.

> **Run it with Docker for the best results.**
> YouTube applies bot checks to requests that come from data-center IP addresses, which is where any cloud host runs. The [live demo](https://tubetrans.onrender.com) is hosted on a free cloud tier, so it works with some videos and fails with others. When it was last checked, `https://www.youtube.com/watch?v=dQw4w9WgXcQ` worked. Running TubeTrans on your own machine sends the requests from your own connection, which YouTube treats much better. See [Run it with Docker](#run-it-with-docker).

## Run it with Docker

You only need [Docker](https://docs.docker.com/get-started/get-docker/). The image works on Windows, Linux and macOS (Intel and Apple Silicon).

```bash
docker run --rm -p 8080:8080 wilmegm/tubetrans
```

Then open <http://localhost:8080>, paste a YouTube link and click **Get transcript**. Press `Ctrl+C` in the terminal to stop it.

To keep it running in the background instead:

```bash
docker run -d --name tubetrans -p 8080:8080 wilmegm/tubetrans
docker stop tubetrans
docker rm tubetrans
```

Please be considerate: YouTube can also rate-limit a home connection if it receives many requests in a short time.

## Live demo (limited)

<https://tubetrans.onrender.com>

It runs on a free tier that puts the service to sleep after a period without traffic, so the first load can take around a minute. Because of the bot checks explained above, many videos will return an error there.

## What it does

Paste a YouTube video link and TubeTrans fetches its captions and shows the full transcript as plain text, in the video's original language, ready to copy with one click.

## Key characteristics

- No accounts and no login
- No database: nothing is stored between requests
- No translation: the transcript is shown in the video's original language

## Tech stack

- ASP.NET Core MVC
- [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) to read captions without an API key
- Bootstrap 5
- xUnit
- Docker

## Running from source

Requires the [.NET SDK](https://dotnet.microsoft.com/download), version 10 or later.

```bash
git clone https://github.com/WilmeGM/TubeTrans.git
cd TubeTrans
dotnet run --project src/TubeTrans
```

The console prints the local URL the app is listening on.

To run the tests:

```bash
dotnet test
```

To build the image yourself:

```bash
docker build -t tubetrans .
docker run --rm -p 8080:8080 tubetrans
```

## Project structure

```text
src/      Application (business logic) and TubeTrans (ASP.NET Core MVC web app)
tests/    Unit tests
docs/     Requirements, architecture and decision records
```

## Documentation

- [Software requirements (SRS)](docs/SRS.md)
- [Architecture](docs/architecture.md)
- [Decision records](docs/decisions/README.md)
- [Changelog](CHANGELOG.md)

## Known limitations

- **Requests from cloud hosts are often blocked.** YouTube may ask for a "sign in to confirm you're not a bot" check on requests that come from data-center IP addresses. That is why the public demo is unreliable and why running the container yourself is recommended ([details](docs/decisions/0010-render-demo-with-known-limitation.md)).
- Captions are read through an unofficial library, so YouTube may change how the data is exposed.
- YouTube Shorts and videos without captions are not supported.

## License

See [LICENSE](LICENSE).

## Author

**Wilme González** — [@WilmeGM](https://github.com/WilmeGM)
