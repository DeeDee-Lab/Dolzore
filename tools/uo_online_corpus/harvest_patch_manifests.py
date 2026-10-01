#!/usr/bin/env python3
"""
Harvest the live Ultima Online Classic Client patch-manifest hierarchy.

Primary source:
  http://patch.uo.eamythic.com/uopatch-sa/legacyrelease/uo/manifest/uo-legacyrelease.prod

Outputs are evidence/provenance files only. Commercial UO art/audio/map/content bytes
are not committed. Raw patch manifests are preserved byte-for-byte because they are
distribution metadata and are needed to reproduce the file inventory.

The filename hash used by the Mythic patch CDN is Jenkins hashlittle2.
The implementation below was independently transcribed from the public algorithm and
cross-checked against andrezaiats/uo-patcher (MIT License, copyright 2026 Andre Zaiats):
  https://github.com/andrezaiats/uo-patcher
"""

from __future__ import annotations

import datetime as dt
import hashlib
import json
import os
from pathlib import Path
import shutil
import struct
import sys
import urllib.request
import xml.etree.ElementTree as ET
import zlib

PRODUCT_URL = os.environ.get(
    "UO_PRODUCT_URL",
    "http://patch.uo.eamythic.com/uopatch-sa/legacyrelease/uo/manifest/uo-legacyrelease.prod",
)
OUT = Path(
    os.environ.get(
        "UO_CORPUS_OUT",
        "research/uo/online/current_patch",
    )
)
USER_AGENT = "Dolzore-UO-Evidence-Harvester/1.0"

PROBE_NAMES = {
    "client.exe",
    "body.def",
    "bodyconv.def",
    "equipconv.def",
    "gump.def",
    "sound.def",
    "mobtypes.txt",
    "skills.mul",
    "skillgrp.mul",
    "tiledata.mul",
    "animdata.mul",
    "radarcol.mul",
    "hues.mul",
    "speech.mul",
    "fonts.mul",
    "multimap.rle",
}


def now_utc() -> str:
    return dt.datetime.now(dt.timezone.utc).replace(microsecond=0).isoformat()


def sha256(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def fetch(url: str, timeout: int = 60) -> bytes:
    req = urllib.request.Request(url, headers={"User-Agent": USER_AGENT})
    with urllib.request.urlopen(req, timeout=timeout) as resp:
        return resp.read()


def safe_write(path: Path, data: bytes) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_bytes(data)


def write_json(path: Path, obj) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(obj, ensure_ascii=False, indent=2, sort_keys=True) + "\n", encoding="utf-8")


def write_jsonl(path: Path, rows) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    with path.open("w", encoding="utf-8", newline="\n") as f:
        for row in rows:
            f.write(json.dumps(row, ensure_ascii=False, sort_keys=True) + "\n")


def _rot(x: int, k: int) -> int:
    return ((x << k) | (x >> (32 - k))) & 0xFFFFFFFF


def _mix(a: int, b: int, c: int):
    a = (a - c) & 0xFFFFFFFF; a ^= _rot(c, 4);  c = (c + b) & 0xFFFFFFFF
    b = (b - a) & 0xFFFFFFFF; b ^= _rot(a, 6);  a = (a + c) & 0xFFFFFFFF
    c = (c - b) & 0xFFFFFFFF; c ^= _rot(b, 8);  b = (b + a) & 0xFFFFFFFF
    a = (a - c) & 0xFFFFFFFF; a ^= _rot(c, 16); c = (c + b) & 0xFFFFFFFF
    b = (b - a) & 0xFFFFFFFF; b ^= _rot(a, 19); a = (a + c) & 0xFFFFFFFF
    c = (c - b) & 0xFFFFFFFF; c ^= _rot(b, 4);  b = (b + a) & 0xFFFFFFFF
    return a, b, c


def _final(a: int, b: int, c: int):
    c ^= b; c = (c - _rot(b, 14)) & 0xFFFFFFFF
    a ^= c; a = (a - _rot(c, 11)) & 0xFFFFFFFF
    b ^= a; b = (b - _rot(a, 25)) & 0xFFFFFFFF
    c ^= b; c = (c - _rot(b, 16)) & 0xFFFFFFFF
    a ^= c; a = (a - _rot(c, 4))  & 0xFFFFFFFF
    b ^= a; b = (b - _rot(a, 14)) & 0xFFFFFFFF
    c ^= b; c = (c - _rot(b, 24)) & 0xFFFFFFFF
    return a, b, c


