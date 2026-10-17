# TubeTrans

Get the transcript of any public YouTube video, instantly: no sign-up, no ads, no tracking.

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

## Running locally

Requires the [.NET SDK](https://dotnet.microsoft.com/download), version 9 or later.

```bash
git clone https://github.com/WilmeGM/TubeTrans.git
cd TubeTrans
dotnet run --project src/TubeTrans
```

The console prints the local URL the app is listening on.

## Running the tests

```bash
dotnet test
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

- Captions are read through an unofficial library, so YouTube may temporarily rate-limit requests or change how the data is exposed
- YouTube Shorts and videos without captions are not supported

## License

See [LICENSE](LICENSE).

## Author

**Wilme González** — [@WilmeGM](https://github.com/WilmeGM)
