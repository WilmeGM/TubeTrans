# 7. Service behind an interface, and the scope of unit tests

- Status: Accepted
- Date: 2026-10-16

## Context

The controller had to be tested without calling YouTube. `YoutubeClient`, from YoutubeExplode, is a concrete class, and registering it in dependency injection does not make it replaceable in a test: only an interface does.

The service also exposed four public methods when the controller used one, and two of them returned YoutubeExplode types, which worked against keeping the library isolated.

## Decision

- Introduce `IYoutubeTranscriptService` with a single method, `GetTranscriptAsync`. The controller depends on the interface. `YoutubeTranscriptService` implements it, and its helper methods are private.
- Unit tests cover the application's own logic: `YoutubeUrlHelper`, the validation chain of `TranscriptRequestViewModel`, `CaptionTrackSelector`, and `TranscriptController` through a hand-written fake of the interface.
- `YoutubeTranscriptService.GetTranscriptAsync` is not unit-tested.

## Consequences

- The controller's behavior is tested, including that no internal detail reaches the user when an unexpected error occurs.
- The public surface of the service is one method.
- The orchestration inside the service has no automated tests: the translation of `VideoUnavailableException`, the "no captions" check and the text consolidation. Testing it would require either calling YouTube, which is slow and runs into the rate limit, or wrapping `YoutubeClient` in another abstraction, which is too much for the size of the project. Manual testing covers it.
