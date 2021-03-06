/*
 * Ra-Ajax - A Managed Ajax Library for ASP.NET and Mono
 * Copyright 2008 - 2009 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the GPL version 3 which 
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

namespace Ra.Extensions.Widgets
{
    /**
     * This is a "level" in a SliderMenu. Every level can have SliderMenuItems which in turn can
     * have SliderMenuLevels and so on recursively. Note that the SliderMenuLevel have support
     * for dynamically rendered nodes in addition to statically rendered nodes. Though NOT both
     * types of items at the same time.
     */
    [ASP.ToolboxData("<{0}:SlidingMenuLevel runat=\"server\"></{0}:SlidingMenuLevel>")]
    public class SlidingMenuLevel : Panel, ASP.INamingContainer
    {
        private bool _hasLoadedDynamicControls;

        /**
         * Returns true if the child menu items of this SlidingMenuLevel have been loaded, otherwise 
         * returns false
         */
        private bool ChildNodesLoaded
        {
            get { return _hasLoadedDynamicControls; }
        }

        /**
         * Raised when the SlidingMenuLevel needs to get its SliderMenuItem children. Note that this
         * event will be raised on EVERY postback for every SliderMenuLevel controls on your page which
         * have dynamically rendered nodes. This means you should make sure the event handler for this
         * event executes FAST!
         */
        public event EventHandler GetChildNodes;

        /**
         * Overridden to provide a sane default value. The default CSS class is "level".
         */
        [DefaultValue("sliding-level")]
        public override string CssClass
        {
            get
            {
                string retVal = base.CssClass;
                if (retVal == string.Empty)
                    retVal = "sliding-level";
                return retVal;
            }
            set { base.CssClass = value; }
        }

        [DefaultValue("")]
        public string Caption
        {
            get { return ViewState["Caption"] == null ? "" : (string)ViewState["Caption"]; }
            set { ViewState["Caption"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            Tag = "ul";
            base.OnInit(e);
            if (this.Parent is SlidingMenu)
                CssClass += " sliding-top-level";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // We MUST call the EnsureChildControls AFTER the ControlState has been loaded
            // since we're depending on some value from ControlState in order to correctly
            // instantiate the composition controls
            EnsureChildControls();
        }

        private SlidingMenu Root
        {
            get
            {
                ASP.Control idx = this.Parent;
                while (idx != null && !(idx is SlidingMenu))
                    idx = idx.Parent;
                return idx as SlidingMenu;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Reloading dynamic items, but only if the have been loaded before...
            if (_hasLoadedDynamicControls)
                RaiseGetChildNodes();

            if (Root.ActiveLevel == this.ID || 
                (string.IsNullOrEmpty(Root.ActiveLevel) && this.Parent is SlidingMenu))
                Root.EnsureBreadCrumbCreated();
        }

        protected override void LoadControlState(object savedState)
        {
            if (savedState != null)
            {
                object[] tmp = savedState as object[];
                // Mono sometimes messes up the types here for what reasons I don't know... :(
                // It appears that it tries to load control state for objects added dynamically this round
                // which obviously is WRONG...!
                if (tmp != null)
                {
                    if (tmp[0] != null && tmp[0].GetType() == typeof(bool))
                    {
                        _hasLoadedDynamicControls = (bool)tmp[0];
                    }
                    base.LoadControlState(tmp[1]);
                }
            }
        }

        protected override object SaveControlState()
        {
            object[] retVal = new object[2];
            retVal[0] = _hasLoadedDynamicControls;
            retVal[1] = base.SaveControlState();
            return retVal;
        }

        internal bool HasChildren
        {
            get
            {
                foreach (ASP.Control idx in Controls)
                {
                    if (idx is SlidingMenuItem)
                        return true;
                }
                return false;
            }
        }

        internal bool EnsureChildNodes()
        {
            if (_hasLoadedDynamicControls)
                return false;
            foreach (ASP.Control idx in Controls)
            {
                if (idx is SlidingMenuItem)
                    return false;
            }
            RaiseGetChildNodes();
            return true;
        }

        public void RaiseGetChildNodes()
        {
            if (GetChildNodes != null)
            {
                GetChildNodes(this, new EventArgs());
                _hasLoadedDynamicControls = true;
            }
        }

        internal void SetForReRendering()
        {
            _hasLoadedDynamicControls = false;
        }
    }
}
