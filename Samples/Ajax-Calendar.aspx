<%@ Page 
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    CodeFile="Ajax-Calendar.aspx.cs" 
    Inherits="AjaxCalendar" 
    Title="Ajax Calendar Sample" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    Assembly="Extensions" 
    Namespace="Ra.Extensions" 
    TagPrefix="ext" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="cnt1" 
    runat="server">

    <h1>Ajax Calendar Sample</h1>
    <p>
        This is our <em>Ajax Calendar</em> reference sample. The Ajax Calendar is an Extension control which
        first of all means that it can be found in the <em>Extensions project</em>. One of the important
        features of the Ra-Ajax Calendar is that first of all it does <em>not add to the JavaScript size
        of your page at all</em>. This is because it is in its entirety composed of other existing Ajax Controls
        like LinkButtons, Labels, DropDownLists and so on.
    </p>
    <ext:Calendar 
        runat="server" 
        ID="calTab" 
        CssClass="calendar" 
        OnSelectedValueChanged="calTab_SelectedValueChanged"
        Value="2008.07.20 23:54" />
    <br />
    <ra:Label 
        runat="server" 
        style="color:#999;font-style:italic;"
        ID="lbl" Text="Watch me as you change the date" />
    <br />
    <p>
        Try to change the Date in the Ajax Calendar above...
    </p>
    <br />
    <h2>Ra-Ajax and Mono/Linux/Apache</h2>
    <p>
        Did you know that even though Ra-Ajax is written in .Net it is still possible (and easy) to deploy
        your Ajax Applications an Linux by using <a href="http://www.mono-project.com/">Mono</a>? Ra-Ajax
        is very dedicated to supporting Mono. In fact without Mono we probably would have implemented
        Ra-Ajax in RoR or J2EE or something...
    </p>
    <p>
        A very good example of a quite complex Ajax application which is written in .Net but runs on
        Mono, Apache and Linux is <a href="http://grurrah.com">Grurrah your environmental friend</a> 
        which all though is not developed in Ra-Ajax but still developed in ASP.NET. Grurrah is written 
        in .Net, compiled using Mono's C# compiler, deployed on Apache and Linux running Mono and 100% 
        Open Source from the bottom and all the way to the top :)
        <br />
        It even uses an Open Source database - <a href="http://mysql.com/">MySQL</a>!
    </p>
    <p>
        A lot of people are claiming (unjust) that Mono is a "child's toy version" of .Net and that they
        will always play catchup with Microsoft. This is not true! Mono is a fully working 
        implementation of .Net. It even have full support for LINQ, generics, ASP.NET and mostly everything
        you would expect to be in a full version of .Net.
    </p>
    <p>
        I happen to know large portions of the Mono team, including Miguel DeIcaza and I know that they
        have a lot of resources on making sure that they are constantly compatible with every aspect
        of Microsoft.NET. Sometimes they probably have more resources on Mono than what Microsoft 
        have on .Net due to Microsoft's "big gun development plan" where they target everything into
        one aspect of their development.
    </p>
    <p>
        Ra-Ajax (including our Ajax Calendar ;) will always be focusing a lot of resources on being
        Mono Compatible so that you can have the choice of deploying your applications on both Linux
        and Windows Servers. Just like we believe in that in the client layer you should always be able
        to run your applications on everything from "Mom's toaster" to your "Cousin's mainframe" we also
        believe that you should be able to deploy your applications on Linux, Windows, Apple and 
        everything which can run either Mono or Windows Servers.
    </p>
    <p>
        In fact you could probably get your Ra-Ajax applications to run on J2EE by using 
        <a href="http://mainsoft.com/">Grasshopper</a> from MainSoft if you wanted. This means you
        could develop in Boo on Linux using MonoDevelop, then compile through GrassHopper on Windows
        and finally deploy on FreeBSD using Tomcat or JBoss. Though you'd have to be pretty insane for
        having such a weird development cycle ;)
        <br />
        But you CAN! And this is what Ra-Ajax is all about! Freedom of choice!
    </p>
    <p>
        <strong>"Build once, deploy anywhere and run all over the place"</strong> is one of our slogans.
        The other is<strong>"Building blocks for the next 5000 years"</strong>. I hope you believe this
        and are willing to <a href="http://code.google.com/p/ra-ajax/">give Ra-Ajax a shot</a>.
        Ra-Ajax is free to use, we don't have any plans for starting charging you for anything else
        than consulting services (if you should need it), you can use it for free in your Closed Source
        applications (due to LGPL) and we will even help you for free in our 
        <a href="http://ra-ajax.org/Forums/Forums.aspx">forums</a> as much as we can.
    </p>
    <p>
        Quote from me;
        <br />
        <em>"Give me some freedom"</em>  ;)
    </p>
    <a href="Ajax-InPlaceEdit.aspx">On to Ajax InplaceEdit</a>
</asp:Content>