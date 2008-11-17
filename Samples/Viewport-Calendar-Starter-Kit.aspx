﻿<%@ Page 
    Language="C#" 
    AutoEventWireup="true"  
    CodeFile="Viewport-Calendar-Starter-Kit.aspx.cs" 
    Inherits="Samples.CalendarStarter" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    Assembly="Extensions" 
    Namespace="Ra.Extensions" 
    TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ra-Ajax Calendar Starter-Kit</title>
    <link href="media/skins/Sapphire/Sapphire-0.8.1.css" rel="stylesheet" type="text/css" />
    <link href="media/Calendar-0.8.1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    
        <!-- Main surface -->
        <ext:ResizeHandler 
            runat="server" 
            OnResized="resizer_Resized"
            ID="resizer" />

        <!-- "Statusbar" part -->
        <ra:Panel 
            runat="server" 
            ID="pnlStatus" 
            CssClass="status">
            Kickstart Your Web Apps with a <strong>Ra-Ajax Starter-Kit</strong>
        </ra:Panel>

        <!-- Menu -->
        <ext:Menu 
            runat="server" 
            ID="menu" 
            CssClass="menu">
            <ext:MenuItems runat="server" ID="mainItems">
                <ext:MenuItem runat="server" ID="file">
                    File
                    <ext:MenuItems runat="server" ID="fileMenus">
                        <ext:MenuItem runat="server" id="openFile">
                            Open file
                        </ext:MenuItem>
                        <ext:MenuItem runat="server" id="saveFile">
                            Save file
                        </ext:MenuItem>
                        <ext:MenuItem runat="server" id="saveFileAs">
                            Save file as...
                        </ext:MenuItem>
                    </ext:MenuItems>
                </ext:MenuItem>
                <ext:MenuItem runat="server" ID="edit">
                    Edit
                    <ext:MenuItems runat="server" ID="editMenus">
                        <ext:MenuItem runat="server" id="copy">
                            Copy
                        </ext:MenuItem>
                        <ext:MenuItem runat="server" id="paste">
                            Paste
                        </ext:MenuItem>
                        <ext:MenuItem runat="server" id="cut">
                            Cut
                        </ext:MenuItem>
                    </ext:MenuItems>
                </ext:MenuItem>
                <ext:MenuItem runat="server" ID="windows">
                    Windows
                    <ext:MenuItems runat="server" ID="windowsSub">
                        <ext:MenuItem runat="server" id="arrange">
                            Arrange
                            <ext:MenuItems runat="server" ID="arrWindows">
                                <ext:MenuItem runat="server" id="leftAligned">
                                    Left-Aligned
                                </ext:MenuItem>
                                <ext:MenuItem runat="server" id="rightAligned">
                                    Right-Aligned
                                </ext:MenuItem>
                            </ext:MenuItems>
                        </ext:MenuItem>
                        <ext:MenuItem runat="server" id="closeAll">
                            Close all
                        </ext:MenuItem>
                    </ext:MenuItems>
                </ext:MenuItem>
            </ext:MenuItems>
        </ext:Menu>

        <!-- Calendar begin -->
        <ext:Calendar 
            runat="server" 
            ID="calendarStart" 
            Caption="Start of period"
            StartsOn="Sunday"
            OnSelectedValueChanged="calendarStart_SelectedValueChanged" 
            OnRenderDay="calendarStart_RenderDay"
            style="width:170px;position:absolute;left:5px;top:63px;"
            CssClass="calendar" />

        <!-- Calendar end -->
        <ext:Calendar 
            runat="server" 
            ID="calendarEnd" 
            Caption="End of period" 
            StartsOn="Sunday"
            OnRenderDay="calendarEnd_RenderDay"
            OnSelectedValueChanged="calendarEnd_SelectedValueChanged"
            style="width:170px;position:absolute;left:180px;top:63px;"
            CssClass="calendar" />

        <!-- Bottom left parts -->
        <ext:Window 
            runat="server" 
            ID="wndBottomLeft" 
            Caption="Top left"
            style="width:345px;position:absolute;left:5px;top:238px;" 
            Movable="false" 
            Closable="false"
            CssClass="window">
            <ra:Panel 
                runat="server" 
                ID="pnlBottomLeft" 
                style="height:284px;overflow:auto;padding:5px;">
                <asp:GridView 
                    runat="server" 
                    ID="grid" 
                    AutoGenerateColumns="false"
                    BackColor="White" 
                    BorderColor="#DEDFDE" 
                    Font-Size="0.8em"
                    BorderStyle="None" 
                    BorderWidth="1px" 
                    CellPadding="4" 
                    Width="100%"
                    ForeColor="Black" 
                    GridLines="Vertical">

                    <FooterStyle BackColor="#CCCC99" />
                    <RowStyle BackColor="#F7F7DE" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />

                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("When", "{0:dddd dd.MMM - yy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Header
                            </HeaderTemplate>
                            <ItemTemplate>
                                <ra:LinkButton 
                                    runat="server" 
                                    Tooltip='<%#Eval("Body")%>'
                                    Text='<%#Eval("Header")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </ra:Panel>
        </ext:Window>

        <!-- Right - Main Content -->
        <ext:Window 
            runat="server" 
            CssClass="window" 
            Closable="false" 
            Movable="false"
            Caption="Main content"
            style="width:600px;position:absolute;top:63px;left:355px;"
            ID="wndRight">
            <ra:Panel 
                runat="server" 
                ID="pnlRight" 
                style="height:300px;overflow:auto;background-color:White;">
                <div style="padding:15px;">
                    <ra:Panel 
                        runat="server" 
                        ID="intro" 
                        style="background:White url('media/ajax.jpg') no-repeat;">
                        <h1>Ra-Ajax Starter-Kit for Calendar Applications</h1>
                        <p>
                            This is an example of how to utilize Ra-Ajax to build a Calendar Application
                            like e.g. GCalendar or something similar. It is written in C# and would probably
                            give you a head start if you need a calendar based application.
                        </p>
                        <p>
                            As you can see to the left you can choose the period to view activities from by 
                            changing the Start and End dates. Then the GridView to the left below the Calendars
                            will re-bind and show whatever activities are within that period.
                        </p>
                        <p>
                            Then when you click one of the activities you will get to see it's details and
                            also edit it.
                        </p>
                        <p>
                            Note that this is just a "Starter-Kit" and by far not a complete application in
                            any ways. Think of it like 200 lines of head-start code which you can use yourself
                            in your own applications if you need a calendar based application yourself.
                        </p>
                    </ra:Panel>
                </div>
            </ra:Panel>
        </ext:Window>

    </form>
    <script type="text/javascript">
var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
    </script>
</body>
</html>
