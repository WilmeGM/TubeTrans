# 2. Separate class library, and a domain model distinct from the view models

- Status: Accepted
- Date: 2026-10-16

## Context

The business logic (parsing URLs, fetching transcripts) has nothing to do with HTTP. Keeping it in the web project would make it easy to mix responsibilities by accident, and harder to reuse or test.

Also, the data the service produces and the data a view needs are not the same thing. The view needs an error state and a message. The service does not know about either.

## Decision

- Split the solution in two projects. `Application` is a class library with the models, services, helpers, enums, exceptions and view models. `TubeTrans` is the web project, with controllers and views. The web project references `Application`, never the reverse.
- The service returns a domain model, `VideoTranscript`. The controller maps it to a `TranscriptResponseViewModel` for the view.

## Consequences

- `Application` contains no ASP.NET Core types, so its logic can be reused outside the web project and tested without a server.
- The service does not depend on presentation decisions.
- The view models live in `Application`, even though they are a presentation concept. A stricter layering would keep them in the web project. This was accepted for simplicity and can be revisited if a second front end appears.
- The mapping costs a few lines in the controller.