def hashlittle2(data: bytes, initval: int = 0, initval2: int = 0):
    length = len(data)
    a = b = c = (0xDEADBEEF + length + initval) & 0xFFFFFFFF
    c = (c + initval2) & 0xFFFFFFFF
    pos = 0
    remaining = length
    while remaining > 12:
        a = (a + int.from_bytes(data[pos:pos+4], "little")) & 0xFFFFFFFF
        b = (b + int.from_bytes(data[pos+4:pos+8], "little")) & 0xFFFFFFFF
        c = (c + int.from_bytes(data[pos+8:pos+12], "little")) & 0xFFFFFFFF
        a, b, c = _mix(a, b, c)
        pos += 12
        remaining -= 12
    rem = data[pos:pos+remaining]
    if remaining:
        padded = rem + b"\x00" * (12 - remaining)
        if remaining >= 1:  a = (a + padded[0]) & 0xFFFFFFFF
        if remaining >= 2:  a = (a + (padded[1] << 8)) & 0xFFFFFFFF
        if remaining >= 3:  a = (a + (padded[2] << 16)) & 0xFFFFFFFF
        if remaining >= 4:  a = (a + (padded[3] << 24)) & 0xFFFFFFFF
        if remaining >= 5:  b = (b + padded[4]) & 0xFFFFFFFF
        if remaining >= 6:  b = (b + (padded[5] << 8)) & 0xFFFFFFFF
        if remaining >= 7:  b = (b + (padded[6] << 16)) & 0xFFFFFFFF
        if remaining >= 8:  b = (b + (padded[7] << 24)) & 0xFFFFFFFF
        if remaining >= 9:  c = (c + padded[8]) & 0xFFFFFFFF
        if remaining >= 10: c = (c + (padded[9] << 8)) & 0xFFFFFFFF
        if remaining >= 11: c = (c + (padded[10] << 16)) & 0xFFFFFFFF
        if remaining >= 12: c = (c + (padded[11] << 24)) & 0xFFFFFFFF
        a, b, c = _final(a, b, c)
    return c, b


def filename_hash(name: str) -> str:
    h1, h2 = hashlittle2(name.lower().encode("ascii"))
    return f"{h1:08x}{h2:08x}"


def parse_hex(value: str | None) -> int:
    if not value:
        return 0
    return int(value, 16)


def decode_manifest(raw: bytes):
    compressed = False
    try:
        xml_bytes = zlib.decompress(raw)
        compressed = True
    except zlib.error:
        xml_bytes = raw
    return xml_bytes, compressed, ET.fromstring(xml_bytes.decode("utf-8", errors="strict"))


def normalize_rel(name: str) -> str:
    return name.replace("\\", "/").lstrip("/")


def pe_version_strings(data: bytes) -> dict:
    """
    Lightweight, dependency-free evidence probe:
    preserve only printable UTF-16LE sequences around common VERSIONINFO keys.
    This is NOT a replacement for a full PE resource parser.
    """
    out = {}
    keys = ["FileVersion", "ProductVersion", "ProductName", "CompanyName", "OriginalFilename"]
    text = data.decode("utf-16le", errors="ignore")
    for key in keys:
        idx = text.find(key)
        if idx >= 0:
            fragment = text[idx:idx+256]
            cleaned = "".join(ch if ch.isprintable() else " " for ch in fragment)
            out[key] = cleaned[:256]
    return out


