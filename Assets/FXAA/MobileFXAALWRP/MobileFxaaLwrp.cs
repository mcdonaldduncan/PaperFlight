namespace UnityEngine.Rendering.LWRP
{
    public class MobileFxaaLwrp : ScriptableRendererFeature
    {
        [System.Serializable]
        public class MobileBlurSettings
        {
            public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;

            public float Sharpness = 4.0f;

            public float Threshold = 0.2f;

            public Material BlitMaterial = null;
        }

        public MobileBlurSettings settings = new MobileBlurSettings();

        MobileFxaaLwrpPass mobileFxaaLwrpPass;

        public override void Create()
        {
            mobileFxaaLwrpPass = new MobileFxaaLwrpPass(settings.Event, settings.BlitMaterial, settings.Sharpness, settings.Threshold, this.name);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            mobileFxaaLwrpPass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(mobileFxaaLwrpPass);
        }
    }
}

