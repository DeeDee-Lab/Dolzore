# FF11 render-asset quantification — source-fidelity record v1

Authority: direct user instruction, 2026-10-01 JST  
Canonical issue: DeeDee-Lab/Dolzore#25  
Rule: RAW_SOURCE -> VERIFIED_FACT -> INTERPRETATION. Missing measurement means NOT_YET_MEASURED, never zero/absent.

## 0. Installed retail source identity

SPC/SOWCIEPC source root:
`C:\Program Files (x86)\PlayOnline\SquareEnix\FINAL FANTASY XI`

Observed full-file manifest:
- file_count = 65329
- total_bytes = 14860167302
- scan lineage: Automation#603
- scan_id = SPC-FF11-20260930T233741Z
- manifest_sha256 = F0453C0E49982B83168727C4B5D09D490C44CA6DC8AA13C76B26BEA01C75BFB3
- later scan internal PASS: Automation run 36819170464 / scan_id spc-ff11-20261001T051909Z
- asset bodies copied = false
- client/game mutation = false
- DC/RDC = false

All protected DAT/model/texture/animation bodies remain in-place on SPC. A same-host authorized scanner must read the original bytes and emit metadata/measurements only.

## 1. Pinned parser source identity

Parser source: jondwillis/kuluu-ffxi  
Pinned commit: `937b9978de923c517435d26e20ea68067fe6a388`

Exact parser files:
- `ffxi-dat/src/skel.rs` blob `466a54dba8a001b317ddf8cfacbe9f4fabdc188a`
- `ffxi-dat/src/resource_dir.rs` blob `b838afe1fe4eafd17a697520cd8197ae70b9a097`
- `ffxi-dat/src/skel_mesh.rs` blob `5b2ee1de068b322cdac59c15c46184246057ed4d`
- `ffxi-dat/src/texture.rs` blob `5b8b8450113211345614f070684d77ebeca40704`
- `ffxi-dat/src/mmb.rs` blob `0ecd19cb663ea9e823d37522a0a62f3e94db5b35`
- `ffxi-dat/src/mzb.rs` blob `261a10617ce3a110498125bcf432b6737fb59940`
- `ffxi-dat/src/anim.rs` blob `10fca21b33fb90479c6044a6297eaf07ca63e674`
- `ffxi-dat/src/main_dll.rs` blob `fb9e57eff0de1b2de5f520e68df2331589080670`

Do not replace these pointers with a paraphrased parser description.

## 2. Exact parser facts relevant to requested metrics

### Skeleton / bone count
`skel.rs::Skeleton` contains `joints: Vec<Joint>`.  
`skel.rs::parse` reads `num_joints = read_u8(data, 0x02) as usize`.  
Therefore race bone/joint count is directly measurable from the original race-config DAT.

Kuluu retail-2026-09 race-config file IDs:
`[7072, 10248, 13424, 16600, 19776, 19776, 23176, 26352]`
for playable look races HumeM=1 through Galka=8. Tarutaru M/F share file_id 19776 in this table.

Community AltanaViewer path mapping:
- HumeM = ROM/27/82.DAT
- HumeF = ROM/32/58.DAT
- ElvaanM = ROM/37/31.DAT
- ElvaanF = ROM/42/4.DAT
- Tarutaru = ROM/46/93.DAT
- Mithra = ROM/51/89.DAT
- Galka = ROM/56/59.DAT

Current exact joint counts: NOT_YET_MEASURED_ON_SPC.

### Character/equipment mesh and triangles
`resource_dir.rs::collect_skel_meshes()` collects VertexOs2 skeleton meshes.  
`skel_mesh.rs::SkelMesh` contains `meshes: Vec<MeshBuffer>`; each `MeshBuffer` contains `vertices: Vec<SkinVertex>` and a `MeshType`.

The parser expands the authored mesh instructions into render vertices. Triangle count must be counted according to `MeshType` and expanded buffers, not guessed from DAT file size.

Noesis standard HumeM example identifies one valid complete player assembly:
- skeleton/animation: ROM/27/82.dat
- face: ROM/27/87.dat
- head: ROM/27/103.dat
- body: ROM/28/7.dat
- hands: ROM/28/52.dat
- waist: ROM/28/84.dat
- legs: ROM/28/116.dat
- weapon: ROM/29/20.dat

Current exact complete-character triangles and per-slot triangle distributions: NOT_YET_MEASURED_ON_SPC.

### Texture dimensions / formats
`texture.rs::DecodedTexture` exposes:
- width: u32
- height: u32
- format_tag: TexFormat

`TexFormat` exact parser variants:
- Dxt1, magic `1TXD`
- Dxt3, magic `3TXD`
- Bgra32, magic `BGRA`
- Argb32, magic `ARGB`

The parser also accepts indexed/palettized image records and returns decoded BGRA32.

Compression math to store as DERIVED fields only:
- DXT1 block = 8 bytes per 4x4 texels => 0.5 bytes/texel, nominal 8:1 vs 32-bit RGBA for multiples of 4.
- DXT3 block = 16 bytes per 4x4 texels => 1 byte/texel, nominal 4:1 vs 32-bit RGBA.
Do not write these calculated ratios into raw_value.

Current full-client resolution and format histograms: NOT_YET_MEASURED_ON_SPC.

### Animation clips / frames / timing
`resource_dir.rs::collect_animations()` collects AnimMo2 chunks.  
`anim.rs` reads:
- header_frames = u16 from body[4..6]
- speed = f32 from body[6..10]
- exported animation frames = header_frames.saturating_sub(1)
- per-bone frame arrays are emitted for each referenced bone.

