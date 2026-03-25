# glTFast vs UnityGLTF — Feature Comparison

| | **glTFast** | **UnityGLTF** |
|---|---|---|
| **Package** | `com.atteneder.gltfast` | `org.khronos.unitygltf` |
| **Version** | 6.16.1 | 2.19.1 |
| **Maintainer** | Andreas Atteneder / Unity Technologies | Khronos Group / Needle |
| **License** | Apache-2.0 | MIT |
| **Min Unity** | 2022.3 | 2021.3 |
| **JSON Parser** | JsonUtility (default), optional Newtonsoft | Newtonsoft JSON (required) |

---

## Workflows

| Workflow | glTFast | UnityGLTF |
|----------|---------|-----------|
| Runtime Import | ✅ | ✅ |
| Runtime Export | ✅ (experimental) | ✅ |
| Editor Import | ✅ (ScriptedImporter) | ✅ (ScriptedImporter) |
| Editor Export | ✅ (experimental) | ✅ |
| DOTS / ECS | ✅ (experimental) | ❌ |
| Timeline Recording | ❌ | ✅ (GltfRecorderTrack) |

---

## Render Pipeline Support

| Pipeline | glTFast | UnityGLTF |
|----------|---------|-----------|
| URP | ✅ Full | ✅ Full |
| HDRP | ✅ Full | ⚠️ Limited (not actively maintained) |
| Built-in | ✅ Full | ✅ Full (2021.3+) |

---

## Performance

| Feature | glTFast | UnityGLTF |
|---------|---------|-----------|
| Burst Compiler | ✅ | ❌ |
| C# Job System | ✅ | ❌ |
| Async / Await | ✅ | ✅ |
| Frame-budget loading | ✅ (TimeBudgetPerFrameDeferAgent) | ✅ (IProgress) |
| Native memory (NativeArray) | ✅ | ❌ |
| Mesh deduplication | ❌ | ✅ |
| Texture deduplication | ❌ | ✅ |
| Pure C# (no native deps) | ❌ (uses Burst/Collections) | ✅ |

---

## glTF Extensions

### Material Extensions

| Extension | glTFast Import | glTFast Export | UnityGLTF Import | UnityGLTF Export |
|-----------|---------------|----------------|------------------|------------------|
| KHR_materials_unlit | ✅ | ✅ | ✅ | ✅ |
| KHR_materials_clearcoat | ✅ | ✅ | ✅ | ✅ |
| KHR_materials_transmission | ☑️ approx | ☑️ approx | ✅ | ✅ |
| KHR_materials_volume | ℹ️ planned | ❌ | ✅ | ✅ |
| KHR_materials_ior | ℹ️ planned | ❌ | ✅ | ✅ |
| KHR_materials_emissive_strength | ❌ | ❌ | ✅ | ✅ |
| KHR_materials_iridescence | ❌ | ❌ | ✅ | ✅ |
| KHR_materials_sheen | ℹ️ planned | ❌ | ✅ | ✅ |
| KHR_materials_specular | ℹ️ planned | ❌ | ☑️ partial | ☑️ partial |
| KHR_materials_anisotropy | ❌ | ❌ | ☑️ partial | ☑️ partial |
| KHR_materials_dispersion | ❌ | ❌ | ✅ | ✅ |
| KHR_materials_variants | ✅ | ❌ | ❌ | ✅ |
| KHR_materials_pbrSpecularGlossiness | ☑️ legacy | ❌ | ☑️ legacy | ❌ |

### Mesh & Compression Extensions

| Extension | glTFast Import | glTFast Export | UnityGLTF Import | UnityGLTF Export |
|-----------|---------------|----------------|------------------|------------------|
| KHR_draco_mesh_compression | ✅ | ✅ | ✅ | ❌ |
| EXT_meshopt_compression | ✅ | ❌ | ✅ | ❌ |
| KHR_mesh_quantization | ✅ | ❌ | ✅ | ❌ |
| KHR_texture_basisu | ✅ | ❌ | ✅ | ❌ |
| EXT_mesh_gpu_instancing | ✅ | ❌ | ✅ | ❌ |

### Animation & Scene Extensions

