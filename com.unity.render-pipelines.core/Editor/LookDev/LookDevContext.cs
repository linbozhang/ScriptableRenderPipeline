using UnityEditor.AnimatedValues;
using UnityEngine;

namespace UnityEditor.Rendering.LookDev
{
    public class LookDevContext : ScriptableObject
    {
        public LayoutContext layout { get; private set; } = new LayoutContext();
        public ViewContext viewA { get; private set; } = new ViewContext();
        public ViewContext viewB { get; private set; } = new ViewContext();
        public LookDevCameraState cameraA { get; private set; } = new LookDevCameraState();
        public LookDevCameraState cameraB { get; private set; } = new LookDevCameraState();
    }

    public class LayoutContext
    {
        // /!\ WARNING: these value name are used as uss file too.
        // if your rename here, rename in the uss too.
        public enum Layout { FullA, FullB, HorizontalSplit, VerticalSplit, CustomSplit, CustomCircular }

        public Layout viewLayout;
        public bool showEnvironmentPanel;

        internal LookDevGizmoState gizmoState = new LookDevGizmoState();

        public bool isSimpleView => viewLayout == Layout.FullA || viewLayout == Layout.FullB;
        public bool isMultiView => viewLayout == Layout.HorizontalSplit || viewLayout == Layout.VerticalSplit;
        public bool isCombinedView => viewLayout == Layout.CustomSplit || viewLayout == Layout.CustomCircular;
    }

    public class ViewContext
    {
        //[TODO: add object]
        //[TODO: add object position]
        //[TODO: add camera frustum]
        //[TODO: add HDRI]
        //[TODO: manage shadow and lights]
    }
    
}
