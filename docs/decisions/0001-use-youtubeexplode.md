# 1. Use YoutubeExplode instead of the official YouTube API

- Status: Accepted
- Date: 2026-10-16

## Context

TubeTrans must work with no sign-up and no API keys, using free tiers only (requirements RNF-02 and RNF-03 of the SRS).

The official YouTube Data API requires an API key and is subject to a quota. Its caption download also requires authorization from the owner of the video, so it cannot serve the transcript of an arbitrary public video.

## Decision

Use [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode), a NuGet library that reads the same data the YouTube website uses, without keys or authentication.

All calls to it are isolated in `YoutubeTranscriptService`, behind `IYoutubeTranscriptService`.

## Consequences

- No keys, accounts or quotas to manage.
- The access is unofficial: YouTube can change or block it without notice. Keeping every call in one service limits the code that would have to change.
- YouTube rate-limits by IP address. During development, many requests in a short time produced a `RequestLimitExceededException`. The user gets a generic message and the error is logged.
- Under real traffic this would be a recurring problem. Possible mitigations, none implemented: a short-lived in-memory cache, or moving to the official API, which would bring back API keys and conflict with RNF-03.
