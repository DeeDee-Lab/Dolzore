from __future__ import annotations
import re
from pathlib import Path
import unittest

ROOT=Path(__file__).resolve().parents[1]
TEXT_SUFFIXES={".md",".txt",".json",".html",".css",".js",".py",".yml",".yaml",".xml",".gitignore"}

SECRET_PATTERNS={
    "private_key": re.compile(r"BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY",re.I),
    "openai_key": re.compile(r"\bsk-[A-Za-z0-9_-]{20,}"),
    "github_pat": re.compile(r"\bgithub_pat_[A-Za-z0-9_]{20,}"),
    "github_classic_pat": re.compile(r"\bghp_[A-Za-z0-9]{20,}"),
    "aws_access_key": re.compile(r"\bAKIA[0-9A-Z]{16}\b"),
}
FORBIDDEN_LITERALS=(
    "TECBUILD",
    "gold.clover",
    "downloads/bgm",
)

class PublicSafetyTests(unittest.TestCase):
    def test_repository_contains_no_public_full_audio_files(self):
        audio_ext={".mp3",".wav",".flac",".m4a",".aac",".ogg"}
        found=[str(p.relative_to(ROOT)) for p in ROOT.rglob("*") if p.is_file() and p.suffix.lower() in audio_ext and ".git" not in p.parts]
        self.assertEqual(found,[],f"public/full audio file found: {found}")

    def test_repository_contains_no_known_sensitive_markers(self):
        findings=[]
        for p in ROOT.rglob("*"):
            if not p.is_file() or ".git" in p.parts:
                continue
            if p.suffix.lower() not in TEXT_SUFFIXES and p.name!=".gitignore":
                continue
            text=p.read_text(encoding="utf-8",errors="ignore")
            for literal in FORBIDDEN_LITERALS:
                if literal.lower() in text.lower():
                    findings.append((str(p.relative_to(ROOT)),literal))
            for name,pattern in SECRET_PATTERNS.items():
                if pattern.search(text):
                    findings.append((str(p.relative_to(ROOT)),name))
        self.assertEqual(findings,[],f"sensitive/public-safety findings: {findings}")

if __name__=="__main__":
    unittest.main()
