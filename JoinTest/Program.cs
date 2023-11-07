using System;
using System.Data;
using JoinTest;

public class Program
{
    public static void Main(string[] args)
    {
        var a = new DataTable("a")
                    .AddColumn<int>("id")
                    .AddColumn<string>("full name");
        a.AddRow(new object[] { 1, "James Jones" })
         .AddRow(new object[] { 2, "John Cena" })
         .AddRow(new object[] { 3, "Kevin Kart" })
         .AddRow(new object[] { 4, "Rocky Wang" });

        var b = new DataTable("b")
                    .AddColumn<int>("id")
                    .AddColumn<string>("job")
                    .AddColumn<int>("age");
        b.AddRow(new object[] { 1, "PM", 39 })
         .AddRow(new object[] { 2, "CEO", 42})
         .AddRow(new object[] { 3, "PG", 47 });

        a.LeftJoin(b, "id");
        a.Print();

    }



}