| Extension | glTFast Import | glTFast Export | UnityGLTF Import | UnityGLTF Export |
|-----------|---------------|----------------|------------------|------------------|
| KHR_lights_punctual | ✅ | ✅ | ✅ | ✅ |
| KHR_texture_transform | ✅ | ✅ | ✅ | ✅ |
| KHR_animation_pointer | ❌ | ❌ | ✅ | ✅ |
| KHR_interactivity | ❌ | ❌ | ❌ | ✅ (editor only) |
| KHR_audio_emitter | ❌ | ❌ | ✅ (experimental) | ❌ |
| KHR_node_visibility | ❌ | ❌ | ✅ | ✅ |
| MSFT_lod | ❌ | ❌ | ✅ | ✅ |

---

## Animation

| Feature | glTFast | UnityGLTF |
|---------|---------|-----------|
| Legacy Animation import | ✅ | ✅ |
| Mecanim import | ☑️ (clips generated, not auto-assigned) | ✅ |
| Humanoid retargeting | ❌ | ✅ |
| Animation export | ❌ | ✅ |
| Morph target animation | ✅ import | ✅ import & export |
| KHR_animation_pointer | ❌ | ✅ |
| Timeline recording | ❌ | ✅ |
| Runtime animation recording | ❌ | ✅ (GLTFRecorder) |

---

## Mesh Features

| Feature | glTFast | UnityGLTF |
|---------|---------|-----------|
| Skinning | ✅ | ✅ |
| Morph targets (blend shapes) | ✅ import | ✅ import & export |
| Sparse accessors | ☑️ (positions & morph targets only) | ☑️ (blend shapes) |
| Draco export | ✅ | ❌ |
| 16-bit mesh indices | ✅ preserved | ✅ |
| Triangle strip / fan | ✅ import | ✅ import |
| Points / Lines | ✅ import & export | ❌ |

---

## Texture Formats

| Format | glTFast | UnityGLTF |
|--------|---------|-----------|
| PNG | ✅ | ✅ |
| JPEG | ✅ | ✅ |
| KTX2 / Basis Universal | ✅ (requires package) | ✅ (requires package) |
| WebP | ❌ | ✅ (requires package) |
| EXR | ❌ | ✅ (Unity 6+) |

---

## Instantiation

| Feature | glTFast | UnityGLTF |
|---------|---------|-----------|
| GameObject | ✅ (GameObjectInstantiator) | ✅ (GLTFSceneImporter) |
| Bounds calculation | ✅ (GameObjectBoundsInstantiator) | ✅ |
| ECS Entities | ✅ (EntityInstantiator, experimental) | ❌ |
| Custom instantiation interface | ✅ (IInstantiator) | ✅ (plugin system) |
| Scene duplication / caching | ❌ | ✅ (ref-counted cache) |

---

## Extensibility

| Feature | glTFast | UnityGLTF |
|---------|---------|-----------|
| Plugin system | ✅ (ImportAddon) | ✅ (GLTFImportPlugin / GLTFExportPlugin) |
| Custom material generators | ✅ (IMaterialGenerator) | ✅ (shader export callbacks) |
| Custom download providers | ✅ (IDownloadProvider) | ✅ (IDataLoader) |
| Export hooks (before/after node, material, etc.) | Limited | ✅ (extensive callbacks) |
| Visual Scripting export | ❌ | ✅ (KHR_interactivity) |

---

## Special Features

| Feature | glTFast | UnityGLTF |
|---------|---------|-----------|
| Bake Particle Systems | ❌ | ✅ (experimental) |
| Bake Canvas UI | ❌ | ✅ (experimental) |
| Bake TextMeshPro | ❌ | ✅ (experimental) |
| Bake Sprite Meshes | ❌ | ✅ |
| Shader pass stripping | ❌ | ✅ |
| LOD support (MSFT_lod) | ❌ | ✅ |
| VisionOS / MaterialX | ❌ | ✅ (branches) |

---

## Summary

**Choose glTFast when:**
- Performance is critical (Burst + Jobs)
- HDRP support is needed
- DOTS / ECS integration is needed
- Draco export is needed
- Minimal dependencies preferred (JsonUtility, no Newtonsoft requirement)

**Choose UnityGLTF when:**
- Full roundtrip export is important (animations, morph targets, material variants)
- Advanced material extensions needed (transmission, volume, IOR, iridescence, sheen, dispersion)
- KHR_animation_pointer or KHR_interactivity is needed
- Humanoid retargeting is needed
- Timeline recording / runtime animation capture is needed
- LOD support is needed
- Baking Particle Systems, Canvas UI, or TextMeshPro to glTF

**Both packages can coexist** in the same Unity project. glTFast takes priority as the default importer; override per-file or globally with `UNITYGLTF_FORCE_DEFAULT_IMPORTER_ON`.
