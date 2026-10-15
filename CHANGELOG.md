# Changelog

All notable changes to this project are documented in this file.
The format is based on Keep a Changelog, and this project adheres to Semantic Versioning.

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