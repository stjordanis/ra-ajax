/*
 * Ra-Ajax - A Managed Ajax Library for ASP.NET and Mono
 * Copyright 2008 - 2009 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the LGPL version 3 which 
 * can be found in the license.txt file on disc.
 * 
 */

using System;
using System.ComponentModel;
using WEBCTRLS = System.Web.UI.WebControls;
using ASP = System.Web.UI;
using Ra.Widgets;
using System.IO;
using HTML = System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace Ra.Extensions
{
    /**
     * A SliderMenu is maybe easiest to define as a combination of a Tree and a Menu. Though while the
     * Tree can display every active and opened nodes at the same time, the SliderMenu can only display
     * one "level" at the time. Then when you select SliderMenuItems that have child menu items the 
     * "current" level will "phase out" and the child menu items of the newly selected item will
     * be displayed. It's extremely versatile for displaying MASSIVE menu hierarchies due to that
     * in addition to having static menu items it also have support for having dynamically rendered
     * menu items, just like the TreeView. The SliderMenu also features a Bread Crumb at the top which
     * the user can use to fast scroll backwards in the hierarchy to his original place.
     * A SliderMenu consists of SliderMenuLevel items. Which in turn consists of SliderMenuItem items
     * which in turn can have SliderMenuLevel items and so on.
     */
    [ASP.ToolboxData("<{0}:SliderMenu runat=\"server\"></{0}:SliderMenu>")]
    public class SliderMenu : Panel, ASP.INamingContainer
    {
        private Panel _bread = new Panel();

        /**
         * Raised when a SliderMenuItem is selected by the user
         */
        public event EventHandler ItemClicked;

        /**
         * Overridden to provide a sane default value. The default CSS class of the SliderMenu is
         * "slider-menu".
         */
        [DefaultValue("slider-menu")]
        public override string CssClass
        {
            get
            {
                string retVal = base.CssClass;
                if (retVal == string.Empty)
                    retVal = "slider-menu";
                return retVal;
            }
            set { base.CssClass = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // We MUST call the EnsureChildControls AFTER the ControlState has been loaded
            // since we're depending on some value from ControlState in order to correctly
            // instantiate the composition controls
            EnsureChildControls();

            // Defaulting all SliderMenuLevels to non-visible
            if (ActiveLevel == null)
            {
                foreach (ASP.Control idx in Controls)
                {
                    if (idx is SliderMenuLevel)
                    {
                        SetAllChildrenNonVisible(idx);
                    }
                }
            }
        }

        protected override void CreateChildControls()
        {
            CreateBreadCrumbWrapper();
        }

        internal void EnsureBreadCrumbCreated()
        {
            if (ActiveLevel != null)
            {
                UpdateBreadCrumb(AjaxManager.Instance.FindControl<SliderMenuLevel>(ActiveLevel));
            }
        }

        private void CreateBreadCrumbWrapper()
        {
            _bread.ID = "bread";
            _bread.CssClass = "bread-crumb";

            // To make sure it shows even when no path...
            WEBCTRLS.Literal lit = new WEBCTRLS.Literal();
            lit.ID = "litDummy";
            lit.Text = "&nbsp;";
            _bread.Controls.Add(lit);

            Controls.AddAt(0, _bread);
        }

        internal void SetAllChildrenNonVisible(ASP.Control from)
        {
            foreach (ASP.Control idx in from.Controls)
            {
                if (idx is SliderMenuLevel)
                {
                    (idx as SliderMenuLevel).Style["display"] = "none";
                }
                SetAllChildrenNonVisible(idx);
            }
        }

        internal void RaiseItemClicked(SliderMenuItem item)
        {
            if (ItemClicked != null)
                ItemClicked(item, new EventArgs());
        }

        internal string ActiveLevel
        {
            get { return ViewState["ActiveLevel"] as string; }
            set { ViewState["ActiveLevel"] = value; }
        }

        internal void SetActiveLevel(SliderMenuLevel level)
        {
            if (level == null)
            {
                ActiveLevel = "breadCrumbHome";
            }
            else
            {
                ActiveLevel = level.ID;
            }
            _bread.Controls.Clear();
            UpdateBreadCrumb(level);
            _bread.ReRender();
        }

        private void UpdateBreadCrumb(SliderMenuLevel to)
        {
            ASP.Control idx = to == null ? null : to.Parent.Parent;
            while (true)
            {
                if (idx == null || idx is SliderMenu)
                {
                    break; // Finished
                }
                else if (idx is SliderMenuItem)
                {
                    LinkButton btn = new LinkButton();
                    foreach (ASP.Control idx2 in idx.Controls)
                    {
                        if (idx2 is SliderMenuLevel)
                            btn.ID = "BTN" + idx2.ID;
                    }
                    btn.Text = (idx as SliderMenuItem).Text;
                    btn.Click += btn_Click;
                    _bread.Controls.AddAt(0, btn);
                }
                idx = idx.Parent;
            }
            if (to != null)
            {
                LinkButton home = new LinkButton();
                home.ID = "BTNbreadGoHome";
                home.Text = "Home";
                home.Click += btn_Click;
                _bread.Controls.AddAt(0, home);
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            string idOfToBecomeActive = btn.ID.Substring(3);
            SliderMenuLevel previousActive = AjaxManager.Instance.FindControl<SliderMenuLevel>(ActiveLevel);
            SliderMenuLevel level = AjaxManager.Instance.FindControl<SliderMenuLevel>(idOfToBecomeActive);
            int noLevels = 0;
            ASP.Control idxLevel = previousActive.Parent;
            while (true)
            {
                if (idxLevel is SliderMenuLevel)
                    noLevels += 1;
                if (idxLevel == level)
                    break;
                idxLevel = idxLevel.Parent;
            }
            SetActiveLevel(level);

            // Animating Menu levels...
            ASP.Control rootLevel = null;
            foreach (ASP.Control idx in Controls)
            {
                if (idx is SliderMenuLevel)
                {
                    rootLevel = idx;
                    break;
                }
            }
            new Ra.Extensions.SliderMenuItem.EffectRollOut(rootLevel, 800, true, noLevels)
                .Render();
        }
    }
}