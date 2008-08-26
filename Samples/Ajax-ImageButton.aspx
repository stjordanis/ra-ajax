<%@ Page 
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    CodeFile="Ajax-ImageButton.aspx.cs" 
    Inherits="AjaxImageButton" 
    Title="Ajax ImageButton Sample" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="cnt1" 
    runat="server">

    <h1>Ajax ImageButton Sample</h1>
    <p>
        An <em>Ajax ImageButton</em> is a Button which instead of showing Text as the main content
        displays an Image. The Ajax ImageButton is a wrapper around the <em>&lt;input type="image"</em>
        control and as all other Ajax Controls in Ra-Ajax follows the Open Standards as given to use by
        the <a href="http://w3.org">W3C</a> 100%. Try to
        click the ImageButton below to see some interactions.
    </p>
    <ra:ImageButton 
        runat="server" 
        ID="imgBtn" 
        AlternateText="Winnie the Pooh on AJAX" 
        OnClick="imgBtn_Click"
        ImageUrl="media/180px-Winniethepooh.png" />
    <br />
    <ra:Label 
        runat="server" 
        ID="lblResults" 
        Text="Watch me change" 
        style="color:#999;font-style:italic;" />
    <br />
    <br />
    <h2>Ra-Ajax and Open Standards</h2>
    <p>
        Ra-Ajax follows the Open Standards given to use by W3C on everything. This might sound foolish
        for some people since it according to some rumours <em>is so much easier to create killer features
        by bypassing the Open Standards and create some "custom plugin"</em> or something like that.
    </p>
    <p>
        The first time I started creating Web Applications I thought it was very difficult and when
        I learned how ActiveX worked I was thrilled. With ActiveX I could do "whatever I wished" and
        I was not "locked into" thinking in HTML, CSS and JavaScript.
    </p>
    <p>
        In 1998 I created a website for me and my family. This website still exists (though I am too ashamed
        to show it) and it still runs in fact and works. This was even though I had to make a LOT of *really*
        dirty hacks to be able to have it running since this was at the *PEAK* of the Browser Wars. It still
        works and most browser will to some extend even display it roughly correct. This website was created
        using JavaScript, HTML and CSS.
    </p>
    <p>
        *NONE* of the ActiveX "websites" I created after that works today. ActiveX is today on the "scrap yard
        of IT" and nobody takes it seriously today and it has transformed into a "hall of shame technology word".
    </p>
    <br />
    <h2>The ActiveX2.0 de-evolution</h2>
    <p>
        Today there are lots of companies that wants you to believe that they have the "new and improved HTML
        and CSS and JavaScript framework". Though for some of us which have been around for quite a while
        those arguments gives us and echo feeling. The *EXACT SAME* arguments was being used by Microsoft
        in the late 90s about ActiveX.
    </p>
    <p>
        The slogan of Ra-Ajax is; <strong>Bulding blocks for the next 5000 years</strong>. And all though
        that might be slightly exaggurating I am in no doubt what so ever that web applications built
        on top of Open Web Standards instead of "ActiveX2.0" will definitely be here orders of magnitudes
        longer than the "ActiveX2.0" technologies will be here.
    </p>
    <p>
        So when it comes to using Open Standards it is basically two questions you need to ask yourself.
        Do I want to build applications that are still here 50 years from now or am I happy with
        building stuff that must be rebuilt or thrown away 5 years from now. And the second question
        is; Do I want my applications to run on *EVERYTHING* or is "only Windows Vista with SP2
        OK for me".
    </p>
    <p>
        If you build on Open Standards then your applications will probably still function 100 years
        from now and they will run on everything from your Mom's toaster to your cousin's mainframe
        system. Not to mention that nobody will own you in regards to the very foundation of your
        existance. Open Web MATTERS! And Open Web is equivalent to using Open Standards! Open Web
        is about YOU being in control of what you do and saying NO to be in the "pockets" of some
        big proprietary ActiveX2.0 Framework vendor.
    </p>
    <a href="Ajax-Label.aspx">On to Ajax Label</a>
</asp:Content>

