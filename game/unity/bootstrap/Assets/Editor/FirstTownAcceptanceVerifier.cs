using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Dolzore.Editor
{
    public static class FirstTownAcceptanceVerifier
    {
        private const float Step = 0.5f;
        private const float MinX = -23.5f;
        private const float MaxX = 23.5f;
        private const float MinY = -16.5f;
        private const float MaxY = 16.5f;
        private const float InteractionRadius = 2.15f;

        private static readonly Vector2 PlayerHalfSize = new Vector2(0.34f, 0.46f);
        private const float PlayerColliderOffsetY = -0.26f;

        public static void AssertCurrentScene()
        {
            GameObject player = GameObject.Find("Player SORA");
            if (player == null)
                throw new InvalidOperationException("DOLZORE_ACCEPTANCE_PLAYER_MISSING");

            Physics2D.SyncTransforms();

            Collider2D[] colliders = UnityEngine.Object.FindObjectsOfType<Collider2D>();
            List<ExpandedObstacle> obstacles = BuildObstacles(colliders, player);
            string topologySha = ComputeTopologySha(colliders, player);

            Dictionary<string, string> targets = new Dictionary<string, string>
            {
                { "BAR", DolzoreIds.FirstTownBar },
                { "JOURNAL", DolzoreIds.FirstTownJournal },
                { "RIVERSIDE", DolzoreIds.FirstTownRiverside },
                { "STATION", DolzoreIds.FirstTownStation }
            };

            InteractionAnchor[] anchors = UnityEngine.Object.FindObjectsOfType<InteractionAnchor>();
            Dictionary<string, Vector2> anchorPositions = new Dictionary<string, Vector2>();
            foreach (InteractionAnchor anchor in anchors)
            {
                if (anchor == null || string.IsNullOrEmpty(anchor.TargetEntityId))
                    continue;
                if (anchorPositions.ContainsKey(anchor.TargetEntityId))
                    throw new InvalidOperationException("DOLZORE_ACCEPTANCE_DUPLICATE_ANCHOR:" + anchor.TargetEntityId);
                anchorPositions.Add(anchor.TargetEntityId, anchor.transform.position);
            }

            Vector2 start = player.transform.position;
            Dictionary<string, int> routeSteps = new Dictionary<string, int>();
            foreach (KeyValuePair<string, string> target in targets)
            {
                Vector2 goal;
                if (!anchorPositions.TryGetValue(target.Value, out goal))
                    throw new InvalidOperationException("DOLZORE_ACCEPTANCE_ANCHOR_MISSING:" + target.Key);

                int steps = FindRoute(start, goal, obstacles);
                if (steps < 0)
                    throw new InvalidOperationException("DOLZORE_ROUTE_" + target.Key + "=FAIL");

                routeSteps[target.Key] = steps;
                Debug.Log("DOLZORE_ROUTE_" + target.Key + "=PASS steps=" + steps);
            }

            AssertUiSurface();

            string receipt = BuildReceipt(topologySha, colliders.Length, obstacles.Count, start, anchorPositions, targets, routeSteps);
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string artifactDir = Path.Combine(projectRoot, "BuildArtifacts");
            Directory.CreateDirectory(artifactDir);
            string receiptPath = Path.Combine(artifactDir, "first-town-acceptance.json");
            File.WriteAllText(receiptPath, receipt, new UTF8Encoding(false));

            Debug.Log("DOLZORE_COLLIDER_TOPOLOGY_SHA256=" + topologySha);
            Debug.Log("DOLZORE_ACCEPTANCE_RECEIPT=" + receiptPath);
            Debug.Log("DOLZORE_ROUTE_ACCEPTANCE_SUCCESS");
        }

        private static void AssertUiSurface()
        {
            Canvas canvas = UnityEngine.Object.FindObjectsOfType<Canvas>().FirstOrDefault(c => c.gameObject.name == "HUD Canvas");
            if (canvas == null)
                throw new InvalidOperationException("DOLZORE_HUD_CANVAS_MISSING");

            string[] requiredNames =
            {
                "Zone Header",
                "Player HUD",
                "Minimap Panel",
                "Interaction Prompt",
                "World Clock"
            };

            foreach (string name in requiredNames)
            {
                Transform found = FindDescendant(canvas.transform, name);
                if (found == null)
                    throw new InvalidOperationException("DOLZORE_HUD_ELEMENT_MISSING:" + name);
                Debug.Log("DOLZORE_HUD_ELEMENT_" + name.Replace(" ", "_").ToUpperInvariant() + "=PASS");
            }

            FirstTownRuntime runtime = UnityEngine.Object.FindObjectOfType<FirstTownRuntime>();
            if (runtime == null ||
                runtime.playerNameLabel == null ||
                runtime.metaLabel == null ||
                runtime.districtLabel == null ||
                runtime.statusTargetLabel == null ||
                runtime.interactionLabel == null ||
                runtime.minimapMarker == null ||
                runtime.minimapRect == null)
                throw new InvalidOperationException("DOLZORE_HUD_RUNTIME_BINDING_MISSING");

            Debug.Log("DOLZORE_HUD_RUNTIME_BINDINGS=PASS");
        }

        private static Transform FindDescendant(Transform root, string name)
        {
            foreach (Transform child in root)
            {
                if (child.name == name) return child;
                Transform nested = FindDescendant(child, name);
                if (nested != null) return nested;
            }
            return null;
        }

        private static List<ExpandedObstacle> BuildObstacles(Collider2D[] colliders, GameObject player)
        {
            List<ExpandedObstacle> result = new List<ExpandedObstacle>();
            foreach (Collider2D collider in colliders)
            {
                if (collider == null || !collider.enabled || collider.isTrigger)
                    continue;
                if (collider.gameObject == player)
                    continue;

                Bounds b = collider.bounds;
                result.Add(new ExpandedObstacle
                {
                    name = collider.gameObject.name,
                    minX = b.min.x - PlayerHalfSize.x,
                    maxX = b.max.x + PlayerHalfSize.x,
                    minY = b.min.y - PlayerHalfSize.y - PlayerColliderOffsetY,
                    maxY = b.max.y + PlayerHalfSize.y - PlayerColliderOffsetY
                });
            }
            return result;
        }

        private static int FindRoute(Vector2 start, Vector2 goal, List<ExpandedObstacle> obstacles)
        {
            int width = Mathf.RoundToInt((MaxX - MinX) / Step) + 1;
            int height = Mathf.RoundToInt((MaxY - MinY) / Step) + 1;
            int startX = Mathf.Clamp(Mathf.RoundToInt((start.x - MinX) / Step), 0, width - 1);
            int startY = Mathf.Clamp(Mathf.RoundToInt((start.y - MinY) / Step), 0, height - 1);

            if (Blocked(Point(startX, startY), obstacles))
                throw new InvalidOperationException("DOLZORE_ROUTE_START_BLOCKED");

            bool[,] seen = new bool[width, height];
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(new Node(startX, startY, 0));
            seen[startX, startY] = true;

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                Vector2 p = Point(node.x, node.y);
                if ((p - goal).sqrMagnitude <= InteractionRadius * InteractionRadius)
                    return node.steps;

                for (int d = 0; d < 4; d++)
                {
                    int nx = node.x + dx[d];
                    int ny = node.y + dy[d];
                    if (nx < 0 || nx >= width || ny < 0 || ny >= height || seen[nx, ny])
                        continue;

                    Vector2 np = Point(nx, ny);
                    if (Blocked(np, obstacles))
                        continue;

                    seen[nx, ny] = true;
                    queue.Enqueue(new Node(nx, ny, node.steps + 1));
                }
            }

            return -1;
        }

        private static Vector2 Point(int x, int y)
        {
            return new Vector2(MinX + x * Step, MinY + y * Step);
        }

        private static bool Blocked(Vector2 p, List<ExpandedObstacle> obstacles)
        {
            foreach (ExpandedObstacle obstacle in obstacles)
            {
                if (p.x > obstacle.minX && p.x < obstacle.maxX &&
                    p.y > obstacle.minY && p.y < obstacle.maxY)
                    return true;
            }
            return false;
        }

        private static string ComputeTopologySha(Collider2D[] colliders, GameObject player)
        {
            List<string> rows = new List<string>();
            foreach (Collider2D collider in colliders)
            {
                if (collider == null || collider.gameObject == player)
                    continue;
                Bounds b = collider.bounds;
                rows.Add(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}|{1}|{2:F3},{3:F3}|{4:F3},{5:F3}",
                    collider.gameObject.name,
                    collider.isTrigger ? "T" : "S",
                    b.min.x, b.min.y, b.max.x, b.max.y));
            }
            rows.Sort(StringComparer.Ordinal);
            string payload = string.Join("\n", rows);
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(payload));
                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash) sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }

        private static string BuildReceipt(
            string topologySha,
            int colliderCount,
            int obstacleCount,
            Vector2 start,
            Dictionary<string, Vector2> anchorPositions,
            Dictionary<string, string> targets,
            Dictionary<string, int> routeSteps)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            sb.Append("  \"schema\": \"dolzore.first_town.acceptance.v1\",\n");
            sb.Append("  \"collider_topology_sha256\": \"").Append(topologySha).Append("\",\n");
            sb.Append("  \"collider_count\": ").Append(colliderCount).Append(",\n");
            sb.Append("  \"static_obstacle_count\": ").Append(obstacleCount).Append(",\n");
            sb.AppendFormat(CultureInfo.InvariantCulture, "  \"start\": [ {0:F2}, {1:F2} ],\n", start.x, start.y);
            sb.Append("  \"mandatory_routes\": {\n");

            int index = 0;
            foreach (KeyValuePair<string, string> target in targets)
            {
                Vector2 goal = anchorPositions[target.Value];
                sb.Append("    \"").Append(target.Key).Append("\": { ");
                sb.Append("\"entity_id\": \"").Append(target.Value).Append("\", ");
                sb.AppendFormat(CultureInfo.InvariantCulture, "\"interaction_anchor\": [ {0:F2}, {1:F2} ], ", goal.x, goal.y);
                sb.Append("\"grid_step\": ").Append(Step.ToString("F2", CultureInfo.InvariantCulture)).Append(", ");
                sb.Append("\"steps\": ").Append(routeSteps[target.Key]).Append(", \"result\": \"PASS\" }");
                index++;
                sb.Append(index < targets.Count ? ",\n" : "\n");
            }

            sb.Append("  },\n");
            sb.Append("  \"hud_runtime_bindings\": \"PASS\",\n");
            sb.Append("  \"result\": \"PASS\"\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        private sealed class ExpandedObstacle
        {
            public string name;
            public float minX;
            public float maxX;
            public float minY;
            public float maxY;
        }

        private readonly struct Node
        {
            public readonly int x;
            public readonly int y;
            public readonly int steps;

            public Node(int x, int y, int steps)
            {
                this.x = x;
                this.y = y;
                this.steps = steps;
            }
        }
    }
}
