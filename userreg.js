   window.onload = function() {

        document.getElementById("button").onclick = click;

       }

       function click() {

        var uName = document.getElementById("un").value;

        var pWord = document.getElementById("pw").value;
        
        var eMail = document.getElementById("em").value;

        var output = document.getElementsByClassName("outputArea");

        output[0].innerHTML = "<u><em>Registered Course:</u></em>" + "<br>" + uName + " " + pWord + " has registered to the following course <br><br>" + eMail + ":<br>" + eMail;

        un.value="";

        pw.value="";

      
       }
