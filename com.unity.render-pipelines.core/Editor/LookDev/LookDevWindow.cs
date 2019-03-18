using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Rendering.LookDev
{

    public enum Layout { ViewA, ViewB, HorizontalSplit, VerticalSplit, CustomSplit, CustomCircular }

    internal class LookDevWindow : EditorWindow
    {
        VisualElement views;
        VisualElement environment;

        const string oneViewClass = "oneView";
        const string twoViewsClass = "twoViews";
        const string showHDRIClass = "showHDRI";

        LayoutContext.Layout layout
        {
            get => LookDev.currentContext.layout.viewLayout;
            set
            {
                if (LookDev.currentContext.layout.viewLayout != value)
                {
                    if (value == LayoutContext.Layout.HorizontalSplit || value == LayoutContext.Layout.VerticalSplit)
                    {
                        if (views.ClassListContains(oneViewClass))
                        {
                            views.RemoveFromClassList(oneViewClass);
                            views.AddToClassList(twoViewsClass);
                        }
                    }
                    else
                    {
                        if (views.ClassListContains(twoViewsClass))
                        {
                            views.RemoveFromClassList(twoViewsClass);
                            views.AddToClassList(oneViewClass);
                        }
                    }

                    if (views.ClassListContains(LookDev.currentContext.layout.viewLayout.ToString()))
                        views.RemoveFromClassList(LookDev.currentContext.layout.viewLayout.ToString());
                    views.AddToClassList(value.ToString());

                    LookDev.currentContext.layout.viewLayout = value;
                }
            }
        }


        bool showHDRI
        {
            get => LookDev.currentContext.layout.showHDRI;
            set
            {
                if (LookDev.currentContext.layout.showHDRI != value)
                {
                    if (value)
                    {
                        if (!views.ClassListContains(showHDRIClass))
                            views.AddToClassList(showHDRIClass);
                    }
                    else
                    {
                        if (views.ClassListContains(showHDRIClass))
                            views.RemoveFromClassList(showHDRIClass);
                    }

                    LookDev.currentContext.layout.showHDRI = value;
                }
            }
        }

        void OnEnable()
        {
            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(LookDevStyle.k_uss));

            var toolbar = new Toolbar() { name = "toolBar" };
            rootVisualElement.Add(toolbar);
            var customToolbar = new Toolbar();
            var button1 = new ToolbarButton() { text = "1" };
            button1.Add(new Image() { image = Texture2D.whiteTexture });
            var button2 = new ToolbarToggle() { text = "2" };
            button2.Q(null, ToolbarToggle.inputUssClassName).Add(new Image() { image = Texture2D.whiteTexture });
            var button3 = new ToolbarToggle() { text = "" };
            button3.Q(null, ToolbarToggle.inputUssClassName).Add(new Image() { image = Texture2D.whiteTexture });
            button3.Q(null, ToolbarToggle.inputUssClassName).Add(new Label() { text = "3" });
            var button4 = new ToolbarToggle() { text = "4" };
            customToolbar.Add(button1);
            customToolbar.Add(new ToolbarSpacer());
            customToolbar.Add(button2);
            customToolbar.Add(new ToolbarSpacer());
            customToolbar.Add(button3);
            customToolbar.Add(new ToolbarSpacer());
            customToolbar.Add(button4);
            toolbar.Add(customToolbar);

            toolbar.Add(new ToolbarSpacer());

            var trueRadioBar = new ToolbarRadio() { name = "toolBar" };
            trueRadioBar.AddRadios(new[] {
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSingle1"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSingle2"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSideBySide"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSideBySide"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSplit"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevZone"),
                });
            trueRadioBar.RegisterCallback((ChangeEvent<int> evt) => Debug.Log(evt.newValue));
            toolbar.Add(trueRadioBar);

            views = new VisualElement() { name = "viewContainers" };
            views.AddToClassList(LookDev.currentContext.layout.isMultiView ? twoViewsClass : oneViewClass);
            views.AddToClassList("container");
            if (showHDRI)
                views.AddToClassList(showHDRIClass);
            rootVisualElement.Add(views);

            var viewA = new VisualElement() { name = "viewA" };
            views.Add(viewA);
            var viewB = new VisualElement() { name = "viewB" };
            views.Add(viewB);
            var hdri = new VisualElement() { name = "HDRI" };
            views.Add(hdri);

            viewA.Add(new Image() { image = UnityEngine.Texture2D.whiteTexture, scaleMode = UnityEngine.ScaleMode.ScaleToFit });

            rootVisualElement.Add(new Button(() =>
            {
                if (layout == LayoutContext.Layout.HorizontalSplit)
                    layout = LayoutContext.Layout.FullA;
                else if (layout == LayoutContext.Layout.FullA)
                    layout = LayoutContext.Layout.HorizontalSplit;
            })
            { text = "One/Two views" });

            rootVisualElement.Add(new Button(() => showHDRI ^= true)
            { text = "Show HDRI" });

        }
    }
    
}
