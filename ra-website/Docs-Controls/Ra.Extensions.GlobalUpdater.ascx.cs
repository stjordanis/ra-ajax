﻿/*
 * Copyright 2008 - 2009 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the the MIT license which 
 * means you can use it (almost) exactly as you wish in your 
 * own projects and copy and paste as much as you want.
 */

using System;
using Ra.Extensions;

public partial class Docs_Controls_Accordion : System.Web.UI.UserControl
{
    protected void btn_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        lbl.Text = "Since we had a Thread Sleep on the server for about 2 seconds the Updater kicked in";
    }
}
