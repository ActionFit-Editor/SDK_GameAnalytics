# AI Guide - ActionFit GameAnalytics SDK Bridge

This file is shipped inside the UPM package so an AI assistant in a consuming Unity project can understand the package without access to the source project's `Docs/AI` folder.

## Package Identity

- Package ID: `com.actionfit.sdk.gameanalytics`
- Display name: ActionFit GameAnalytics SDK Bridge
- Repository: `https://github.com/ActionFit-Editor/SDK_GameAnalytics.git`
- Repository visibility: Public
- Current package version at generation time: `1.0.2`
- Unity version: `6000.2`
- Custom Package Manager dependency: `1.1.96`

## Purpose

This source-only bridge declares a versioned `ActionFitSdkInstallProfile` for installing the official `com.gameanalytics.sdk@7.10.3` package from OpenUPM. It contains no GameAnalytics SDK binaries, archives, credentials, game keys, secret keys, or vendor configuration files.

## Project Router Registration

This package should be listed in `Packages/com.actionfit.custompackagemanager/PACKAGE_AI_GUIDE_ROUTER.md`.

Requested router entry:

- `Packages/com.actionfit.sdk.gameanalytics/AI_GUIDE.md` - ActionFit GameAnalytics SDK Bridge owns the source-only public profile for inspecting and explicitly applying `com.gameanalytics.sdk@7.10.3` from OpenUPM. Read before changing the profile, source evidence, install behavior, metadata, or release flow.

If the router file is not already included in the AI assistant's default reading sequence, connect it through an existing primary project entry point such as `PROJECT.md`, `AGENTS.md`, `CLAUDE.md`, or `GEMINI.md`. Do not silently create a new project documentation hierarchy.

Read this guide when:

- changing files under `Packages/com.actionfit.sdk.gameanalytics/`;
- diagnosing the GameAnalytics SDK install profile in a consuming project;
- changing the pinned SDK version, OpenUPM registry scopes, official source evidence, detection rules, or package metadata;
- preparing a release for `com.actionfit.sdk.gameanalytics`.

## Source And License Evidence

- Official vendor repository: `https://github.com/GameAnalytics/GA-SDK-UNITY`
- Immutable vendor release: `https://github.com/GameAnalytics/GA-SDK-UNITY/releases/tag/7.10.3`
- Package identity at that tag: `com.gameanalytics.sdk@7.10.3`
- Registry: `https://package.openupm.com`
- License: MIT at `https://github.com/GameAnalytics/GA-SDK-UNITY/blob/7.10.3/LICENSE.md`
- Vendor support documentation: `https://docs.gameanalytics.com/event-tracking-and-integrations/sdks-and-collection-api/game-engine-sdks/unity/`

Re-check all official sources before changing the pinned SDK version. A newer OpenUPM version is not authorization to upgrade consuming projects.

## Install Profile Contract

- `Editor/SDKInstallProfile.json` is schema version 1 and is the package's installation contract.
- The required `core` module declares one registry dependency: `com.gameanalytics.sdk@7.10.3`.
- The OpenUPM registry uses `https://package.openupm.com` with `com.gameanalytics` and `com.google.external-dependency-manager` scopes.
- A matching dependency and registry are adoptable. The default plan should preserve compatible user-managed entries instead of taking destructive ownership.
- A different dependency value, a conflicting scope assignment, or `Assets/GameAnalytics` legacy content must block execution instead of being overwritten.
- SDK installation, repair, update, removal, and recovery must use the installed Custom Package Manager's reviewed-plan APIs. Do not write `Packages/manifest.json` or `ProjectSettings/ActionFitSdkProfiles.json` directly.

## Project-Owned State Boundary

The bridge does not own or migrate GameAnalytics settings, game keys, secret keys, scenes, prefabs, Addressables entries, Proguard files, Gradle templates, or project analytics adapters. Preserve those files and values. Inspect them structurally without exposing credentials, and request separate authorization before any migration or cleanup.

## Editing Rules

- Keep the package source-only and Public. Do not add vendor DLLs, native binaries, frameworks, archives, generated SDK folders, credentials, `google-services.json`, `GoogleService-Info.plist`, or vendor configuration assets.
- Keep package ID, repository name, profile ID, bridge package ID, asmdef names, and `.meta` GUIDs stable.
- Keep `package.json` dependent on the exact installed `com.actionfit.custompackagemanager` release used by the profile contract.
- Update `README.md`, this guide, `THIRD_PARTY_NOTICES.md`, profile contract tests, PackageInfo metadata, and release notes when profile behavior or pinned source metadata changes.
- Run tests only against isolated temporary project contexts. Never apply this profile to the consuming project's real manifest during validation.

## Package Tools Menu

- Unity menu root: `Tools/Package/GameAnalytics SDK Bridge/`.
- `README` opens the installed package README.
- SDK profile execution remains under `Tools/Package/Custom Package Manager/SDK Profiles`.
- Keep this package in the README-only menu priority band.

## Validation

- Run the package-owned Custom Package Manager contract validator for `com.actionfit.sdk.gameanalytics`.
- Run `com.actionfit.sdk.gameanalytics.Editor.Tests` in an isolated Unity project.
- Verify the final package has no vendor binary, archive, credential, symlink, or oversized file.
- Verify exact official tag, package identity, license URL, support URL, registry URL, and pinned SDK version before release.

## Release Note Rules

- `ActionFitPackageInfo_SO.ReleaseNote` must contain only the release currently being prepared.
- Write release notes in Korean with 3-6 user-visible bullets.
- Do not accumulate prior release notes or place a version heading inside the field.

## Publish Notes

- Publishing is manual through Custom Package Manager.
- Repository creation, Git push, tag creation, catalog append, and consuming-project manifest migration require explicit approval separate from package authoring.
- Before reusing a version, check the remote Git repository and exact tag. Published tags are immutable.
- If this package changes after publication, bump to the next unused patch version.
