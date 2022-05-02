namespace UnityEngine.Rendering.LWRP
{
    internal class MobileFxaaLwrpPass : ScriptableRenderPass
    {
        public Material material;

        private readonly string tag;
        private readonly float sharpness;
        private readonly float threshold;

        CommandBuffer cmd;

        static readonly int tempCopyString = Shader.PropertyToID("_TempCopy");
        static readonly int sharpnessString = Shader.PropertyToID("_Sharpness");
        static readonly int thresholdString = Shader.PropertyToID("_Threshold");

        private RenderTargetIdentifier source;
        private RenderTargetIdentifier tempCopy = new RenderTargetIdentifier(tempCopyString);

        public MobileFxaaLwrpPass(RenderPassEvent renderPassEvent, Material material,
            float sharpness, float threshold, string tag)
        {
            this.renderPassEvent = renderPassEvent;
            this.tag = tag;
            this.material = material;
            this.sharpness = sharpness;
            this.threshold = threshold;
        }
        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
            cmd = CommandBufferPool.Get(tag);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            //fuck this shit MAAAN
            cmd.GetTemporaryRT(
                tempCopyString, opaqueDesc, FilterMode.Bilinear);
            cmd.CopyTexture(source, tempCopy);

            material.SetFloat(sharpnessString, sharpness);
            material.SetFloat(thresholdString, threshold);

            cmd.Blit(tempCopy, source, material, 0);

            context.ExecuteCommandBuffer(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempCopyString);
        }
    }
}