`resource_dir.rs::collect_schedulers()` exposes scheduler stages, including stage frame, duration_frames and loop metadata.

Existing historical HumeM mod observation:
- BLM starting-cast motion: 14 frames; mb00/mb01/mb02 split across ROM/27/82 and ROM/27/86.
- NIN starting-cast motion: 15 frames; mn00/mn01/mn02 split across ROM/27/82 and ROM/27/86.
This is COMMUNITY_RESEARCH, not SPC_CURRENT measurement.

Kuluu retail-DAT observation embedded in source:
HumeM `dead` routine uses `ded?` for 116 half-frames = 58 real frames, then `cor?`.
Retain the source pointer; do not generalize this single routine to all clips.

Current per-race clip census/frame histogram/speed distribution: NOT_YET_MEASURED_ON_SPC.

### Zone geometry / objects / building geometry
`mzb.rs` parses collision triangle counts and MMB placements from zone DATs.  
`mmb.rs::MmbModel` contains vertices and explicit indices; render triangle count = indices length / 3 for the decoded model.

`mzb::parse_mmb_placements` returns object placements. Thus:
- Zone object count = placement count (with exact definition retained).
- Building/object geometry can be joined from placement asset ID -> resolved MMB model -> model triangle/material/texture counts.
- Zone render geometry must distinguish base MZB collision triangles from placed MMB render triangles. Never merge these into one unlabeled number.

Existing Kuluu retail-derived community evidence:
- Port Jeuno placement-grid correction produced 5405 placements.
This is COMMUNITY_RESEARCH until reproduced on SPC.

Current per-zone SPC triangle/object census and per-building geometry distribution: NOT_YET_MEASURED_ON_SPC.

### Material counts
For zone MMB, one parsed `MmbModel` carries one texture_name plus render/blending state. Material/draw grouping must preserve:
- texture_name
- blending/render_state
- vertex_blend_enabled
and any other state used by the retail-faithful renderer.

"Materials in one screen" is camera/frustum-dependent and cannot be derived from total zone assets alone. It requires a defined camera pose/FOV plus visible-placement culling, then unique active render-state/material keys. Current exact values: NOT_YET_MEASURED.

### LOD / culling distance
Existing parser/reimplementation evidence shows zone-authored draw distance and fog range are client data, with a retail multiplier of 1.0 in the referenced implementation. This does NOT by itself prove one universal culling distance.

Community target/draw observation of ~50 yalms for character visibility is not accepted as client-static truth until runtime/client evidence is captured.

Current exact per-zone authored distance distribution and actor/object culling thresholds: NOT_YET_MEASURED_ON_SPC.

## 3. Required scanner outputs

The next read-only SPC scanner must emit metadata only:

1. `race_skeletons.jsonl`
   - race/look id
   - race label
   - source file_id
   - exact resolved path
   - source SHA-256
   - joint_count
   - joint_reference_count
   - bounding_box_count

2. `character_mesh_samples.jsonl`
   - assembly id
   - race
   - exact source DAT set
   - per-part mesh_buffer_count
   - per-part render_vertex_count
   - per-part triangle_count
   - total triangle_count
   - texture/material keys

3. `equipment_mesh_census.jsonl`
   - race, equipment slot, model id/file_id/path/hash
   - mesh count, render vertices, triangles, material/texture names

4. `texture_census.jsonl`
   - source DAT identity + chunk identity
   - raw flag/magic
   - decoded width/height
   - TexFormat
   - compressed payload bytes where determinable
   - decoded RGBA bytes as DERIVED only
   - compression ratio as DERIVED only

5. `animation_census.jsonl`
   - race source DAT
   - clip/chunk id
   - header_frames raw
   - exported frames derived
   - speed raw
   - per-bone channel count
   - scheduler stage frame/duration/loop records separately

6. `zone_census.jsonl`
   - zone/file identity
   - MZB collision triangle count
   - MMB placement count
   - unique placement asset count
   - resolved placed-render triangle sum
   - unresolved asset count

7. `object_geometry_census.jsonl`
   - zone + placement id/name
   - resolved MMB identity
   - triangles
   - vertices
   - submesh/material count
   - placement multiplicity
   - AABB

8. `draw_distance_census.jsonl`
   - zone/source identity
   - raw authored draw/fog values
   - exact parser/source symbol
   - no universalization

9. `screen_material_samples.jsonl`
   - runtime/static camera sample id
   - zone, position, yaw/pitch/FOV/resolution
   - visible placements
   - unique active material keys
   - draw groups
   - culling definition

## 4. Truth status for the 11 requested metrics

- Character triangles: extractor path proven; exact SPC number pending.
- Race bone counts: direct byte field/parser proven; exact SPC number pending.
- Texture resolution distribution: direct decoder fields proven; full histogram pending.
- Texture format/compression: exact supported formats proven; installed distribution pending.
- Equipment part mesh amount: direct skel-mesh parser proven; census pending.
- Animation clip count: direct AnimMo2 collector proven; census pending.
- Animation frame/timing: raw fields proven; full current distribution pending.
- Zone triangle count: MZB/MMB parse path proven; per-zone census pending.
- Building geometry: placement->MMB join proven possible; classification/census pending.
- One-screen materials: requires camera-defined visibility sample; pending.
- LOD/Culling distance: authored zone distance path exists; exact current distributions/runtime thresholds pending.
- Zone object count: MMB placement count is directly measurable; current per-zone census pending.

NO_FALSE_COMPLETION=true
EXACT_FF11_RENDER_BUDGETS_COMPLETE=false
