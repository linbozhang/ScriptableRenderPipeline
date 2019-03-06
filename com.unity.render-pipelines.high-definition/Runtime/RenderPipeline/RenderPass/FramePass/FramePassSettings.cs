using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline.Attributes;
using static UnityEngine.Experimental.Rendering.HDPipeline.MaterialDebugSettings;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    /// <summary>Engine lighting property.</summary>
    [Flags]
    public enum LightingProperty
    {
        None = 0,
        /// <summary>Render only diffuse.</summary>
        DiffuseOnly = 1 << 0,
        /// <summary>Render only specular.</summary>
        SpecularOnly = 1 << 1,
    }

    /// <summary>Output a specific debug mode.</summary>
    public enum DebugFullScreen
    {
        None,
        Depth,
        ScreenSpaceAmbientOcclusion,
        MotionVectors
    }

    /// <summary>Frame pass settings.</summary>
    public unsafe struct FramePassSettings
    {
        /// <summary>Default settings.</summary>
        public static FramePassSettings @default = new FramePassSettings
        {
            m_MaterialProperty = MaterialSharedProperty.None,
            m_LightingProperty = LightingProperty.None,
            m_DebugFullScreen = DebugFullScreen.None,
            m_LightFilterProperty = DebugLightFilterMode.None
        };

        MaterialSharedProperty m_MaterialProperty;
        LightingProperty m_LightingProperty;
        DebugLightFilterMode m_LightFilterProperty;
        DebugFullScreen m_DebugFullScreen;

        FramePassSettings* thisPtr
        {
            get
            {
                fixed (FramePassSettings* pThis = &this)
                    return pThis;
            }
        }

        /// <summary>Create a new instance by copying values from <paramref name="other"/>.</summary>
        /// <param name="other"></param>
        public FramePassSettings(FramePassSettings other)
        {
            m_MaterialProperty = other.m_MaterialProperty;
            m_LightingProperty = other.m_LightingProperty;
            m_DebugFullScreen = other.m_DebugFullScreen;
            m_LightFilterProperty = other.m_LightFilterProperty;
        }

        /// <summary>State the property to render. In case of several SetFullscreenOutput chained call, only last will be used.</summary>
        public ref FramePassSettings SetFullscreenOutput(MaterialSharedProperty materialProperty)
        {
            m_MaterialProperty = materialProperty;
            return ref *thisPtr;
        }

        /// <summary>State the property to render. In case of several SetFullscreenOutput chained call, only last will be used.</summary>
        public ref FramePassSettings SetFullscreenOutput(LightingProperty lightingProperty)
        {
            m_LightingProperty = lightingProperty;
            return ref *thisPtr;
        }

        /// <summary>State the property to render. In case of several SetFullscreenOutput chained call, only last will be used.</summary>
        public ref FramePassSettings SetFullscreenOutput(DebugFullScreen debugFullScreen)
        {
            m_DebugFullScreen = debugFullScreen;
            return ref *thisPtr;
        }

        /// <summary>Set the light filter to use.</summary>
        public ref FramePassSettings SetLightFilter(DebugLightFilterMode filter)
        {
            m_LightFilterProperty = filter;
            return ref *thisPtr;
        }

        // Usage example:
        // (new FramePassSettings(FramePassSettings.@default)).SetFullscreenOutput(prop).FillDebugData((RenderPipelineManager.currentPipeline as HDRenderPipeline).debugDisplaySettings.data);
        public void FillDebugData(DebugDisplaySettings debug)
        {
            debug.SetDebugViewCommonMaterialProperty(m_MaterialProperty);

            switch (m_LightingProperty)
            {
                case LightingProperty.DiffuseOnly:
                    debug.SetDebugLightingMode(DebugLightingMode.DiffuseLighting);
                    break;
                case LightingProperty.SpecularOnly:
                    debug.SetDebugLightingMode(DebugLightingMode.SpecularLighting);
                    break;
                default:
                {
                    debug.SetDebugLightingMode(DebugLightingMode.None);
                    break;
                }
            }

            debug.SetDebugLightFilterMode(m_LightFilterProperty);

            switch (m_DebugFullScreen)
            {
                case DebugFullScreen.None:
                    debug.SetFullScreenDebugMode(FullScreenDebugMode.None);
                    break;
                case DebugFullScreen.Depth:
                    debug.SetFullScreenDebugMode(FullScreenDebugMode.DepthPyramid);
                    break;
                case DebugFullScreen.ScreenSpaceAmbientOcclusion:
                    debug.SetFullScreenDebugMode(FullScreenDebugMode.SSAO);
                    break;
                case DebugFullScreen.MotionVectors:
                    debug.SetFullScreenDebugMode(FullScreenDebugMode.MotionVectors);
                    break;
                default:
                    throw new ArgumentException("Unknown DebugFullScreen");
            }
        }
    }
}

