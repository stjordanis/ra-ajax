/*
 * Ra-Ajax - A Managed Ajax Library for ASP.NET and Mono
 * Copyright 2008 - 2009 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the LGPL version 3 which 
 * can be found in the license.txt file on disc.
 * 
 */

using System;
using ASPCTRLS = System.Web.UI;
using Ra.Widgets;
using Ra.Extensions;
using System.Drawing;

namespace Samples
{
    public partial class TreeView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Here you can see that you can in addition to adding up Behaviors in
            // .ASPX markup also add up Behaviors in codebehind, though it you add them
            // up in codebehind they will be considered "dynamically added controls"
            // which means all "normal rules" of dynamically added controls exists
            // and you must re-add them on every callback/postback just as you would
            // do with other dynamic controls...
            // Since the "Huge" TreeNode spends quite some time doing its stuff we
            // add up a BehaviorUpdater to its ExpanderControl (since we're expanding
            // nodes in the tree by clicking the plus sign and not the whole node)
            // to make sure it will animate nicely when being asked for new nodes...
            BehaviorUpdater updater = new BehaviorUpdater();
            updater.ID = "updaterForHuge";
            updater.Color = Color.FromArgb(80, 80, 80);
            updater.Delay = 200;
            huge_collection.ExpanderControl.Controls.Add(updater);
        }

        protected void good_2_GetChildNodes(object sender, EventArgs e)
        {
            // Item that was expanded
            TreeNodes parent = sender as TreeNodes;

            // First child TreeNode
            TreeNode item = new TreeNode();
            item.ID = "HTML_normal_tree";
            item.Text = "HTML";

            // Creating more children for the HTML node
            TreeNodes htmlNodes = new TreeNodes();
            htmlNodes.ID = "html_children";
            htmlNodes.GetChildNodes += new EventHandler(htmlNodes_GetChildNodes);
            item.Controls.Add(htmlNodes);

            parent.Controls.Add(item);

            // Second child TreeNode
            item = new TreeNode();
            item.ID = "XHTML_tree";
            item.Text = "XHTML";
            parent.Controls.Add(item);
        }

        void htmlNodes_GetChildNodes(object sender, EventArgs e)
        {
            // Item that was expanded
            TreeNodes parent = sender as TreeNodes;

            TreeNode item = new TreeNode();
            item.ID = "HTML1";
            item.Text = "HTML 1";
            parent.Controls.Add(item);

            item = new TreeNode();
            item.ID = "HTML2";
            item.Text = "HTML 2";
            parent.Controls.Add(item);

            item = new TreeNode();
            item.ID = "HTML3";
            item.Text = "HTML 3";
            parent.Controls.Add(item);

            item = new TreeNode();
            item.ID = "HTML4";
            item.Text = "HTML 4";
            parent.Controls.Add(item);

            item = new TreeNode();
            item.ID = "HTML5";
            item.Text = "HTML 5";
            parent.Controls.Add(item);
        }

        protected void get_huge(object sender, EventArgs e)
        {
            for (int idx = 0; idx < 50; idx++)
            {
                TreeNode t = new TreeNode();
                t.EnableViewState = false;
                t.ID = "huge_" + idx;
                t.Text = "Huge # " + idx;
                switch (idx)
                {
                    case 0:
                        t.Text += " - <em>when you</em>";
                        break;
                    case 1:
                        t.Text += " - <em>expanded</em>";
                        break;
                    case 2:
                        t.Text += " - <em>this huge</em>";
                        break;
                    case 3:
                        t.Text += " - <em>tree node</em>";
                        break;
                    case 4:
                        t.Text += " - <em>your entire</em>";
                        break;
                    case 5:
                        t.Text += " - <em>page will</em>";
                        break;
                    case 6:
                        t.Text += " - <em>feel</em>";
                        break;
                    case 7:
                        t.Text += " - <em>as going</em>";
                        break;
                    case 8:
                        t.Text += " - <em>through</em>";
                        break;
                    case 9:
                        t.Text += " - <em>syrup</em>";
                        break;
                    case 10:
                        t.Text += " - <em>and feel</em>";
                        break;
                    case 11:
                        t.Text += " - <em>less</em>";
                        break;
                    case 12:
                        t.Text += " - <em>responsive</em>";
                        break;
                    case 13:
                        t.Text += " - <em>this has nothing</em>";
                        break;
                    case 14:
                        t.Text += " - <em>todo with </em>";
                        break;
                    case 15:
                        t.Text += " - <em>Ra-Ajax but</em>";
                        break;
                    case 16:
                        t.Text += " - <em>rather the</em>";
                        break;
                    case 17:
                        t.Text += " - <em>sheer amount </em>";
                        break;
                    case 18:
                        t.Text += " - <em>of DOM nodes</em>";
                        break;
                    case 19:
                        t.Text += " - <em>in your page</em>";
                        break;
                    case 20:
                        t.Text += " - <em>after the</em>";
                        break;
                    case 21:
                        t.Text += " - <em>expansion.</em>";
                        break;
                }
                huge_collection_node.Controls.Add(t);
            }
        }

        protected void lnkCool1_Click(object sender, EventArgs e)
        {
            (sender as LinkButton).Text = "I was CLICKED! :)";
            new EffectSize(pnl, 500, 250, 160)
                .JoinThese(new EffectHighlight())
                .Render();

            lblCSS.Text = "Easter egg ;)";
        }

        protected void lnkCool2_Click(object sender, EventArgs e)
        {
            (sender as LinkButton).Text = "I was CLICKED! :)";
            new EffectSize(pnl, 500, 300, 170)
                .JoinThese(new EffectHighlight())
                .Render();

            lblCSS.Text = "Easter egg ;)";
        }

        protected void allowMultiSelectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (tree.SelectedNodes.Length > 0)
                tree.SelectedNodes = new TreeNode[1] { tree.SelectedNodes[tree.SelectedNodes.Length - 1] };
            tree.AllowMultipleSelectedItems = allowMultiSelectionCheckBox.Checked;
        }

        protected void selected(object sender, EventArgs e)
        {
            string selected = string.Empty;
            foreach (TreeNode node in tree.SelectedNodes)
                selected += node.ID + ", ";

            pnlOutput1.Text = selected + " were selected";
            if (tree.SelectedNodes.Length > 0)
            {
                pnlOutput2.Text = GetTextForSelection(tree.SelectedNodes[tree.SelectedNodes.Length - 1].ID);
            }
            else
            {
                pnlOutput1.Text = string.Empty;
                pnlOutput2.Text = "Please Select a Node, it is too lonely in here :(";
            }

            new EffectHighlight(pnl, 500).ChainThese(
                new EffectHighlight(pnlOutput1, 500),
                new EffectHighlight(pnlOutput2, 500)
            ).Render();
        }

        private string GetTextForSelection(string id)
        {
            switch (id)
            {
                case "good":
                    return "These are the really good things for developers in todays world";
                case "ajax":
                    return "The 3rd IT revolution";
                case "jQuery":
                    return "Great low-level Ajax Framework for skilled JavaScript developers";
                case "prototype":
                    return "Together with Dojo the first 'real' JavaScript library out there";
                case "mooTools":
                    return "Probably one of the BEST JavaScript libraries out there, EXTREMELY small, lightweight and slick (for being a JavaScript library)";
                case "html":
                    return "Basically the foundation of the 'second' IT revolution with the web as the communication channel";
                case "css":
                    return "Brilliant way to separate looks and content by our friends from Opera";
                case "why_cool":
                    return "Just proof of that you can combine controls with TreeViewItems! Try to CLICK those LinkButtons...";
                case "why_cool2":
                    return "Another proof! Try to CLICK those LinkButtons...";
                case "bad":
                    return "Evil attempts to try to shut down the Open Web";
                case "flex":
                    return "Adobe's attempt at trying to 'inherit' the dead king";
                case "silverlight":
                    return "Microsoft's attempt at trying to get 'another 30 years' with the throne";
                case "activex":
                    return "Binary BLOB technology for making 'rich internet applications'";
                case "activex1":
                    return "First attempt, we all agree on that this is bad today";
                case "activex2":
                    return "Collection of 'new' ActiveX technologies, includes Flex and Silverlight";
                case "HTML_normal_tree":
                    return "First attempt, lacks fundamental qualities needed in a document format";
                case "XHTML_tree":
                    return "Second attempt, contains guarantees and stricter validation that makes it a 'better fit' than HTML";
                case "HTML5":
                    return "This is especially exciting since it comes with video, audio, canvas and a lot of other goodies that hopefully will help us kill ActiveX2.0 ;)";
                default:
                    return "I don't really have any intelligent to say about that theme...";
            }
        }
    }
}
