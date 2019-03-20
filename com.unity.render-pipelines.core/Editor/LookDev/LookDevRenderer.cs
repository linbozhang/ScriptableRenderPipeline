using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.LookDev
{
    class LookDevRenderTextureCache
    {
        public enum RT { First, Second, Composite };

        RenderTexture[] m_RTs = new RenderTexture[3];

        RenderTexture this[RT index]
            => m_RTs[(int)index];

        //TODO: check resizing
        public void UpdateSize(Rect rect, RT index)
            => m_RTs[(int)index] = new RenderTexture(
                (int)rect.width, (int)rect.height, 0,
                RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
    }

    /// <summary>
    /// Rendering logic
    /// </summary>
    internal class LookDevRenderer
    {
        LookDevRenderTextureCache m_Textures;

        LookDevContext context => LookDev.currentContext;

        public void Render()
        {
            //if (Event.current.type == EventType.Repaint)
            //{
            //    if (m_LookDevConfig.rotateObjectMode)
            //        m_ObjRotationAcc = Math.Min(m_ObjRotationAcc + Time.deltaTime * 0.5f, 1.0f);
            //    else
            //        // Do brutal stop because weoften want to stop at a particular position
            //        m_ObjRotationAcc = 0.0f; // Math.Max(m_ObjRotationAcc - Time.deltaTime * 0.5f, 0.0f);

            //    if (m_LookDevConfig.rotateEnvMode)
            //        m_EnvRotationAcc = Math.Min(m_EnvRotationAcc + Time.deltaTime * 0.5f, 1.0f);
            //    else
            //        // Do brutal stop because weoften want to stop at a particular position
            //        m_EnvRotationAcc = 0.0f; // Math.Max(m_EnvRotationAcc - Time.deltaTime * 0.5f, 0.0f);

            //    // Handle objects/env rotation
            //    // speed control (in degree) - Time.deltaTime is in seconds
            //    m_CurrentObjRotationOffset = (m_CurrentObjRotationOffset + Time.deltaTime * 360.0f * 0.3f * m_LookDevConfig.objRotationSpeed * m_ObjRotationAcc) % 360.0f;
            //    m_LookDevConfig.lookDevContexts[0].envRotation = (m_LookDevConfig.lookDevContexts[0].envRotation + Time.deltaTime * 360.0f * 0.03f * m_LookDevConfig.envRotationSpeed * m_EnvRotationAcc) % 720.0f; // 720 to match GUI
            //    m_LookDevConfig.lookDevContexts[1].envRotation = (m_LookDevConfig.lookDevContexts[1].envRotation + Time.deltaTime * 360.0f * 0.03f * m_LookDevConfig.envRotationSpeed * m_EnvRotationAcc) % 720.0f; // 720 to match GUI

                switch (context.layout.viewLayout)
                {
                    case LayoutContext.Layout.FullA:
                    RenderPreviewSingle(LookDevRenderTextureCache.RT.First);
                    break;
                case LayoutContext.Layout.FullB:
                    RenderPreviewSingle(LookDevRenderTextureCache.RT.Second);
                    break;
                    case LayoutContext.Layout.HorizontalSplit:
                    case LayoutContext.Layout.VerticalSplit:
                        RenderPreviewSideBySide();
                        break;
                    case LayoutContext.Layout.CustomSplit:
                    case LayoutContext.Layout.CustomCircular:
                        RenderPreviewDualView();
                        break;
                }
            //}
        }

        void RenderPreviewSingle(LookDevRenderTextureCache.RT index)
        {

            //UpdateRenderTexture(m_PreviewRects[2]);

            //RenderScene(m_PreviewRects[2], m_Textures[index], m_PreviewUtilityContexts[index], m_LookDevConfig.currentObjectInstances[index], m_LookDevConfig.cameraState[index], false);
            //RenderCompositing(m_PreviewRects[2], m_PreviewUtilityContexts[index], m_PreviewUtilityContexts[index], false);
        }

        void RenderPreviewSideBySide()
        {

        }

        void RenderPreviewDualView()
        {

        }


    }
}
