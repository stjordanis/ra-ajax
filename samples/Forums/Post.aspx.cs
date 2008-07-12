/*
 * Ra Ajax - An Ajax Library for Mono ++
 * Copyright 2008 - Thomas Hansen polterguy@gmail.com
 * This code is licensed under an MIT(ish) kind of license which 
 * can be found in the license.txt file on disc.
 * 
 */

using System;
using Entity;
using NHibernate.Expression;
using Ra.Widgets;

public partial class Forums_Post : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Hiding LoginPanel (if we should)
            if (Operator.Current == null)
            {
                pnlLinkToLogin.Visible = true;
            }
            else
            {
                pnlLinkToLogin.Visible = false;
            }

            // Retrieving post...
            string idOfPost = Request.Params["id"] + ".forum";
            ForumPost post = ForumPost.FindOne(Expression.Eq("Url", idOfPost), Expression.Eq("ParentPost", 0));
            if (post == null)
                Response.Redirect("Forums.aspx", true);
            headerParent.InnerHtml = post.Header;
            dateParent.InnerHtml = post.Created.ToString("dd.MMM yy - HH:mm");
            contentParent.InnerHtml = post.Body;
            operatorInfo.InnerHtml =
                string.Format("Posted by; {0} who have {1} posts",
                    post.Operator.Username,
                    post.Operator.NumberOfPosts);
            this.Title = post.Header;
            header.Text = "Re: " + post.Header;
            header.Focus();
            header.Select();

            // Binding replies...
            DataBindReplies(post);
            if (Operator.Current != null)
            {
                pnlReply.Visible = true;
            }
            else
            {
                pnlReply.Visible = false;
            }
        }
    }

    private void DataBindReplies(ForumPost post)
    {
        repReplies.DataSource = ForumPost.FindAll(Expression.Eq("ParentPost", post.Id));
        repReplies.DataBind();
    }

    protected void newSubmit_Click(object sender, EventArgs e)
    {
        // Simple valdation
        if (header.Text.Length < 5)
            return;

        // Creating new post
        ForumPost post = new ForumPost();
        post.Body = body.Text;
        post.Body += string.Format(@"
-- 
<em> {0} </em>", Operator.Current.Signature);
        post.Created = DateTime.Now;
        post.Header = header.Text;
        post.Operator = Operator.Current;

        string idOfPost = Request.Params["id"] + ".forum";
        ForumPost parent = ForumPost.FindOne(Expression.Eq("Url", idOfPost));
        post.ParentPost = parent.Id;
        
        post.Save();

        // Flashing panel
        Effect effect = new EffectFadeIn(pnlReply, 0.4M);
        effect.Render();

        effect = new EffectFadeIn(postsWrapper, 0.4M);
        effect.Render();
        body.Focus();
        body.Select();

        // Re-rendering posts to get the newly added item
        DataBindReplies(parent);
        postsWrapper.SignalizeReRender();
    }
}