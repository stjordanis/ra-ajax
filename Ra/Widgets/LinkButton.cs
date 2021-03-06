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
using Ra.Builder;

namespace Ra.Widgets
{
    /**
     * LinkButton control. Thi sis the equivalent of the HTML anchor element.
     * Note though that this will NOT be a link but overridden through JavaScript to handle click events.
     * DO NOT use this element instead of hyperlinks if you want search engine visibility! Think
     * of it as an alternative to the Button and ImageButton controls.
     */
    [DefaultProperty("Text")]
    [ASP.ToolboxData("<{0}:LinkButton runat=server />")]
    public class LinkButton : RaWebControl
    {
        /**
         * Raised when button looses focus, opposite of Focused
         */
        public event EventHandler Blur;

        /**
         * Raised when button receives Focus, opposite of Blur
         */
        public event EventHandler Focused;

        #region [ -- Properties -- ]

        /**
         * The text that is displayed within the linkbutton, default value is string.Empty.
         * Note that the Ra LinkButton can have Child Controls in the Controls Collection. If it
         * does however then the Text property will NOT be rendered but overridden by the rendering
         * of the Child Controls.
         */
        [DefaultValue("")]
        public string Text
        {
            get { return ViewState["Text"] == null ? "" : (string)ViewState["Text"]; }
            set
            {
                if (value != Text)
                    SetJSONValueString("Text", value);
                ViewState["Text"] = value;
            }
        }

        /**
         * The keyboard shortcut for clicking the button. Most browsers implements
         * some type of keyboard shortcut logic like for instance FireFox allows
         * form elements to be triggered by combining the AccessKey value (single character)
         * together with ALT and SHIFT. Meaning if you have e.g. "H" as keyboard shortcut
         * you can click this button by doing ALT+SHIFT+H on your keyboard. The combinations
         * to effectuate the keyboard shortcuts however vary from browsers to browsers.
         */
        [DefaultValue("")]
        public string AccessKey
        {
            get { return ViewState["AccessKey"] == null ? "" : (string)ViewState["AccessKey"]; }
            set
            {
                if (value != AccessKey)
                    SetJSONValueString("AccessKey", value);
                ViewState["AccessKey"] = value;
            }
        }

        #endregion

        #region [ -- Overridden (abstract/virtual) methods from RaControl -- ]

        public override void DispatchEvent(string name)
        {
            switch (name)
            {
                case "blur":
                    if (Blur != null)
                        Blur(this, new EventArgs());
                    break;
                case "focus":
                    if (Focused != null)
                        Focused(this, new EventArgs());
                    break;
                default:
                    base.DispatchEvent(name);
                    break;
            }
        }

        protected override string GetEventsRegisterScript()
        {
            string evts = base.GetEventsRegisterScript();
            if (Blur != null)
            {
                if (evts.Length != 0)
                    evts += ",";
                evts += "['blur']";
            }
            if (Focused != null)
            {
                if (evts.Length != 0)
                    evts += ",";
                evts += "['focus']";
            }
			return evts;
        }

        protected override void RenderRaControl(HtmlBuilder builder)
        {
            using (Element el = builder.CreateElement("a"))
            {
                AddAttributes(el);
                el.Write(Text);
                RenderChildren(builder.Writer);
            }
        }

        protected override void AddAttributes(Element el)
        {
            el.AddAttribute("href", "javascript:Ra.emptyFunction();");
            if (!string.IsNullOrEmpty(AccessKey))
                el.AddAttribute("accesskey", AccessKey);
            base.AddAttributes(el);
        }

        #endregion
    }
}
