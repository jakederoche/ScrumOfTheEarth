using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;




namespace marketplace
{
    /// <summary>
    /// Summary description for marketplace
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class marketplace : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)] //NOTICE: gotta enable session on each individual method
        public bool LogOn(string uid, string pass)
        {
            //we return this flag to tell them if they logged in or not
            bool success = false;

            //our connection string comes from our web.config file like we talked about earlier
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //here's our query.  A basic select with nothing fancy.  Note the parameters that begin with @
            //NOTICE: we added admin to what we pull, so that we can store it along with the id in the session
            string sqlSelect = "SELECT UserID FROM User_Data WHERE UserName=@idValue and UserPassword=@passValue";

            //set up our connection object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            //set up our command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //tell our command to replace the @parameters with real values
            //we decode them because they came to us via the web so they were encoded
            //for transmission (funky characters escaped, mostly)
            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(uid));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(pass));

            //a data adapter acts like a bridge between our command object and 
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);
            //check to see if any rows were returned.  If they were, it means it's 
            //a legit account
            if (sqlDt.Rows.Count > 0)
            {
                //if we found an account, store the id and admin status in the session
                //so we can check those values later on other method calls to see if they 
                //are 1) logged in at all, and 2) and admin or not
                Session["id"] = sqlDt.Rows[0]["UserID"];

                success = true;
            }
            //return the result!
            return success;
        }

        [WebMethod(EnableSession = true)]
        public bool LogOff()
        {
            //if they log off, then we remove the session.  That way, if they access
            //again later they have to log back on in order for their ID to be back
            //in the session!
            Session.Abandon();
            return true;
        }

        //EXAMPLE OF AN UPDATE QUERY WITH PARAMS PASSED IN
        [WebMethod(EnableSession = true)]
        public void UpdateAccount(string UserID, string UserName, string UserPassword, string UserSecQuestion, string UserSecAnswer, string UserEmail, string UserShipAddress, string UserAbout)
        {

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //this is a simple update, with parameters to pass in values
            string sqlSelect = "update user_data set UserName=@userName, UserPassword=@passValue, UserSecQuestion=@secQuestionAnswer, userSecAnswer=@secAnswer, " +
                "UserEmail=@emailValue, UserShipAddress=@shipAddress, UserAbout=@userAbout where UserID=@uID";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@userName", HttpUtility.UrlDecode(UserName));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(UserPassword));
            sqlCommand.Parameters.AddWithValue("@secQuestionAnswer", HttpUtility.UrlDecode(UserSecQuestion));
            sqlCommand.Parameters.AddWithValue("@secAnswer", HttpUtility.UrlDecode(UserSecAnswer));
            sqlCommand.Parameters.AddWithValue("@emailValue", HttpUtility.UrlDecode(UserEmail));
            sqlCommand.Parameters.AddWithValue("@shipAddress", HttpUtility.UrlDecode(UserShipAddress));
            sqlCommand.Parameters.AddWithValue("@userAbout", HttpUtility.UrlDecode(UserAbout));
            sqlCommand.Parameters.AddWithValue("@uID", HttpUtility.UrlDecode(UserID));

            sqlConnection.Open();
            //we're using a try/catch so that if the query errors out we can handle it gracefully
            //by closing the connection and moving on
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            sqlConnection.Close();

        }

        //EXAMPLE OF A DELETE QUERY
        [WebMethod(EnableSession = true)]
        public void DeleteAccount(string id)
        {


            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //this is a simple update, with parameters to pass in values
            string sqlSelect = "delete from user_data where UserId=@idValue";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(id));

            sqlConnection.Open();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            sqlConnection.Close();

        }

        //EXAMPLE OF AN INSERT QUERY WITH PARAMS PASSED IN.  BONUS GETTING THE INSERTED ID FROM THE DB!
        [WebMethod(EnableSession = true)]
        public void CreateAccount( string UserName, string UserPassword, string UserSecQuestion, string UserSecAnswer, string UserEmail, string UserShipAddress, string UserAbout)
        {
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //the only thing fancy about this query is SELECT LAST_INSERT_ID() at the end.  All that
            //does is tell mySql server to return the primary key of the last inserted row.
            string sqlSelect = "insert into user_data (UserName, UserPassword, UserSecQuestion, UserSecAnswer, UserEmail, UserShipAddress, UserAbout)" +
                "values(@userName, @passValue, @secQuestionAnswer, @secAnswer, @emailValue, @shipAddress, @userAbout); SELECT LAST_INSERT_ID();";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@userName", HttpUtility.UrlDecode(UserName));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(UserPassword));
            sqlCommand.Parameters.AddWithValue("@secQuestionAnswer", HttpUtility.UrlDecode(UserSecQuestion));
            sqlCommand.Parameters.AddWithValue("@secAnswer", HttpUtility.UrlDecode(UserSecAnswer));
            sqlCommand.Parameters.AddWithValue("@emailValue", HttpUtility.UrlDecode(UserEmail));
            sqlCommand.Parameters.AddWithValue("@shipAddress", HttpUtility.UrlDecode(UserShipAddress));
            sqlCommand.Parameters.AddWithValue("@userAbout", HttpUtility.UrlDecode(UserAbout));
           //qlCommand.Parameters.AddWithValue("@uID", HttpUtility.UrlDecode(UserID));

            //this time, we're not using a data adapter to fill a data table.  We're just
            //opening the connection, telling our command to "executescalar" which says basically
            //execute the query and just hand me back the number the query returns (the ID, remember?).
            //don't forget to close the connection!
            sqlConnection.Open();
            //we're using a try/catch so that if the query errors out we can handle it gracefully
            //by closing the connection and moving on
            try
            {
                int accountID = Convert.ToInt32(sqlCommand.ExecuteScalar());
                //here, you could use this accountID for additional queries regarding
                //the requested account.  Really this is just an example to show you
                //a query where you get the primary key of the inserted row back from
                //the database!
            }
            catch (Exception e)
            {
                throw e;
            }
            sqlConnection.Close();
        }


    }
}
