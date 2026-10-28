# 11. Distribute the application as an image on Docker Hub

- Status: Accepted
- Date: 2026-10-27

## Context

The limitation described in [ADR 0010](0010-render-demo-with-known-limitation.md) depends on where the requests come from. YouTube treats a data-center IP address, like the one of any cloud host, much worse than a residential connection. A free hosting tier cannot change that.

The application already builds into a container image, which includes the runtime it needs. Anyone with Docker can run it in one command, and the requests then leave from their own connection.

## Decision

- Publish a public image, `wilmegm/tubetrans`, on Docker Hub.
- Tag every release with its version (`1.1.0`) and also move the `latest` tag to it.
- Build for `linux/amd64` and `linux/arm64`, so it runs on Windows, Linux, Intel Macs and Apple Silicon Macs.
- Lead the README with a one-line `docker run` command.

To build for two architectures without emulation, the build stage of the Dockerfile uses `--platform=$BUILDPLATFORM`, so the SDK runs natively on the build machine. The published output is portable: framework-dependent and managed code only, published without a runtime identifier and without the platform-specific executable (`UseAppHost=false`). The final stage uses the official runtime image, which Docker resolves for each target architecture.

Alternatives considered:

- GitHub Container Registry. It would live next to the code, but every command needs a `ghcr.io/` prefix. Docker Hub is the default registry, so `docker run wilmegm/tubetrans` works as is.
- Ask people to build the image from the repository. It needs the source and more time, and it defeats the purpose of "one command".

## Consequences

- A person with Docker can use TubeTrans reliably with one command, and no data passes through a third-party server.
- Users need Docker installed.
- Docker Hub allows unlimited public repositories on the free plan, and limits anonymous pulls per IP address. That is enough for normal use.
- Each release needs a manual build and push. Automating it with a workflow on the release tag is a possible next step.
- The image is built by cross-publishing, but it has not been executed on an arm64 machine by the project. The manifest on Docker Hub lists both architectures.
- Even a home connection can be rate-limited by YouTube if used heavily, so the README asks users to be considerate.
