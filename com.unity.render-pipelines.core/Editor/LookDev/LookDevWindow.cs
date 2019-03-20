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
        // /!\ WARNING:
        //The following const are used in the uss.
        //If you change them, update the uss file too.
        const string k_MainContainerName = "mainContainer";
        const string k_EnvironmentContainerName = "environmentContainer";
        const string k_ViewContainerName = "viewContainer";
        const string k_FirstViewName = "firstView";
        const string k_SecondViewName = "secondView";
        const string k_ToolbarName = "toolbar";
        const string k_ToolbarRadioName = "toolbarRadio";
        const string k_SharedContainerClass = "container";
        const string k_OneViewClass = "oneView";
        const string k_TwoViewsClass = "twoViews";
        const string k_ShowEnvironmentPanelClass = "showEnvironmentPanel";

        VisualElement m_MainContainer;
        VisualElement m_ViewContainer;
        VisualElement m_EnvironmentContainer;

        Image m_FirstView;
        Image m_SecondView;

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
                        if (m_ViewContainer.ClassListContains(k_OneViewClass))
                        {
                            m_ViewContainer.RemoveFromClassList(k_OneViewClass);
                            m_ViewContainer.AddToClassList(k_TwoViewsClass);
                        }
                    }
                    else
                    {
                        if (m_ViewContainer.ClassListContains(k_TwoViewsClass))
                        {
                            m_ViewContainer.RemoveFromClassList(k_TwoViewsClass);
                            m_ViewContainer.AddToClassList(k_OneViewClass);
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
        
        bool showEnvironmentPanel
        {
            get => LookDev.currentContext.layout.showEnvironmentPanel;
            set
            {
                if (LookDev.currentContext.layout.showEnvironmentPanel != value)
                {
                    if (value)
                    {
                        if (!m_MainContainer.ClassListContains(k_ShowEnvironmentPanelClass))
                            m_MainContainer.AddToClassList(k_ShowEnvironmentPanelClass);
                    }
                    else
                    {
                        if (m_MainContainer.ClassListContains(k_ShowEnvironmentPanelClass))
                            m_MainContainer.RemoveFromClassList(k_ShowEnvironmentPanelClass);
                    }

                    LookDev.currentContext.layout.showEnvironmentPanel = value;
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
            
            m_MainContainer = new VisualElement() { name = k_MainContainerName };
            m_MainContainer.AddToClassList(k_SharedContainerClass);
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

            rootVisualElement.Add(new Button(() => showEnvironmentPanel ^= true)
            { text = "Show HDRI" });
        }

        void CreateToolbar()
        {
            var toolbarRadio = new ToolbarRadio() { name = k_ToolbarRadioName };
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

            var toolbar = new Toolbar() { name = k_ToolbarName };
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

            m_ViewContainer = new VisualElement() { name = k_ViewContainerName };
            m_ViewContainer.AddToClassList(LookDev.currentContext.layout.isMultiView ? k_TwoViewsClass : k_OneViewClass);
            m_ViewContainer.AddToClassList(k_SharedContainerClass);
            m_MainContainer.Add(m_ViewContainer);

            m_FirstView = new Image() { name = k_FirstViewName, image = Texture2D.blackTexture };
            m_ViewContainer.Add(m_FirstView);
            m_SecondView = new Image() { name = k_SecondViewName, image = Texture2D.blackTexture };
            m_ViewContainer.Add(m_SecondView);
        }

        void CreateEnvironment()
        {
            if (m_MainContainer == null || m_MainContainer.Equals(null))
                throw new System.MemberAccessException("m_MainContainer should be assigned prior CreateEnvironment()");

            m_EnvironmentContainer = new VisualElement() { name = k_EnvironmentContainerName };
            m_MainContainer.Add(m_EnvironmentContainer);
            if (showEnvironmentPanel)
                m_MainContainer.AddToClassList(k_ShowEnvironmentPanelClass);

            //to complete
        }
    }
    
}
