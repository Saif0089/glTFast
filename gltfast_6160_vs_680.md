# glTFast 6.16.1 vs 6.8.0 — What Changed

18 releases over ~18 months (2024-09 to 2026-02).

---

## Mesh

### New in 6.16.1

- **16-bit mesh indices** preserved (no longer upconverted to 32-bit). 8-bit indices convert to 16-bit instead of 32-bit. Reduces memory for meshes with <65k vertices per sub-mesh.
- **TRIANGLE_STRIP and TRIANGLE_FAN** primitive modes supported (6.8.0 added initial support; 6.16.1 fixed triangle fan with non-first center vertex).
- **Sub-mesh merging**: mesh primitives with identical vertex buffer layout now produce a single Unity Mesh with multiple sub-meshes instead of separate meshes. Applies to both standard and Draco-compressed meshes.
- `IGltfReadable.GetSourceMesh` returns the de-serialized glTF source mesh.
- `GltfImportBase.Meshes`, `GetMeshCount`, `GetMesh` for fine-grained mesh access.
- `IBufferView.ByteStride` exposed.

### Fixed

- Multi-primitive skinned meshes import correctly as sub-meshes.
- Morph targets on multi-primitive meshes (identical vertex buffer references).
- Draco-compressed morph targets: proper completion and resource disposal.
- Per-submesh bounding boxes preserved.
- Meshes with >8 UV sets no longer crash (capped gracefully).
- Invalid mesh bounds on single-submesh meshes.
- LINE_LOOP topology.

### Performance

- Mesh indices imported as unsigned (no signed conversion).
- Faster index conversion for line loop, triangle strip, and triangle fan modes.

---

## Materials & Shaders

### New

- **Overridable shader loading** on `MaterialGenerator` subclasses — enables Addressables integration:
  - `MaterialGenerator.FindShader`
  - `BuiltInMaterialGenerator.FindShaderMetallicRoughness/SpecularGlossiness/Unlit`
  - `ShaderGraphMaterialGenerator.LoadShaderByName`
- **HDRP material validation**.
- **Textures Readable** setting and inspector checkbox — textures accessible from scripts after import.

### Fixed

- Normal map unpacking on Android (default normal map).
- Normal map scale on shader graphs.
- Emission color now in correct color space (shader graphs + built-in).
- Alpha blending/clipping with invalid color space conversion on base color map alpha and vertex color alpha (shader graphs).
- Built-in shaders now factor in vertex color alpha (metallic-roughness and specular-glossiness).
- Specular-Glossiness shader graph no longer ignores the Glossiness parameter.
- Specular-Glossiness materials with alpha mode MASK no longer incorrectly blended in URP/HDRP.
- Transmissive materials without transmissive texture: no NullReferenceException.
- Clearcoat without texture: no NullReferenceException.
- XYZ-style normals used in shaders even if DXT5nm-style is enabled.

### Export-Specific Material Changes

- `StandardMaterialExport` replaced by `LitMaterialExport` (URP) and `BuiltInStandardMaterialExport` (Built-in).
- Smoothness value baked into ORM roughness channel when a smoothness texture exists.
- Texture scaling preserved on URP/Lit export.
- No empty ORM textures created when no smoothness texture is assigned.
- Missing texture transform on vertically-only-scaled textures fixed.

---

## Animation

### Added then Removed

- Playables option was added in 6.12.0, then **removed** in 6.13.1 (not usable in builds).

### Fixed

- Morph target weights applied from the node (not primitive) level.
- `GltfAsset` properly cleans up Animation component on repeated loads.
- Support for accessors without buffer view in animation clips.
- Increased resilience against invalid animation data.

---

## Export

### New

- **Skinned mesh export**.
- **Non-readable mesh export** (previously blocked).
- **Vertex-compressed mesh export** (16-bit float positions/normals/tangents/UVs converted to 32-bit).
- **JpgQuality** option in ExportSettings.
- Buffer view targets set properly.
- Error message for unsupported meshopt compression export.

### Fixed

