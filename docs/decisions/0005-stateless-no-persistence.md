# 5. Stateless application with no persistence

- Status: Accepted
- Date: 2026-10-16

## Context

The requirements ask for no database, no accounts and no data kept between requests (RNF-01, RNF-02 and RF-11 of the SRS). This also keeps the privacy policy simple and honest.

## Decision

- No database, session, cookies of its own or cache. The services are registered as scoped and hold no state.
- The result of a lookup lives only in `ViewBag` for the duration of one request. The POST renders the page directly instead of redirecting (Post/Redirect/Get), because a redirect would need the transcript to survive until the next request.
- Logs include the submitted URL, to diagnose errors, but never the transcript content. The privacy policy says so.

## Consequences

- There is nothing to secure, migrate or clean up, and no history of user activity exists.
- Repeating a request calls YouTube again, which makes the rate limit described in [ADR 0001](0001-use-youtubeexplode.md) more likely.
- Refreshing the page after a lookup makes the browser offer to re-submit the form.
- If an in-memory cache is added in the future, it must be short-lived, kept in memory only, and reflected in the privacy policy.
