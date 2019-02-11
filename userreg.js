   window.onload = function() {

        document.getElementById("button").onclick = click;

       }

       function click() {

        var uName = document.getElementById("un").value;

        var pWord = document.getElementById("pw").value;
        
        var eMail = document.getElementById("em").value;

        var output = document.getElementsByClassName("outputArea");

        var output2 = document.getElementsByClassName("outputArea2");

        if(uName.length < 8 && uName.length != 0){

        output[0].innerHTML = "Success! Your username is " + uName;

        }
        else
        {

          output[0].innerHTML = "Please enter a valid username.";

        }

        if(pWord.length > 10 || pWord.length == 0){

          output[0].innerHTML = "Please enter a valid password.";

        }

        if(eMail.length == 0){

          output[0].innerHTML = "Please enter a valid email.";

        }



        un.value="";

        pw.value="";

        email.value="";

      
       }


       