import hashlib
from pathlib import Path
import socket
import sys
import tempfile
import unittest
from urllib.request import urlopen

SCRIPTS=Path(__file__).resolve().parents[1]/'scripts'
if SCRIPTS.is_dir(): sys.path.insert(0,str(SCRIPTS))
from dolzore_visual_local import PROFILES, PREVIEWS, local_site, collect_artifacts


class VisualTests(unittest.TestCase):
    def test_original_output_dirs(self):
        self.assertEqual({k:v[4] for k,v in PROFILES.items()}, {'first-town':'qa-first-town','slice':'qa-slice1','visual':'qa-screenshots'})

    def test_own_loopback_listener_and_cleanup(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp); (root/'index.html').write_text('synthetic QA fixture')
            with local_site(root,0) as port:
                self.assertEqual(urlopen(f'http://127.0.0.1:{port}/',timeout=3).read(),b'synthetic QA fixture')
            with socket.socket() as sock:
                sock.settimeout(1)
                self.assertNotEqual(sock.connect_ex(('127.0.0.1',port)),0)

    def test_occupied_port_not_reused(self):
        with tempfile.TemporaryDirectory() as tmp, local_site(Path(tmp),0) as port:
            with self.assertRaises(OSError):
                with local_site(Path(tmp),port): pass

    def test_artifact_hash_and_copy(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp).resolve(); out=root/'qa-slice1'; out.mkdir(); (out/'sample.png').write_bytes(b'fixture')
            result=collect_artifacts(root,'qa-slice1',root/'evidence','slice')
            self.assertEqual(result,[{'name':'sample.png','bytes':7,'sha256':hashlib.sha256(b'fixture').hexdigest()}])
            self.assertEqual((root/'evidence'/'sample.png').read_bytes(),b'fixture')

    def test_visual_requires_all_previews(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp).resolve(); out=root/'qa-screenshots'; out.mkdir(); (out/'one.png').write_bytes(b'fixture')
            with self.assertRaisesRegex(ValueError,'REQUIRED_PREVIEWS_MISSING'):
                collect_artifacts(root,'qa-screenshots',root/'evidence','visual')
            for name in PREVIEWS: (out/name).write_bytes(b'fixture')
            self.assertEqual(len(collect_artifacts(root,'qa-screenshots',root/'evidence','visual')),7)

    def test_empty_and_nested_artifacts_rejected(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp).resolve(); out=root/'qa-slice1'; out.mkdir()
            with self.assertRaises(ValueError): collect_artifacts(root,'qa-slice1',root/'evidence','slice')
            (out/'nested').mkdir()
            with self.assertRaises(ValueError): collect_artifacts(root,'qa-slice1',root/'evidence','slice')

    def test_symlink_site_rejected_before_bind(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp); (root/'linked').symlink_to(root/'missing')
            with self.assertRaisesRegex(ValueError,'UNSAFE_SITE_SYMLINK'):
                with local_site(root,0): pass

    def test_partial_visual_artifacts_preserved_without_success(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp).resolve(); out=root/'qa-screenshots'; out.mkdir(); (out/'one.png').write_bytes(b'fixture')
            self.assertEqual(len(collect_artifacts(root,'qa-screenshots',root/'evidence','visual',require_complete=False)),1)
