#!/usr/bin/env python3
"""Generate public 20-second samples from the current DOLZORE source library.

IMPORTANT:
- This script writes ONLY bounded preview clips to docs/audio/samples.
- It never copies full purchased tracks into docs/.
- The source endpoint is migration-only and must not be referenced by public HTML/JS.
"""
from __future__ import annotations
import json, subprocess, tempfile, urllib.request, shutil
from pathlib import Path

ROOT=Path(__file__).resolve().parents[1]
TRACKS=ROOT/"docs/data/tracks.json"
OUT=ROOT/"docs/audio/samples"
SOURCE_BASE="https://dolzore.lovable.app/downloads/bgm/"
START_SECONDS=8
DURATION_SECONDS=20

def source_filename(track: dict) -> str:
    stem=track["title"].replace(" ","_")
    if track["number"]=="001":
        stem += "_V2"
    return f'DOLZORE_BGM_{track["number"]}_{stem}_v1.mp3'

def main() -> None:
    data=json.loads(TRACKS.read_text(encoding="utf-8"))
    tracks=data["tracks"]
    if len(tracks)!=60:
        raise SystemExit(f"expected 60 tracks, got {len(tracks)}")
    OUT.mkdir(parents=True,exist_ok=True)
    with tempfile.TemporaryDirectory() as td:
        td=Path(td)
        for track in tracks:
            src=td/source_filename(track)
            url=SOURCE_BASE+source_filename(track)
            dst=OUT/track["sampleFile"]
            print(f'{track["id"]}: {track["title"]}')
            urllib.request.urlretrieve(url,src)
            ffmpeg=shutil.which("ffmpeg")
            if not ffmpeg:
                try:
                    import imageio_ffmpeg
                    ffmpeg=imageio_ffmpeg.get_ffmpeg_exe()
                except Exception as exc:
                    raise SystemExit("ffmpeg unavailable; install ffmpeg or imageio-ffmpeg") from exc
            subprocess.run([
                ffmpeg,"-hide_banner","-loglevel","error","-y",
                "-ss",str(START_SECONDS),"-i",str(src),"-t",str(DURATION_SECONDS),
                "-af","afade=t=in:st=0:d=0.25,afade=t=out:st=19.5:d=0.5",
                "-codec:a","libmp3lame","-b:a","96k",str(dst)
            ],check=True)
            src.unlink(missing_ok=True)
    samples=sorted(OUT.glob("bgm-*.mp3"))
    if len(samples)!=60:
        raise SystemExit(f"expected 60 samples, got {len(samples)}")
    print("generated",len(samples),"sample-only files")

if __name__=="__main__":
    main()
