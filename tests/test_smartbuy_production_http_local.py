"""Offline contract tests for the pinned production HTTP smoke script."""
import contextlib
import io
import json
import os
from pathlib import Path
import runpy
import unittest
from datetime import datetime, timezone, timedelta
from unittest.mock import patch

SCRIPT = Path(__file__).resolve().parents[1] / "scripts" / "smartbuy_production_http_local.py"
BASE = "https://smartbuy-hub-one.vercel.app"
FEED = "https://raw.githubusercontent.com/DeeDee-Lab/Dolzore/main/docs/data/smartbuy-projectors.json"

class Response(io.BytesIO):
    status = 200
    headers = {"Access-Control-Allow-Origin": "*", "Content-Type": "text/plain"}

class SmokeContract(unittest.TestCase):
    def setUp(self):
        self.payloads = {
            BASE + "/": "<title>SmartBuy Hub</title>",
            BASE + "/styles.css": ".shell",
            BASE + "/app.js": "./data/latest.json publicGitHubMarketFallback github.com/DeeDee-Lab/Dolzore/actions",
            BASE + "/data/latest.json": json.dumps({"schema_version": "1.0-url-control-plane", "recommendations": [], "system": {"source": "DOLZORE-public-github-observation"}}),
            BASE + "/data/runs.json": "[]",
            FEED: json.dumps({"generatedAt": datetime.now(timezone.utc).isoformat(), "observedUsed": []}),
        }
        self.cors = "*"
        self.calls = []

    def execute(self):
        def open_fixture(request, timeout):
            self.assertEqual(timeout, 20)
            self.calls.append(request.full_url)
            response = Response(self.payloads[request.full_url].encode())
            response.headers = {"Access-Control-Allow-Origin": self.cors}
            return response
        output = io.StringIO()
        with patch.dict(os.environ), patch("urllib.request.urlopen", side_effect=open_fixture), contextlib.redirect_stdout(output):
            runpy.run_path(str(SCRIPT), run_name="__main__")
        return output.getvalue()

    def test_success(self):
        self.assertIn("SMARTBUY_PRODUCTION_SMOKE_PASS=true", self.execute())
        self.assertEqual(self.calls, list(self.payloads))

    def test_stale_feed(self):
        self.payloads[FEED] = json.dumps({"generatedAt": (datetime.now(timezone.utc)-timedelta(days=3)).isoformat(), "observedUsed": []})
        with self.assertRaisesRegex(AssertionError, "stale"):
            self.execute()

    def test_cors(self):
        self.cors = "https://invalid.example"
        with self.assertRaisesRegex(AssertionError, "CORS"):
            self.execute()

    def test_legacy_api(self):
        self.payloads[BASE + "/app.js"] += " /api/collection-request"
        with self.assertRaisesRegex(AssertionError, "Railway"):
            self.execute()

    def test_invalid_json(self):
        self.payloads[BASE + "/data/runs.json"] = "{"
        with self.assertRaises(json.JSONDecodeError):
            self.execute()

if __name__ == "__main__":
    unittest.main()
