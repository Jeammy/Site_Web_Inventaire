<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Administration.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--JCOTE--%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <br />
    <h1>Liste des commandes effectuées</h1>
    <asp:Label ID="LabelTrieDate" runat="server" Text="Trier par date: "></asp:Label>
    &nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="DropDownListOrder" runat="server" DataTextField="Genre" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem>Ordre croissant</asp:ListItem>
        <asp:ListItem>Ordre décroissant</asp:ListItem>
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="Commander.aspx">Faire une nouvelle commande</a><br />
    <br />
    <div>
        
        <asp:table ID="Jeux" CssClass="table" runat="server" style="margin-left: 59px" Width="597px" BorderColor="Black" BorderWidth="1px" GridLines="Both"></asp:table>
    </div>
    
    &nbsp;<br />
    <%--JCOTE--%>
    </asp:Content>

