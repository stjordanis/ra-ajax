<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="WebMethods.aspx.cs" 
    Inherits="RaWebMethods" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    src="NestedMethods.ascx" 
    tagname="NestedMethods" 
    tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Untitled Page</title>

        <script type="text/javascript">
function init() {
  Ra.$('results').setContent('unknown');
}


function callMethod1() {
  Ra.Control.callServerMethod('foo', {
    onSuccess: function(retVal) {
      if( retVal == 'thomasTrue5' )
        Ra.$('results').setContent('success');
    },
    onError: function(status, fullTrace) {
      alert(fullTrace);
    }
  }, [5, true, 'thomas']);
}


function callMethod2() {
  Ra.Control.callServerMethod('NestedMethods1.NestedMethods21.foo', {
    onSuccess: function(retVal) {
      if( retVal == '5thomas' )
        Ra.$('results').setContent('success');
    },
    onError: function(status, fullTrace) {
      alert(fullTrace);
    }
  }, [5, 'thomas']);
}

        </script>

    </head>
    <body>
        <form id="form1" runat="server">
        <div id="results">
            Unknown
        </div>
            <button id="btn" onclick="callMethod1();return false;">Click me</button>
            <button id="btn2" onclick="callMethod2();return false;">Click me 2</button>
            <ra:Label runat="server" ID="lbl" />
            <uc1:NestedMethods 
                ID="NestedMethods1" 
                runat="server" />
        </form>
    </body>
</html>
