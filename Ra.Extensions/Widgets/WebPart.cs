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
using System.Drawing;
using Ra.Behaviors;
using Ra.Effects;

namespace Ra.Extensions.Widgets
{
    /**
     * WebPart - kind of like a "mini Window" or a "special panel". Useful for creating among other things
     * portal applications like iGoogle, PageFlakes or NetVibes.
     */
    [ASP.ToolboxData("<{0}:WebPart runat=\"server\"></{0}:WebPart>")]
    [ASP.PersistChildren(true)]
    [ASP.ParseChildren(true, "Content")]
    public class WebPart : Panel, ASP.INamingContainer
    {
        private Panel _header = new Panel();
        private Label _caption = new Label();
        private LinkButton _toggle = new LinkButton();
        private LinkButton _maximize = new LinkButton();
        private LinkButton _close = new LinkButton();
        private Panel _content = new Panel();

        /**
         * Raised when WebPart is being expanded
         */
        public event EventHandler Expanded;

        /**
         * Raised when WebPart is being collapsed
         */
        public event EventHandler Collapsed;

        /**
         * Raised when WebPart is being closed
         */
        public event EventHandler Closed;

        /**
         * Raised when WebPart is being maximized
         */
        public event EventHandler Maximized;

        /**
         * Raised when WebPart is being restored
         */
        public event EventHandler Restored;

        /**
         * Overridden to provide a sane default value
         */
        [DefaultValue("webpart")]
        public override string CssClass
        {
            get
            {
                string retVal = base.CssClass;
                if (retVal == string.Empty)
                    retVal = "webpart";
                return retVal;
            }
            set { base.CssClass = value; }
        }

        /**
         * Header text of WebPart
         */
        [DefaultValue("&nbsp;")]
        public string Caption
        {
            get { return _caption.Text; }
            set { _caption.Text = value; }
        }

        /**
         * If true (default value) then WebPart will have a "Close" icon which makes it possible
         * to close it by clicking.
         */
        [DefaultValue(true)]
        public bool IsClosable
        {
            get { return _close.Visible; }
            set { _close.Visible = value; }
        }

        /**
         * If true (default value) then WebPart will have a "Maximize" icon which makes it possible
         * to maximize and restore.
         */
        [DefaultValue(true)]
        public bool IsMaximizable
        {
            get { return _maximize.Visible; }
            set { _maximize.Visible = value; }
        }

        /**
         * If true (default value) then WebPart will have a "Maximize" icon which makes it possible
         * to maximize and restore.
         */
        [DefaultValue(true)]
        public bool IsExpandable
        {
            get { return _toggle.Visible; }
            set { _toggle.Visible = value; }
        }

        /**
         * If true (default value) then WebPart is expanded
         */
        [DefaultValue(true)]
        public bool IsExpanded
        {
            get { return _content.Style[Styles.display] != "none"; }
            set
            {
                _content.Style[Styles.display] = value ? "" : "none";
                _content.Style[Styles.opacity] = value ? "1" : "0";
                _toggle.CssClass = value ? "toggle" : "toggled";
            }
        }

        /**
         * If true then WebPart is maximized. The default value is false.
         */
        [DefaultValue(false)]
        public bool IsMaximized
        {
            get { return this.Style[Styles.width] == "100%" && this.Style[Styles.height] == "100%"; }
            set
            {
                if (value)
                {
                    Maximize();
                }
                else
                {
                    Restore();
                }
            }
        }

        private void Maximize()
        {
            IsExpanded = true;
            _maximize.CssClass = "maximized";
            string width = this.Style[Styles.width];
            string height = this.Style[Styles.height];
            if (string.IsNullOrEmpty(width))
            {
                width = "-1";
            }
            if (string.IsNullOrEmpty(height))
            {
                height = "-1";
            }
            RestoreSize = new Size(
                int.Parse(width.Replace("px", ""), System.Globalization.NumberStyles.AllowLeadingSign),
                int.Parse(height.Replace("px", ""), System.Globalization.NumberStyles.AllowLeadingSign));
            this.Style[Styles.width] = "100%";
            this.Style[Styles.height] = "100%";
        }

