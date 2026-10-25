# 9. Forwarded headers are enabled by the host, not by the code

- Status: Accepted
- Date: 2026-10-19

## Context

Hosting providers put a reverse proxy in front of the application. The proxy terminates TLS and forwards each request to the app as plain HTTP, adding the headers `X-Forwarded-Proto` (the original scheme) and `X-Forwarded-For` (the original client IP). By default, ASP.NET Core ignores them.

The effect was reproduced with a minimal app listening only on HTTP, in Production, running the same `UseHsts()` and `UseHttpsRedirection()` calls as TubeTrans:

| Situation | Result |
|---|---|
| No forwarded headers | The app treats every request as HTTP, even when the proxy says it was HTTPS. The warning `Failed to determine the https port for redirect` is logged, the HSTS header is never sent, and the client IP is the proxy's |
| The app knows an HTTPS port, no forwarded headers | Every request is redirected to HTTPS, including the ones that already came through HTTPS. Behind a proxy, this is an infinite redirect loop |
| Forwarded headers enabled | The app sees the original scheme and client IP. No warning for proxied requests, no redirect, and the HSTS header is sent |

The app does not reject HTTP requests in any of these cases. The issue is that it misreads where the request came from.

## Decision

Enable the forwarded headers middleware through the environment variable `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true`, set in the host's configuration (the Render dashboard). No code is added to `Program.cs`.

Alternatives considered:

- Call `UseForwardedHeaders` in code, unconditionally. It is explicit and versioned, but it would trust the `X-Forwarded-*` headers on every host, including one where the app is directly reachable and a client could forge them.
- Set the variable in the Dockerfile. The image would then trust proxies wherever it runs.

## Consequences

- The application code stays the same on every host, in line with the portability requirement (RNF-06). Only a host that sits behind a proxy needs the variable.
- In this mode the framework trusts any proxy, not just loopback ones. That is acceptable on Render because the service is not directly reachable from the public internet. On a host that exposes the app directly, the variable must not be set.
- The deployment will not work as intended if the variable is forgotten. It is listed in the deployment steps, and the symptoms are the ones in the table above.
- On Render specifically, the proxy already redirects HTTP to HTTPS at its edge, so the application-level redirect is redundant there. It is kept for local runs and for other hosts.
