# 6. Enable static web assets to simulate Production locally

- Status: Superseded by [ADR 0008](0008-simulate-production-with-staging.md)
- Date: 2026-10-16

## Context

The application serves `wwwroot` with `MapStaticAssets`. The framework enables static web assets automatically only in the Development environment. When the app runs unpublished (`dotnet run` or F5) with `ASPNETCORE_ENVIRONMENT=Production`, the CSS and JS files return 404 and the pages render without styles.

We wanted to test the Production behavior (error page, HSTS) from the development machine.

## Decision

Call `builder.WebHost.UseStaticWebAssets()` when the environment is Production, with a comment in `Program.cs` explaining why.

Alternatives considered:

- Go back to `UseStaticFiles()`. It works in every environment, but gives up the compression and fingerprinting that `MapStaticAssets` provides.
- Publish and run the published output. It is the most faithful test, but slows down the development loop.
- Simulate with a `Staging` environment and call `UseStaticWebAssets()` only there. The Microsoft documentation recommends enabling the feature only in the environment that needs it.

## Consequences

- Production can be simulated locally, with styles.
- As written, the call also runs in a real Production deployment. A published deployment should not need it, and it was not verified that it is harmless there.
- Open item, to settle before deploying: either switch to the `Staging` approach, or confirm in the first deployment that the call causes no problem.
