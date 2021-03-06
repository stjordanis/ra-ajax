using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class RaControlBasics : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        testStyleSerialized.Style["height"] = "100px";
    }

    protected void testCallback_Click(object sender, EventArgs e)
    {
        setInVisible.Visible = false;
    }

    protected void testCallbackSetButtonVisible_Click(object sender, EventArgs e)
    {
        setVisible.Visible = true;
    }

    protected void testAddStyles_Click(object sender, EventArgs e)
    {
        testAddStyles.Style["width"] = "150px";
        testAddStyles.Style["font-weight"] = "bold";
    }

    protected void testSettingTextProperty_Click(object sender, EventArgs e)
    {
        testSettingTextProperty.Text = "New Text";
    }

    protected void testChangeStyleValue_Click(object sender, EventArgs e)
    {
        testChangeStyleValue.Style["color"] = "Yellow";
    }

    protected void testVerifyStyleValue_Click(object sender, EventArgs e)
    {
        if (testChangeStyleValue.Style["color"] == "Yellow")
        {
            testVerifyStyleValue.Text = "success";
        }
    }

    protected void testLinkButton_Click(object sender, EventArgs e)
    {
        testLinkButton.Text = "success";
    }
}
