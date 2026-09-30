using UnityEngine;

namespace Dolzore
{
    public sealed class DolzoreAvatarRenderer : MonoBehaviour
    {
        [SerializeField] private AvatarAppearanceData appearance = new AvatarAppearanceData();
        [SerializeField] private AvatarFacing facing = AvatarFacing.Down;
        [SerializeField] private int frame;
        [SerializeField] private bool facingLeft;
        [SerializeField] private float walkFrameSeconds = 0.18f;

        private SpriteRenderer shadow;
        private SpriteRenderer bodyOutline;
        private SpriteRenderer body;
        private SpriteRenderer face;
        private SpriteRenderer bottomOutline;
        private SpriteRenderer bottom;
        private SpriteRenderer topOutline;
        private SpriteRenderer top;
        private SpriteRenderer topAccentOutline;
        private SpriteRenderer topAccent;
        private SpriteRenderer shoesOutline;
        private SpriteRenderer shoes;
        private SpriteRenderer hairOutline;
        private SpriteRenderer hair;
        private SpriteRenderer accessoryOutline;
        private SpriteRenderer accessory;
        private Rigidbody2D rigidbody2d;
        private float walkClock;

        public AvatarAppearanceData Appearance => appearance;

        private void Awake()
        {
            EnsureLayers();
            rigidbody2d = GetComponent<Rigidbody2D>();
            Refresh();
        }

        private void Update()
        {
            if (rigidbody2d == null) return;

            Vector2 velocity = rigidbody2d.linearVelocity;
            bool moving = velocity.sqrMagnitude > 0.02f;

            if (moving)
            {
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                {
                    facing = AvatarFacing.Side;
                    facingLeft = velocity.x < 0f;
                }
                else
                {
                    facing = velocity.y >= 0f ? AvatarFacing.Up : AvatarFacing.Down;
                    facingLeft = false;
                }

                walkClock += Time.deltaTime;
                int nextFrame = Mathf.FloorToInt(walkClock / walkFrameSeconds) & 1;
                if (nextFrame != frame)
                {
                    frame = nextFrame;
                    Refresh();
                }
                else
                {
                    ApplyFlip();
                }
            }
            else
            {
                walkClock = 0f;
                if (frame != 0)
                {
                    frame = 0;
                    Refresh();
                }
            }
        }

        private void LateUpdate()
        {
            int worldOrder = 120 - Mathf.RoundToInt(transform.position.y * 2f);
            SetOrder(shadow, worldOrder - 2);
            SetOrder(bodyOutline, worldOrder - 1);
            SetOrder(body, worldOrder);
            SetOrder(bottomOutline, worldOrder);
            SetOrder(bottom, worldOrder + 1);
            SetOrder(topOutline, worldOrder + 1);
            SetOrder(top, worldOrder + 2);
            SetOrder(topAccentOutline, worldOrder + 2);
            SetOrder(topAccent, worldOrder + 3);
            SetOrder(shoesOutline, worldOrder + 3);
            SetOrder(shoes, worldOrder + 4);
            SetOrder(face, worldOrder + 5);
            SetOrder(hairOutline, worldOrder + 5);
            SetOrder(hair, worldOrder + 6);
            SetOrder(accessoryOutline, worldOrder + 6);
            SetOrder(accessory, worldOrder + 7);
        }

        public void Configure(AvatarAppearanceData profile)
        {
            appearance = profile ?? new AvatarAppearanceData();
            EnsureLayers();
            Refresh();
        }

        public void SetFacing(AvatarFacing direction, bool left = false, int walkFrame = 0)
        {
            facing = direction;
            facingLeft = left;
            frame = Mathf.Clamp(walkFrame, 0, 1);
            Refresh();
        }

        private void EnsureLayers()
        {
            shadow = Layer("Avatar Shadow");
            bodyOutline = Layer("Avatar Body Outline");
            body = Layer("Avatar Body");
            bottomOutline = Layer("Avatar Bottom Outline");
            bottom = Layer("Avatar Bottom");
            topOutline = Layer("Avatar Top Outline");
            top = Layer("Avatar Top");
            topAccentOutline = Layer("Avatar Top Accent Outline");
            topAccent = Layer("Avatar Top Accent");
            shoesOutline = Layer("Avatar Shoes Outline");
            shoes = Layer("Avatar Shoes");
            face = Layer("Avatar Face");
            hairOutline = Layer("Avatar Hair Outline");
            hair = Layer("Avatar Hair");
            accessoryOutline = Layer("Avatar Accessory Outline");
            accessory = Layer("Avatar Accessory");

            if (shadow != null)
            {
                shadow.color = new Color(1f, 1f, 1f, 0.70f);
                shadow.transform.localPosition = new Vector3(0f, -0.05f, 0f);
            }
        }

