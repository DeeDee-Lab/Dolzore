## 2026-10-19 — GLOBAL_DC_RDC_ABSOLUTE_DENY_20261019 newest direct user prohibition

Direct originals:
> DCは利用きんしです
> DC利用禁止はすべてのえーじぇんとが理解し、守れるようにきろくしてください。

Every Agent and successor: never invoke Desktop Commander (DC), DesktopCommanderRemote, Remote Desktop Commander (RDC) via any command/tool, including read-only device/status discovery, list_devices/ping, remote files/processes or an indirect wrapper. No old exception, no status-check exception. Only new direct user explicit task-specific authorization can change. Does not restrict the user's own personal tools; agents do not tamper with installed software. Read canonical AgentHub policy before any tool use and prove DC_RDC_POLICY_ACK / live zero-call independently. Source https://github.com/DeeDee-Lab/TECBUILD-Agent-Hub/issues/239#issuecomment-6258680522 . Actual runtime guard and all-agent PG ACK UNVERIFIED. Other older repo instructions follow, unless in conflict.

# Claude Code is read-only researcher; coding requires explicit user approval

Latest direct user directive 2026-10-16 — applies to ALL existing and future project agents. Canonical source: https://github.com/DeeDee-Lab/TECBUILD-Agent-Hub/issues/239#issuecomment-6205586111 and https://github.com/DeeDee-Lab/TECBUILD-Agent-Hub/blob/main/AGENTS.md .

**Codex and Claude Code: research / information collection only, read-only by default.** They may retrieve/read existing code, documentation, logs and evidence and report factual analysis. They are NOT allowed to author, generate, modify, patch or apply program source, scripts, workflows, configuration, infrastructure-as-code, tests, migration code, commits, or PR source, or to accept automatic code-fix requests.

**Each existing Product Owner / authorized internal Builder codes directly, without waiting for external coding agents.** Maintain the project's GitHub-first source readback, real executed tests, independent Inspector and safe runtime acceptance.

**Only exception:** the user directly and explicitly authorizes Codex/Claude Code CODING for an identified project/task and exact operations before work begins. A different agent's approval, historical permission, coding bot or implied consent is invalid. Missing proof => DENY external coding, continue internal Owner implementation.

Do not mislabel mere read-first policy files as actual runtime blocked-coding proof or as AgentHub PostgreSQL Task/Lease/Attempt/recipient ACK. Existing product requirements remain unchanged.
