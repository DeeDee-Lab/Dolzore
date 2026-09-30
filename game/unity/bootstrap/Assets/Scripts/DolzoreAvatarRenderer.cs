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
        private SpriteRenderer body;
        private SpriteRenderer face;
        private SpriteRenderer bottom;
        private SpriteRenderer top;
        private SpriteRenderer shoes;
        private SpriteRenderer hair;
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
            SetOrder(body, worldOrder);
            SetOrder(bottom, worldOrder + 1);
            SetOrder(top, worldOrder + 2);
            SetOrder(shoes, worldOrder + 3);
            SetOrder(face, worldOrder + 4);
            SetOrder(hair, worldOrder + 5);
            SetOrder(accessory, worldOrder + 6);
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
            body = Layer("Avatar Body");
            bottom = Layer("Avatar Bottom");
            top = Layer("Avatar Top");
            shoes = Layer("Avatar Shoes");
            face = Layer("Avatar Face");
            hair = Layer("Avatar Hair");
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
            body.sprite = Load("body_" + suffix);
            face.sprite = Load("face_" + suffix);
            hair.sprite = Load("hair_" + appearance.hairStyle.ToString().ToLowerInvariant() + "_" + suffix);
            top.sprite = Load("top_" + appearance.topStyle.ToString().ToLowerInvariant() + "_" + suffix);
            bottom.sprite = Load("bottom_" + appearance.bottomStyle.ToString().ToLowerInvariant() + "_" + suffix);
            shoes.sprite = Load("shoes_" + appearance.shoeStyle.ToString().ToLowerInvariant() + "_" + suffix);

            if (appearance.accessoryStyle == AvatarAccessoryStyle.None)
                accessory.sprite = null;
            else
                accessory.sprite = Load("accessory_" + appearance.accessoryStyle.ToString().ToLowerInvariant() + "_" + suffix);

            body.color = appearance.skinColor;
            face.color = new Color(0.12f, 0.10f, 0.18f, 1f);
            hair.color = appearance.hairColor;
            top.color = appearance.topColor;
            bottom.color = appearance.bottomColor;
            shoes.color = appearance.shoeColor;
            accessory.color = appearance.accessoryColor;

            // Top accent is encoded as a separate tint on the accessory layer only when the profile has no external accessory.
            if (appearance.accessoryStyle == AvatarAccessoryStyle.None && top != null)
                top.color = appearance.topColor;

            ApplyFlip();
        }

        private void ApplyFlip()
        {
            bool flip = facing == AvatarFacing.Side && facingLeft;
            if (shadow != null) shadow.flipX = flip;
            if (body != null) body.flipX = flip;
            if (face != null) face.flipX = flip;
            if (hair != null) hair.flipX = flip;
            if (top != null) top.flipX = flip;
            if (bottom != null) bottom.flipX = flip;
            if (shoes != null) shoes.flipX = flip;
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
