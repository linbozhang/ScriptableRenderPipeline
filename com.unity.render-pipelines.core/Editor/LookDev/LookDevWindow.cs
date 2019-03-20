using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Rendering.LookDev
{
    public enum Layout
    {
        ViewA,
        ViewB,
        HorizontalSplit,
        VerticalSplit,
        CustomSplit,
        CustomCircular
    }

    /// <summary>
    /// Displayer and User Interaction 
    /// </summary>
    internal class LookDevWindow : EditorWindow
    {
        VisualElement m_MainContainer;
        VisualElement m_ViewContainer;
        VisualElement m_EnvironmentContainer;

        const string oneViewClass = "oneView";
        const string twoViewsClass = "twoViews";
        const string showEnvironmentTabClass = "showHDRI";

        Image m_FirstView = new Image();
        Image m_SecondView = new Image();

        public Texture2D firstOrFullView
        {
            set => m_FirstView.image = value;
        }

        public Texture2D secondView
        {
            set => m_SecondView.image = value;
        }

        public Rect firstOrFullViewRect => m_FirstView.contentRect;
        public Rect secondViewRect => m_SecondView.contentRect;

        LayoutContext.Layout layout
        {
            get => LookDev.currentContext.layout.viewLayout;
            set
            {
                if (LookDev.currentContext.layout.viewLayout != value)
                {
                    if (value == LayoutContext.Layout.HorizontalSplit || value == LayoutContext.Layout.VerticalSplit)
                    {
                        if (m_ViewContainer.ClassListContains(oneViewClass))
                        {
                            m_ViewContainer.RemoveFromClassList(oneViewClass);
                            m_ViewContainer.AddToClassList(twoViewsClass);
                        }
                    }
                    else
                    {
                        if (m_ViewContainer.ClassListContains(twoViewsClass))
                        {
                            m_ViewContainer.RemoveFromClassList(twoViewsClass);
                            m_ViewContainer.AddToClassList(oneViewClass);
                        }
                    }

                    if (m_ViewContainer.ClassListContains(LookDev.currentContext.layout.viewLayout.ToString()))
                        m_ViewContainer.RemoveFromClassList(LookDev.currentContext.layout.viewLayout.ToString());
                    m_ViewContainer.AddToClassList(value.ToString());

                    LookDev.currentContext.layout.viewLayout = value;

                    OnLayoutChanged?.Invoke(value);
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
                        if (!m_MainContainer.ClassListContains(showEnvironmentTabClass))
                            m_MainContainer.AddToClassList(showEnvironmentTabClass);
                    }
                    else
                    {
                        if (m_MainContainer.ClassListContains(showEnvironmentTabClass))
                            m_MainContainer.RemoveFromClassList(showEnvironmentTabClass);
                    }

                    LookDev.currentContext.layout.showHDRI = value;
                }
            }
        }

        public event Action<LayoutContext.Layout> OnLayoutChanged;
        // add other event here

        void OnEnable()
        {
            rootVisualElement.styleSheets.Add(
                AssetDatabase.LoadAssetAtPath<StyleSheet>(LookDevStyle.k_uss));
            
            CreateToolbar();
            
            m_MainContainer = new VisualElement() { name = "main" };
            m_MainContainer.AddToClassList("container");
            rootVisualElement.Add(m_MainContainer);

            CreateViews();
            CreateEnvironment();



            //Below is for TESTS only
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

        void CreateToolbar()
        {
            var toolbarRadio = new ToolbarRadio() { name = "toolBar" };
            toolbarRadio.AddRadios(new[] {
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSingle1"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSingle2"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSideBySide"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSideBySide"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevSplit"),
                CoreEditorUtils.LoadIcon(LookDevStyle.k_IconFolder, "LookDevZone"),
                });
            toolbarRadio.RegisterCallback((ChangeEvent<int> evt)
                => OnLayoutChanged?.Invoke((LayoutContext.Layout)evt.newValue));
            toolbarRadio.SetValueWithoutNotify((int)layout);

            var toolbar = new Toolbar();
            toolbar.Add(new Label() { text = "Layout:" });
            toolbar.Add(toolbarRadio);
            toolbar.Add(new ToolbarSpacer());
            //to complete

            rootVisualElement.Add(toolbar);
        }

        void CreateViews()
        {
            if (m_MainContainer == null || m_MainContainer.Equals(null))
                throw new System.MemberAccessException("m_MainContainer should be assigned prior CreateViews()");

            m_ViewContainer = new VisualElement() { name = "viewContainers" };
            m_ViewContainer.AddToClassList(LookDev.currentContext.layout.isMultiView ? twoViewsClass : oneViewClass);
            m_ViewContainer.AddToClassList("container");
            m_MainContainer.Add(m_ViewContainer);

            m_EnvironmentContainer = new VisualElement() { name = "environmentContainer" };
            m_MainContainer.Add(m_EnvironmentContainer);

            m_FirstView = new Image() { name = "viewA", image = Texture2D.blackTexture };
            m_ViewContainer.Add(m_FirstView);
            m_SecondView = new Image() { name = "viewB", image = Texture2D.blackTexture };
            m_ViewContainer.Add(m_SecondView);
        }

        void CreateEnvironment()
        {
            if (m_MainContainer == null || m_MainContainer.Equals(null))
                throw new System.MemberAccessException("m_MainContainer should be assigned prior CreateViews()");

            m_EnvironmentContainer = new VisualElement() { name = "HDRI" };
            if (showHDRI)
                m_MainContainer.AddToClassList(showEnvironmentTabClass);

            //to complete

            //var hdri = new VisualElement() { name = "HDRI" };
            //m_EnvironmentContainer.Add(hdri);
        }
    }
    
}