def main():
    if OUT.exists():
        shutil.rmtree(OUT)
    OUT.mkdir(parents=True, exist_ok=True)

    retrieved_at = now_utc()

    product_raw = fetch(PRODUCT_URL)
    safe_write(OUT / "raw/uo-legacyrelease.prod", product_raw)
    product_root = ET.fromstring(product_raw.decode("utf-8", errors="strict"))
    product = product_root.find("product")
    if product is None:
        raise RuntimeError("product element not found")

    manifest_repos = [r.get("url") for r in product.findall("./manifestrepos/repo")]
    file_repos = [r.get("url") for r in product.findall("./filerepos/repo")]
    if not manifest_repos or not file_repos:
        raise RuntimeError("manifest/file repo missing")
    manifest_repo = manifest_repos[0]
    file_repo = file_repos[0]

    product_record = {
        "retrieved_at_utc": retrieved_at,
        "source_url": PRODUCT_URL,
        "raw_sha256": sha256(product_raw),
        "raw_bytes": len(product_raw),
        "product_attributes": dict(product.attrib),
        "manifest_repos": manifest_repos,
        "file_repos": file_repos,
        "stages": [],
    }

    manifest_records = []
    unpacked = []
    pack_entries = []
    visited = set()

    def collect(stage_name: str, package_name: str, pkg_rpath: str, manifest_name: str):
        key = (pkg_rpath, manifest_name)
        if key in visited:
            return
        visited.add(key)
        url = f"{manifest_repo}{pkg_rpath}/{manifest_name}"
        raw = fetch(url)
        xml_bytes, compressed, root = decode_manifest(raw)

        raw_path = OUT / "raw/manifests" / normalize_rel(pkg_rpath) / manifest_name
        xml_path = OUT / "raw/manifests_xml" / normalize_rel(pkg_rpath) / (manifest_name + ".xml")
        safe_write(raw_path, raw)
        safe_write(xml_path, xml_bytes)

        manifest_records.append({
            "stage": stage_name,
            "package": package_name,
            "package_rpath": pkg_rpath,
            "manifest_name": manifest_name,
            "source_url": url,
            "raw_repo_path": raw_path.as_posix(),
            "xml_repo_path": xml_path.as_posix(),
            "raw_sha256": sha256(raw),
            "raw_bytes": len(raw),
            "xml_sha256": sha256(xml_bytes),
            "xml_bytes": len(xml_bytes),
            "zlib_compressed": compressed,
        })

        manifest = root.find("manifest")
        if manifest is None:
            manifest = root

        files_el = manifest.find("files")
        if files_el is not None:
            for f in files_el.findall("f"):
                name = f.get("n")
                if not name:
                    continue
                record = {
                    "kind": "unpacked",
                    "stage": stage_name,
                    "package": package_name,
                    "package_rpath": pkg_rpath,
                    "source_manifest": manifest_name,
                    "name": name,
                    "uncompressed_bytes": parse_hex(f.get("ul")),
                    "compression_type": int(f.get("ct", "0")),
                    "compressed_bytes": parse_hex(f.get("cl")),
                    "manifest_attributes": dict(f.attrib),
                    "filename_hashlittle2": filename_hash(name),
                }
                record["download_url"] = f"{file_repo}base/unpacked/{record['filename_hashlittle2']}"
                unpacked.append(record)

        packs_el = manifest.find("packs")
        if packs_el is not None:
            for p in packs_el.findall("p"):
                pack_name = p.get("name")
                pack_rpath = p.get("rpath")
                files = p.find("files")
                if files is None:
                    continue
                for f in files.findall("f"):
                    ph = (f.get("ph") or "0").lower()
                    sh = (f.get("sh") or "0").lower()
                    record = {
                        "kind": "pack_entry",
                        "stage": stage_name,
                        "package": package_name,
                        "package_rpath": pkg_rpath,
                        "source_manifest": manifest_name,
                        "pack_name": pack_name,
                        "pack_rpath": pack_rpath,
                        "ph": ph,
                        "sh": sh,
                        "uncompressed_bytes": parse_hex(f.get("ul")),
                        "compression_type": int(f.get("ct", "0")),
                        "compressed_bytes": parse_hex(f.get("cl")),
                        "manifest_attributes": dict(f.attrib),
                    }
                    record["download_url"] = (
                        f"{file_repo}base/{pack_rpath}/{int(ph,16):08x}{int(sh,16):08x}"
                    )
                    pack_entries.append(record)

        manifests_el = manifest.find("manifests")
        if manifests_el is not None:
            for sub in manifests_el.findall("manifest"):
                sub_name = sub.get("n")
                if sub_name:
                    collect(stage_name, package_name, pkg_rpath, sub_name)

    for stage_el in product.findall("./stages/stage"):
        stage_name = stage_el.get("name")
        stage_rec = {"name": stage_name, "attributes": dict(stage_el.attrib), "packages": []}
        for pkg_el in stage_el.findall("./packages/package"):
            package_name = pkg_el.get("name")
            pkg_rpath = pkg_el.get("rpath")
            mf = pkg_el.find("manifest")
            if mf is None:
                continue
            manifest_name = mf.get("n")
            pkg_rec = {
                "name": package_name,
                "rpath": pkg_rpath,
                "attributes": dict(pkg_el.attrib),
                "manifest_attributes": dict(mf.attrib),
            }
            stage_rec["packages"].append(pkg_rec)
            collect(stage_name, package_name, pkg_rpath, manifest_name)
        product_record["stages"].append(stage_rec)

    # Exact inventory files (no semantic rewriting).
    write_json(OUT / "product_manifest.json", product_record)
    write_jsonl(OUT / "manifest_sources.jsonl", sorted(manifest_records, key=lambda x: (x["package_rpath"], x["manifest_name"])))

    # Deduplicate only exact manifest duplicates, while preserving every original record in *_all.jsonl.
    write_jsonl(OUT / "unpacked_files_all.jsonl", unpacked)
    write_jsonl(OUT / "pack_entries_all.jsonl", pack_entries)

    unpacked_unique = {}
    for r in unpacked:
        unpacked_unique[(r["name"], r["uncompressed_bytes"], r["compression_type"], r["compressed_bytes"])] = r
    pack_unique = {}
    for r in pack_entries:
        pack_unique[(r["pack_name"], r["ph"], r["sh"], r["uncompressed_bytes"])] = r

    unpacked_u = sorted(unpacked_unique.values(), key=lambda x: x["name"].lower())
    pack_u = sorted(pack_unique.values(), key=lambda x: (x["pack_name"] or "", x["ph"], x["sh"]))
    write_jsonl(OUT / "unpacked_files_unique.jsonl", unpacked_u)
    write_jsonl(OUT / "pack_entries_unique.jsonl", pack_u)

    (OUT / "unpacked_filenames.txt").write_text(
        "".join(r["name"] + "\n" for r in unpacked_u), encoding="utf-8"
    )

    # Probe selected structural files by downloading the actual CDN bytes,
    # decompressing when the manifest says ct=1, hashing final bytes, and deleting them.
    probes = []
    by_name = {r["name"].lower(): r for r in unpacked_u}
    for wanted in sorted(PROBE_NAMES):
        rec = by_name.get(wanted.lower())
        if not rec:
            probes.append({"name": wanted, "status": "not_listed"})
            continue
        cdn_raw = fetch(rec["download_url"])
        final = zlib.decompress(cdn_raw) if rec["compression_type"] == 1 else cdn_raw
        probe = {
            "name": rec["name"],
            "status": "downloaded_for_hash_only",
            "download_url": rec["download_url"],
            "manifest_uncompressed_bytes": rec["uncompressed_bytes"],
            "manifest_compressed_bytes": rec["compressed_bytes"],
            "downloaded_bytes": len(cdn_raw),
            "downloaded_sha256": sha256(cdn_raw),
            "final_bytes": len(final),
            "final_sha256": sha256(final),
            "compression_type": rec["compression_type"],
        }
        if rec["name"].lower() == "client.exe":
            probe["pe_version_string_probe"] = pe_version_strings(final)
            probe["dos_magic"] = final[:2].hex()
            if len(final) >= 0x40:
                pe_off = struct.unpack_from("<I", final, 0x3C)[0]
                probe["pe_offset"] = pe_off
                probe["pe_signature"] = final[pe_off:pe_off+4].hex() if pe_off + 4 <= len(final) else None
        probes.append(probe)
    write_json(OUT / "probe_file_hashes.json", probes)

    uop_loose = [r for r in unpacked_u if r["name"].lower().endswith(".uop")]
    write_jsonl(OUT / "uop_archives_as_loose_files.jsonl", uop_loose)

    summary = {
        "retrieved_at_utc": retrieved_at,
        "product_url": PRODUCT_URL,
        "product_raw_sha256": sha256(product_raw),
        "product_serial": product.get("serial"),
        "launchfile": product.get("launchfile"),
        "manifest_repo": manifest_repo,
        "file_repo": file_repo,
        "manifest_count": len(manifest_records),
        "unpacked_records_all": len(unpacked),
        "unpacked_records_unique": len(unpacked_u),
        "pack_records_all": len(pack_entries),
        "pack_records_unique": len(pack_u),
        "loose_uop_archive_count": len(uop_loose),
        "unpacked_total_uncompressed_bytes_unique": sum(r["uncompressed_bytes"] for r in unpacked_u),
        "unpacked_total_transfer_bytes_unique": sum(
            r["compressed_bytes"] if r["compression_type"] == 1 else r["uncompressed_bytes"] for r in unpacked_u
        ),
        "pack_total_uncompressed_bytes_unique": sum(r["uncompressed_bytes"] for r in pack_u),
        "pack_total_transfer_bytes_unique": sum(
            r["compressed_bytes"] if r["compression_type"] == 1 else r["uncompressed_bytes"] for r in pack_u
        ),
        "probe_count": len(probes),
    }
    summary["combined_transfer_bytes_unique"] = (
        summary["unpacked_total_transfer_bytes_unique"] + summary["pack_total_transfer_bytes_unique"]
    )
    write_json(OUT / "summary.json", summary)

    print(json.dumps(summary, indent=2, sort_keys=True))


if __name__ == "__main__":
    main()