        private SpriteRenderer Layer(string name)
        {
            Transform existing = transform.Find(name);
            GameObject go;
            if (existing != null) go = existing.gameObject;
            else
            {
                go = new GameObject(name);
                go.transform.SetParent(transform, false);
            }

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) sr = go.AddComponent<SpriteRenderer>();
            return sr;
        }

        private void Refresh()
        {
            EnsureLayers();
            string dir = facing == AvatarFacing.Up ? "up" : facing == AvatarFacing.Side ? "side" : "down";
            string suffix = dir + "_" + frame;

            shadow.sprite = Load("shadow_" + suffix);
            bodyOutline.sprite = Load("outline_body_" + suffix);
            body.sprite = Load("body_" + suffix);
            face.sprite = Load("face_" + suffix);

            string hairName = "hair_" + appearance.hairStyle.ToString().ToLowerInvariant() + "_" + suffix;
            hairOutline.sprite = Load("outline_" + hairName);
            hair.sprite = Load(hairName);

            string topName = "top_" + appearance.topStyle.ToString().ToLowerInvariant() + "_" + suffix;
            topOutline.sprite = Load("outline_" + topName);
            top.sprite = Load(topName);

            string accentName = "topaccent_" + appearance.topStyle.ToString().ToLowerInvariant() + "_" + suffix;
            topAccentOutline.sprite = Load("outline_" + accentName);
            topAccent.sprite = Load(accentName);

            string bottomName = "bottom_" + appearance.bottomStyle.ToString().ToLowerInvariant() + "_" + suffix;
            bottomOutline.sprite = Load("outline_" + bottomName);
            bottom.sprite = Load(bottomName);

            string shoesName = "shoes_" + appearance.shoeStyle.ToString().ToLowerInvariant() + "_" + suffix;
            shoesOutline.sprite = Load("outline_" + shoesName);
            shoes.sprite = Load(shoesName);

            if (appearance.accessoryStyle == AvatarAccessoryStyle.None)
            {
                accessory.sprite = null;
                accessoryOutline.sprite = null;
            }
            else
            {
                string accessoryName = "accessory_" + appearance.accessoryStyle.ToString().ToLowerInvariant() + "_" + suffix;
                accessoryOutline.sprite = Load("outline_" + accessoryName);
                accessory.sprite = Load(accessoryName);
            }

            Color outlineColor = AvatarProfileLibrary.Hex("#28264A");
            bodyOutline.color = outlineColor;
            hairOutline.color = outlineColor;
            topOutline.color = outlineColor;
            topAccentOutline.color = outlineColor;
            bottomOutline.color = outlineColor;
            shoesOutline.color = outlineColor;
            accessoryOutline.color = outlineColor;

            body.color = appearance.skinColor;
            face.color = new Color(0.10f, 0.08f, 0.18f, 1f);
            hair.color = appearance.hairColor;
            top.color = appearance.topColor;
            topAccent.color = appearance.topAccentColor;
            bottom.color = appearance.bottomColor;
            shoes.color = appearance.shoeColor;
            accessory.color = appearance.accessoryColor;

            ApplyFlip();
        }

        private void ApplyFlip()
        {
            bool flip = facing == AvatarFacing.Side && facingLeft;
            if (shadow != null) shadow.flipX = flip;
            if (bodyOutline != null) bodyOutline.flipX = flip;
            if (body != null) body.flipX = flip;
            if (face != null) face.flipX = flip;
            if (hairOutline != null) hairOutline.flipX = flip;
            if (hair != null) hair.flipX = flip;
            if (topOutline != null) topOutline.flipX = flip;
            if (top != null) top.flipX = flip;
            if (topAccentOutline != null) topAccentOutline.flipX = flip;
            if (topAccent != null) topAccent.flipX = flip;
            if (bottomOutline != null) bottomOutline.flipX = flip;
            if (bottom != null) bottom.flipX = flip;
            if (shoesOutline != null) shoesOutline.flipX = flip;
            if (shoes != null) shoes.flipX = flip;
            if (accessoryOutline != null) accessoryOutline.flipX = flip;
            if (accessory != null) accessory.flipX = flip;
        }

        private static Sprite Load(string name)
        {
            return Resources.Load<Sprite>("Avatars/V1/" + name);
        }

        private static void SetOrder(SpriteRenderer sr, int order)
        {
            if (sr != null) sr.sortingOrder = order;
        }
    }
}
