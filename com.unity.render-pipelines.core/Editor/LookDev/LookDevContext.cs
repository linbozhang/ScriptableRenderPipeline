using UnityEditor.AnimatedValues;
using UnityEngine;

namespace UnityEditor.Rendering.LookDev
{
    public class LookDevContext : ScriptableObject
    {
        public LayoutContext layout { get; private set; } = new LayoutContext();
        public ViewContext viewA { get; private set; } = new ViewContext();
        public ViewContext viewB { get; private set; } = new ViewContext();
        public CameraState cameraA { get; private set; } = new CameraState();
        public CameraState cameraB { get; private set; } = new CameraState();
    }

    public class LayoutContext
    {
        public enum Layout { FullA, FullB, HorizontalSplit, VerticalSplit, CustomSplit, CustomCircular }

        public Layout viewLayout;
        public bool showHDRI;

        internal LookDevGizmoState gizmoState;

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
