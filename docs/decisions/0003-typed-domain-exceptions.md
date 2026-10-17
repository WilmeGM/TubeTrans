# 3. Typed domain exceptions with user-facing messages

- Status: Accepted
- Date: 2026-10-16

## Context

The first version of the controller had a single `catch (Exception)` that always showed the same generic message, so a video without captions was indistinguishable from a network failure. The requirements ask for specific messages in some cases (RF-08, RF-09) and a generic one in others (RF-10, RNF-05).

Showing `ex.Message` for every exception is not an option: messages of exceptions the application does not control can reveal internal details.

## Decision

- Define an abstract base, `TubeTransException`, and two subclasses: `TranscriptNotAvailableException` and `VideoNotFoundException`. Their messages are written to be shown to the user.
- The service translates YoutubeExplode's `VideoUnavailableException` into `VideoNotFoundException`, so library types do not leak to callers.
- The controller catches `TubeTransException` first, shows its message and logs a warning. A second `catch (Exception)` shows a generic message and logs the full exception as an error.

## Consequences

- A new expected error needs only a new subclass. The controller does not change.
- Any exception that is not a `TubeTransException` is treated as unexpected, and its details never reach the user. A controller test checks this.
- Everyone who throws a `TubeTransException` must keep its message safe to display.
- Unexpected errors share one generic message. A rate limit from YouTube, for example, looks the same to the user as any other failure.
