﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string numestire = Session["Search"].ToString();
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
        con.Open();

        string query = "SELECT Id, Titlu, Poza, DataAdaugare FROM Stiri WHERE Titlu LIKE '%" + numestire +"%' ORDER BY DataAdaugare DESC";
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataReader queryCommandReader = cmd.ExecuteReader();
        DataTable dataTable = new DataTable();
        dataTable.Load(queryCommandReader);

        int numarLinii = dataTable.Rows.Count;
        for (int i = 0; i < numarLinii; i++)
            dataTable.Rows[i]["Poza"] = "Imagini/" + dataTable.Rows[i]["Poza"];

        Repeater1.DataSource = dataTable;
        Repeater1.DataBind();
        con.Close();

        if (numarLinii == 0) notfound.InnerHtml = "Nu a fost gasita nici o stire care sa contina " + numestire;
        else
        notfound.InnerHtml = "Rezultatele cautarii pentru " + numestire;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //selectam butonul pe care am dat click si ii luam valoarea din CommandArgument
        var button = (Button)sender;
        string id = button.CommandArgument;
        Session["Id"] = int.Parse(id);
        Response.Redirect("News.aspx");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string id = button.CommandArgument;

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
        con.Open();
        string query = "DELETE FROM STIRI WHERE Id=" + id;
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.ExecuteNonQuery();
        con.Close();

        Response.Redirect("Search.aspx");
    }
}