- Writing to files on web via IndexedDB.
- HDRP spotlight inner cone angle export.
- Sub-meshes with non-zero base vertex exported with correct indices.
- Missing inverseBindMatrices/bindPoses on Draco-compressed skinned meshes.
- Editor no longer hangs after exporting non-readable meshes.
- Unlit materials: no expendable JSON when no color/texture applied.

---

## Performance & Memory

### Loading Performance

- **Background thread loading**: large glTF-Binary content shifted to background thread when it won't fit in the current update loop.
- **Chunked loading**: large glTF-Binary loaded in smaller chunks, keeping frame rate smooth.
- **Limited copy buffer**: GC allocations no longer scale with glTF-Binary size.
- **Zero-init skipped**: glTF-Binary buffers not initialized with zeros before population.
- **NativeArray buffers**: base64 data URIs decoded into NativeArray instead of managed byte[].
- **ReadOnlySpan for data URIs**: avoids copying entire URI string.
- **No managed memory copy**: texture data loaded directly via Texture2D.LoadImage (Unity 6+).
- **No implicit managed copy**: glTF/KTX data stays in native memory by default (Unity 2021+).

### Internal

- Internal buffer representation changed to `ReadOnlyBuffer<byte>`.
- `INativeDownload` interface for zero-copy download access.
- NativeArray used directly instead of AccessorData/managed arrays.
- Safer NativeCollections in index jobs (replacing raw unsafe pointers).

---

## Textures & Images

### New

- **KTX textures from data URIs** (6.15.0).
- **Content-based glTF/GLB detection** — format detected from content, not just extension (Unity 2021+).
- Loading succeeds even if an individual image fails (returns true with warning).

### Fixed

- KTX textures loaded as readable when required by platform or settings.
- Data URI with incorrect media-type or undersized content length handled gracefully.
- No pointless copying of GLB-embedded textures when ImageConversion is disabled.
- Unknown texture extensions (e.g. WebP/EXT_texture_webp) no longer block loading.
- Android StreamingAssets with Unicode characters in relative URIs.
- Sampler settings conflicts produce proper warning.

### Dependency Changes

- KTX support moved from `com.atteneder.ktx` to `com.unity.cloud.ktx` (min 3.6.0).
- Draco minimum raised to 5.4.0.
- meshoptimizer minimum raised to 0.2.0-exp.1.

---

## API Changes

### New APIs

- `GltfImport.Load(NativeArray<byte>.ReadOnly)` overload.
- `GltfImportBase.Logger` and `DeferAgent` properties.
- `GltfAsset.PlayAutomatically` and `SceneId` setters.
- `OperationCanceledException` thrown on cancellation (instead of silent failure).

### Deprecated

- `GltfImport.LoadGltfBinary` → use generic `GltfImport.Load`.
- `IGltfReadable.GetAccessor` → `GetAccessorData` (also deprecated, replacement coming).
- `IGltfReadable.ImageCount` / `GetImage` — unreliable, use textures instead.
- `GLTFast.ManagedNativeArray` — will be removed from public API.
- `StandardMaterialExport` → use `LitMaterialExport` or `BuiltInStandardMaterialExport`.

### Removed

- Playables animation option.
- Hybrid Renderer (com.unity.rendering.hybrid) support.

### Dependency Changes

- `com.unity.collections` lowered to 1.2.4 (from 1.5.1) — avoids conflicts.
- `com.unity.mathematics` lowered to 1.2.6 (from 1.3.1).
- `com.unity.burst` bumped to 1.8.24.

---

## Editor & CI

- EditorConfig added for consistent code style.
- Graphics tests added.
- Renovate CI for auto-updating dependencies.
- Runtime import performance tests.
- Procedurally generated test glTFs.
- `OpenGltfScene` test utility with runtime support, camera framing, entity loading.
- Menu items for switching test setups and render pipelines.
- `GLTFAST_IGNORE_MESHOPT_OUTDATED_ERROR` scripting define to suppress outdated meshopt errors.
