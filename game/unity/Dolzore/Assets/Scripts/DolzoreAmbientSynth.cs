using UnityEngine;

namespace Dolzore
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class DolzoreAmbientSynth : MonoBehaviour
    {
        private const int SampleRate = 44100;
        private const float Duration = 12f;

        private void Awake()
        {
            AudioSource source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.spatialBlend = 0f;
            source.volume = 0.18f;
            if (source.clip == null)
                source.clip = BuildClip();
            if (PlayerPrefs.GetInt("dolzore.bgm", 1) == 1)
                source.Play();
        }

        private static AudioClip BuildClip()
        {
            int count = Mathf.RoundToInt(SampleRate * Duration);
            float[] data = new float[count];
            float[] notes = { 73.42f, 98.00f, 110.00f, 146.83f, 196.00f };

            for (int i = 0; i < count; i++)
            {
                float time = (float)i / SampleRate;
                float slow = 0.5f + 0.5f * Mathf.Sin(time * Mathf.PI * 2f / Duration);
                float value = 0f;

                for (int n = 0; n < notes.Length; n++)
                {
                    float amp = 0.018f / (1f + n * 0.55f);
                    float drift = 1f + Mathf.Sin(time * 0.11f + n) * 0.0018f;
                    value += Mathf.Sin(time * notes[n] * drift * Mathf.PI * 2f + n * 0.7f) * amp;
                }

                float pulse = Mathf.Sin(time * Mathf.PI * 2f * 0.25f);
                value += Mathf.Sin(time * 36.71f * Mathf.PI * 2f) * 0.012f * (0.55f + 0.45f * pulse);
                data[i] = value * Mathf.Lerp(0.78f, 1f, slow);
            }

            AudioClip clip = AudioClip.Create("DOLZORE_Title_Ambience", count, 1, SampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
