# glTFast 6.8.0 (Stock) vs TraceXR/glTFast Fork

**Base version**: glTFast 6.8.0 (`com.unity.cloud.gltfast`)
**Fork**: [TraceXR/glTFast](https://github.com/TraceXR/glTFast) — branch `trace-main`
**Contributors**: Martin (martin@trace3d.app), Ehtisham (Saif0089)

The TraceXR fork adds **5 custom commits** (+ 3 merge commits) on top of stock 6.8.0, organized into 3 PRs plus 2 follow-up fixes. All changes are additive — no upstream code was removed or rewritten.

---

## Summary of Custom Changes

| PR / Commit | Author | Date | Purpose |
|-------------|--------|------|---------|
| PR #1: `TRACE_materials_occlusion` (2 commits) | Martin | 2024-10-05/06 | Custom occlusion material extension |
| PR #3: `override-root-node-name` | Martin | 2025-10-31 | Root node renaming for animation retargeting |
| PR #4: `fix/occlusion_polyspatial` | Ehtisham | 2025-12-30 | visionOS/PolySpatial occlusion fix |
| PR #5: `fix/alphaclip_fix_polyspatial` | Ehtisham | 2025-12-30 | Alpha cutoff shader graph fix |

---

## 1. TRACE_materials_occlusion Extension (Martin — PR #1)

A custom glTF extension (`TRACE_materials_occlusion`) for occlusion-only materials. These are invisible materials that write to the depth buffer but not the color buffer — used in AR/XR to make virtual objects appear hidden behind real-world surfaces.

### What it does

**Import**: When a glTF material has the `TRACE_materials_occlusion` extension, glTFast loads it using a dedicated occlusion shader instead of the standard PBR pipeline.

**Export**: When a Unity material uses an occlusion shader (`Shader Graphs/glTF-occlusion`, `Universal Render Pipeline/VR/SpatialMapping/Occlusion`, or `AR/Basic Occlusion`), it exports as a glTF material with the `TRACE_materials_occlusion` extension. The exported material has black base color and double-sided rendering.

### Files changed

**Schema (new extension definition)**:
- `Runtime/Scripts/Schema/MaterialOcclusion.cs` — **New file**. Schema class for the extension (serializable, with JSON writer).
- `Runtime/Scripts/Schema/MaterialExtensions.cs` — Added `TRACE_materials_occlusion` field and JSON serialization.
- `Runtime/Scripts/Schema/FakeSchema/MaterialExtension.cs` — Added `TRACE_materials_occlusion` field for Newtonsoft fallback parsing.
- `Runtime/Scripts/Schema/Root.cs` — Null-check for the extension in fake schema reconciliation.

**Extension registration**:
- `Runtime/Scripts/Extensions.cs` — Added `MaterialsOcclusion` enum value and `"TRACE_materials_occlusion"` constant string.
- `Runtime/Scripts/GltfImport.cs` — Registered extension in `supportedExtensions` list.

**Material generation (import)**:
- `Runtime/Scripts/Material/ShaderGraphMaterialGenerator.cs` — Added occlusion shader GUID, name constant (`glTF-occlusion`), static shader cache fields, `GetOcclusionMaterial()` method, `GetOcclusionShader()` method, and detection logic in `GenerateMaterial()` that routes occlusion materials to the dedicated shader.

**Export**:
- `Runtime/Scripts/Export/MaterialExportBase.cs` — Added `ExportOcclusion()` (sets black base color, double-sided, registers extension) and `IsOcclusion()` (detects occlusion shaders by name).
- `Runtime/Scripts/Export/GltfMaterialExporter.cs` — Early return for occlusion materials before PBR export path.
- `Runtime/Scripts/Export/StandardMaterialExport.cs` — Added occlusion detection branch in the Standard material export flow.

**Shader (new)**:
- `Runtime/Shader/URP/glTF-occlusion.shader` — **New file**. A URP shader that writes depth only (ZWrite On, ColorMask 0, no color output). Named `Shader Graphs/glTF-occlusion` so the `ShaderGraphMaterialGenerator` picks it up. Renders at `Queue = Geometry-1` (before opaque), double-sided.

---

## 2. Override Root Node Name (Martin — PR #3)

An import setting to rename the root node of an imported glTF scene. Primarily useful for **animation retargeting** — when retargeting humanoid animations, the root bone name must match between the source and target.

### What it does

A new `OverrideRootNodeName` property on `ImportSettings`. When set (non-empty string), the first node in the glTF hierarchy (`m_NodeNames[0]`) is renamed to this value during import, after all other node name processing.

### Files changed

- `Runtime/Scripts/ImportSettings.cs` — Added `OverrideRootNodeName` property with getter/setter, serialized field `overrideRootNodeName` with tooltip explaining it alters the first node name (distinct from the parent object name).
- `Runtime/Scripts/GltfImport.cs` — Added 4-line block after node name processing: if `OverrideRootNodeName` is set and nodes exist, overwrite `m_NodeNames[0]`.

---

## 3. Occlusion Fix for PolySpatial / visionOS (Ehtisham — PR #4)

Fixes the occlusion extension to work on Apple Vision Pro via PolySpatial.

### What it does

On visionOS (`UNITY_VISIONOS` define), the custom `glTF-occlusion` shader graph doesn't work with PolySpatial's rendering pipeline. This fix makes `GetOcclusionShader()` use Apple's built-in `AR/Basic Occlusion` shader instead when building for visionOS.

Additionally, the `glTF-pbrMetallicRoughness.shadergraph` was modified (980 insertions, 143 deletions) — likely shader graph node reorganization needed for PolySpatial compatibility.

### Files changed

- `Runtime/Scripts/Material/ShaderGraphMaterialGenerator.cs` — Added `#if UNITY_VISIONOS` preprocessor block in `GetOcclusionShader()` that returns `Shader.Find("AR/Basic Occlusion")` instead of the custom shader graph.
- `Runtime/Shader/glTF-pbrMetallicRoughness.shadergraph` — Shader graph modifications for PolySpatial compatibility.

---

## 4. Alpha Cutoff Shader Graph Fix (Ehtisham — PR #5)

Replaced the Alpha Cutoff property node in the PBR metallic-roughness shader graph.

### What it does

Removed the old Alpha Cutoff property node and replaced it with a new one, updating node references and positions. This fixes property node issues (potentially related to PolySpatial or shader graph version compatibility).

### Files changed

- `Runtime/Shader/glTF-pbrMetallicRoughness.shadergraph` — 128 lines changed (64 insertions, 64 deletions). Node ID and position updates for the Alpha Cutoff property.

---

## Feature Comparison Table

| Feature | Stock 6.8.0 | TraceXR Fork |
|---------|-------------|--------------|
| TRACE_materials_occlusion (import) | ❌ | ✅ |
| TRACE_materials_occlusion (export) | ❌ | ✅ |
| Occlusion URP shader (depth-only) | ❌ | ✅ |
| Override root node name on import | ❌ | ✅ |
| visionOS/PolySpatial occlusion support | ❌ | ✅ |
| Alpha Cutoff shader graph fix | ❌ | ✅ |

---

## Why These Changes Exist

The TraceXR fork targets **XR/AR use cases** (particularly Apple Vision Pro via PolySpatial):

1. **Occlusion materials** are essential in AR — they let real-world geometry "hide" virtual objects, creating the illusion that virtual content is behind physical surfaces. The standard glTF spec has no occlusion material extension, so TraceXR created a custom one (`TRACE_materials_occlusion`) for roundtrip support in their pipeline.

2. **Root node name override** enables animation retargeting workflows where the skeleton root must have a specific name to match across different assets.

3. **PolySpatial fixes** ensure the fork works on Apple Vision Pro, where Unity's standard shader graphs need adjustments to render correctly through Apple's PolySpatial rendering layer.
