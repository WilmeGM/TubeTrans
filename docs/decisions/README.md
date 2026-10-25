# Architecture decision records

An architecture decision record (ADR) explains **why** a significant design choice was made: the context, the decision, and what it costs. The code shows what the application does. These records keep the reasoning that the code cannot show.

## Index

| # | Decision | Status |
|---|---|---|
| [0001](0001-use-youtubeexplode.md) | Use YoutubeExplode instead of the official YouTube API | Accepted |
| [0002](0002-layered-solution-and-domain-model.md) | Separate class library, and a domain model distinct from the view models | Accepted |
| [0003](0003-typed-domain-exceptions.md) | Typed domain exceptions with user-facing messages | Accepted |
| [0004](0004-heuristic-caption-track-selection.md) | Heuristic selection of the caption track | Accepted |
| [0005](0005-stateless-no-persistence.md) | Stateless application with no persistence | Accepted |
| [0006](0006-static-web-assets-in-production.md) | Enable static web assets to simulate Production locally | Superseded by 0008 |
| [0007](0007-service-interface-and-test-scope.md) | Service behind an interface, and the scope of unit tests | Accepted |
| [0008](0008-simulate-production-with-staging.md) | Simulate production locally with a Staging environment | Accepted |
| [0009](0009-forwarded-headers-set-by-the-host.md) | Forwarded headers are enabled by the host, not by the code | Accepted |

## Format

Each record has a status, a context, a decision and its consequences. A decision that is replaced is not edited: a new record supersedes it, and the old one is marked as superseded. That keeps the history of the reasoning.

To add one, copy the structure of an existing record and use the next number.

```markdown
# N. Title

- Status: Proposed | Accepted | Superseded by [N](file.md)
- Date: YYYY-MM-DD

## Context
## Decision
## Consequences
```
