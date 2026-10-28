# 10. Keep the Render deployment as a demo with a known limitation

- Status: Accepted
- Date: 2026-10-27

## Context

TubeTrans was deployed to Render's free tier as a Docker container. The deployment itself works: the image builds, the service starts, it listens on the expected port, it passes the health check, and the logs are readable in the dashboard.

The main feature, however, is unreliable from there. Some videos return their transcript and others fail with "video not available". A diagnostic endpoint, added temporarily and then removed, showed what YouTube answers to a plain HTTP request from Render's IP address:

- For a video that failed, the watch page came back with HTTP 200 but with a playability status of `LOGIN_REQUIRED` and the reason "Sign in to confirm you're not a bot".
- For other videos, including one extremely popular one, the same request returned the normal page with status `OK`.

The same image, with the same code, works from a home connection. Earlier, a home IP address was itself rate-limited by YouTube for a while (`RequestLimitExceededException`), so even that is not guaranteed. The error does not come from the application, from Docker, or from the library: it is YouTube's decision about the origin of the request, and the library's maintainers state that they cannot work around it.

## Decision

- Keep the Render deployment as a public demo, and say plainly that it works only with some videos.
- Change the message shown when a video cannot be accessed, so it no longer claims that the video does not exist. It now covers the case where YouTube refuses to serve the video to the server.
- Keep the original exception as the inner exception of `VideoNotFoundException`, so the logs show the real reason.
- Provide a reliable way to run the application, by distributing the container image ([ADR 0011](0011-distribute-image-on-docker-hub.md)).

Alternatives considered:

- Sign in to YouTube with the cookies of a Google account. The library supports it, but the cookies expire, the account may be flagged, and secrets of a personal account would live on a server. It conflicts with the goal of a service that runs by itself.
- Use a residential proxy. It is paid, which conflicts with RNF-03.
- Try another Render region. It was not tried: the cause is the type of IP address, not its location.
- Host it from a home computer through a tunnel. It would stop being a platform deployment, and it depends on a machine that must stay on.
- Do not deploy. The deployment is still valuable as a working pipeline, and the demo shows the interface and the error handling.

## Consequences

- The public demo is not a dependable way to use the application, and the README says so at the top.
- The free tier also puts the service to sleep after a period without traffic, so the first request can take around a minute.
- The deployment pipeline (Dockerfile, environment variables, forwarded headers, health check, automatic deploys) is complete and reusable if a different provider or IP range is tried later.
- The risk recorded in the SRS has materialized. Whether YouTube changes its behavior is outside the project's control, so this decision should be revisited if the demo's behavior changes.
