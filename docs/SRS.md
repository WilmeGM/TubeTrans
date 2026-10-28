# Software Requirements Specification (SRS)

## Project: TubeTrans

**Version:** 2.2
**Date:** October 2026
**Project type:** Web application (ASP.NET Core MVC)
**Author:** Wilme González Martínez

---

## 1. Introduction

### 1.1 Purpose

This document specifies the functional and non-functional requirements of **TubeTrans**, a web application that lets a user enter the URL of a public YouTube video and get its transcript as plain text, in the original language of the video, with no sign-up and no persistent storage of data.

### 1.2 Scope

The application:

- Lets the user enter a YouTube video URL.
- Retrieves the captions available for the video, in its original language (no translation).
- Shows the result as a **single block of continuous plain text** (no timestamps and no visual division by caption line), with a button to copy it to the clipboard.
- Requires no user account or login, and keeps no data between sessions or requests (no database and no persistence of any kind).
- Can be deployed to an ASP.NET Core compatible hosting provider, on a free tier, and is distributed as a Docker image so that anyone can run it on their own machine.

Explicitly out of scope:

- Translating the transcript into another language.
- Automatic speech recognition for videos that have no captions.
- Downloading video or audio files.
- YouTube Shorts links. They are recognized and rejected with a specific message.
- Query history, favorites, or any persistence between requests.
- Multi-user support, authentication or roles.

### 1.3 References

- [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) (NuGet library): extraction of video metadata and captions from YouTube without an API key.
- [Architecture](architecture.md): how the solution is organized and how a request flows through it.
- [Decision records](decisions/README.md): the reasons behind the main design choices.

---

## 2. Overall Description

### 2.1 Product perspective

A standalone web application with a single main feature and no integration with previous systems of its own. For every request, it consumes a third-party external service in real time to obtain the captions of the requested video.

### 2.2 Product functions (summary)

1. Receive a YouTube URL from a form.
2. Validate that the URL is a valid YouTube video link.
3. Extract the video identifier and obtain its available transcript in the original language.
4. Consolidate the transcript into a single block of plain text and show it, together with a button to copy it to the clipboard.
5. Handle errors clearly (video without captions, invalid URL, external service unavailable, etc.).

### 2.3 User characteristics

A single, anonymous kind of user, with no technical knowledge required: anyone who wants to quickly consult the spoken content of a YouTube video as text.

### 2.4 Constraints

- No paid API key and no registration process may be required, neither from the end user nor for the operation of the system.
- The system must operate within the free limits of the external service used to obtain captions.
- The system must be deployable on a free hosting plan.
- Availability from cloud hosting providers is best-effort: YouTube may apply bot checks to requests that come from data-center IP addresses (see section 7).

### 2.5 Assumptions and dependencies

- The YouTube video is assumed to be **public** and to have captions (manual or auto-generated) available in at least one language.
- The system depends on the availability and stability of an external service that the development team does not control. Captions are extracted through unofficial mechanisms, because YouTube offers no general-purpose public captions API that works without the channel owner's authorization. Changes to that mechanism may require future adjustments (see section 7, Risks).

---

## 3. Specific Requirements

### 3.1 Functional requirements

