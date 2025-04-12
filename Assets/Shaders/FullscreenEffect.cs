using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullscreenEffect : ScriptableRendererFeature
{
  class FullscreenRenderPass : ScriptableRenderPass
    {
        public Material material;

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("Fullscreen Effect");
            
            // Obtener el objetivo de renderizado de la cámara
            var cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
            
            // Aplicar el efecto
            Blit(cmd, cameraColorTarget, cameraColorTarget, material);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material effectMaterial;
    private FullscreenRenderPass fullscreenPass;

    public override void Create()
    {
        fullscreenPass = new FullscreenRenderPass();
        fullscreenPass.material = effectMaterial;
        fullscreenPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (effectMaterial != null)
        {
            renderer.EnqueuePass(fullscreenPass);
        }
    }
}