        private void Restore()
        {
            _maximize.CssClass = "maximize";
            if (RestoreSize.Width != -1)
            {
                this.Style[Styles.width] = RestoreSize.Width.ToString() + "px";
            }
            else
            {
                this.Style[Styles.width] = "";
            }
            if (RestoreSize.Height != -1)
            {
                this.Style[Styles.height] = RestoreSize.Height.ToString() + "px";
            }
            else
            {
                this.Style[Styles.height] = "";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();
            base.OnInit(e);
        }

        /**
         * The actual "content part" of the Window. This is where your child controls from the markup
         * will be added. If you add up items dynamically to the Window then this is where you should
         * add them up. So instead of using "myWindow.Controls.Add" you should normally use 
         * "myWindow.Content.Add" when adding Controls dynamically to the Window.
         */
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ASP.PersistenceMode(ASP.PersistenceMode.InnerDefaultProperty)]
        public ASP.ControlCollection Content
        {
            get { return _content.Controls; }
        }

        protected override void CreateChildControls()
        {
            CreateWebPartControls();
        }

        private void CreateWebPartControls()
        {
            _header.ID = "head";
            _header.CssClass = "header";

            _caption.ID = "capt";
            _caption.CssClass = "caption";

            _toggle.ID = "toggle";
            if (string.IsNullOrEmpty(_toggle.CssClass))
                _toggle.CssClass = "toggle";
            _toggle.Text = "&nbsp;";
            _toggle.Click += _toggle_Click;

            _maximize.ID = "max";
            if (string.IsNullOrEmpty(_maximize.CssClass))
                _maximize.CssClass = "maximize";
            _maximize.Text = "&nbsp;";
            _maximize.Click += _maximize_Click;

            _close.ID = "close";
            _close.CssClass = "close";
            _close.Text = "&nbsp;";
            _close.Click += _close_Click;

            _content.ID = "cont";
            _content.CssClass = "content";

            _header.Controls.Add(_caption);
            _header.Controls.Add(_toggle);
            _header.Controls.Add(_maximize);
            _header.Controls.Add(_close);
            Controls.Add(_header);
            Controls.Add(_content);
        }

        void _close_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (Closed != null)
                Closed(this, new EventArgs());
        }

        private Size RestoreSize
        {
            get { return ViewState["RestoreSize"] == null ? new Size(-1, -1) : (Size)ViewState["RestoreSize"]; }
            set { ViewState["RestoreSize"] = value; }
        }

        void _maximize_Click(object sender, EventArgs e)
        {
            if (IsMaximized)
            {
                Restore();
                if (Restored != null)
                    Restored(this, new EventArgs());
            }
            else
            {
                Maximize();
                if (Maximized != null)
                    Maximized(this, new EventArgs());
            }
        }

        void _toggle_Click(object sender, EventArgs e)
        {
            if (_toggle.CssClass == "toggle")
            {
                _toggle.CssClass = "toggled";
                new EffectRollUp(_content, 300)
                    .JoinThese(new EffectFadeOut())
                    .Render();
                if (Collapsed != null)
                    Collapsed(this, new EventArgs());
            }
            else
            {
                _toggle.CssClass = "toggle";
                new EffectRollDown(_content, 300)
                    .JoinThese(new EffectFadeIn())
                    .Render();
                if (Expanded != null)
                    Expanded(this, new EventArgs());
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Making sure that all Behaviors are in WebPart and NOT in "content Panel"
            List<ASP.Control> tmp = new List<System.Web.UI.Control>();
            foreach (ASP.Control idx in _content.Controls)
            {
                if (idx is Behavior)
                    tmp.Add(idx);
            }
            foreach (ASP.Control idx in tmp)
            {
                _content.Controls.Remove(idx);
                Controls.Add(idx);
            }

            // Calling base...
            base.OnPreRender(e);
        }
    }
}