| ID | Requirement | Priority |
|---|---|---|
| RF-01 | The system must present a form with a text field to enter the video URL. | High |
| RF-02 | The system must validate that the entered URL is a valid YouTube video link before processing it. Links to YouTube Shorts must be rejected with a specific message. | High |
| RF-03 | The system must extract the video identifier from the entered URL. | High |
| RF-04 | The system must obtain the list of caption tracks available for the video. | High |
| RF-05 | When more than one caption track is available, the system must automatically select the one that most likely matches the original language of the video. The selection rules are described in [ADR 0004](decisions/0004-heuristic-caption-track-selection.md). | High |
| RF-06 | The system must download the full transcript in the original language of the video and consolidate the text of all its lines into a single continuous block, without timestamps. | High |
| RF-07 | The system must show the resulting block of text inside a read-only text container. | High |
| RF-07.1 | The system must offer a button that copies the full content of the text block to the user's clipboard. | High |
| RF-07.2 | The system must visually indicate to the user when the copy to the clipboard succeeded (for example, a change in the button's text or icon). | Low |
| RF-08 | The system must clearly inform the user if the video has no captions available. | Medium |
| RF-09 | The system must inform the user if the entered URL is not valid, or if the video does not exist, is not public or cannot be accessed, including when YouTube refuses to serve it to the server. | Medium |
| RF-10 | The system must inform the user if the external caption service is temporarily unavailable, without exposing internal technical details. | Medium |
| RF-11 | The system must not store any transcript or keep any history of user activity. The submitted URL may appear in diagnostic logs, which never contain transcript content, and the privacy policy must say so. | High |

### 3.2 Non-functional requirements

| ID | Requirement |
|---|---|
| RNF-01 | **No persistence**: the application must not use a database or write user data to disk. Diagnostic logs are covered by RF-11. |
| RNF-02 | **No authentication**: there must be no login, registration or user session mechanism. |
| RNF-03 | **Free of charge**: every external dependency (libraries, services, hosting) must operate within free tiers. |
| RNF-04 | **Response time**: for a video of typical length (up to ~30 minutes), fetching and displaying the transcript should not take more than ~5 seconds under normal conditions. |
| RNF-05 | **Robust error handling**: no unhandled exception may break the application or expose internal details (such as stack traces) to the end user in production. |
| RNF-06 | **Portability**: the application must be deployable on any ASP.NET Core compatible provider, including as a container image, without code changes. |
| RNF-07 | **Usability**: a simple interface built around a single form, with no need for additional instructions for the user. |
| RNF-08 | **Client-side copying**: copying the text to the clipboard must be done in the browser (with no additional request to the server), using the browser's standard clipboard API, which requires a secure context (HTTPS or localhost). |
| RNF-09 | **Container distribution**: the application must be published as a public container image, for the `linux/amd64` and `linux/arm64` architectures, that starts with a single command and needs no configuration. |

### 3.3 External interface requirements

#### 3.3.1 User interface (UI)

- **Main page**: a form with the URL field and a "Get transcript" button. After a lookup, the form stays at the top of the page and the result appears below it.
- **Result**: a read-only block of plain text with the transcript, with a "Copy" button, or the corresponding error message.
- **Privacy policy page**: explains what happens with the user's information.
- **Error page**: a generic page, shown in production when an unexpected error happens outside the transcript lookup.

#### 3.3.2 External interfaces (third-party services)

| Service | Purpose | Authentication |
|---|---|---|
| YoutubeExplode (NuGet package) | Obtain the video metadata and download the caption track in its original language | None (it does not depend on the official YouTube API) |

---

## 4. Use Cases

### UC-01: Consult the transcript of a video

**Actor:** Anonymous user
**Precondition:** The user has the URL of a public YouTube video that has captions available.

**Main flow:**

1. The user opens the main page.
2. The user enters the video URL.
3. The user submits the form.
4. The system extracts the video identifier and requests its transcript.
5. The system shows the block of text with the consolidated transcript, in the original language of the video, next to the "Copy" button.

**Alternative flows:**

- 3a. The URL is not a valid YouTube video link, or it points to a Short → the system shows a validation message under the field and does not process the request (RF-02).
- 4a. The video has no captions available → the system shows an informative message (RF-08).
- 4b. The video does not exist, is private, is no longer available, or YouTube refuses to serve it → the system shows an error message (RF-09).
- 4c. The external service fails or does not respond → the system shows a generic error message (RF-10).

---

## 5. Proposed Technology Stack

- **Framework:** ASP.NET Core MVC (.NET 10)
- **Language:** C#
- **Libraries:**
  - `YoutubeExplode`: extraction of metadata and captions
- **Front end:** Razor views and Bootstrap 5
- **Testing:** xUnit
- **Packaging:** Docker, with a multi-architecture image published on Docker Hub
- **Demo hosting:** Render, free tier, running the container

---

## 6. Deployment Plan (summary)

1. Build the container image from the `Dockerfile` in the repository.
2. **Demo on Render:** create a Docker web service linked to the repository, tracking `master`, and set `ASPNETCORE_ENVIRONMENT=Production`, `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` and `PORT=8080`, with `/` as the health check path. Every push to `master` redeploys it.
3. **Docker Hub:** for every release, build the image for `linux/amd64` and `linux/arm64` and push it with the version tag and `latest`.
4. Validate the behavior with real videos after each release, confirming acceptable response times and correct error handling.

The deployment decisions are recorded in [ADR 0010](decisions/0010-render-demo-with-known-limitation.md) and [ADR 0011](decisions/0011-distribute-image-on-docker-hub.md). Operational notes are in [Architecture](architecture.md#deployment).

---

## 7. Risks

| Risk                                                                                                                            | Impact                                                                       | Mitigation                                                                                                                                                                                                                                                                                                           |
| ------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| YouTube changes or restricts the internal mechanism the extraction library relies on (there is no official general-purpose API) | High: the main feature could stop working without notice                     | Robust error handling (RF-10). The dependency is isolated in a single service, which makes it easy to replace without affecting the rest of the system ([ADR 0001](decisions/0001-use-youtubeexplode.md))                                                                                                            |
| YouTube applies bot checks and rate limits to requests coming from data-center IP addresses, such as those of a cloud host      | High: **materialized** on the Render demo, which works only with some videos | The error message explains it. The application is distributed as a Docker image to run from the user's own connection ([ADR 0010](decisions/0010-render-demo-with-known-limitation.md), [ADR 0011](decisions/0011-distribute-image-on-docker-hub.md)). A short-lived in-memory cache is a possible future mitigation |
| The video has no captions in any language                                                                                       | Low                                                                          | Clear informative message to the user (RF-08)                                                                                                                                                                                                                                                                        |
| On videos with several caption tracks, the automatically selected one is not the original language                              | High                                                                         | The selection rules are documented and tested ([ADR 0004](decisions/0004-heuristic-caption-track-selection.md)). Manual selection is a possible future improvement                                                                                                                                                   |
| Very long videos produce long blocks of text, affecting the load time of the page or the reading experience                     | Low                                                                          | The text is shown in a fixed-height block with internal scrolling                                                                                                                                                                                                                                                    |

---

## 8. Future Improvements (outside the current scope)

- Manual selection of the caption language when the video offers more than one track.
- Export of the transcript to a text file or `.srt` format.
