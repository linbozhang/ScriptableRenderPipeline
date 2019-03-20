using UnityEngine.Rendering;
using UnityEngine.Rendering.LookDev;

using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Rendering.LookDev
{
    /// <summary>
    /// Main entry point for scripting LookDev
    /// </summary>
    public static class LookDev
    {
        static ILookDevDataProvider dataProvider => RenderPipelineManager.currentPipeline as ILookDevDataProvider;

        /// <summary>
        /// Does LookDev is supported with the current render pipeline?
        /// </summary>
        public static bool supported => dataProvider != null;

        [MenuItem("Window/Experimental/NEW Look Dev", false, -1)]
        public static void ShowLookDevTool()
        {
            LoadConfig();

            LookDevWindow window = EditorWindow.GetWindow<LookDevWindow>();
            window.titleContent = LookDevStyle.WindowTitleAndIcon;

            renderer = new LookDevRenderer(window, currentContext, sceneContents);
        }

        static LookDevRenderer renderer;

        public static LookDevContent sceneContents { get; set; } = new LookDevContent();


        const string lastRenderingDataSavePath = "Library/LookDevConfig.asset";

        public static LookDevContext currentContext { get; set; }

        static LookDev() => currentContext = LoadConfigInternal() ?? GetDefaultContext();

        static LookDevContext GetDefaultContext() => UnityEngine.ScriptableObject.CreateInstance<LookDevContext>();

        public static void ResetConfig() => currentContext = GetDefaultContext();

        static LookDevContext LoadConfigInternal(string path = lastRenderingDataSavePath)
        {
            var last = InternalEditorUtility.LoadSerializedFileAndForget(path)?[0] as LookDevContext;
            if (last != null && !last.Equals(null))
                return ((LookDevContext)last);
            return null;
        }

        public static void LoadConfig(string path = lastRenderingDataSavePath)
        {
            var last = LoadConfigInternal(path);
            if (last != null)
                currentContext = last;
        }

        public static void SaveConfig(string path = lastRenderingDataSavePath)
            => InternalEditorUtility.SaveToSerializedFileAndForget(new[] { currentContext }, path, true);
    }
}
