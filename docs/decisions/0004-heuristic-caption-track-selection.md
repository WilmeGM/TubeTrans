# 4. Heuristic selection of the caption track

- Status: Accepted
- Date: 2026-10-16

## Context

A video can offer several caption tracks, and the transcript must be in the original language of the video (RF-05). YoutubeExplode lists the available tracks but does not say which one matches the original audio. The official YouTube API exposes the default audio language, but it requires an API key (see [ADR 0001](0001-use-youtubeexplode.md)).

## Decision

`CaptionTrackSelector` applies these rules in order:

1. No tracks: there is no result.
2. A single track: use it.
3. Exactly one manually created track: use it, since a creator-supplied track is usually in the main language.
4. Otherwise: use the first track in the list, assuming YouTube lists the main audio track first.

## Consequences

- No extra dependency or request is needed.
- The last rule is an assumption, not a guarantee. On videos with several auto-generated tracks, such as dubbed ones, the selected language may not be the original one.
- The behavior is documented in the code and covered by unit tests, so any change to the rules is deliberate.
- Letting the user choose the track is listed as a future improvement in the SRS.
