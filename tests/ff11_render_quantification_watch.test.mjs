const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('fs');

test('FF11 render watch is GitHub-only, structured, and no-false-inference', () => {
  const text = fs.readFileSync('.github/workflows/ff11-render-quantification-watch.yml', 'utf8');
  assert.match(text, /RENDER_ASSET_QUANTIFICATION_STATUS_V1\.json/);
  assert.match(text, /FF11_RENDER_WATCH_STATE_V1/);
  assert.match(text, /Unconfirmed values are not inferred/);
  assert.match(text, /schedule:/);
  assert.match(text, /issues: write/);
  assert.doesNotMatch(text, /self-hosted|Desktop Commander|Remote_Desktop|DPC/i);
});
