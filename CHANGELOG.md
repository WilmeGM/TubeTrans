# Changelog

All notable changes to this project are documented in this file.
The format is based on Keep a Changelog, and this project adheres to Semantic Versioning.

## [1.1.0] - 2026-10-27

### Added
- `Dockerfile` and `.dockerignore` to build a container image that runs on `linux/amd64` and `linux/arm64`.
- Public Docker image on Docker Hub (`wilmegm/tubetrans`), with run instructions in the README.
- Demo deployment on Render (free tier).
- Decision records 0008 to 0011: Staging environment for local production simulation, forwarded headers, the Render demo and its limitation, and Docker Hub distribution.

### Changed
- When a video cannot be accessed, the message no longer claims it does not exist: it also covers YouTube refusing to serve it to the server. The original exception is now kept in the logs.
- `UseStaticWebAssets` now runs in the `Staging` environment instead of `Production`.
- README, SRS and architecture documentation updated, with the known limitation of cloud hosting stated at the top of the README.

## [1.0.5] - 2026-10-18

### Added
- XML documentation for every public member, with documentation file generation enabled in both projects.
- Architecture overview (`docs/architecture.md`) with diagrams, and seven architecture decision records (`docs/decisions/`).
- Software requirements specification (`docs/SRS.md`), aligned with the implemented behavior.
- README with instructions to run the app and the tests, and links to the documentation.

### Changed
- Solution restructured: application projects now live under `src/` and tests under `tests/`.
- The error page no longer relies on a view model.

### Removed
- Unused `ErrorViewModel`.

## [1.0.4] - 2026-10-15

### Added
- Unit test project (xUnit) covering URL parsing, request validation, caption track selection and the transcript controller.
- The controller now depends on the `IYoutubeTranscriptService` interface instead of the concrete service.

## [1.0.3] - 2026-10-14

### Changed
- `YoutubeTranscriptService` now exposes only `GetTranscriptAsync`; its helper methods are private, keeping YoutubeExplode types out of the service's public surface.

## [1.0.2] - 2026-10-14

### Fixed
- YouTube links where the `v` parameter is not the first query parameter (e.g. `watch?feature=shared&v=...`) are now accepted.

## [1.0.1] - 2026-10-10

### Added
- Custom error page, shown when an unexpected error happens outside the transcript flow, without exposing any technical details.
- Request duration (in milliseconds) in the log entry of each successful transcript request.

## [1.0.0] - 2026-10-08

### Added
- Transcript extraction for public YouTube videos in their original language.
- Minimalist search-style UI with one-click copy of the transcript.
- URL validation, with a specific message for unsupported YouTube Shorts.
- Typed domain errors for videos without captions or unavailable videos.
- Structured logging for successful requests, known errors and unexpected failures.
- Privacy policy page.