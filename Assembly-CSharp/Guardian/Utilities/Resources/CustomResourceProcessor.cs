using System.Collections;
using UnityEngine;

namespace Guardian.Utilities.Resources
{
    public class CustomResourceProcessor : MonoBehaviour
    {
        private void Start()
        {
            // Load custom textures and audio clips
            {
                if (ResourceLoader.TryGetAsset("Custom/Textures/hud.png", out Texture2D hudTextures))
                {
                    GameObject backgroundGo = GameObject.Find("Background");
                    if (backgroundGo != null)
                    {
                        Material uiMat = backgroundGo.GetComponent<UISprite>().material;
                        uiMat.mainTextureScale = hudTextures.GetScaleVector(2048, 2048);
                        uiMat.mainTexture = hudTextures;
                    }
                }

                StartCoroutine(WaitAndSetParticleTextures());
            }
        }

        private IEnumerator WaitAndSetParticleTextures()
        {
            // Load custom textures and audio clips
            ResourceLoader.TryGetAsset("Custom/Textures/dust.png", out Texture2D dustTexture);
            ResourceLoader.TryGetAsset("Custom/Textures/blood.png", out Texture2D bloodTexture);
            ResourceLoader.TryGetAsset("Custom/Textures/gun_smoke.png", out Texture2D gunSmokeTexture);

            while (true)
            {
                foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
                {
                    if (dustTexture != null
                        && (ps.name.Contains("smoke")
                            || ps.name.StartsWith("boom")
                            || ps.name.StartsWith("bite")
                            || ps.name.StartsWith("Particle System 2")
                            || ps.name.StartsWith("Particle System 3")
                            || ps.name.StartsWith("Particle System 4")
                            || ps.name.Contains("colossal_steam")
                            || ps.name.Contains("FXtitan")
                            || ps.name.StartsWith("dust"))
                        && !ps.name.StartsWith("3dmg"))
                    {
                        ps.renderer.material.mainTexture = dustTexture;
                    }

                    if (bloodTexture != null && ps.name.Contains("blood"))
                    {
                        ps.renderer.material.mainTexture = bloodTexture;
                    }

                    if (gunSmokeTexture != null && ps.name.Contains("shotGun"))
                    {
                        ps.renderer.material.mainTexture = gunSmokeTexture;
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}