# UO OPEN-SOURCE SERVER ARCHITECTURE REFERENCE

Authority: issue #27
Evidence class: OPEN_SOURCE_EMULATION_REFERENCE
Critical boundary: this file describes open-source emulator implementation evidence. It is NOT evidence of Origin/EA/Broadsword retail server internals.

## ModernUO repository
Repository: https://github.com/modernuo/ModernUO
Observed commit in initial research: edd7553c6f35420df33543e0d24fb1c7f785b427

Observed code/document surfaces:
- Projects/Server/World/World.cs — persistent world coordination surface.
- Projects/UOContent/World Saves/AutoSave.cs — automatic world-save orchestration.
- Projects/UOContent/World Saves/AutoArchive.cs — world-save archive/restore surface.
- Projects/Server/Network/NetState/NetState.cs — connection/session state object.
- Projects/Server/Network/NetState/NetState.Network.cs — network I/O surface.
- Projects/Server/Network/NetState/NetState.Movement.cs — movement-related network state surface.
- Projects/Server/Maps/Map.MobileEnumerator.cs — spatial mobile queries.
- Projects/Server/Maps/Map.ItemEnumerator.cs — spatial item queries.
- Projects/Server/Maps/Map.ClientEnumerator.cs — spatial client queries.
- Projects/Server/Mobiles/Mobile.cs — base class documented by repository as representing players, NPCs and creatures.
- Projects/Server/Items/Item.cs — item entity/serialization surface.
- dev-docs/networking-packets.md — ModernUO binary packet framework documentation.
- CLAUDE.md at observed commit — repository engineering guidance states game-state logic is kept on the main loop and recommends spatial queries instead of whole-world mobile/item iteration.

## Structural observations usable for DOLZORE design research

1. World state is modeled independently from a rendering scene.
2. Players/NPCs/creatures share a common persistent entity family in the emulator model.
3. Items are persistent entities with serialization behavior rather than only client-side visual objects.
4. Maps expose localized/spatial queries instead of requiring global iteration for nearby interactions.
5. Network session/state and movement handling are explicit server domains.
6. World save/archive behavior is an explicit operational domain.
7. Content/game systems are separated from lower-level server/world/network code in repository layout.

These are implementation-reference observations only.
DOLZORE must implement original architecture appropriate to Unity/Web/server stack and its own scale.

## ServUO repository
Repository: https://github.com/ServUO/ServUO
Observed default branch: pub57
Use: independent emulator reference to cross-check architectural concepts.
Do not copy code into DOLZORE unless license compatibility and user direction are separately reviewed.

## DOLZORE safety rule
Never write statements such as 'retail UO uses ModernUO architecture'.
Allowed statement: 'ModernUO is an open-source UO emulator that demonstrates one workable implementation pattern for a persistent UO-like world.'

OPEN_SOURCE_SERVER_REFERENCE_V1=true
