using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dolzore.Editor
{
    public static class FirstTownVisualPlacementVerifier
    {
        private static readonly string[] PrimaryBuildings =
        {
            "BAR 13",
            "CAFE LUMA",
            "JOURNAL",
            "CIVIC CLOCK",
            "HOME / RESIDENTIAL",
            "HOUSE / EAST",
            "MARKET HALL",
            "WORKSHOP",
            "BACK ALLEY DEPOT",
            "RIVERSIDE KIOSK",
            "HOUSE / NORTH",
            "STATION"
        };

        public static void AssertCurrentScene()
        {
            GameObject roadsGo = GameObject.Find("Roads");
            if (roadsGo == null)
                throw new InvalidOperationException("DOLZORE_V7_ROADS_MISSING");

            Tilemap roads = roadsGo.GetComponent<Tilemap>();
            if (roads == null)
                throw new InvalidOperationException("DOLZORE_V7_ROAD_TILEMAP_MISSING");

            List<BuildingInfo> buildings = new List<BuildingInfo>();
            for (int i = 0; i < PrimaryBuildings.Length; i++)
            {
                GameObject go = GameObject.Find(PrimaryBuildings[i]);
                if (go == null)
                    throw new InvalidOperationException("DOLZORE_V7_PRIMARY_BUILDING_MISSING:" + PrimaryBuildings[i]);

                Collider2D collider = go.GetComponent<Collider2D>();
                SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
                if (collider == null || renderer == null)
                    throw new InvalidOperationException("DOLZORE_V7_BUILDING_COMPONENT_MISSING:" + PrimaryBuildings[i]);

                Bounds cb = collider.bounds;
                Bounds rb = renderer.bounds;

                if (rb.min.x < -24.5f || rb.max.x > 24.5f || rb.min.y < -17.5f || rb.max.y > 18.5f)
                    throw new InvalidOperationException(
                        "DOLZORE_V7_BUILDING_OUT_OF_WORLD:" + PrimaryBuildings[i] +
                        ":bounds=" + Format(rb));

                AssertColliderOffRoad(roads, PrimaryBuildings[i], cb);

                buildings.Add(new BuildingInfo
                {
                    name = PrimaryBuildings[i],
                    colliderBounds = cb,
                    renderBounds = rb
                });
            }

            for (int i = 0; i < buildings.Count; i++)
            {
                for (int j = i + 1; j < buildings.Count; j++)
                {
                    float colliderOverlapX = Overlap(
                        buildings[i].colliderBounds.min.x, buildings[i].colliderBounds.max.x,
                        buildings[j].colliderBounds.min.x, buildings[j].colliderBounds.max.x);
                    float colliderOverlapY = Overlap(
                        buildings[i].colliderBounds.min.y, buildings[i].colliderBounds.max.y,
                        buildings[j].colliderBounds.min.y, buildings[j].colliderBounds.max.y);

                    if (colliderOverlapX > 0.08f && colliderOverlapY > 0.08f)
                        throw new InvalidOperationException(
                            "DOLZORE_V7_BUILDING_COLLIDER_OVERLAP:" +
                            buildings[i].name + ":" + buildings[j].name +
                            ":x=" + colliderOverlapX.ToString("F2") +
                            ":y=" + colliderOverlapY.ToString("F2"));

                    float renderOverlapX = Overlap(
                        buildings[i].renderBounds.min.x, buildings[i].renderBounds.max.x,
                        buildings[j].renderBounds.min.x, buildings[j].renderBounds.max.x);
                    float renderOverlapY = Overlap(
                        buildings[i].renderBounds.min.y, buildings[i].renderBounds.max.y,
                        buildings[j].renderBounds.min.y, buildings[j].renderBounds.max.y);

                    // A tiny amount of roof-edge overlap is tolerable; obvious card stacking is not.
                    if (renderOverlapX > 0.45f && renderOverlapY > 0.45f)
                        throw new InvalidOperationException(
                            "DOLZORE_V7_BUILDING_VISUAL_OVERLAP:" +
                            buildings[i].name + ":" + buildings[j].name +
                            ":x=" + renderOverlapX.ToString("F2") +
                            ":y=" + renderOverlapY.ToString("F2"));
                }
            }

            AssertNoPeopleInsideBuildings(buildings);

            Debug.Log("DOLZORE_V7_PLACEMENT_ACCEPTANCE=PASS buildings=" + buildings.Count);
        }

        private static void AssertColliderOffRoad(Tilemap roads, string name, Bounds buildingBounds)
        {
            BoundsInt bounds = roads.cellBounds;
            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                if (!roads.HasTile(cell)) continue;

                Vector3 center = roads.GetCellCenterWorld(cell);
                if (buildingBounds.Contains(new Vector3(center.x, center.y, buildingBounds.center.z)))
                    throw new InvalidOperationException(
                        "DOLZORE_V7_BUILDING_ON_ROAD:" + name + ":cell=" + cell);
            }
        }

        private static void AssertNoPeopleInsideBuildings(List<BuildingInfo> buildings)
        {
            string[] people =
            {
                "Player SORA","MELO","YUZU","PON","NAMI","GARU","MORI","REI","HANA"
            };

            for (int p = 0; p < people.Length; p++)
            {
                GameObject go = GameObject.Find(people[p]);
                if (go == null) continue;

                Vector2 point = go.transform.position;
                for (int b = 0; b < buildings.Count; b++)
                {
                    Bounds bounds = buildings[b].colliderBounds;
                    if (point.x > bounds.min.x && point.x < bounds.max.x &&
                        point.y > bounds.min.y && point.y < bounds.max.y)
                        throw new InvalidOperationException(
                            "DOLZORE_V7_PERSON_INSIDE_BUILDING:" + people[p] + ":" + buildings[b].name);
                }
            }
        }

        private static float Overlap(float aMin, float aMax, float bMin, float bMax)
        {
            return Mathf.Max(0f, Mathf.Min(aMax, bMax) - Mathf.Max(aMin, bMin));
        }

        private static string Format(Bounds b)
        {
            return "[" +
                b.min.x.ToString("F2") + "," + b.min.y.ToString("F2") + "]-[" +
                b.max.x.ToString("F2") + "," + b.max.y.ToString("F2") + "]";
        }

        private sealed class BuildingInfo
        {
            public string name;
            public Bounds colliderBounds;
            public Bounds renderBounds;
        }
    }
}
