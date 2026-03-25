# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**glTFast** (`com.atteneder.gltfast`) is a Unity package for efficient runtime and editor import/export of glTF 3D files. Version 6.x, requires Unity 2022.3+. Licensed under Apache-2.0.

This is the OpenUPM fork. The Unity Technologies fork lives at `com.unity.cloud.gltfast`. Development tools, tests, and test projects are only available in the Unity fork — this repo is for distribution.

## Build & Test

This is a **Unity Package Manager (UPM) package**, not a standalone project. It must be consumed inside a Unity project.

- **No standalone build command** — open the containing Unity project and Unity compiles the package.
- **Tests**: Run via Unity Test Runner (*Window > General > Test Runner*). Test assets live in a separate package (`Packages/com.unity.cloud.gltfast.tests`), which is only in the Unity fork.
- **Code formatting**: Uses `dotnet format whitespace . --folder` (CI runs via `.github/workflows/dotnet-format.yml`).
- **Performance tests**: Require the Performance Testing Package and pre-generated glTFs (*Tools > glTFast > Create performance test glTFs*).

## Architecture

### Assembly Structure (4 assemblies)

| Assembly | asmdef | Namespace | Purpose |
|----------|--------|-----------|---------|
| **glTFast** | `Runtime/Scripts/glTFast.asmdef` | `GLTFast` | Core import/export runtime. Allows unsafe code. |
| **glTFast.Export** | `Runtime/Scripts/Export/glTFast.Export.asmdef` | `GLTFast.Export` | glTF export. Not auto-referenced. |
| **glTFast.Editor** | `Editor/Scripts/glTFast.Editor.asmdef` | `GLTFast.Editor` | Editor tools, ScriptedImporter, UI. |
| **glTFast.dots** | `Runtime/Scripts/DOTS/glTFast.dots.asmdef` | `GLTFast` | ECS/Entities integration. |

### Key Classes

- **`GltfImport` / `GltfImportBase<TRoot>`** (`Runtime/Scripts/GltfImport.cs`) — Core import engine. Handles downloading, parsing, and converting glTF data.
- **`GltfAsset`** (`Runtime/Scripts/GltfAsset.cs`) — MonoBehaviour for scene-based loading (drag-and-drop).
- **`IInstantiator`** — Interface for creating Unity objects from parsed glTF. Implementations: `GameObjectInstantiator`, `GameObjectBoundsInstantiator`, `EntityInstantiator`.
- **`IMaterialGenerator`** — Strategy for material creation per render pipeline. Implementations: `BuiltInMaterialGenerator`, `UniversalRPMaterialGenerator`, `HighDefinitionRPMaterialGenerator`.
- **`IDownloadProvider`** — Abstraction over file/HTTP downloading. Implementations: `DefaultDownloadProvider`, `CustomHeaderDownloadProvider`.
- **`ImportAddon` / `ImportAddonRegistry`** — Extension system for custom import behavior.
- **`GameObjectExport`** (`Runtime/Scripts/Export/`) — Runtime export of GameObjects to glTF.

### Key Namespaces

- `GLTFast.Schema` (61 files) — C# classes mirroring the glTF JSON schema.
- `GLTFast.Jobs` — Burst-compiled Unity Jobs for mesh/texture processing.
- `GLTFast.Materials` — Material generation per render pipeline (URP/HDRP/Built-in).
- `GLTFast.Loading` — Download providers and URI handling.
- `GLTFast.Newtonsoft` (39 files) — Optional JSON.NET parser as alternative to Unity's JsonUtility.

### Conditional Compilation

The codebase uses preprocessor defines extensively for optional dependencies:

- `DRACO_IS_RECENT` / `DRACO_IS_ENABLED` — Draco mesh compression
- `KTX_IS_RECENT` / `KTX_IS_ENABLED` — KTX/Basis Universal textures
- `MESHOPT_IS_RECENT` / `MESHOPT_IS_ENABLED` — meshoptimizer compression
- `GLTFAST_THREADS` — Managed threads (disabled on WebGL)
- `USING_URP` / `USING_HDRP` — Render pipeline detection

### Shaders

`Runtime/Shader/` contains shader graphs and includes organized by pipeline:
- `Built-In/`, `URP/`, `HDRP/` — Pipeline-specific shaders
- `SubGraphs/` — Reusable shader sub-graphs
- `Includes/` — Common `.cginc` includes

## Code Style

- License header on every file: `// SPDX-FileCopyrightText: 2023 Unity Technologies and the glTFast authors` + `// SPDX-License-Identifier: Apache-2.0`
- Whitespace formatting enforced by `dotnet format`
- All `.meta` files must be preserved (Unity asset tracking)
