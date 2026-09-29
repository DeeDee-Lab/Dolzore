using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dolzore.Editor
{
    public static class FirstTownVisualEnhancer
    {
        private const string ArtRoot = "Assets/Art/Generated/TownV2";

        private static readonly Color Cream = Hex("#F4E6BE");
        private static readonly Color Ink = Hex("#182039");
        private static readonly Color RoadInk = Hex("#2F3440");
        private static readonly Color Warm = Hex("#E69B55");
        private static readonly Color Aqua = Hex("#6BC6C3");

        public static void Apply()
        {
            Directory.CreateDirectory(Abs(ArtRoot));
            AssetDatabase.Refresh();

            GenerateBuildings();
            GenerateCharacters();
            GenerateProps();
            ReplaceBuildings();
            ReplacePlayer();
            HideWorldLabels();
            AddResidents();
            AddTownLife();
            AddLandmarks();
            AddStreetDetails();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerateBuildings()
        {
            SaveResidence("home.png", "#42617B", "#6F8FA6", "#E9D8A5", true);
            SaveResidence("house_north.png", "#745548", "#BEA177", "#F0D9A4", false);
            SaveResidence("house_east.png", "#3E644F", "#6F9270", "#E7D49A", false);

            SaveCafe("cafe.png");
            SaveBar("bar.png");
            SaveMarket("market.png");
            SaveJournal("journal.png");
            SaveCivic("civic.png");
            SaveWorkshop("workshop.png");
            SaveKiosk("kiosk.png");
            SaveStation("station.png");
            SaveDepot("depot.png");
        }

        private static void GenerateCharacters()
        {
            SaveCharacter("sora.png", "#273A4F", "#E4B68C", "#4F8587", "#F3A65A", 0);
            SaveCharacter("melo.png", "#7A453B", "#E1AE8A", "#8B4354", "#E2A34D", 1);
            SaveCharacter("yuzu.png", "#2A2B37", "#E2B28B", "#B18D45", "#65733F", 2);
            SaveCharacter("pon.png", "#4D657A", "#DFAF87", "#315477", "#F08C42", 3);
            SaveCharacter("resident_child.png", "#543D37", "#E5B98F", "#5B79A6", "#F2D17C", 4);
            SaveCharacter("resident_worker.png", "#383B40", "#D6A77F", "#6D5F55", "#D2A754", 5);
            SaveCharacter("resident_commuter.png", "#4A3E55", "#E0B28D", "#5B6D7D", "#8CC5BA", 6);
        }

        private static void GenerateProps()
        {
            SaveBench();
            SaveVending();
            SaveMailbox();
            SaveHydrant();
            SaveTrash();
            SaveBicycle();
            SaveCar();
            SaveUtilityPole();
            SaveFlowers();
            SaveCat();
            SaveWaterTower();
            SaveCrosswalk();
            SaveManhole();
            SaveRoadSign();
        }

        private static void ReplaceBuildings()
        {
            ReplaceSprite("HOME / RESIDENTIAL", "home.png");
            ReplaceSprite("HOUSE / NORTH", "house_north.png");
            ReplaceSprite("CAFE LUMA", "cafe.png");
            ReplaceSprite("HOUSE / EAST", "house_east.png");
            ReplaceSprite("BAR 13", "bar.png");
            ReplaceSprite("MARKET HALL", "market.png");
            ReplaceSprite("JOURNAL", "journal.png");
            ReplaceSprite("CIVIC CLOCK", "civic.png");
            ReplaceSprite("WORKSHOP", "workshop.png");
            ReplaceSprite("RIVERSIDE KIOSK", "kiosk.png");
            ReplaceSprite("STATION", "station.png");
            ReplaceSprite("BACK ALLEY DEPOT", "depot.png");
        }

        private static void ReplacePlayer()
        {
            GameObject player = GameObject.Find("Player SORA");
            if (player == null) return;
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = Sprite("sora.png");
        }

        private static void HideWorldLabels()
        {
            TextMesh[] labels = UnityEngine.Object.FindObjectsByType<TextMesh>(FindObjectsSortMode.None);
            for (int i = 0; i < labels.Length; i++)
                labels[i].gameObject.SetActive(false);
        }

        private static void AddResidents()
        {
            AddResident("MELO", "melo.png", new Vector2(-10.7f, 3.3f), "entity.npc.melo");
            AddResident("YUZU", "yuzu.png", new Vector2(7.4f, 3.4f), "entity.npc.yuzu");
            AddResident("PON", "pon.png", new Vector2(4.1f, -0.8f), "entity.npc.pon");
            AddResident("NAMI", "resident_child.png", new Vector2(-4.8f, 9.1f), "entity.npc.nami");
            AddResident("GARU", "resident_worker.png", new Vector2(-11.8f, -0.4f), "entity.npc.garu");
            AddResident("REI", "resident_commuter.png", new Vector2(9.3f, -10.2f), "entity.npc.rei");
        }

        private static void AddTownLife()
        {
            AddProp("Bench North", "bench.png", new Vector2(4.8f, 9.5f), true);
            AddProp("Bench Riverside", "bench.png", new Vector2(7.4f, -1.3f), true);
            AddProp("Vending Machine", "vending.png", new Vector2(-6.4f, -0.4f), true);
            AddProp("Mailbox", "mailbox.png", new Vector2(-12.2f, 9.0f), true);
            AddProp("Hydrant", "hydrant.png", new Vector2(5.7f, 3.0f), false);
            AddProp("Trash Bin", "trash.png", new Vector2(-5.8f, 3.1f), true);
            AddProp("Bicycle", "bicycle.png", new Vector2(11.7f, 8.4f), false);
            AddProp("Parked Car", "car.png", new Vector2(-5.4f, 6.9f), true);
            AddProp("Utility Pole A", "utility_pole.png", new Vector2(-19.3f, 8.4f), true);
            AddProp("Utility Pole B", "utility_pole.png", new Vector2(18.9f, 2.2f), true);
            AddProp("Flowers A", "flowers.png", new Vector2(-11.0f, 14.7f), false);
            AddProp("Flowers B", "flowers.png", new Vector2(6.2f, 14.2f), false);
            AddProp("Flowers C", "flowers.png", new Vector2(8.7f, -7.8f), false);
            AddProp("Odd Cat", "cat.png", new Vector2(-8.0f, -8.5f), false);
        }

        private static void AddLandmarks()
        {
            AddProp("Residential Water Tower", "water_tower.png", new Vector2(-20.5f, 12.1f), true);

            GameObject largeTree = GameObject.Find("Tree 10");
            if (largeTree != null)
            {
                largeTree.name = "Riverside Landmark Tree";
                largeTree.transform.position = new Vector3(8.6f, -7.8f, 0f);
                largeTree.transform.localScale = new Vector3(1.65f, 1.65f, 1f);
            }

            AddProp("Station Road Sign", "road_sign.png", new Vector2(9.4f, -11.2f), false);
            AddProp("Main Street Road Sign", "road_sign.png", new Vector2(5.2f, 7.4f), false);
        }

        private static void AddStreetDetails()
        {
            AddProp("Crosswalk North", "crosswalk.png", new Vector2(0.5f, 8.7f), false, 5);
            AddProp("Crosswalk Central", "crosswalk.png", new Vector2(0.5f, 2.8f), false, 5);
            AddProp("Crosswalk South", "crosswalk.png", new Vector2(0.5f, -10.7f), false, 5);
            AddProp("Manhole Main", "manhole.png", new Vector2(-1.1f, 5.0f), false, 6);
            AddProp("Manhole South", "manhole.png", new Vector2(1.1f, -9.7f), false, 6);
        }

        private static void AddResident(string name, string file, Vector2 position, string entityId)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite(file);
            sr.sortingOrder = 80 - Mathf.RoundToInt(position.y);

            CapsuleCollider2D capsule = go.AddComponent<CapsuleCollider2D>();
            capsule.size = new Vector2(0.70f, 0.90f);
            capsule.offset = new Vector2(0f, -0.25f);

            EntityIdentity identity = go.AddComponent<EntityIdentity>();
            identity.Configure(entityId, DolzoreIds.FirstTownRegion, "npc.resident");
        }

        private static void AddProp(string name, string file, Vector2 position, bool collider, int sortingOrder = -1)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite(file);
            sr.sortingOrder = sortingOrder >= 0 ? sortingOrder : 38 - Mathf.RoundToInt(position.y);

            if (collider)
            {
                BoxCollider2D box = go.AddComponent<BoxCollider2D>();
                box.size = new Vector2(Mathf.Max(0.35f, sr.bounds.size.x * 0.72f), Mathf.Max(0.25f, sr.bounds.size.y * 0.35f));
                box.offset = new Vector2(0f, -sr.bounds.size.y * 0.25f);
            }
        }

        private static void ReplaceSprite(string objectName, string file)
        {
            GameObject go = GameObject.Find(objectName);
            if (go == null) return;
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = Sprite(file);
        }

        private static Sprite Sprite(string file)
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/" + file);
        }

        private static void SaveResidence(string file, string roofHex, string wallHex, string lightHex, bool porch)
        {
            const int w = 72, h = 82;
            Color32[] p = Transparent(w * h);
            Color32 roof = C(roofHex), wall = C(wallHex), light = C(lightHex), dark = C("#273044"), trim = C("#D8C79C");

            Rect(p,w,h,10,10,52,45,wall);
            for (int row=0; row<16; row++)
                Rect(p,w,h,5+row,55+row,62-(row*2),1,roof);
            Rect(p,w,h,7,53,58,4,dark);
            Rect(p,w,h,15,26,12,16,dark);
            Rect(p,w,h,45,26,12,16,dark);
            Rect(p,w,h,18,29,6,9,light);
            Rect(p,w,h,48,29,6,9,light);
            Rect(p,w,h,31,10,11,26,dark);
            Rect(p,w,h,34,13,5,20,roof);
            Rect(p,w,h,12,46,48,3,trim);
            if (porch)
            {
                Rect(p,w,h,25,5,23,5,C("#8D7457"));
                Rect(p,w,h,22,2,29,3,C("#5E5042"));
            }
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveCafe(string file)
        {
            const int w=80,h=76; Color32[] p=Transparent(w*h);
            Rect(p,w,h,8,8,64,48,C("#B66557"));
            Rect(p,w,h,5,56,70,8,C("#693D42"));
            Rect(p,w,h,12,42,56,8,C("#F0D08A"));
            for(int x=12;x<68;x+=8) Rect(p,w,h,x,42,4,8,(x/8)%2==0?C("#E8B05F"):C("#F4E5C3"));
            Rect(p,w,h,13,17,25,22,C("#263246"));
            Rect(p,w,h,17,21,17,14,C("#7FB4B3"));
            Rect(p,w,h,48,8,13,28,C("#263246"));
            Rect(p,w,h,51,11,7,23,C("#E7C38A"));
            Rect(p,w,h,9,5,62,4,C("#4C3540"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveBar(string file)
        {
            const int w=72,h=82; Color32[] p=Transparent(w*h);
            Rect(p,w,h,8,8,56,52,C("#4B3048"));
            Rect(p,w,h,4,60,64,10,C("#221B2A"));
            Rect(p,w,h,12,46,48,9,C("#1A2334"));
            Rect(p,w,h,17,49,38,3,C("#F1A254"));
            Rect(p,w,h,14,21,15,19,C("#20283A"));
            Rect(p,w,h,43,21,15,19,C("#20283A"));
            Rect(p,w,h,18,24,7,11,C("#E3A46A"));
            Rect(p,w,h,47,24,7,11,C("#6CC6C1"));
            Rect(p,w,h,31,8,11,30,C("#171C2B"));
            Rect(p,w,h,34,12,5,23,C("#A2656E"));
            Rect(p,w,h,5,5,62,4,C("#171522"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveMarket(string file)
        {
            const int w=96,h=72; Color32[] p=Transparent(w*h);
            Rect(p,w,h,6,8,84,42,C("#A86D48"));
            Rect(p,w,h,3,50,90,8,C("#5E4536"));
            Rect(p,w,h,8,38,80,10,C("#E9C370"));
            for(int x=8;x<88;x+=10) Rect(p,w,h,x,38,5,10,(x/10)%2==0?C("#8F463F"):C("#E9D59C"));
            Rect(p,w,h,12,14,20,20,C("#26364A"));
            Rect(p,w,h,39,14,18,20,C("#26364A"));
            Rect(p,w,h,65,14,18,20,C("#26364A"));
            Rect(p,w,h,42,8,12,27,C("#49372F"));
            Rect(p,w,h,5,5,86,4,C("#46342D"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveJournal(string file)
        {
            const int w=78,h=88; Color32[] p=Transparent(w*h);
            Rect(p,w,h,8,8,62,55,C("#456F8D"));
            Rect(p,w,h,5,63,68,8,C("#293D54"));
            Rect(p,w,h,13,48,52,6,C("#D9D1B3"));
            Rect(p,w,h,15,22,14,20,C("#213047"));
            Rect(p,w,h,49,22,14,20,C("#213047"));
            Rect(p,w,h,19,26,6,12,C("#86C9C6"));
            Rect(p,w,h,53,26,6,12,C("#86C9C6"));
            Rect(p,w,h,33,8,12,31,C("#1F293B"));
            Rect(p,w,h,36,12,6,24,C("#C7A56D"));
            Rect(p,w,h,10,5,58,4,C("#26354A"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveCivic(string file)
        {
            const int w=70,h=100; Color32[] p=Transparent(w*h);
            Rect(p,w,h,10,8,50,50,C("#70798E"));
            Rect(p,w,h,18,58,34,21,C("#556074"));
            for(int row=0;row<12;row++) Rect(p,w,h,13+row,79+row,44-row*2,1,C("#303949"));
            Circle(p,w,h,35,68,9,C("#F1E0AD"));
            Circle(p,w,h,35,68,6,C("#273044"));
            Rect(p,w,h,34,68,2,5,C("#F1E0AD"));
            Rect(p,w,h,35,67,5,2,C("#F1E0AD"));
            Rect(p,w,h,17,23,10,17,C("#253044"));
            Rect(p,w,h,43,23,10,17,C("#253044"));
            Rect(p,w,h,29,8,12,28,C("#2C3342"));
            Rect(p,w,h,8,5,54,4,C("#353C4A"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveWorkshop(string file)
        {
            const int w=82,h=70; Color32[] p=Transparent(w*h);
            Rect(p,w,h,6,8,70,43,C("#71665F"));
            Rect(p,w,h,3,51,76,8,C("#423E3C"));
            Rect(p,w,h,14,14,38,28,C("#30343B"));
            for(int y=17;y<41;y+=6) Rect(p,w,h,17,y,32,2,C("#5F6568"));
            Rect(p,w,h,59,15,11,25,C("#2B3038"));
            Rect(p,w,h,60,43,10,5,C("#D0A253"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveKiosk(string file)
        {
            const int w=54,h=58; Color32[] p=Transparent(w*h);
            Rect(p,w,h,9,7,36,32,C("#4E8178"));
            Rect(p,w,h,4,39,46,8,C("#2E514D"));
            Rect(p,w,h,6,31,42,7,C("#E0CB88"));
            Rect(p,w,h,13,16,28,13,C("#233943"));
            Rect(p,w,h,17,19,20,7,C("#78B9AF"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveStation(string file)
        {
            const int w=104,h=80; Color32[] p=Transparent(w*h);
            Rect(p,w,h,6,8,92,47,C("#465A76"));
            Rect(p,w,h,2,55,100,8,C("#263449"));
            Rect(p,w,h,10,41,84,8,C("#E0B86C"));
            Rect(p,w,h,13,17,23,20,C("#223149"));
            Rect(p,w,h,68,17,23,20,C("#223149"));
            Rect(p,w,h,42,8,20,33,C("#29384D"));
            Rect(p,w,h,47,13,10,25,C("#9AB7C1"));
            Circle(p,w,h,52,64,8,C("#F2E0AD"));
            Circle(p,w,h,52,64,5,C("#2D3A4B"));
            Rect(p,w,h,51,64,2,4,C("#F2E0AD"));
            Rect(p,w,h,52,63,4,2,C("#F2E0AD"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveDepot(string file)
        {
            const int w=86,h=66; Color32[] p=Transparent(w*h);
            Rect(p,w,h,5,8,76,42,C("#5A4E62"));
            Rect(p,w,h,2,50,82,7,C("#302B39"));
            Rect(p,w,h,12,14,45,28,C("#2F3039"));
            for(int x=15;x<55;x+=8) Rect(p,w,h,x,17,3,22,C("#50515B"));
            Rect(p,w,h,65,16,9,20,C("#292A34"));
            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveCharacter(string file, string hairHex, string skinHex, string clothesHex, string accentHex, int variant)
        {
            const int w=32,h=48; Color32[] p=Transparent(w*h);
            Color32 hair=C(hairHex), skin=C(skinHex), clothes=C(clothesHex), accent=C(accentHex), dark=C("#1D2637"), shoe=C("#E8E0CB");

            Rect(p,w,h,10,35,12,8,hair);
            Rect(p,w,h,8+(variant%3),32,16-(variant%2),5,hair);
            Rect(p,w,h,10,27,12,9,skin);
            Rect(p,w,h,11,33,10,5,hair);
            Rect(p,w,h,12,30,2,2,dark);
            Rect(p,w,h,18,30,2,2,dark);
            Rect(p,w,h,9,15,14,14,clothes);
            Rect(p,w,h,8,23,16,4,accent);
            Rect(p,w,h,6,16,4,11,skin);
            Rect(p,w,h,22,16,4,11,skin);
            Rect(p,w,h,10,5,5,11,dark);
            Rect(p,w,h,17,5,5,11,dark);
            Rect(p,w,h,9,3,7,3,shoe);
            Rect(p,w,h,16,3,7,3,shoe);

            if (variant==1) Rect(p,w,h,22,34,4,4,accent);
            if (variant==2) Rect(p,w,h,23,12,5,11,C("#E8E1C8"));
            if (variant==3) { Rect(p,w,h,23,14,6,13,C("#9B7950")); Rect(p,w,h,7,24,18,3,accent); }
            if (variant==4) Rect(p,w,h,7,12,18,4,accent);
            if (variant==5) Rect(p,w,h,5,26,5,5,C("#D5B266"));
            if (variant==6) Rect(p,w,h,21,12,7,10,C("#6C5A78"));

            SaveSprite(file,w,h,p,16f);
        }

        private static void SaveBench()
        {
            const int w=48,h=24; Color32[] p=Transparent(w*h);
            Rect(p,w,h,5,12,38,5,C("#8C6747")); Rect(p,w,h,7,18,34,4,C("#A37B55"));
            Rect(p,w,h,10,4,4,9,C("#3C4146")); Rect(p,w,h,34,4,4,9,C("#3C4146"));
            SaveSprite("bench.png",w,h,p,16f);
        }

        private static void SaveVending()
        {
            const int w=24,h=48; Color32[] p=Transparent(w*h);
            Rect(p,w,h,3,3,18,42,C("#C35B5F")); Rect(p,w,h,6,27,12,12,C("#E8E3D1"));
            Rect(p,w,h,7,29,10,8,C("#78B8C0")); Rect(p,w,h,7,8,10,14,C("#263247"));
            for(int y=10;y<20;y+=4) for(int x=8;x<16;x+=4) Rect(p,w,h,x,y,2,2,C("#F1C36A"));
            SaveSprite("vending.png",w,h,p,16f);
        }

        private static void SaveMailbox()
        {
            const int w=24,h=34; Color32[] p=Transparent(w*h);
            Rect(p,w,h,6,8,12,17,C("#53759B")); Rect(p,w,h,5,22,14,6,C("#3D5978"));
            Rect(p,w,h,9,3,6,6,C("#3D414A")); Rect(p,w,h,8,14,8,3,C("#E4D6A3"));
            SaveSprite("mailbox.png",w,h,p,16f);
        }

        private static void SaveHydrant()
        {
            const int w=16,h=24; Color32[] p=Transparent(w*h);
            Rect(p,w,h,5,4,6,14,C("#D65E52")); Rect(p,w,h,3,14,10,5,C("#B94642"));
            Rect(p,w,h,2,8,12,4,C("#E47A61")); SaveSprite("hydrant.png",w,h,p,16f);
        }

        private static void SaveTrash()
        {
            const int w=18,h=26; Color32[] p=Transparent(w*h);
            Rect(p,w,h,4,4,10,17,C("#53646A")); Rect(p,w,h,3,21,12,3,C("#36444B"));
            for(int y=7;y<19;y+=5) Rect(p,w,h,6,y,6,2,C("#6E7C7F")); SaveSprite("trash.png",w,h,p,16f);
        }

        private static void SaveBicycle()
        {
            const int w=42,h=24; Color32[] p=Transparent(w*h);
            Ring(p,w,h,9,8,6,C("#2C303B")); Ring(p,w,h,33,8,6,C("#2C303B"));
            Line(p,w,h,9,8,20,15,C("#C1784B")); Line(p,w,h,20,15,33,8,C("#C1784B"));
            Line(p,w,h,9,8,25,8,C("#C1784B")); Line(p,w,h,25,8,20,15,C("#C1784B"));
            Line(p,w,h,20,15,22,20,C("#2C303B")); SaveSprite("bicycle.png",w,h,p,16f);
        }

        private static void SaveCar()
        {
            const int w=64,h=32; Color32[] p=Transparent(w*h);
            Rect(p,w,h,6,7,52,13,C("#5E7897")); Rect(p,w,h,14,20,35,8,C("#405B78"));
            Rect(p,w,h,18,21,12,6,C("#8DB9C0")); Rect(p,w,h,32,21,12,6,C("#8DB9C0"));
            Circle(p,w,h,17,6,5,C("#202633")); Circle(p,w,h,49,6,5,C("#202633"));
            Circle(p,w,h,17,6,2,C("#7C858B")); Circle(p,w,h,49,6,2,C("#7C858B"));
            SaveSprite("car.png",w,h,p,16f);
        }

        private static void SaveUtilityPole()
        {
            const int w=18,h=68; Color32[] p=Transparent(w*h);
            Rect(p,w,h,8,4,3,55,C("#594C3C")); Rect(p,w,h,2,52,15,4,C("#514434"));
            Rect(p,w,h,3,57,3,5,C("#2A2E35")); Rect(p,w,h,13,57,3,5,C("#2A2E35"));
            SaveSprite("utility_pole.png",w,h,p,16f);
        }

        private static void SaveFlowers()
        {
            const int w=32,h=16; Color32[] p=Transparent(w*h);
            for(int x=3;x<30;x+=6) { Rect(p,w,h,x,2,2,8,C("#3D7D4B")); Circle(p,w,h,x+1,11,3,(x%12==3)?C("#F1A1A1"):C("#E8D66D")); }
            SaveSprite("flowers.png",w,h,p,16f);
        }

        private static void SaveCat()
        {
            const int w=24,h=16; Color32[] p=Transparent(w*h);
            Rect(p,w,h,6,4,12,7,C("#D4A66D")); Rect(p,w,h,16,8,5,4,C("#D4A66D"));
            Rect(p,w,h,17,12,2,3,C("#D4A66D")); Rect(p,w,h,20,12,2,3,C("#D4A66D"));
            Rect(p,w,h,3,5,4,3,C("#D4A66D")); Rect(p,w,h,2,7,2,6,C("#D4A66D"));
            Rect(p,w,h,18,10,1,1,C("#263044")); Rect(p,w,h,21,10,1,1,C("#263044"));
            SaveSprite("cat.png",w,h,p,16f);
        }

        private static void SaveWaterTower()
        {
            const int w=48,h=96; Color32[] p=Transparent(w*h);
            Rect(p,w,h,20,4,4,51,C("#4A5058")); Rect(p,w,h,8,4,4,51,C("#4A5058")); Rect(p,w,h,36,4,4,51,C("#4A5058"));
            Line(p,w,h,10,15,38,45,C("#4A5058")); Line(p,w,h,38,15,10,45,C("#4A5058"));
            Rect(p,w,h,8,55,32,22,C("#687A82")); Rect(p,w,h,5,62,38,12,C("#687A82"));
            Rect(p,w,h,11,77,26,4,C("#3D4A52")); Rect(p,w,h,18,82,12,8,C("#52646B"));
            SaveSprite("water_tower.png",w,h,p,16f);
        }

        private static void SaveCrosswalk()
        {
            const int w=64,h=32; Color32[] p=Transparent(w*h);
            for(int x=3;x<61;x+=10) Rect(p,w,h,x,3,6,26,new Color32(224,219,199,185));
            SaveSprite("crosswalk.png",w,h,p,16f);
        }

        private static void SaveManhole()
        {
            const int w=16,h=16; Color32[] p=Transparent(w*h);
            Circle(p,w,h,8,8,6,C("#373C45")); Ring(p,w,h,8,8,5,C("#666B70"));
            Line(p,w,h,4,8,12,8,C("#666B70")); SaveSprite("manhole.png",w,h,p,16f);
        }

        private static void SaveRoadSign()
        {
            const int w=28,h=48; Color32[] p=Transparent(w*h);
            Rect(p,w,h,13,3,3,28,C("#545B60")); Rect(p,w,h,4,28,21,12,C("#426E82"));
            Rect(p,w,h,7,31,15,2,C("#E8D9A8")); Rect(p,w,h,7,35,10,2,C("#E8D9A8"));
            SaveSprite("road_sign.png",w,h,p,16f);
        }

        private static void SaveSprite(string file, int w, int h, Color32[] pixels, float ppu)
        {
            string assetPath=ArtRoot+"/"+file;
            Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);
            tex.SetPixels32(pixels); tex.Apply(false,false);
            File.WriteAllBytes(Abs(assetPath),tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);

            AssetDatabase.ImportAsset(assetPath,ImportAssetOptions.ForceUpdate);
            TextureImporter importer=AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if(importer!=null)
            {
                importer.textureType=TextureImporterType.Sprite;
                importer.spriteImportMode=SpriteImportMode.Single;
                importer.spritePixelsPerUnit=ppu;
                importer.mipmapEnabled=false;
                importer.filterMode=FilterMode.Point;
                importer.textureCompression=TextureImporterCompression.Uncompressed;
                importer.wrapMode=TextureWrapMode.Clamp;
                importer.alphaIsTransparency=true;
                importer.SaveAndReimport();
            }
        }

        private static Color32[] Transparent(int count)
        {
            Color32[] p=new Color32[count];
            for(int i=0;i<count;i++) p[i]=new Color32(0,0,0,0);
            return p;
        }

        private static void Rect(Color32[] p,int w,int h,int x,int y,int rw,int rh,Color32 color)
        {
            int x0=Mathf.Clamp(x,0,w), x1=Mathf.Clamp(x+rw,0,w);
            int y0=Mathf.Clamp(y,0,h), y1=Mathf.Clamp(y+rh,0,h);
            for(int yy=y0;yy<y1;yy++) for(int xx=x0;xx<x1;xx++) p[yy*w+xx]=color;
        }

        private static void Circle(Color32[] p,int w,int h,int cx,int cy,int r,Color32 color)
        {
            int rr=r*r;
            for(int y=-r;y<=r;y++) for(int x=-r;x<=r;x++)
            {
                if(x*x+y*y>rr) continue;
                int px=cx+x, py=cy+y;
                if(px>=0&&px<w&&py>=0&&py<h) p[py*w+px]=color;
            }
        }

        private static void Ring(Color32[] p,int w,int h,int cx,int cy,int r,Color32 color)
        {
            int outer=r*r, inner=(r-2)*(r-2);
            for(int y=-r;y<=r;y++) for(int x=-r;x<=r;x++)
            {
                int d=x*x+y*y; if(d>outer||d<inner) continue;
                int px=cx+x, py=cy+y;
                if(px>=0&&px<w&&py>=0&&py<h) p[py*w+px]=color;
            }
        }

        private static void Line(Color32[] p,int w,int h,int x0,int y0,int x1,int y1,Color32 color)
        {
            int dx=Math.Abs(x1-x0), sx=x0<x1?1:-1;
            int dy=-Math.Abs(y1-y0), sy=y0<y1?1:-1;
            int err=dx+dy;
            while(true)
            {
                if(x0>=0&&x0<w&&y0>=0&&y0<h) p[y0*w+x0]=color;
                if(x0==x1&&y0==y1) break;
                int e2=2*err;
                if(e2>=dy){err+=dy;x0+=sx;}
                if(e2<=dx){err+=dx;y0+=sy;}
            }
        }

        private static string Abs(string relative)
        {
            string root=Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root,relative.Replace('/',Path.DirectorySeparatorChar)));
        }

        private static Color Hex(string hex)
        {
            Color c; if(!ColorUtility.TryParseHtmlString(hex,out c)) throw new ArgumentException(hex); return c;
        }

        private static Color32 C(string hex) { return (Color32)Hex(hex); }
    }
}
