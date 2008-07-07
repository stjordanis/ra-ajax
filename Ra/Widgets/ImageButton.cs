/*
 * Ra Ajax - An Ajax Library for Mono ++
 * Copyright 2008 - Thomas Hansen polterguy@gmail.com
 * This code is licensed under an MIT(ish) kind of license which 
 * can be found in the license.txt file on disc.
 * 
 */

using System;
using System.ComponentModel;
using WEBCTRLS = System.Web.UI.WebControls;
using ASP = System.Web.UI;
using Ra.Helpers;

namespace Ra.Widgets
{
    [DefaultProperty("Text")]
    [ASP.ToolboxData("<{0}:Button runat=server />")]
    public class ImageButton : RaWebControl, IRaControl
    {
        public event EventHandler Click;

        #region [ -- Properties -- ]

        [DefaultValue("")]
        public string ImageUrl
        {
            get { return ViewState["ImageUrl"] == null ? "" : (string)ViewState["ImageUrl"]; }
            set
            {
                if (value != ImageUrl)
                    SetJSONGenericValue("src", value);
                ViewState["ImageUrl"] = value;
            }
        }

        [DefaultValue("")]
        public string AlternateText
        {
            get { return ViewState["AlternateText"] == null ? "" : (string)ViewState["AlternateText"]; }
            set
            {
                if (value != AlternateText)
                    SetJSONGenericValue("alt", value);
                ViewState["AlternateText"] = value;
            }
        }

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

        [DefaultValue(true)]
        public bool Enabled
        {
            get { return ViewState["Enabled"] == null ? true : (bool)ViewState["Enabled"]; }
            set
            {
                if (value != Enabled)
                    SetJSONGenericValue("disabled", (value ? "" : "disabled"));
                ViewState["Enabled"] = value;
            }
        }

        #endregion

        #region [ -- Overridden (abstract/virtual) methods from RaControl -- ]

        // Override this one to handle events fired on the client-side
        public override void DispatchEvent(string name)
        {
            switch (name)
            {
                case "click":
                    if (Click != null)
                        Click(this, new EventArgs());
                    break;
                default:
                    throw new ApplicationException("Unknown event fired for control");
            }
        }

        // Override this one to create specific initialization script for your widgets
        public override string GetClientSideScript()
        {
            if (Click == null)
                return string.Format("Ra.C('{0}');", ClientID);
            else
                return string.Format("Ra.C('{0}', {{evts:['click']}});", ClientID);
        }

        // Override this one to create specific HTML for your widgets
        public override string GetHTML()
        {
            if (string.IsNullOrEmpty(ImageUrl) || string.IsNullOrEmpty(AlternateText))
                throw new ApplicationException("Cannot have empty Src or AlternateText of ImageButton");
            string accessKey = string.IsNullOrEmpty(AccessKey) ? "" : string.Format(" accesskey=\"{0}\"", AccessKey);

            // Note that since the input type="image" creates a SUBMIT button we need to handle the onclick and return false 
            // to prevent the form from submitting for ImageButtons...
            return string.Format("<input onclick=\"return false;\" type=\"image\" id=\"{0}\" src=\"{1}\" alt=\"{5}\"{2}{3}{4}{6} />",
                ClientID,
                ImageUrl,
                GetCssClassHTMLFormatedAttribute(),
                GetStyleHTMLFormatedAttribute(),
                accessKey,
                AlternateText,
                (Enabled ? "" : "disabled=\"disabled\""));
        }

        #endregion
    }
}