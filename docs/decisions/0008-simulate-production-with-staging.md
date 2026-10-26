# 8. Simulate production locally with a Staging environment

- Status: Accepted. Supersedes [ADR 0006](0006-static-web-assets-in-production.md)
- Date: 2026-10-19

## Context

[ADR 0006](0006-static-web-assets-in-production.md) enabled static web assets when the environment is `Production`, so the app could be run unpublished (`dotnet run` or F5) in a production-like mode without losing its CSS and JS. It left one question open: because the condition was `Production`, the call would also run in a real deployment, and it was not verified that this was harmless. The Microsoft documentation recommends enabling this feature only in the environment that needs it.

## Decision

- Condition the call on the `Staging` environment instead: `if (builder.Environment.IsStaging()) builder.WebHost.UseStaticWebAssets();`
- Add a local launch profile that sets `ASPNETCORE_ENVIRONMENT=Staging`, to be used when we want to test production-like behavior without publishing.
- In a real deployment the environment is `Production`, so this call never runs there.

`Staging` behaves like `Production` for everything the application does differently outside Development, such as the exception handler and HSTS, because those blocks check `!IsDevelopment()`.

## Consequences

- Production-like behavior can still be tested locally, with styles.
- A real deployment runs exactly the published code path, with nothing extra enabled.
- The assumption is that a published application serves its static assets correctly without the workaround. It is confirmed by running the published output locally in `Production`, and again in the first deployment.
- `Staging` is a convention of this project, not a deployment target. Nothing is deployed to a Staging environment.
